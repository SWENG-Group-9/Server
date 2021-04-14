using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace register
{
    class manageDevices
    {
        static int type = 0;
        static string tempID;
        static string deviceconn;

        const string connectionString ="HostName=sweng.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=7EVv/KvGgCAToj4TeHKRoo5I812ttE6I2O/wvnOUaJ4=";     
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
                        string newId = id.Trim('"');
                        Server.Program.devices.Add(newId);
                        if(properties.Contains("'in'")){
                            Server.Program.types.Add(0);
                        }
                        else
                            Server.Program.types.Add(1);
                        Server.Program.keys.Add(Server.Program.keys.Count);
                        Console.WriteLine(id);
                    }
                }
            }
            catch (DeviceAlreadyExistsException dvcEx)
            {
                Console.WriteLine("Error : {0}", dvcEx);
            }
        }

        public static string addDeviceEntrance(string newID,int doorType)
        {
            type = doorType;
            deviceconn = "Unable to add device";
            tempID = newID;
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
                Server.Program.devices.Add(deviceID);
                var dev = RM.GetDeviceAsync(deviceID).Result;
                deviceconn ="HostName=sweng.azure-devices.net;DeviceId=" + deviceID +";SharedAccessKey=" + dev.Authentication.SymmetricKey.PrimaryKey;
                var twin = RM.GetTwinAsync(deviceID);
                if(type == 0){
                    var patch =
                    @"{
                       tags: {
                            location: null,
                            door: in
                        }
                    }";
                    RM.UpdateTwinAsync(deviceID,patch,device.ETag).Wait();
                    
                }
                else if(type == 1){
                    var patch =
                    @"{
                       tags: {
                            location: null,
                            door: out
                        }
                    }";
                    RM.UpdateTwinAsync(deviceID,patch,device.ETag).Wait();
                    List<string> idList = new List<string>();
                    idList.Add(deviceID);
                    InvokeDeviceMethod.Program.deviceMethod("out",idList).Wait();
                }
                
            }
            catch (DeviceAlreadyExistsException dvcEx)
            {
                Console.WriteLine("Error : {0}", dvcEx);
            }
        }
    }
}


