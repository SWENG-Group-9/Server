using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace InvokeDeviceMethod
{
    public class Program
    {
        private static ServiceClient s_serviceClient;
        
        // Connection string for your IoT Hub
        // az iot hub show-connection-string --hub-name {your iot hub name} --policy-name service
        public static string s_connectionString = "HostName=sweng.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=NTkLL9oPk7Gu8RrfadVc7+DoO9HnBvDWv9x311tv7nM=";

        public static async Task deviceMethod(string methodName,System.Collections.Generic.List<string> deviceID)
        {
            // This sample accepts the service connection string as a parameter, if present
            ValidateConnectionString();

            // Create a ServiceClient to communicate with service-facing endpoint on your hub.
            s_serviceClient = ServiceClient.CreateFromConnectionString(s_connectionString);

            await InvokeMethodAsync(methodName,deviceID);

            s_serviceClient.Dispose();
        }

        // Invoke the direct method on the device, passing the payload
        private static async Task InvokeMethodAsync(string method,System.Collections.Generic.List<string> iotDevice)
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
                    var response = await s_serviceClient.InvokeDeviceMethodAsync(iotDevice[i], methodInvocation);
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
            
            if(!Server.Program.manual)
            {
                List<string> entrances = new List<string>();
                for(int i = 0; i< Server.Program.devices.Count;i++)
                {
                    if(Server.Program.types[i] == 0)
                    {
                        entrances.Add(Server.Program.devices[i]);
                    }
                }
                if(Server.Program.current<Server.Program.max)
                    {
                    Server.Program.locked = false;
                    InvokeDeviceMethod.Program.deviceMethod("unlock",entrances).Wait();
                }
                else
                {
                    Server.Program.locked = true;
                    InvokeDeviceMethod.Program.deviceMethod("lock",entrances).Wait();
                }
            }
        }

        public static void overrideDoor()
        {
            if(Server.Program.manual)
            {
                Server.Program.manual = !Server.Program.manual;
                setDoor();
            }
            else
            {
                if(Server.Program.locked)
                {
                    InvokeDeviceMethod.Program.deviceMethod("unlock",Server.Program.devices).Wait();
                }
                else
                {
                    InvokeDeviceMethod.Program.deviceMethod("lock",Server.Program.devices).Wait();
                }
                Server.Program.locked = !Server.Program.locked;
                Server.Program.manual = !Server.Program.manual;

            }

        }
    }
}