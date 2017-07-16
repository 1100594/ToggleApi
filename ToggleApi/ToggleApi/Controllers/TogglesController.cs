using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToggleApi.Commands;
using ToggleApi.Properties;
using ToggleApi.Queries;
using ToggleApi.Utilities;
using static ToggleApi.Utilities.Utils;

namespace ToggleApi.Controllers
{
    [Route("api/toggles")]
    public class TogglesController : Controller
    {
        private readonly ICommandHandler _commandHandler;
        private readonly IToggleClientParser _toggleClientParser;
        private readonly IQueryHandler _queryHandler;
        private readonly ILogger<TogglesController> _log;
        public TogglesController(IToggleClientParser toggleClientParser, IQueryHandler queryHandler, ICommandHandler commandHandler, ILogger<TogglesController> log)
        {
            ThrowOnNullArgument(toggleClientParser, nameof(toggleClientParser));
            ThrowOnNullArgument(queryHandler, nameof(queryHandler));
            ThrowOnNullArgument(commandHandler, nameof(commandHandler));
            ThrowOnNullArgument(log, nameof(log));
            _toggleClientParser = toggleClientParser;
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
            _log = log;
        }


        /// <summary>
        /// Gets a list of toggles for a specific service/application (clients).
        /// </summary>
        /// <remarks>
        /// Note that the version is a string and not an integer. Examples: 1.1.1.1, *, 1
        /// </remarks>
        /// <param name="clientId">Id of the client</param>
        /// <param name="clientVersion">Version of the client</param>
        /// <returns></returns>
        /// <response code="200">Returns the requested toggle</response>
        /// <response code="404">Invalid request call</response>
        /// <response code="403">If the toggle is not found</response>
        /// <response code="500">Internal error</response>
        [HttpGet("{clientId}/{clientVersion}")]
        public IActionResult Get(string clientId, string clientVersion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var fetchTogglesForClientQuery = new FetchTogglesForClient(clientId, clientVersion);
                var toggles = _queryHandler.Execute(fetchTogglesForClientQuery);

                if (toggles.Any())
                    return Ok(toggles);

                var notFoundMessage = $"The {clientId}:{clientVersion} client does not have any toggles";
                _log.LogError($"Resource not found:{notFoundMessage}");
                return NotFound(notFoundMessage);
            }
            catch (Exception e)
            {
                _log.LogError($"{Resources.InternalErrorMessage}:{e.Message}");
                return StatusCode(500, Resources.InternalErrorMessage);
            }
        }

        /// <summary>
        /// Gets a specific toggle by its name
        /// </summary>     
        /// <response code="200">Returns the requested toggle</response>
        /// <response code="404">Invalid request call</response>
        /// <response code="403">If the toggle is not found</response>
        /// <response code="500">Internal error</response>
        [HttpGet("{toggleName}")]
        public IActionResult Get(string toggleName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var fetchToggleByName = new FetchToggleByName(toggleName);
                var toggle = _queryHandler.Execute(fetchToggleByName);

                if (toggle.IsNull())
                {
                    var notFoundMessage = $"The {toggleName} toggle was not found";
                    _log.LogError($"Resource not found:{notFoundMessage}");
                    return NotFound(notFoundMessage);
                }

                return Ok(toggle);
            }
            catch (Exception e)
            {
                _log.LogError($"{Resources.InternalErrorMessage}:{e.Message}");
                return StatusCode(500, Resources.InternalErrorMessage);
            }
        }

        /// <summary>
        /// Creates a new toggle
        /// </summary>
        /// <remarks>
        /// Note that the regex  expression contains the permissions of one or more services/applications (clients).
        /// 
        /// Rules: 
        /// {id1:v1,id2:v1} - expression to add one or more clients to the whitelist of a toggle.
        /// [^id:v1] - expression to exclude one or more clients.
        /// The version can only contains 4 digits. 
        /// The wildcard can be used to include all the versions, for example: {id1:\*}.
        /// 
        /// Some examples of the use of expressions:
        /// {a-d1_5:11.\*}
        /// {a1d:\*}{a1d:\*}
        /// {a1d:1.1.2.3}
        /// [^a1d:\*,a1:3]
        /// </remarks>
        /// <param name="toggleName">Name of the toggle</param>
        /// <param name="toggleValue">Value of the toggle</param>
        /// <param name="expression">Expressions with permissions of the client</param>
        /// <returns></returns>
        /// <response code="200">If the toggle was created</response>
        /// <response code="400">If the request is not valid</response>
        /// <response code="500">Internal error</response>
        [HttpPost("{toggleName}={toggleValue}&{expression}")]
        public IActionResult Post(string toggleName, bool toggleValue, string expression)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createCmd = new CreateToggle(toggleName, toggleValue);
                _commandHandler.Execute(createCmd);

                ParseClientPermissions(toggleName, expression);

                return Ok();
            }
            catch (Exception e)
            {
                _log.LogError($"{Resources.InternalErrorMessage}:{e.Message}");
                return StatusCode(500, Resources.InternalErrorMessage);
            }
        }

        /// <summary>
        /// Updates the value and permissions of a toggle 
        /// </summary>
        ///<remarks>
        /// Note that the regex expression contains the permissions of one or more services/applications (clients).
        /// 
        /// Rules: 
        /// {id1:v1,id2:v1} - expression to add one or more clients to the whitelist of a toggle.
        /// [^id:v1] - expression to exclude one or more clients.
        /// The version can only contains 4 digits. 
        /// The wildcard can be used to include all the versions, for example: {id1:\*}.
        /// </remarks>
        /// <param name="toggleName">Name of the toggle to update</param>
        /// <param name="toggleValue">New value of the toggle</param>
        /// <param name="expression">New permissions of the toggle</param>
        /// <response code="200">If the toggle was edited</response>
        /// <response code="404">Invalid request call</response>
        /// <response code="404">Tiggle not found</response>
        /// <response code="500">Internal error</response>
        [HttpPut("{toggleName}")]
        public IActionResult Put(string toggleName, bool toggleValue, [Optional] string expression)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updateToggleValueCmd = new UpdateToggleValue(toggleName, toggleValue);
                _commandHandler.Execute(updateToggleValueCmd);

                if (!expression.IsNull())
                    //TODO how to remove the old permissions 
                    ParseClientPermissions(toggleName, expression);

                return Ok();
            }
            catch (ArgumentException e)
            {
                _log.LogError($"Resource not found:{e.Message}");
                return NotFound($"The {toggleName} toggle was not found");
            }
            catch (Exception e)
            {
                _log.LogError($"{Resources.InternalErrorMessage}:{e.Message}");
                return StatusCode(500, Resources.InternalErrorMessage);
            }
        }

        /// <summary>
        /// Deletes a toggle
        /// </summary>
        /// <param name="toggleName">Name of the toggle to delete</param>
        /// <returns></returns>
        /// <response code="200">If the toggle was deleted</response>
        /// <response code="400">If the request is not valid</response>
        /// <response code="404">If the toggle was not found</response>
        /// <response code="500">Internal error</response>
        [HttpDelete("{toggleName}")]
        public IActionResult Delete(string toggleName)
        {
            try
            {
                //TODO Handler errors
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var deleteCmd = new DeleteToggle(toggleName);
                _commandHandler.Execute(deleteCmd);
                return Ok();
            }
            catch (ArgumentException e)
            {
                _log.LogError($"Resource not found:{e.Message}");
                return NotFound($"The {toggleName} toggle was not found");
            }
            catch (Exception e)
            {
                _log.LogError($"{Resources.InternalErrorMessage}:{e.Message}");
                return StatusCode(500, Resources.InternalErrorMessage);
            }
        }

        private void ParseClientPermissions(string toggleName, string expression)
        {
            _toggleClientParser.Input = expression;
            if (_toggleClientParser.IsValid())
            {
                var clientPermissions = _toggleClientParser.Extract();
                var whitelist = clientPermissions.Whitelist;
                var customValues = clientPermissions.CustomValues;
                if (whitelist.Any())
                {
                    var whitelistCmd = new AddToWhitelist(toggleName, whitelist);
                    _commandHandler.Execute(whitelistCmd);
                }
                if (customValues.Any())
                {
                    var customValuesCmd = new AddToCustomValues(toggleName, customValues);
                    _commandHandler.Execute(customValuesCmd);
                }
            }
        }
    }
}
