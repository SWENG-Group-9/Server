using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Server.Controllers
{
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        [HttpGet]
        public ActionResult <string> getDoorStats()
        {
            string fullFilePath = Environment.GetEnvironmentVariable("HOME") != null
                ? Environment.GetEnvironmentVariable("HOME").ToString() + "\\site\\wwroot\\src\\data.json"
                : @"src/data.json";
            string data = System.IO.File.ReadAllText(fullFilePath);
          // string data = Directory.GetCurrentDirectory();
           return Ok(data);
        }
    }
}
