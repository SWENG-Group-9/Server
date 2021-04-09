using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server;

namespace Server.Controllers
{
    [Route("api/max")]
    [ApiController]
    public class MaxController : ControllerBase
    {
        [HttpGet]
        public ActionResult<int> GetMax()
        {
            var commandItem = Server.Program.max;
            return Ok(commandItem);
        }
        [HttpPut("{id}")]
        public ActionResult PutMax(int id)
        {
            Server.Program.max = id;
            return NoContent();
        }
    }
}