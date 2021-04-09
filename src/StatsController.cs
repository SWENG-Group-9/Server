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
            JObject data = JObject.Parse(System.IO.File.ReadAllText(@"src/data.json"));
            string dataString = data.ToString();
            return Ok(dataString);
        }
    }
}
