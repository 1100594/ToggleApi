using Microsoft.AspNetCore.Mvc;
using ToggleApi.Commands;
using ToggleApi.Repository;

namespace ToggleApi.Controllers
{
    [Route("api/toggles")]
    public class TogglesController : Controller
    {
        private readonly CommandHandler commandHandler;

        public TogglesController()
        {
             commandHandler = new CommandHandler(new ToggleClientRepository());
        }

        // GET api/toogles/abc/1.0.0.0
        [HttpGet("{clientId}/{clientVersion}")]
        public IActionResult Get(string clientID, string clientVersion)
        {
            return Forbid();
        }

        // POST api/toggles?toggleName=isButtonBlue&toggleValue=false&permissions=[^abc:2]{abc:1, a:*}
        [HttpPost]
        public void Post(string toggleName, bool toggleValue, string permissions)
        {
            var createCmd = new CreateToggle(toggleName, toggleValue);
            commandHandler?.Execute(createCmd);

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
            commandHandler?.Execute(deleteCmd);
        }
    }
}
