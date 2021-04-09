using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Server.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        [HttpGet]
        public ActionResult <string> getDoorStats()
        {
            string data = System.IO.File.ReadAllText(@"src/data.json");
            return Ok(data);
        }
    }
}
