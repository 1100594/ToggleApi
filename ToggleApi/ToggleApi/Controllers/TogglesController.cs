using Microsoft.AspNetCore.Mvc;
using ToggleApi.Commands;
using ToggleApi.Repository;
using ToggleApi.Utilities;

namespace ToggleApi.Controllers
{
    [Route("api/toggles")]
    public class TogglesController : Controller
    {
        private readonly CommandHandler _commandHandler;

        public TogglesController()
        {
            _commandHandler = new CommandHandler(new ToggleClientRepository());

            Utils.ThrowOnNullArgument(_commandHandler, nameof(_commandHandler));
        }

        // GET api/toogles/abc/1.0.0.0
        [HttpGet("{clientId}/{clientVersion}")]
        public IActionResult Get(string clientId, string clientVersion)
        {
            return Forbid();
        }

        // POST api/toggles/myToggle=true&{v1:*}[^id1:*]
        [HttpPost("{toggleName}={toggleValue}&{permissions}")]
        public void Post(string toggleName, bool toggleValue, string permissions)
        {
            var createCmd = new CreateToggle(toggleName, toggleValue);
            _commandHandler.Execute(createCmd);

            //Add permissions to list

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
