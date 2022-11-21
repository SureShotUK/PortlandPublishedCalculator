using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortlandPublishedCalculator.Utility
{
    public class DebugLogger
    {
        // This is a function to write strings to a local .txt file
        // This is used to debug the Portland Diesel CIF NWE calculation, as we can record what each of the variables that go into the calculation
        // equal when it is automated in the morning. 
        private readonly static StringBuilder DebugLogString = new();
        public static void WriteLine(string str)
        {
            DebugLogString.Append(str).Append(Environment.NewLine);
        }
        public static void Write(string str)
        {
            DebugLogString.Append(str);

        }
        public static void SaveLog()
        {   
            string cwd = Directory.GetCurrentDirectory();
            string path = cwd + "\\Output.txt";
            //string path = "C:\\Users\\MilesVellozzo\\Documents\\Dump For Console to Txt\\Output.txt";
            using StreamWriter sw = File.AppendText(path);
            sw.Write(DebugLogString.ToString());
            sw.Close();
            sw.Dispose();

        }
    }
}
