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
            propane,
            hvo_frb,
            hvo_cif_nwe
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
                Grade.propane,
                Grade.hvo_frb,
                Grade.hvo_cif_nwe

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
            Grade.hvo_frb => "HVO FRB",
            Grade.hvo_cif_nwe => "HVO CIF NWE",
            _ => ""
        };
    }
}
