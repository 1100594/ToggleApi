using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ToggleApi.Controllers
{
    [Route("api/toggles")]
    public class TogglesController : Controller
    {
        // GET api/toogles/abc/1.0.0.0
        [HttpGet("{clientId}/{clientVersion}")]
        public string Get(string clientID, string clientVersion)
        {
            return "value";
        }

        // POST api/toggles?toggleName=isButtonBlue&toggleValue=false&permissions=[^abc:2]{abc:1, a:*}
        [HttpPost]
        public void Post(string toggleName, bool toggleValue, string permissions)
        {

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
        }
    }
}
