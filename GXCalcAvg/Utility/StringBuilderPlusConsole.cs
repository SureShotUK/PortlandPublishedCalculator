using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortlandPublishedCalculator.Utility
{
    public class StringBuilderPlusConsole
    {
        private readonly static StringBuilder LogString = new();
        // Writes the inputted string to both a new console line and adds a new line to the StringBuilder string for the emails.
        public static void WriteLine(string str)
        {
            Console.WriteLine(str);
            LogString.Append(str + "<br>");
        }
        // Only appends a new string line to the StringBuilder string - will not be printed to console.
        public static void WriteLineSBOnly(string str)
        {
            LogString.Append(str + "<br>");
        }
        public static void GradeAppend(string gradename, double? price)
        {
            LogString.Append("<b>" + gradename + ": " + "</b>" + price + ",<br>");
            Console.WriteLine(gradename + ": " + price);
        }
        public static StringBuilder GetLogString()
        {
            return LogString;
        }
    }
}
