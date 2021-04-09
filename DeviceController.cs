using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server;
using register;
using System.Text;

namespace Server.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        [HttpPost("{id}")]
      public ActionResult addDevice(string id,int type)
        {
            string connstring = manageDevices.addDeviceEntrance(id,type);
            return Ok(connstring);
        }
        [HttpGet]
        public ActionResult getDevices()
        {
            StringBuilder returnString = new StringBuilder();
            returnString.Append("[");
            for(int i = 0; i < Server.Program.devices.Count;i++)
            {
                returnString.Append("[" + Server.Program.devices[i] + ",");
                returnString.Append(Server.Program.types[i] + ",");
                returnString.Append(Server.Program.keys[i] + "] , ");
            }
            returnString.Remove(returnString.Length - 1,1);
            returnString.Append("]");
            return Ok(returnString.ToString());
        }
    }
} 