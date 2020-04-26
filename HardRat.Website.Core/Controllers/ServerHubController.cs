using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HardRat.Website.Core.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HardRat.Website.Core.Controllers
{
    [Route("api/[controller]")]
    public class ServerHubController : Controller
    {
        private readonly IHubContext<ServerHub> hub;

        public ServerHubController(IHubContext<ServerHub> hub)
        {
            this.hub = hub;
        }

        [HttpGet("Clients")]
        public IActionResult Clients()
        {
            return Ok(ServerHub.sessions.Select(c => new { ip = c.Value, connectionId = c.Key }));
        }

        public class Client
        {
            public string ConnectionId { get; set; }
            public string Ip { get; set; }
        }
    }
}
