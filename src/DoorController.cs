using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server;

namespace Server.Controllers
{
    [Route("api/door")]
    [ApiController]
    public class DoorController : ControllerBase
    {

        [HttpPut("{id}")]
      public ActionResult overrideDoor(int id)
        {
            InvokeDeviceMethod.Program.overrideDoor(id);
            return NoContent();
        }

        [HttpGet]
        public ActionResult<bool> doorStatus()
        {
            return Ok();
        }

        [HttpPost("{id}")]
        public ActionResult disable(int i)
        {
            if(i == 0)
            {
                Server.Program.disabled =false;
            }
            else
            {
                Server.Program.disabled = true;
            }
            InvokeDeviceMethod.Program.setDoor();
            return NoContent();
        }
    }
}