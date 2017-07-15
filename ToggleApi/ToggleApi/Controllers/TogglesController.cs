using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToggleApi.Commands;
using ToggleApi.Models;
using ToggleApi.Repository;
using ToggleApi.Utilities;

namespace ToggleApi.Controllers
{
    [Route("api/toggles")]
    public class TogglesController : Controller
    {
        private readonly CommandHandler _commandHandler;
        private readonly IToggleClientParser _toggleClientParser;


        public TogglesController(IToggleClientParser toogleClientParser)
        {
            _commandHandler = new CommandHandler(new ToggleClientRepository());
            Utils.ThrowOnNullArgument(_commandHandler, nameof(_commandHandler));
            _toggleClientParser = toogleClientParser;
            Utils.ThrowOnNullArgument(_toggleClientParser, nameof(toogleClientParser));
        }

        // GET api/toogles/abc/1.0.0.0
        [HttpGet("{clientId}/{clientVersion}")]
        public IActionResult Get(string clientId, string clientVersion)
        {
            return Forbid();
        }

        // POST api/toggles/myToggle=true&{v1:*}[^id1:*]
        [HttpPost("{toggleName}={toggleValue}&{expression}")]
        public void Post(string toggleName, bool toggleValue, string expression)
        {
            var createCmd = new CreateToggle(toggleName, toggleValue);
            _commandHandler.Execute(createCmd);
            _toggleClientParser.Input = expression;
            if (_toggleClientParser.IsValid())
            {
                _toggleClientParser.Extract(out ICollection<Client> whilelist, out IDictionary<Client, bool> customValues);
            }
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
