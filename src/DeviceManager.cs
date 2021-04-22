using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using devices;
namespace register
{
    class manageDevices
    {
        static int ids = 0;
        static string type = "";
        static string tempID;
        static string deviceconn;

        static string connectionString = Server.Program.config["Owner"];     
        public static void updateDeviceList()
        {
            GetDeviceIdAsync().Wait();
        }
        private static async Task GetDeviceIdAsync()
        {
            RegistryManager registryManager =
              RegistryManager.CreateFromConnectionString(connectionString);
            try
            {
                
                var query = registryManager.CreateQuery("SELECT * FROM devices", 100);
                while (query.HasMoreResults)
                {
                    var page = await query.GetNextAsTwinAsync();
                    foreach (var twin in page)
                    {
                        string id = JsonConvert.SerializeObject(twin.DeviceId);
                        string properties = JsonConvert.SerializeObject(twin.Tags);
                        Console.WriteLine(properties);
                        string newId = id.Trim('"');
                        if(properties.Contains("in")){
                            Server.Program.devices.Add(new device(newId,"in",ids));
                        }
                        else if(properties.Contains("out")){
                            Server.Program.devices.Add(new device(newId,"out",ids));
                        }
                        else{
                            Server.Program.devices.Add(new device(newId,"both",ids));
                        }
                        ids++;
                            
                        Console.WriteLine(id);
                    }
                }
            }
            catch (DeviceAlreadyExistsException dvcEx)
            {
                Console.WriteLine("Error : {0}", dvcEx);
            }
        }
        public static string addDeviceEntrance(string newName,string doorType)
        {
            type = doorType;
            deviceconn = "Unable to add device";
            tempID = newName;
            addDeviceToHub().Wait();
            return deviceconn;
        }

        public static async Task addDeviceToHub()
        {
            string deviceID = tempID;
            Device device;
            RegistryManager RM =
              RegistryManager.CreateFromConnectionString(connectionString);
            try
            {
                device = await RM.AddDeviceAsync(new Device(deviceID));
                Server.Program.devices.Add(new device(deviceID,type,ids));
                ids++;
                var dev = RM.GetDeviceAsync(deviceID).Result;
                deviceconn ="HostName=" + Server.Program.config["Hub Name"] + ".azure-devices.net;DeviceId=" + deviceID +";SharedAccessKey=" + dev.Authentication.SymmetricKey.PrimaryKey;
                
                    try
                    {
                        if(type == "in"){
                            var patch =
                        @"{
                        tags: {
                                location: null,
                                door:in
                            }
                        }";
                        RM.UpdateTwinAsync(deviceID,patch,device.ETag).Wait();
                    
                        }
                        else if(type == "out"){
                            var patch =
                            @"{
                            tags: {
                                    location: null,
                                    door: out
                                }
                            }";
                            RM.UpdateTwinAsync(deviceID,patch,device.ETag).Wait();
                        }
                        else{
                            var patch =
                            @"{
                            tags: {
                                    location: null,
                                    door: both
                                }
                            }";
                            RM.UpdateTwinAsync(deviceID,patch,device.ETag).Wait();
                        }
                    }
                    catch(Newtonsoft.Json.JsonReaderException syserr)
                    {
                        Console.WriteLine(syserr);
                    }
                    
            
                List<device> idList = new List<device>();
                idList.Add(Server.Program.devices[Server.Program.devices.Count-1]);
                InvokeDeviceMethod.Program.deviceMethod(type,idList).Wait();
            }
            catch (DeviceAlreadyExistsException dvcEx)
            {
                Console.WriteLine("Error : {0}", dvcEx);
            }
        }

        public static void removeDevice(int doorId)
        {
            int temp = -1;
            for(int i = 0; i<Server.Program.devices.Count;i++)
            {
                if(Server.Program.devices[i].id == doorId)
                {
                    temp = i;
                    goto foundDoor;
                }
            }
        foundDoor:
            RegistryManager RM = RegistryManager.CreateFromConnectionString(connectionString);
            RM.RemoveDeviceAsync(Server.Program.devices[temp].name);
            Server.Program.devices.RemoveAt(temp);
        }
    }
}