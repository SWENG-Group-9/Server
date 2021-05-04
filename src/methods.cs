using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using devices;
namespace InvokeDeviceMethod
{
    public class Program
    {
        private static ServiceClient s_serviceClient;
        
        // Connection string for your IoT Hub
        // az iot hub show-connection-string --hub-name {your iot hub name} --policy-name service
        public static string s_connectionString = Server.Program.config["Service"];

        public static async Task deviceMethod(string methodName,List<device> deviceID)
        {
            // This sample accepts the service connection string as a parameter, if present
            ValidateConnectionString();

            // Create a ServiceClient to communicate with service-facing endpoint on your hub.
            s_serviceClient = ServiceClient.CreateFromConnectionString(s_connectionString);

            await InvokeMethodAsync(methodName,deviceID);

            s_serviceClient.Dispose();
        }

        // Invoke the direct method on the device, passing the payload
        private static async Task InvokeMethodAsync(string method,List<device> iotDevice)
        {
            var methodInvocation = new CloudToDeviceMethod(method)
            {
                ResponseTimeout = TimeSpan.FromSeconds(30),
            };
            methodInvocation.SetPayloadJson("10");

            for(int i = 0;i < iotDevice.Count; i++){
                 // Invoke the direct method asynchronously and get the response from the simulated device.
                 try
                 {
                    var response = await s_serviceClient.InvokeDeviceMethodAsync(iotDevice[i].name, methodInvocation);
                    //Console.WriteLine($"\nResponse status: {response.Status}, payload:\n\t{response.GetPayloadAsJson()}");
                 }
                  catch(Exception e)
                 {
                     //Console.WriteLine(e);
                 }
                
            }
        }

        private static void ValidateConnectionString()
        {
            try
            {
                _ = IotHubConnectionStringBuilder.Create(s_connectionString);
            }
            catch (Exception)
            {
                Console.WriteLine("Not a valid Service string");
            }
        }
        public static void setDoor()
        { 
            if(!Server.Program.disabled)
            {
                List<device> doors = new List<device>();
                for(int i = 0; i < Server.Program.devices.Count;i++)
                {
                    if(Server.Program.devices[i].type == "in"||Server.Program.devices[i].type == "both")
                    {
                        if(Server.Program.devices[i].operation == false)
                        {
                            doors.Add(Server.Program.devices[i]);
                        }
                    }
                }
                if(Server.Program.current<Server.Program.max)
                {
                    deviceMethod("unlock",doors).Wait();
                    for(int i = 0; i < doors.Count;i++)
                    {
                        doors[i].status = false;
                    }
                }
                else
                {
                    deviceMethod("lock",doors).Wait();
                    for(int i = 0;i < doors.Count;i++ )
                    {
                        doors[i].status = true;
                    }
                } 
            }
            else
            {
                deviceMethod("unlock",Server.Program.devices).Wait();
                for (int i = 0;i<Server.Program.devices.Count;i++)
                {
                    Server.Program.devices[i].status = false;
                }
            }
        }

        public static void overrideDoor(int door)
        {
            int temp = -1;
            for(int i = 0; i<Server.Program.devices.Count;i++)
            {
                if(Server.Program.devices[i].id == door)
                {
                    temp = i;
                    goto gotDoor;
                }
            }
        gotDoor:
            try
            {
                if(Server.Program.devices[temp].operation)
                {
                    Server.Program.devices[temp].operation = false;
                    if(Server.Program.devices[temp].type == "out"||Server.Program.devices[temp].type == "both")
                    {
                        Server.Program.devices[temp].status = false;
                        List<device> methodList = new List<device>();methodList.Add(Server.Program.devices[temp]);
                        deviceMethod("unlock",methodList).Wait();
                    }
                }
                else
                {
                    Server.Program.devices[temp].operation = true;
                    if(Server.Program.devices[temp].status)
                    {
                        List<device> methodList = new List<device>();methodList.Add(Server.Program.devices[temp]);
                        deviceMethod("unlock",methodList).Wait();
                        Server.Program.devices[temp].status = false;
                    }
                    else
                    {
                        List<device> methodList = new List<device>();methodList.Add(Server.Program.devices[temp]);
                        deviceMethod("lock",methodList).Wait();
                        Server.Program.devices[temp].status = true;
                    }
                    

                }
            }
            catch(Exception)
            {
                
            }
        }
    }
}