using PortlandPublishedCalculator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PortlandPublishedCalculator.Utility.GradesEnum;

namespace PortlandPublishedCalculator.Calculations
{
    public static class Calculated
    {
        // A switch statement used to retrieve the Portland Published price for the grades
        public static double? Price(Grade grade, DateOnly date) => (grade) switch
        {
            Grade.diesel => Calculations.Portland_Diesel_CIF_NWE(date, true),
            Grade.fame_10 => Calculations.Portland_FAME_minus_ten(date),
            Grade.fame0 => Calculations.Portland_FAME_Zero(date),
            Grade.petrol => Calculations.Portland_Unleaded_CIF_NWE(date),
            Grade.ethanol => Calculations.Portland_Ethanol_EUR_CBM(date),
            Grade.jet => Calculations.Portland_Jet_CIF_NWE(date),
            Grade.propane => Calculations.Portland_Propane_CIF_NWE(date),
            Grade.hvo_frb => Calculations.TEMPORARY_Portland_HVO_FRB(date), // Set to use TEMPORARY calculation. Revert back when Prima is fixed. 
            Grade.hvo_cif_nwe => Calculations.Portland_HVO_CIF_NWE(date),
            Grade.diesel_frb => Calculations.Portland_Diesel_FRB(date),
            Grade.gasoil => Calculations.Portland_Gasoil1percent_CIF_NWE(date),
            Grade.fueloil => Calculations.Portland_FuelOil3point5_FRB(date),
            Grade.mfo => Calculations.Portland_MFO0point5_FRB(date),
            Grade.nyh_diesel => Calculations.Portland_NYH_Diesel(date),
            _ => null,
        };
    }
}
