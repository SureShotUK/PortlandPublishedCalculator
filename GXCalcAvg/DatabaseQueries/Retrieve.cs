using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicHoliday;
using PortlandPublishedCalculator.Dates;
using PortlandPublishedCalculator.Prices;

namespace PortlandPublishedCalculator.DatabaseQueries
{
    // This class retrieves all the needed data from the database
    public static class Retrieve
    {
        // Create database connection
        readonly static PricesContext db = new();
        readonly static UKBankHoliday bh = new();
        // Retrieves the LSG price for a given date
        public static double? LSG(DateOnly date)
        {   
            try
            {
                double? lsg = db.ZIcefutLsgs.Where(x => x.PublishedDate == date && x.RelativeMonth == "M001").Select(x => x.Price).First();
                return lsg;

            }
            catch
            {
                return null;
            }
        }
        // Retrieves the LSG value for a given date if it is the LSG rollover date
        public static double? LSGBeforeRollover(DateOnly date)
        {
            try
            {
                double? lsg = db.ZIcefutLsgs.Where(x => x.PublishedDate == date && x.RelativeMonth == "M002").Select(x => x.Price).First();
                return lsg;

            }
            catch
            {
                return null;
            }
        }
        // This function finds the most recent date in the Portland Prices database that has BOTH a published Portland Diesel Price and LSG for the date
        public static DateOnly MostRecentDieselAndLSG(DateOnly date)
        {

            // Finds the most recent previously published Portland Diesel Price from the database that is a working day.
            DateOnly mostRecentDieselAndLSG = MostRecentPublishedDiesel(date);

            // Checks if a value exists in the z_icefut_lsg table with the above found date, and if not, loops back a working day until a date is found with an LSG and Published Portland Disel Price
            while (!db.ZIcefutLsgs.Any(x => x.PublishedDate == mostRecentDieselAndLSG && x.RelativeMonth == "M001"))
            {
                mostRecentDieselAndLSG = MostRecentPublishedDiesel(date.AddDays(-1));
            }


            return mostRecentDieselAndLSG;
        }

        // Finds the most recent previously published Portland Diesel Price from the database
        public static DateOnly MostRecentPublishedDiesel(DateOnly date)
        {
            DateOnly mostRecentPublishedDiesel = db.YPublishedWholesales.OrderBy(x => x.PublishedDate).
                Where(x => x.PublishedDate < date && x.DieselCifNwe.HasValue).
                Select(x => x.PublishedDate).LastOrDefault();

            // This will make sure that the date being returned is the most recent pubished Portland Diesel Price that is NOT a bank holiday/weekend. 
            while (!bh.IsWorkingDay(mostRecentPublishedDiesel.ToDateTime(TimeOnly.Parse("00:00:00"))))
            {
                mostRecentPublishedDiesel = db.YPublishedWholesales.OrderBy(x => x.PublishedDate).
                Where(x => x.PublishedDate < mostRecentPublishedDiesel && x.DieselCifNwe.HasValue).
                Select(x => x.PublishedDate).LastOrDefault();
            }
            return mostRecentPublishedDiesel;
        }
        // Retrieves the diesel GX_Price used in the Portland Diesel CIF NWE calculation
        public static double? GX_Price(DateOnly date)
        {   
            try
            {
                double? gxprice = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0000093).First();
                return gxprice;
            }
            catch
            {
                return null;
            }

        }
        public static double? Prax_DollarPerMT(DateOnly date)
        {
            try
            {
                double? prax = db.YSupplierPrices.Where(x => x.PublishedDate == date && x.SupplierId == 3 && x.GradeId == 1).Select(x => x.Price).First();
                return prax;
            }
            catch
            {
                return null;
            }
        }
        public static double? Ineos_DollarPerMT(DateOnly date)
        {   
            try
            {
                double? ineos = db.YSupplierPrices.Where(x => x.PublishedDate == date && x.SupplierId == 1 && x.GradeId == 1).Select(x => x.Price).First();
                return ineos;
            }
            catch
            {
                return null;
            }

        }

        public static double? UKF_DollarPerMT(DateOnly date)
        {   
            try
            {
                double? ukf = db.YSupplierPrices.Where(x => x.PublishedDate == date && x.SupplierId == 2 && x.GradeId == 1).Select(x => x.Price).First();
                return ukf;
            }
            catch
            {
                return null;
            }

        }
        // Retrieves the Portland Diesel CIF NWE price from the database for a given date. Used when we need to retrieve and use the previously published Portland Diesel Price.
        public static double? Portland_Diesel_Price(DateOnly date)
        {
            try
            {
                double? diesel = db.YPublishedWholesales.Where(x => x.PublishedDate == date).Select(x => x.DieselCifNwe).First();
                return diesel;
            }
            catch
            {
                return null;
            }
        }
        public static double? Previous_Portland_FAME_minus_10(DateOnly date)
        {
            try
            {
                DateOnly previousWorkingDate = Date.PreviousWorkingDay(date);
                double? previous_Portland_FAME_minus_10 = db.YPublishedWholesales.Where(x => x.PublishedDate == previousWorkingDate).Select(x => x.Fame10).First();
                return previous_Portland_FAME_minus_10;
            }
            catch
            {
                return null;
            }
        }
        public static double? Prima_FAME_minus_ten(DateOnly date)
        {
            try
            {   // Need to find a FAME-10 price from prima_forward_prices where the published date's month is the same month as delivery 
                // if the published date is the last day of the month, then the delivery month moves to the next month. 

                double? prima_FAME_minus_ten;
                string deliveryDate;

                // if it is NOT the last day of the month:
                if (!Date.IsDateLastDayOfMonth(date))
                {
                    deliveryDate = Date.DeliveryDateFormat(date);
                    prima_FAME_minus_ten = db.PrimaForwardPrices.Where(x => x.PublishedDate == date && x.Grade == "RED FAME-10" && x.Delivery == deliveryDate)
                        .Select(x => x.Price).First();

                    return prima_FAME_minus_ten;
                }
                // if it IS the last day of the month:
                else
                {
                    deliveryDate = Date.DeliveryDateFormat(date.AddMonths(1));
                    prima_FAME_minus_ten = db.PrimaForwardPrices.Where(x => x.PublishedDate == date && x.Grade == "RED FAME-10" && x.Delivery == deliveryDate)
                        .Select(x => x.Price).First();
                    return prima_FAME_minus_ten;
                }
            }
            catch
            {
                return null;
            }
        }
        public static double? ArgusOMR_FAME_minus_ten(DateOnly date)
        {
            try
            {
                double? argusOMR_FAME_minus_ten = db.YArgusomrBiofuelsNewers.Where(x => x.PublishedDate == date).Select(x => x.Fame10).First();
                return argusOMR_FAME_minus_ten;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the GX Unleaded Petrol price for the Portland Published Unleaded CIF NWE price
        public static double? GX_Unleaded_Petrol(DateOnly date)
        {
            try
            {
                double? petrol = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0000006).First();
                return petrol;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the GX Jet price for the Portland Published Jet CIF NWE price
        public static double? GX_Jet(DateOnly date)
        {
            try
            {
                double? jet = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0000015).First();
                return jet;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the GX Propane price for the Portland Published Propane CIF NWE price
        public static double? GX_Propane(DateOnly date)
        {
            try
            {
                double? propane = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0000686).First();
                return propane;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the Argus OMR Ethanol price for the Portland Published Ethanol EUR CBM price
        public static double? ArgusOMR_Ethanol(DateOnly date)
        {
            try
            {
                double? ethanol = db.YArgusomrBiofuelsNewers.Where(x => x.PublishedDate == date).Select(x => x.Ethanol).First();
                double? euroToUsd = GetEuroToUsdConversionRate(date);
                if (euroToUsd == null) { return null; }
                // Above price comes in USD and in tonnes - we need to convert it to Euros and apply the conversion factor to get it to cubic. 
                ethanol = (ethanol / euroToUsd) / 1.268; // 1.268 is the conversion factor for tonnes to cubic.
                return ethanol;
            }
            catch
            {
                return null;
            }
        }
        // Retrives the Euro to Usd conversion rate for a given day from the y_fteur table in the datebase. Used for ArgusOMG_Ethanol conversion
        private static double? GetEuroToUsdConversionRate(DateOnly date)
        {
            try
            {
                double? rate = db.YFteurs.Where(x => x.PublishedDate == date).Select(x => x.Usd).First();
                return rate;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the Prima T2 Physical Ethanol price for the Portland Published Ethanol EUR CBM price
        public static double? Prima_T2_Physical_Ethanol(DateOnly date)
        {
            try
            {   // Need to find a T2 Physical FOB ARA Ethanol price from prima_forward_prices where the published date's month is the same month as delivery 
                // if the published date is the last day of the month, then the delivery month moves to the next month. 

                double? prima_T2_physical_ethanol;
                string deliveryDate;

                // if it is NOT the last day of the month:
                if (!Date.IsDateLastDayOfMonth(date))
                {
                    deliveryDate = Date.DeliveryDateFormat(date);
                    prima_T2_physical_ethanol = db.PrimaForwardPrices.Where(x => x.PublishedDate == date && x.Grade == "T2 Physical FOB ARA Ethanol" && x.Delivery == deliveryDate)
                        .Select(x => x.Price).First();

                    return prima_T2_physical_ethanol;
                }
                // if it IS the last day of the month:
                else
                {
                    deliveryDate = Date.DeliveryDateFormat(date.AddMonths(1));
                    prima_T2_physical_ethanol = db.PrimaForwardPrices.Where(x => x.PublishedDate == date && x.Grade == "T2 Physical FOB ARA Ethanol" && x.Delivery == deliveryDate)
                        .Select(x => x.Price).First();
                    return prima_T2_physical_ethanol;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
