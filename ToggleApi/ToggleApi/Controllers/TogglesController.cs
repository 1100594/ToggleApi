using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToggleApi.Commands;
using ToggleApi.Models;
using ToggleApi.Queries;
using ToggleApi.Repository;
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

        public TogglesController(IToggleClientParser toogleClientParser, IQueryHandler queryHandler, 
            IToggleClientRepository toggleClientRepository, ICommandHandler commandHandler)
        {
            ThrowOnNullArgument(toggleClientRepository, nameof(toggleClientRepository));
            ThrowOnNullArgument(toogleClientParser, nameof(toogleClientParser));
            ThrowOnNullArgument(queryHandler, nameof(queryHandler));
            ThrowOnNullArgument(commandHandler, nameof(commandHandler));
            _toggleClientParser = toogleClientParser;
            _queryHandler = queryHandler;
            _commandHandler = commandHandler;
        }

        // GET api/toogles/abc/1.0.0.0
        [HttpGet("{clientId}/{clientVersion}")]
        public IActionResult Get(string clientId, string clientVersion)
        {
            var fetchTogglesForClientQuery = new FetchTogglesForClient()
            {
                ClientId = clientId,
                ClientVersion = clientVersion
            };

            var toggles = _queryHandler.Execute(fetchTogglesForClientQuery);

            if (toggles.Any())
                return Ok(toggles);
            return NotFound();
        }

        // POST api/toggles/myToggle=true&{v1:*}[^id1:*]
        [HttpPost("{toggleName}={toggleValue}&{expression}")]
        public IActionResult Post(string toggleName, bool toggleValue, string expression)
        {
            //TODO Create a error handler
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //TODO Check if toggle already exists 
            var createCmd = new CreateToggle(toggleName, toggleValue);
            _commandHandler.Execute(createCmd);

            //TODO Put this inside logic inside a method?
            _toggleClientParser.Input = expression;
            _toggleClientParser.ToggleValue = toggleValue;
            if (_toggleClientParser.IsValid())
            {
                _toggleClientParser.Extract(out ICollection<Client> whitelist, out IDictionary<Client, bool> customValues);
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


            return Ok();
        }

        // PUT api/toggles/isButtonBlue?toggleValue=false
        [HttpPut("{toggleName}")]
        public void Put(string toggleName, string toggleValue, string permissions)
        {

        }

        // DELETE api/toggles/isButtonBlue
        [HttpDelete("{toggleName}")]
        public void Delete(string toggleName)
        {
            var deleteCmd = new DeleteToggle(toggleName);
            _commandHandler.Execute(deleteCmd);
        }
    }
}
