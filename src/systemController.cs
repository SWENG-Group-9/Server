using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server;
using devices;

namespace Server.Controllers
{
    [Route("api/system")]
    [ApiController]
    public class systemController : ControllerBase
    {

        [HttpPut("{id}")]
        public ActionResult systemControl(string id)
        {
            if (id == "disable")
            {
                Program.systemDisabled=true;
            }
            if (id == "enable")
            {
                Program.systemDisabled=false;
            }
            return NoContent();
        }
    }
}