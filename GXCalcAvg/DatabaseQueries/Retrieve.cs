using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicHoliday;
using PortlandPublishedCalculator.Dates;
using PortlandPublishedCalculator.Prices;
using PortlandPublishedCalculator.Calculations;

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
        // Retrieves the Published Portland Diesel CIF NWE price (which should have been published at this point, and if it doesn't exist, then it generators a price.
        public static double? Diesel_CIF_NWE(DateOnly date)
        {
            try
            {
                double? diesel_cif_nwe = db.YPublishedWholesales.Where(x => x.PublishedDate == date)
                    .Select(x => x.DieselCifNwe).FirstOrDefault();
                return diesel_cif_nwe;
            }
            catch
            {
                try
                {
                    return Portland_Diesel_Price(date);
                }
                catch
                {
                    return null;
                }

            }
        }
        // Retrieves the diesel GX_Price used in the Portland Diesel CIF NWE calculation
        public static double? GX_Diesel(DateOnly date)
        {   
            try
            {
                double? gxprice = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0000257).First();
                return gxprice;
            }
            catch
            {
                return null;
            }

        }
        public static double? Diesel_Prax_SupQuote(DateOnly date)
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
        public static double? Diesel_Ineos_SupQuote(DateOnly date)
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
        public static double? Diesel_UKF_SupQuote(DateOnly date)
        {   
            try
            {
                double? diesel_ukf = db.YSupplierPrices.Where(x => x.PublishedDate == date && x.SupplierId == 2 && x.GradeId == 1).Select(x => x.Price).First();
                return diesel_ukf;
            }
            catch
            {
                return null;
            }

        }
        public static double? Unleaded_UKF_SupQuote(DateOnly date)
        {
            try
            {
                double? unleaded_ukf = db.YSupplierPrices.Where(x => x.PublishedDate == date && x.SupplierId == 2 && x.GradeId == 7).Select(x => x.Price).First();
                return unleaded_ukf;
            }
            catch { return null; }
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
        // Retrieves the previous Portland Published FAME-10 price from a given date 
        public static double? Previous_Portland_FAME_minus_10(DateOnly date)
        {
            try
            {
                DateOnly previousWorkingDate = Date.PreviousWorkingDay(date);
                double? previous_Portland_FAME_minus_10 = db.YPublishedWholesales.Where(x => x.PublishedDate == previousWorkingDate).Select(x => x.Fame10).First();
                return previous_Portland_FAME_minus_10;
            }
            catch { return null; }
        }
        // Retrieves the previous Portland Published FAME0 price from a given date
        public static double? Previous_Portland_FAME_Zero(DateOnly date)
        {
            try
            {
                DateOnly previousWorkingDate = Date.PreviousWorkingDay(date);
                double? previous_Portland_FAME_zero = db.YPublishedWholesales.Where(x => x.PublishedDate == previousWorkingDate).Select(x => x.Fame0).First();
                return previous_Portland_FAME_zero;
            }
            catch { return null; }
        }
        // Retrieves the previous Portland Published Ethanol EUR CBM price from a given date
        public static double? Previous_Portland_Ethanol_EUR_CBM(DateOnly date)
        {
            try
            {
                DateOnly previousWorkingDate = Date.PreviousWorkingDay(date);
                double? previous_portland_ethanol_eur_cbm = db.YPublishedWholesales.Where(x => x.PublishedDate == previousWorkingDate).Select(x => x.EthanolEurCbm).First();
                return previous_portland_ethanol_eur_cbm;
            }
            catch { return null; }
        }
        // Retrieves the previous Portland Published Unleaded CIF NWE price from a given date 
        public static double? Previous_Portland_Unleaded_CIF_NWE(DateOnly date)
        {
            try
            {
                DateOnly previousWorkingDate = Date.PreviousWorkingDay(date);
                double? previous_portland_unleaded = db.YPublishedWholesales.Where(x => x.PublishedDate == previousWorkingDate).Select(x => x.UnleadedCifNwe).First();
                return previous_portland_unleaded;
            }
            catch { return null; }
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
        // Retrieves the ArgusOMR Fame0 price for the Portland Published Fame0 price calc
        public static double? ArgusOMR_FAME_Zero(DateOnly date)
        {
            try
            {
                double? argusOMR_FAME_zero = db.YArgusomrBiofuelsNewers.Where(x => x.PublishedDate == date).Select(x => x.Fame0).First();
                return argusOMR_FAME_zero;
            }
            catch { return null; }
        }
        // Retrieves the Prima Forward Fame0 price for the Portland Published Fame0 price calc
        public static double? Prima_FAME_Zero(DateOnly date)
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
                    prima_FAME_minus_ten = db.PrimaForwardPrices.Where(x => x.PublishedDate == date && x.Grade == "RED FAME0" && x.Delivery == deliveryDate)
                        .Select(x => x.Price).First();

                    return prima_FAME_minus_ten;
                }
                // if it IS the last day of the month:
                else
                {
                    deliveryDate = Date.DeliveryDateFormat(date.AddMonths(1));
                    prima_FAME_minus_ten = db.PrimaForwardPrices.Where(x => x.PublishedDate == date && x.Grade == "RED FAME0" && x.Delivery == deliveryDate)
                        .Select(x => x.Price).First();
                    return prima_FAME_minus_ten;
                }
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
        // Returns the HVO Production Cost from y_hvo_production_cost.
        // Note: This is not a daily upload to the db, and instead the function will return the closest HVO production cost value to the given date.
        public static double? HVO_Production_Cost(DateOnly date)
        {   
            // Will order the hv_production_cost table by the published_date column, and then will find the first date that is less than or equal to the givendate. 
            double? hvo_production_cost = db.YHvoProductionCosts.OrderBy(x => x.PublishedDate)
                .Where(x => x.PublishedDate <= date).Select(x => x.ProductionCost).Last();
            return hvo_production_cost;

        }
        // Retrieves the most recent Prima T1 price that is the same date or less than the specified date.
        public static double? Prima_T1_UCO_CIF_ARA(DateOnly date)
        {
            // Will order the prima_spot_prices table by the published_date column, and then will find the first date that is less than or equal to the given date. 
            // Will find the most recent price - not the one for the given date. This ensures a price is always found.
            double? prima_t1_uco_cif_ara = db.PrimaSpotPrices.OrderBy(x => x.PublishedDate)
                .Where(x => x.PublishedDate <= date && x.Grade == "T1 UCO CIF ARA").Select(x => x.Price).Last();
            return prima_t1_uco_cif_ara;
        }
        // Retrieves the HVO Blend Percentage for the given date from the hvo_blend_percentages table in the database
        public static double? HVO_Blend_Percentages(DateOnly date)
        {
            double? hvo_blend_percentage = db.YHvoBlendPercentages.OrderBy(x => x.PublishedDate)
                .Where(x => x.PublishedDate <= date).Select(x => x.Hvo).Last();
            return hvo_blend_percentage;
        }
        // Retrieves the ArgusOMR THG Konventionelle low and high prices, and then creates an average of the two
        public static double? ArgusOMR_THG_Konventionelle(DateOnly date)
        {   
            // Retrieves the most recent date that has THG Konventionelle data - just incase there was no data for the given date. 
            DateOnly mostRecentTHKonventionelleDate = db.YArgusomrThgs.OrderBy(x => x.PublishedDate).Where(x => x.PublishedDate <= date).Select(x => x.PublishedDate).Last();

            double? argusomr_thg_konventionelle_low = db.YArgusomrThgs.Where(x => x.PublishedDate == mostRecentTHKonventionelleDate && x.Grade == "Konventionelle").Select(x => x.Low).First();
            double? argusomr_thg_konventionelle_high = db.YArgusomrThgs.Where(x => x.PublishedDate == mostRecentTHKonventionelleDate && x.Grade == "Konventionelle").Select(x => x.High).First();
            double? avg = (argusomr_thg_konventionelle_low + argusomr_thg_konventionelle_high) / 2;
            return avg;
        }
        // Retrieves the GBP - EUR exchange rate for a given day from the y_ftgbps table in the database
        public static double? FTGbp_To_Eur(DateOnly date)
        {
            try
            {
                double? ftgbp_to_eur = db.YFtgbps.Where(x => x.PublishedDate == date).Select(x =>x.Eur).First();
                return ftgbp_to_eur;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the GBP - USD exchange rate for a given day from the y_ftgbps table in the database
        public static double? FTGbp_To_Usd(DateOnly date)
        {
            try
            {
                double? ftgbp_to_usd = db.YFtgbps.Where(x => x.PublishedDate == date).Select(x =>x.Usd).First();
                return ftgbp_to_usd;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the GX Price for the 093 code from the y_gimid's table in the database
        public static double? GX_093(DateOnly date)
        {
            // Will order the YGimids table by the published_date column, and then will find the first date that is less than or equal to the given date. 
            // Will find the most recent price - not the one for the given date. This ensures a price is always found.
            double? gxprice = db.YGimids.OrderBy(x => x.PublishedDate)
                .Where(x => x.PublishedDate <= date).Select(x => x.Gx0000093).Last();
            return gxprice;
        }
        // Retrieves the GX Price for the 258 code from the y_gimid's table in the database
        public static double? GX_258(DateOnly date)
        {
            // Will order the YGimids table by the published_date column, and then will find the first date that is less than or equal to the given date. 
            // Will find the most recent price - not the one for the given date. This ensures a price is always found.
            double? gxprice = db.YGimids.OrderBy(x => x.PublishedDate)
                .Where(x => x.PublishedDate <= date).Select(x => x.Gx0000258).Last();
            return gxprice;
        }
        // Retrieves the GX Price for the 257 code from the y_gimid's table in the database
        public static double? GX_257(DateOnly date)
        {
            // Will order the YGimids table by the published_date column, and then will find the first date that is less than or equal to the given date. 
            // Will find the most recent price - not the one for the given date. This ensures a price is always found.
            double? gxprice = db.YGimids.OrderBy(x => x.PublishedDate)
                .Where(x => x.PublishedDate <= date).Select(x => x.Gx0000257).Last();
            return gxprice;
        }
        // Retrieves the GX Price for the 082 code from the y_gimid's table in the database
        public static double? GX_082(DateOnly date)
        {
            try
            {
                double? gxprice = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0000082).First();
                return gxprice;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the GX Price for the 266 code from the y_gimid's table in the database
        public static double? GX_266(DateOnly date)
        {
            try
            {
                double? gxprice = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0000266).First();
                return gxprice;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the GX Price for the 087 code from the y_gimid's table in the database
        public static double? GX_087(DateOnly date)
        {
            try
            {
                double? gxprice = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0000087).First();
                return gxprice;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the GX Price for the 1032 code from the y_gimid's table in the database
        public static double? GX_1032(DateOnly date)
        {
            try
            {
                double? gxprice = db.YGimids.Where(x => x.PublishedDate == date).Select(x => x.Gx0001032).First();
                return gxprice;
            }
            catch
            {
                return null;
            }
        }
        // Retrieves the ArgusOMR HVO Class II value from the y_argusomr_biofuels_newer table in the database
        public static double? ArgusOMR_HVO_Class_II(DateOnly date)
        {
            // Will order the y_argusomr_biofuelds_newers table by the published_date column, and then will find the first date that is less than or equal to the given date. 
            // Will find the most recent price - not the one for the given date. This ensures a price is always found.
            double? argusomr_hvo_class_ii = db.YArgusomrBiofuelsNewers.OrderBy(x => x.PublishedDate).
                Where(x => x.PublishedDate <= date).Select(x => x.HvoClassIi).Last();
            return argusomr_hvo_class_ii;
        }
        // Retrieves the most recent Prima HVO Plant price from the prima_spot_price table in the database
        public static double? Prima_HVO_Plant(DateOnly date)
        {
            // Will order the prima_spot_prices table by the published_date column, and then will find the first date that is less than or equal to the given date. 
            // Will find the most recent price - not the one for the given date. This ensures a price is always found.
            double? prima_hvo_plant = db.PrimaSpotPrices.OrderBy(x => x.PublishedDate)
            .Where(x => x.PublishedDate <= date && x.Grade == "HVO Plant (UCO Input)").Select(x => x.Price).Last();
            return prima_hvo_plant;
        }
        // Retrieves the Published Portland HVO FRB price for the given date
        public static double? Portland_HVO_FRB(DateOnly date)
        {
            try
            {
                double? portland_hvo_frb = db.YPublishedWholesales.Where(x => x.PublishedDate == date)
                    .Select(x => x.HvoFrb).First();
                return portland_hvo_frb;
            }
            catch
            {
                return null;
            }
        }
    }
}
