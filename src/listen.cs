using System;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs.Consumer;
using System.Text;
using System.Threading;
using System.Timers;
using Server;

namespace devicemessages
{
    public class Event
    { 
        private static System.Timers.Timer aTimer;
        private static string e_endpointName = Server.Program.config["Event"];
        public static string message = "";
        private static CancellationTokenSource __tokenSource = new CancellationTokenSource();
        private static CancellationToken ct = __tokenSource.Token;

        public static DateTimeOffset startTime = DateTime.Now;
        public static async Task ReceiveMessagesFromDeviceAsync()
        { 
            await using var consumer = new EventHubConsumerClient(
                EventHubConsumerClient.DefaultConsumerGroupName,
                e_endpointName);

            //Console.WriteLine("Listening for messages on all partitions.");

            try
            {
                aTimer = new System.Timers.Timer(500);
                aTimer.Elapsed += OnTimedEvent;
                await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync(ct))
                {
                    if(partitionEvent.Data.EnqueuedTime.CompareTo(startTime) <= 0) continue;
                    Console.WriteLine($"\nMessage received on partition {partitionEvent.Partition.PartitionId}:");
                    message = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                    Console.WriteLine(message);
                    if(message == "i"){
                        Program.current++;
                        Console.Write(Program.current);
                        InvokeDeviceMethod.Program.setDoor();
                        toFront.updates.updateCurrent();
                    }
                    else if(message == "d"){
                       if(Program.current > 0)
                        {
                            Program.current--;
                            InvokeDeviceMethod.Program.setDoor();
                            toFront.updates.updateCurrent();
                        }
                        Console.Write(Program.current);
                    }
                    
                }
            }
            catch (TaskCanceledException)
            {
                
            }
            finally
            {
                __tokenSource.Dispose();
            }
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            __tokenSource.Cancel();
        }
    }
}