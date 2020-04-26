using HardRat.Common.Models;
using HardRat.Website.Core.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HardRat.Website.Core.Controllers
{
    [Route("Api/[controller]")]
    public class ClientActionController : Controller
    {
        private readonly IHubContext<ServerHub> hub;

        public ClientActionController(IHubContext<ServerHub> hub)
        {
            this.hub = hub;
        }       

        [HttpPost("Execute")]
        public async Task ExecuteCode([FromBody] ExecuteCodeDto code)
        {
            await hub.Clients.All.SendAsync("ExecuteCode", HttpUtility.UrlDecode(code.Code));
        }

        [HttpPost("Action/{actionName}")]
        public async Task<IActionResult> ExecuteAction([FromRoute] string actionName)
        {
            var fileName = $"ClientExecutions/{actionName}.csprefab";
            if (!System.IO.File.Exists(fileName))
                return NotFound();

            using(StreamReader reader = new StreamReader(new FileStream($"ClientExecutions/{actionName}.csprefab", FileMode.Open)))
            {
                var content = await reader.ReadToEndAsync(); //Optimize to deserialize via stream someday.
                await hub.Clients.All.SendAsync("ExecuteCode", content);
                return Ok();
            }
        }
    }
}
