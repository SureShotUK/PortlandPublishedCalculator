using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortlandPublishedCalculator.DatabaseQueries;
using PortlandPublishedCalculator.Dates;



namespace PortlandPublishedCalculator.Calculations
{
    public class Calculations
    {
        // Calculates the Portland Diesel CIF NWE price for a given date. 
        public static double? Portland_Diesel_CIF_NWE(DateOnly date)
        {
            // Retrieves the most recent working day prior to the date specified that has a published Portland Diesel Price and an LSG value. 
            DateOnly previous_date = Retrieve.MostRecentDieselAndLSG(date);

            double? lsg = Retrieve.LSG(date);
            double? lsg_previous;
            // Is date the rollover date for LSG contracts? If so, lsg_previous is M002 of previous_date, not M001
            if (Date.IsDateIceLSGRolloverDay(date) == true) { lsg_previous = Retrieve.LSGBeforeRollover(previous_date); }
            else { lsg_previous = Retrieve.LSG(previous_date); };

            // Data retrieval from database
            double? gxprice = Retrieve.GX_Price(date);
            double? ineos = Retrieve.Ineos_DollarPerMT(date);
            double? ukf = Retrieve.UKF_DollarPerMT(date);
            double? prax = Retrieve.Prax_DollarPerMT(date);

            // Rule: if there is no LSG for the given date, return the GXPrice as the Portland Diesel price - if there is no GXPrice, return null.
            if (lsg == null || lsg == 0)
            {
                if (gxprice.HasValue && gxprice != 0) { return gxprice; }
                else { return null; } // This is the only possible scenario that the Portland Diesel Price is not able to be calculated 
            }

            // If there is a GXPrice we then need either a UKFuel price or an Ineos price or both to calculate a CalcAvg.
            if ((gxprice.HasValue && gxprice != 0) && (ukf.HasValue  && ukf != 0 || ineos.HasValue && ineos != 0 || prax.HasValue && prax != 0))
            {
                // Calculate CalcAvg here.
                double? calcavg = CalcAvg(lsg, lsg_previous, ineos, ukf, prax);
                double price = Math.Round(ExcelAverage(Convert.ToDouble(gxprice), Convert.ToDouble(calcavg)) * 4, 0, MidpointRounding.AwayFromZero) / 4;

                return price;
            }
            // If there is no GXPrice BUT there is atleast one supplier price that isn't 0 or null:
            else if ((gxprice == 0 || gxprice == null) && (ukf.HasValue && ukf != 0 || ineos.HasValue && ineos != 0 || prax.HasValue && prax != 0))
            {
                // Portland Diesel Price would be the CalcAvg of supplier prices plus the LSG difference
                double? calcavg = CalcAvg(lsg, lsg_previous, ineos, ukf, prax);
                double? price = calcavg + (lsg - lsg_previous);
                return price;
            }
            // If there is a GXPrice BUT there is none of the three supplier prices: 
            else if (gxprice.HasValue && gxprice != 0 && (ineos == 0 || ineos == null) && (ukf == 0 || ukf == null) && (prax == 0 || prax == null))
            {
                double? previous_portlandDieselPrice = Retrieve.Portland_Diesel_Price(previous_date);
                double average = Math.Round(ExcelAverage(Convert.ToDouble(gxprice), Convert.ToDouble(previous_portlandDieselPrice)) * 4, 0, MidpointRounding.AwayFromZero) / 4;
                double? price = average + (lsg - lsg_previous);
                return price;
            }
            // If there is not a GXPrice and no supplier prices, then the Portland Diesel Price is the previous published Portland Price + the LSG of the given date and the date of the previous published Portland Price.
            else 
            {
                double? previous_portlandDieselPrice = Retrieve.Portland_Diesel_Price(previous_date);
                double? price = previous_portlandDieselPrice + (lsg - lsg_previous);
                return price;
            } 

        }
        public static double? CalcAvg(double? lsg, double? lsg_previous, double? ineos, double? ukf, double? prax)
        {
            // Performing the calculations on the retrieved values
            double ineos_calc;
            if (ineos != null && ineos != 0)
            {
                ineos_calc = Supplier_Calc(lsg, lsg_previous, ineos);
            } else ineos_calc = 0;

            double ukf_calc;
            if (ukf != null && ukf != 0)
            {
                ukf_calc = Supplier_Calc(lsg, lsg_previous, ukf);
            } else ukf_calc = 0;

            double prax_calc;
            if (prax != null && prax != 0)
            {
                prax_calc = Supplier_Calc(lsg, lsg_previous, prax);
            }
            else prax_calc = 0;

            // If we only have one of the supplier prices, then only return a CalcAvg that is calculated using that supplier price.
            if (ineos_calc != 0 && ukf_calc == 0 && prax_calc == 0)
            {
                return ineos_calc;
            }
            else if (ineos_calc == 0 && ukf_calc != 0 && prax_calc == 0)
            {
                return ukf_calc;
            }
            else if (ineos_calc == 0 && ukf_calc == 0 && prax_calc != 0)
            {
                return prax_calc;
            }
            // If we have two supplier quotes out of 3: 
            else if (ineos_calc != 0 && ukf_calc != 0 && prax_calc == 0)
            {
                double ineos_ukf_average = ExcelAverage(ineos_calc, ukf_calc);
                double calcavg = Math.Round(ineos_ukf_average * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return calcavg;
            }
            else if (ineos_calc != 0 && ukf_calc == 0 && prax_calc != 0)
            {
                double ineos_prax_average = ExcelAverage(ineos_calc, prax_calc);
                double calcavg = Math.Round(ineos_prax_average * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return calcavg;
            }
            else if (ineos_calc == 0 && ukf_calc != 0 && prax_calc != 0)
            {
                double ukf_prax_average = ExcelAverage(ukf_calc, prax_calc);
                double calcavg = Math.Round(ukf_prax_average * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return calcavg;
            }
            // And finally, if we have all three supplier prices and can generate a SupplierCalc for both :
            else 
            {
                double allSupplier_average = ExcelAverage(ineos_calc, ukf_calc, prax_calc);
                double calcavg = Math.Round(allSupplier_average * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return calcavg;
            }

        }
        private static double Supplier_Calc(double? lsg, double? lsg_previous, double? supplierDollarPerMT)
        {
            return Convert.ToDouble(supplierDollarPerMT + (lsg - lsg_previous));
        }
        private static double ExcelAverage(double? value1, double? value2)
        {
            return Convert.ToDouble(value1 + value2) / 2;
        }
        private static double ExcelAverage(double? value1, double? value2, double? value3)
        {
            return Convert.ToDouble(value1 + value2 + value3) / 3;
        }
        // Calculates the Portland FAME-10 price
        public static double? Portland_FAME_minus_ten(DateOnly date)
        {
            // First, retrieve the ArgusOMR FAME and PrimaFAME prices : 
            double? argusOMR_FAME_minus_ten = Retrieve.ArgusOMR_FAME_minus_ten(date);
            double? prima_FAME_minus_ten = Retrieve.Prima_FAME_minus_ten(date);

            // If they are both null, then return the previous day's Portland_FAME_minus_ten
            if ((argusOMR_FAME_minus_ten == null | argusOMR_FAME_minus_ten == 0) && (prima_FAME_minus_ten == null | prima_FAME_minus_ten == 0)) { return Retrieve.Previous_Portland_FAME_minus_10(date); }

            // If we have one of the values only, then return just that
            if ((argusOMR_FAME_minus_ten != 0 | argusOMR_FAME_minus_ten != null) && (prima_FAME_minus_ten == null | prima_FAME_minus_ten == 0)) { return argusOMR_FAME_minus_ten; }
            if ((argusOMR_FAME_minus_ten == null | argusOMR_FAME_minus_ten == 0) && (prima_FAME_minus_ten != 0 | prima_FAME_minus_ten != null)) { return prima_FAME_minus_ten; }

            // Else, if we have both values, return an average of them both.
            else
            {
                double? price = Math.Round(ExcelAverage(argusOMR_FAME_minus_ten, prima_FAME_minus_ten) * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return price;
            }
        }
        // Retrieves the GX Unleaded Petrol price to be used as the Portland Published Unleaded CIF NWE price
        public static double? Portland_Unleaded_CIF_NWE(DateOnly date)
        {
            double? price = Retrieve.GX_Unleaded_Petrol(date);
            return price;
        }
        // Retrieves the GX Jet price to be used as the Portland Published Jet CIF NWE price
        public static double? Portland_Jet_CIF_NWE(DateOnly date)
        {
            double? price = Retrieve.GX_Jet(date);
            return price;
        }
        // Retrieves the GX Propane price to be used as the Portland Published Propane CIF NWE price
        public static double? Portland_Propane_CIF_NWE(DateOnly date)
        {
            double? price = Retrieve.GX_Propane(date);
            return price;
        }
        // Calculates the Portland Published Ethanol EUR CBM price by averaging the Prima T2 Physical and ArgusOMR ethanol prices
        public static double? Portland_Ethanol_EUR_CBM(DateOnly date)
        {
            double? prima_t2_physical_ethanol = Retrieve.Prima_T2_Physical_Ethanol(date);
            double? argusomr_ethanol = Retrieve.ArgusOMR_Ethanol(date);
            double portland_ethanol;

            // if we have both prices, create an average between the two
            // if they are both NOT null and both GREATER THAN 0 
            if ((prima_t2_physical_ethanol != null && prima_t2_physical_ethanol > 0) && (argusomr_ethanol != null && argusomr_ethanol > 0))
            {
                portland_ethanol = ExcelAverage(prima_t2_physical_ethanol, argusomr_ethanol);
                // Return the portland_ethanol rounded to the nearest quarter of a dollar:
                return Math.Round(portland_ethanol * 4, 0, MidpointRounding.AwayFromZero) / 4; ;
            }
            else
            {
                // if we have one but not the other, then return the one that is not null
                if ((prima_t2_physical_ethanol != null && prima_t2_physical_ethanol > 0) && (argusomr_ethanol == null || argusomr_ethanol == 0))
                { 
                    return Math.Round(Convert.ToDouble(prima_t2_physical_ethanol) * 4, 0, MidpointRounding.AwayFromZero) / 4; 
                }
                if ((prima_t2_physical_ethanol == null || prima_t2_physical_ethanol == 0) && (argusomr_ethanol != null && argusomr_ethanol > 0)) 
                { 
                    return Math.Round(Convert.ToDouble(argusomr_ethanol) * 4, 0, MidpointRounding.AwayFromZero) /4; 
                }
                // Else, if both are null for a given date, return null:
                else { return null; }
            }
        }
    }
}
