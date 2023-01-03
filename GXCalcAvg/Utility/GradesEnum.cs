using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortlandPublishedCalculator.Utility
{
    public class GradesEnum
    {   
        // The following enum includes each of the grades that this progam will cycle through
        // When a new grade is added to the program, it must be added here
        public enum Grade
        {
            diesel,
            fame_10,
            fame0,
            petrol,
            ethanol,
            jet,
            propane,
            hvo_frb,
            hvo_cif_nwe,
            diesel_frb,
            gasoil,
            fueloil,
            mfo
        }
        public static Grade[] GetAllGradesAsArray()
        {   
            return new Grade[]
            {
                Grade.diesel,
                Grade.fame_10,
                Grade.fame0,
                Grade.petrol,
                Grade.ethanol,
                Grade.jet,
                Grade.propane,
                Grade.hvo_frb,
                Grade.hvo_cif_nwe,
                Grade.diesel_frb,
                Grade.gasoil,
                Grade.fueloil,
                Grade.mfo

            };
        }
        // Switch statement to get the string attached to the grade's name
        public static string GetGradeName(Grade grade) => (grade) switch
        {
            Grade.diesel => "Diesel CIF NWE",
            Grade.fame_10 => "FAME-10",
            Grade.fame0 => "FAME0",
            Grade.petrol => "Unleaded CIF NWE",
            Grade.ethanol => "Ethanol EUR CBM",
            Grade.jet => "Jet CIF NWE",
            Grade.propane => "Propane CIF NWE",
            Grade.hvo_frb => "HVO FRB",
            Grade.hvo_cif_nwe => "HVO CIF NWE",
            Grade.diesel_frb => "Diesel FRB",
            Grade.gasoil => "GasOil 0.1% CIF NWE",
            Grade.fueloil => "FuelOil 3.5% FRB",
            Grade.mfo => "MFO 0.5% FRB",
            _ => ""
        };
    }
}
