using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stats
{
    public class statcollection
    {
        static private bool check = false;

        public static void statsGenerator()
        {
            DateTime dateTime = DateTime.Now;
            if(dateTime.Minute==0 && check == false || dateTime.Minute == 30 && check == true)
            {
            string fullFilePath = Environment.GetEnvironmentVariable("HOME") != null
                ? Environment.GetEnvironmentVariable("HOME").ToString() + "/site/data.json"
                : @"src/data.json";                                           
            string json =  File.ReadAllText(fullFilePath);
            String date = dateTime.ToString("u").Substring(0,10);   
            string time = dateTime.ToString("s").Substring(11,8);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj[date][(dateTime.Hour/2) + (dateTime.Minute/30)]["time"] = time;
            jsonObj[date][(dateTime.Hour/2) + (dateTime.Minute/30)]["value"] = Server.Program.current;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fullFilePath, output);
            check = !check;
            }       
        }
    }  
}