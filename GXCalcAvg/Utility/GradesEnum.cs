using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortlandPublishedCalculator.Utility
{
    public class GradesEnum
    {
        public enum Grade
        {
            diesel,
            fame,
            petrol,
            ethanol,
            jet,
            propane
        }
        public static Grade[] GetAllGradesAsArray()
        {   
            return new Grade[]
            {
                Grade.diesel,
                Grade.fame,
                Grade.petrol,
                Grade.ethanol,
                Grade.jet,
                Grade.propane
            };
        }
        // Switch statement to get the string attached to the grade's name
        public static string GetGradeName(Grade grade) => (grade) switch
        {
            Grade.diesel => "Diesel CIF NWE",
            Grade.fame => "FAME-10",
            Grade.petrol => "Unleaded CIF NWE",
            Grade.ethanol => "Ethanol EUR CBM",
            Grade.jet => "Jet CIF NWE",
            Grade.propane => "Propane CIF NWE",
            _ => ""
        };
        //// Switch statement to get the string attached to the grade's name, but instead parses a string instead of the enum
        //public static string GetGradeNameString(string grade) => (grade) switch
        //{
        //    "diesel" => "Diesel CIF NWE",
        //    "fame" => "FAME-10",
        //    "petrol" => "Unleaded CIF NWE",
        //    "ethanol" => "Ethanol EUR CBM",
        //    "jet" => "Jet CIF NWE",
        //    "propane" => "Propane CIF NWE",
        //    _ => ""
        //};
    }
}
