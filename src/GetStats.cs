using System;
using System.IO;

namespace Stats
{
    public class statcollection
    {

        public static void statsToJson()
        {
            DateTime date = DateTime.Now;                                                                                       //gets current date and time in format DD/MM/YYYY HH:MM:SS AM/PM
            File.WriteAllText(@"C:\Device\server\data.json", "{\"" + date.ToString("yyyy-MM-dd") + "\":[");                     //writes current date to .json file in format YYYY-MM-DD, with some syntax
            while (Equals(date.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")))                                    //while still current day
            {
                String timeAndCurrent = null;                                                                                   //initialise output string
                if (Equals(date.ToString("t"), "23:30")){                                                                       //slight change in syntax for final output
                    timeAndCurrent = "{\"time\":\"" + date.ToString("t") + "\",\n\"value\":" + Server.Program.current + "},]\n";                       
                }
                if (Equals((date.ToString("t")[3] + date.ToString("t")[4]), "00") ||                                            //if time ends in 00 or 30 ie outputs time and value for current every half an hour
                (Equals((date.ToString("t")[3] + date.ToString("t")[4]), "30")))
                {
                    timeAndCurrent = "{\"time\":\"" + date.ToString("t") + "\",\n\"value\":" + Server.Program.current + "},\n";                //creates string with time in format HH:MM and value for current
                }
                File.WriteAllText(@"C:\Device\server\data.json", timeAndCurrent);                                               //writes output file to .json
            }
        }
    }  
}