using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server;

namespace Server.Controllers
{
    [Route("api/door")]
    [ApiController]
    public class DoorController : ControllerBase
    {

        [HttpPut]
      public ActionResult overrideDoor()
        {
            InvokeDeviceMethod.Program.overrideDoor();
            return NoContent();
        }

        [HttpGet]
        public ActionResult<bool> doorStatus()
        {
            return Ok(Server.Program.locked);
        }  
    }
}