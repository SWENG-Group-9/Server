using System.Collections.Generic;
using System.Net.Http;

namespace toFront
{
    public class updates 
    {
        private static readonly HttpClient client = new HttpClient();

        public static void updateCurrent()
        {
            var values = new Dictionary<string, string>
            {
                { "current", Server.Program.current.ToString() }
            };

            var content = new FormUrlEncodedContent(values);

            client.PostAsync(Server.Program.frontendPort, content).Wait();
        }
    }
}