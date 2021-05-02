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
                ? Environment.GetEnvironmentVariable("HOME").ToString() + "/data.json"
                : @"src/data.json";
            string data = System.IO.File.ReadAllText(fullFilePath);
          // string data = Directory.GetCurrentDirectory();
           return Ok(data);
        }
        [HttpGet("{id}")]   //date input format YYYY-MM-DD
        public ActionResult <string> getDateData(string id)
        {
            string fullFilePath = Environment.GetEnvironmentVariable("HOME") != null
                ? Environment.GetEnvironmentVariable("HOME").ToString() + "/data.json"
                : @"src/data.json";
            string data = System.IO.File.ReadAllText(fullFilePath);
          // string data = Directory.GetCurrentDirectory();
          int Start=data.IndexOf(id,0)+id.Length;               //index of id in string
          int End=data.IndexOf("]",Start);                     //index of end of data for month
          return "{\""+id+data.Substring(Start,End-Start)+"]}";  //return data for month in same format as json
        }
    }
}