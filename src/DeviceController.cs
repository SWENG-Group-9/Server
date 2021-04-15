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

        [HttpPost("{id}/{type}")]
      public ActionResult addDevice(string id,string type)
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
                returnString.Append("[" +'"'+ Server.Program.devices[i].name + '"' +",");
                returnString.Append(Server.Program.devices[i].id + ",");
                returnString.Append('"' + Server.Program.devices[i].type + '"' + ",");
                returnString.Append(Server.Program.devices[i].status.ToString().ToLower() + ",");
                returnString.Append(Server.Program.devices[i].operation.ToString().ToLower() + "],");
            }
            returnString.Remove(returnString.Length - 1,1);
            returnString.Append("]");
            return Ok(returnString.ToString());
        }

        [HttpDelete("{id}")]
        public ActionResult removeDevice(int id)
        {
            manageDevices.removeDevice(id);
            return Ok();
        }
    }
}
