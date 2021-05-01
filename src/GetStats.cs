using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stats
{
    public class statcollection
    {
        static private bool check = true;

        public static void statsGenerator()
        {
            DateTime dateTime = DateTime.Now;
            if(dateTime.Minute==0 && check == false || dateTime.Minute == 30 && check == true)
            {
            string fullFilePath = Environment.GetEnvironmentVariable("HOME") != null
                ? Environment.GetEnvironmentVariable("HOME").ToString() + "/data.json"
                : @"src/data.json";  
            string json =  File.ReadAllText(fullFilePath);
            String date = dateTime.ToString("u").Substring(0,10);   
            string time = dateTime.ToString("s").Substring(11,5);
            if(json.Contains(date))
            {
                json = json.Substring(0,json.Length-2);
                json += ",{\"time\":\"" + time + "\",\"value\":" + Server.Program.current + "}]}";
            }
            else
            {
                json = json.Substring(0,json.Length-1);
                json += ",\"" + date +"\":[{\"time\":\"" + time + "\",\"value\":" + Server.Program.current + "}]}";
            }
            File.WriteAllText(fullFilePath, json);
            check = !check;
            }       
        }
    }  
}