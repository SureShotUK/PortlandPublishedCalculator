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
            LogString.Append("<p>" + str + "</p>");
        }
        // Only appends a new string line to the StringBuilder string - will not be printed to console.
        public static void WriteLineSBOnly(string str)
        {
            LogString.Append(str);
        }
        public static void GradeAppend(string gradename, double? price)
        {
            LogString.Append("<p>" + "<b>" + gradename + ": " + "</b>" + price + "</p>");
            Console.WriteLine(gradename + ": " + price);
        }
        public static StringBuilder GetLogString()
        {
            return LogString;
        }
    }
}
