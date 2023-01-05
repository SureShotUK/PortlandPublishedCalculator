using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PortlandPublishedCalculator.DatabaseQueries;
using PortlandPublishedCalculator.Dates;
using PortlandPublishedCalculator.Utility;



namespace PortlandPublishedCalculator.Calculations
{
    public class Calculations
    {
        // Calculates the Portland Diesel CIF NWE price for a given date. 
        public static double? Portland_Diesel_CIF_NWE(DateOnly date, bool sendDebugEmail)
        {
            // Retrieves the most recent working day prior to the date specified that has a published Portland Diesel Price and an LSG value. 
            DateOnly previous_date = Retrieve.MostRecentDieselAndLSG(date);

            double? lsg = Retrieve.LSG(date);
            double? lsg_previous;
            // Is date the rollover date for LSG contracts? If so, lsg_previous is M002 of previous_date, not M001
            if (Date.IsDateIceLSGRolloverDay(date) == true) { lsg_previous = Retrieve.LSGBeforeRollover(previous_date); }
            else { lsg_previous = Retrieve.LSG(previous_date); };

            // Data retrieval from database
            double? gxprice = Retrieve.GX_Diesel(date);
            // Retrieve the supplier quote prices from the PREVIOUS working date
            double? ineos = Retrieve.Diesel_Ineos_SupQuote(previous_date);
            double? ukf = Retrieve.Diesel_UKF_SupQuote(previous_date);
            double? prax = Retrieve.Diesel_Prax_SupQuote(previous_date);
            
            // If enabled, will send the debug email for the Portland Diesel CIF NWE calculation. 
            if (sendDebugEmail)
            {
                Email.SendDieselDebugEmail(@$"<html> 
                <body>
                <p>The following prices were used in the Portland Diesel CIF NWE calculation:</p>
                <hr>
                <p><strong>LSG: </strong>{lsg}</p>
                <p><strong>LSG for Previous Day: </strong>{lsg_previous}</p>
                <p><strong>GXPrice: </strong>{gxprice}</p>
                <p><strong>Ineos: </strong>{ineos}</p>
                <p><strong>UKF: </strong>{ukf}</p>
                <p><strong>Prax: </strong>{prax}</p>
                </body>
                </html>");
            } 

            // Rule: if there is no LSG for the given date, return the GXPrice as the Portland Diesel price - if there is no GXPrice, return null.
            if (IsValueNullOr0(lsg))
            {
                if (IsValueNullOr0(gxprice)) { return gxprice; }
                else { return null; } // This is the only possible scenario that the Portland Diesel Price is not able to be calculated 
            }

            // If there is a GXPrice we then need either a UKFuel price or an Ineos price or both to calculate a CalcAvg.
            if (!IsValueNullOr0(gxprice) && (!IsValueNullOr0(ineos) || !IsValueNullOr0(ukf) || !IsValueNullOr0(prax)))
            {
                // Calculate CalcAvg here.
                double? calcavg = SupplierCalcAvg(ineos, ukf, prax);
                calcavg += (lsg - lsg_previous);
                // Average our calcavg with the lsg difference against the gxprice
                double price = Math.Round(ExcelAverage(Convert.ToDouble(gxprice), Convert.ToDouble(calcavg)) * 4, 0, MidpointRounding.AwayFromZero) / 4;

                return price;
            }
            // If there is no GXPrice BUT there is atleast one supplier price that isn't 0 or null:
            else if (IsValueNullOr0(gxprice) && ( !IsValueNullOr0(ineos) || !IsValueNullOr0(ukf) || !IsValueNullOr0(prax) ) )
            {
                // Portland Diesel Price would be the CalcAvg of supplier prices plus the LSG difference
                double? calcavg = SupplierCalcAvg(ineos, ukf, prax);
                double? price = calcavg + (lsg - lsg_previous);
                return price;
            }
            // If there is a GXPrice BUT there is none of the three supplier prices: 
            else if (!IsValueNullOr0(gxprice) && IsValueNullOr0(ineos) && IsValueNullOr0(ukf) && IsValueNullOr0(prax))
            {
                double? previous_portlandDieselPrice = Retrieve.Portland_Diesel_Price(previous_date);
                previous_portlandDieselPrice += (lsg - lsg_previous);
                double? price = Math.Round(
                    ExcelAverage(previous_portlandDieselPrice, gxprice)
                    * 4, 0, MidpointRounding.AwayFromZero) / 4;
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
        // Averages all of the supplier quotes that we have for a given date
        public static double? SupplierCalcAvg(double? ineos, double? ukf, double? prax)
        {   
            // If we have all three supplier quotes, average them all:
            if(!IsValueNullOr0(ineos) && !IsValueNullOr0(ukf) && !IsValueNullOr0(prax)) { return ExcelAverage(ineos, ukf, prax); }

            // If we have two of the three: 
            // If ukf is null but ineos and prax are not:
            if (!IsValueNullOr0(ineos) && IsValueNullOr0(ukf) && !IsValueNullOr0(prax)) { return ExcelAverage(ineos, prax); }
            // If prax is null but ineos and ukf are not:
            if (!IsValueNullOr0(ineos) && !IsValueNullOr0(ukf) && IsValueNullOr0(prax)) { return ExcelAverage(ineos, ukf); }
            // If ineos is null but ukf and prax are not:
            if (IsValueNullOr0(ineos) && !IsValueNullOr0(ukf) && !IsValueNullOr0(prax)) { return ExcelAverage(ukf, prax); }

            // If we have one of the three:
            // If we have ineos but not ukf or prax:
            if (!IsValueNullOr0(ineos) && IsValueNullOr0(ukf) && IsValueNullOr0(prax)) { return ineos; }
            // If we have ukf but not ineos or prax:
            if (IsValueNullOr0(ineos) && !IsValueNullOr0(ukf) && IsValueNullOr0(prax)) { return ukf; }
            // If we have prax but not ukf or ineos:
            if (IsValueNullOr0(ineos) && IsValueNullOr0(ukf) && !IsValueNullOr0(prax)) { return prax; }

            // If all values fail the IsValueNullOr0 check:
            else { return null; }
            #region OLD CALCULATIONS
            //// Performing the calculations on the retrieved values
            //double ineos_calc;
            //if (ineos != null && ineos != 0)
            //{
            //    ineos_calc = Supplier_Calc(lsg, lsg_previous, ineos);
            //} else ineos_calc = 0;

            //double ukf_calc;
            //if (ukf != null && ukf != 0)
            //{
            //    ukf_calc = Supplier_Calc(lsg, lsg_previous, ukf);
            //} else ukf_calc = 0;

            //double prax_calc;
            //if (prax != null && prax != 0)
            //{
            //    prax_calc = Supplier_Calc(lsg, lsg_previous, prax);
            //}
            //else prax_calc = 0;

            //// If we only have one of the supplier prices, then only return a CalcAvg that is calculated using that supplier price.
            //if (ineos_calc != 0 && ukf_calc == 0 && prax_calc == 0)
            //{
            //    return ineos_calc;
            //}
            //else if (ineos_calc == 0 && ukf_calc != 0 && prax_calc == 0)
            //{
            //    return ukf_calc;
            //}
            //else if (ineos_calc == 0 && ukf_calc == 0 && prax_calc != 0)
            //{
            //    return prax_calc;
            //}
            //// If we have two supplier quotes out of 3: 
            //else if (ineos_calc != 0 && ukf_calc != 0 && prax_calc == 0)
            //{
            //    double ineos_ukf_average = ExcelAverage(ineos_calc, ukf_calc);
            //    double calcavg = Math.Round(ineos_ukf_average * 4, 0, MidpointRounding.AwayFromZero) / 4;
            //    return calcavg;
            //}
            //else if (ineos_calc != 0 && ukf_calc == 0 && prax_calc != 0)
            //{
            //    double ineos_prax_average = ExcelAverage(ineos_calc, prax_calc);
            //    double calcavg = Math.Round(ineos_prax_average * 4, 0, MidpointRounding.AwayFromZero) / 4;
            //    return calcavg;
            //}
            //else if (ineos_calc == 0 && ukf_calc != 0 && prax_calc != 0)
            //{
            //    double ukf_prax_average = ExcelAverage(ukf_calc, prax_calc);
            //    double calcavg = Math.Round(ukf_prax_average * 4, 0, MidpointRounding.AwayFromZero) / 4;
            //    return calcavg;
            //}
            //// And finally, if we have all three supplier prices and can generate a SupplierCalc for both :
            //else 
            //{
            //    double allSupplier_average = ExcelAverage(ineos_calc, ukf_calc, prax_calc);
            //    double calcavg = Math.Round(allSupplier_average * 4, 0, MidpointRounding.AwayFromZero) / 4;
            //    return calcavg;
            //}
            #endregion
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
            if (IsValueNullOr0(argusOMR_FAME_minus_ten) && IsValueNullOr0(prima_FAME_minus_ten)) { return Retrieve.Previous_Portland_FAME_minus_10(date); }

            // If we have one of the values only, then return just that
            if (!IsValueNullOr0(argusOMR_FAME_minus_ten) && IsValueNullOr0(prima_FAME_minus_ten)) { return argusOMR_FAME_minus_ten; }
            if (IsValueNullOr0(argusOMR_FAME_minus_ten) && !IsValueNullOr0(prima_FAME_minus_ten)) { return prima_FAME_minus_ten; }

            // Else, if we have both values, return an average of them both.
            else
            {
                double? price = Math.Round(ExcelAverage(argusOMR_FAME_minus_ten, prima_FAME_minus_ten) * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return price;
            }
        }
        // Calculates the Portland FAME0 price
        public static double? Portland_FAME_Zero(DateOnly date)
        {
            // First, retrieve the ArgusOMR FAME0 and Prima FAME0 prices : 
            double? argusOMR_FAME_Zero = Retrieve.ArgusOMR_FAME_Zero(date);
            double? prima_FAME_Zero = Retrieve.Prima_FAME_Zero(date);

            // If they are both null, then return the previous day's Portland FAME0 price:
            if (IsValueNullOr0(argusOMR_FAME_Zero) && IsValueNullOr0(prima_FAME_Zero)) { return Retrieve.Previous_Portland_FAME_Zero(date); }

            // If we have one of the values only, then return just that
            if (!IsValueNullOr0(argusOMR_FAME_Zero) && IsValueNullOr0(prima_FAME_Zero)) { return argusOMR_FAME_Zero; }
            if (IsValueNullOr0(argusOMR_FAME_Zero) && !IsValueNullOr0(prima_FAME_Zero)) { return prima_FAME_Zero; }

            // Else, if we have both values, return an average of them both.
            else
            {
                double? price = Math.Round(ExcelAverage(argusOMR_FAME_Zero, prima_FAME_Zero) * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return price;
            }
        }
        // Retrieves the GX Unleaded Petrol price to be used as the Portland Published Unleaded CIF NWE price
        public static double? Portland_Unleaded_CIF_NWE(DateOnly date)
        {
            DateOnly previous_date = Date.PreviousWorkingDay(date);

            double? lsg = Retrieve.LSG(date);
            double? lsg_previous;
            // Is date the rollover date for LSG contracts? If so, lsg_previous is M002 of previous_date, not M001
            if (Date.IsDateIceLSGRolloverDay(date) == true) { lsg_previous = Retrieve.LSGBeforeRollover(previous_date); }
            else { lsg_previous = Retrieve.LSG(previous_date); };

            double? ukf_unleaded = Retrieve.Unleaded_UKF_SupQuote(previous_date);
            double? unleaded_gx_price = Retrieve.GX_Unleaded_Petrol(date);

            // If LSG or the previous working date's LSG value is null, then:
            if (IsValueNullOr0(lsg) || IsValueNullOr0(lsg_previous))
            {   
                // if there is a GX Price for the given date, return that:
                if (IsValueNullOr0(unleaded_gx_price)) { return unleaded_gx_price; }
                // else, a value cannot be generated, so return null. 
                else { return null; }
            }
            // if there is a supplier quote in the database for the given date:
            if (!IsValueNullOr0(ukf_unleaded))
            {
                double? supCalc = ukf_unleaded + (lsg - lsg_previous);
                double price = Math.Round(ExcelAverage(Convert.ToDouble(supCalc), Convert.ToDouble(unleaded_gx_price)) * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return price;
            }
            // else if there is not a supplier quote in the database for the given date:
            else
            {   
                double? previousPortlandUnleaded = Retrieve.Previous_Portland_Unleaded_CIF_NWE(date);
                if (IsValueNullOr0(previousPortlandUnleaded)) { return unleaded_gx_price; };
                double? supCalc = previousPortlandUnleaded + (lsg - lsg_previous);
                double? price = Math.Round(ExcelAverage(Convert.ToDouble(supCalc), Convert.ToDouble(unleaded_gx_price)) * 4, 0, MidpointRounding.AwayFromZero) / 4;
                return price; 
            }
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
            if (!IsValueNullOr0(prima_t2_physical_ethanol) && !IsValueNullOr0(argusomr_ethanol))
            {
                portland_ethanol = ExcelAverage(prima_t2_physical_ethanol, argusomr_ethanol);
                // Return the portland_ethanol rounded to the nearest quarter of a dollar:
                return Math.Round(portland_ethanol * 4, 0, MidpointRounding.AwayFromZero) / 4; ;
            }
            else
            {
                // if we have one but not the other, then return the one that is not null
                if (!IsValueNullOr0(prima_t2_physical_ethanol) && IsValueNullOr0(argusomr_ethanol))
                { 
                    return Math.Round(Convert.ToDouble(prima_t2_physical_ethanol) * 4, 0, MidpointRounding.AwayFromZero) / 4; 
                }
                if (IsValueNullOr0(prima_t2_physical_ethanol) && !IsValueNullOr0(argusomr_ethanol)) 
                { 
                    return Math.Round(Convert.ToDouble(argusomr_ethanol) * 4, 0, MidpointRounding.AwayFromZero) /4; 
                }
                // Else, if both are null for a given date, return the previous Portland Ethanol EUR CBM:
                else
                {
                    double? previous_portland_ethanol_eur_cbm = Retrieve.Previous_Portland_Ethanol_EUR_CBM(date);
                    return previous_portland_ethanol_eur_cbm;
                }
            }
        }
        // Calculates the Portland HVO FRB price for a given date
        public static double? Portland_HVO_FRB(DateOnly date)
        {
            double? hvo_production_cost = Retrieve.HVO_Production_Cost(date);
            double? diesel_cif_nwe = Retrieve.Diesel_CIF_NWE(date);
            double? hvo_blend_percentage = Retrieve.HVO_Blend_Percentages(date);

            double? gbp_eur = Retrieve.FTGbp_To_Eur(date);
            double? gbp_usd = Retrieve.FTGbp_To_Usd(date);

            // Data retrieved from Prima/ArgusOMR - each retrieval function will find the most recent price - not the one for the given date. This ensures a price is always found.
            // In the case of the Argus/Prima program failing (which it routinely does...)
            double? prima_t1_uco_cif_ara = Retrieve.Prima_T1_UCO_CIF_ARA(date);
            double? argusomr_thg_konventionelle = Retrieve.ArgusOMR_THG_Konventionelle(date);
            double? argusomr_hvo_class_ii = Retrieve.ArgusOMR_HVO_Class_II(date);
            double? prima_hvo_plant = Retrieve.Prima_HVO_Plant(date);

            // if any of the retrieved values are null, then return null as all are necessary to creating following steps.
            if (IsValueNullOr0(hvo_production_cost) || IsValueNullOr0(prima_t1_uco_cif_ara) || IsValueNullOr0(diesel_cif_nwe) || IsValueNullOr0(hvo_blend_percentage)
                || IsValueNullOr0(argusomr_thg_konventionelle) || IsValueNullOr0(gbp_eur) || IsValueNullOr0(gbp_usd) || IsValueNullOr0(argusomr_hvo_class_ii)
                || IsValueNullOr0(prima_hvo_plant) ) { return null; }

            double? hvo_uco = hvo_production_cost + prima_t1_uco_cif_ara;
            double? hvo_coc = ( ((hvo_blend_percentage * 3.4141) * (argusomr_thg_konventionelle / gbp_eur) / 10 / hvo_blend_percentage)
                + (diesel_cif_nwe / gbp_usd / 11.83) ) * gbp_usd * 11.83;
            double? supplier_quotes = ExcelAverage(argusomr_hvo_class_ii, prima_hvo_plant);
            double? hvo_frb = Math.Round(ExcelAverage(hvo_uco, hvo_coc, supplier_quotes) * 4, 0, MidpointRounding.AwayFromZero) / 4;

            return hvo_frb;
        }
        // Calculates the Portland CIF NWE price for a given date
        public static double? Portland_HVO_CIF_NWE(DateOnly date)
        {
            double? hvo_frb = Retrieve.Portland_HVO_FRB(date);
            if (hvo_frb == null) { return null; }
            double? gx093 = Retrieve.GX_093(date);
            double? gx258 = Retrieve.GX_258(date);
            double? hvo_cif_nwe = hvo_frb + (gx093 - gx258);
            return hvo_cif_nwe;
        }
        // Calculates the Portland Diesel FRB price for a given date
        public static double? Portland_Diesel_FRB(DateOnly date)
        {
            double? gx257 = Retrieve.GX_257(date);
            double? gx258 = Retrieve.GX_258(date);
            double? diesel_frb = Retrieve.Portland_Diesel_Price(date) - (gx257 - gx258);
            return diesel_frb;
        }
        // Calculates the Portland Gasoil 1% CIF NWE price for a given date
        public static double? Portland_Gasoil1percent_CIF_NWE(DateOnly date)
        {
            double? gasoil = Retrieve.GX_082(date);
            return gasoil;
        }
        // Calculates the Portland FuelOil 3.5% FRB price for a given date
        public static double? Portland_FuelOil3point5_FRB(DateOnly date)
        {
            double? fueloil = Retrieve.GX_266(date);
            return fueloil;
        }
        // Calculates the Portland MFO 0.5% price for a given date
        public static double? Portland_MFO0point5_FRB(DateOnly date)
        {
            double? mfo = Retrieve.GX_087(date);
            return mfo;
        }
        // Calculates the Portland NYH Diesel price for a given date
        public static double? Portland_NYH_Diesel(DateOnly date)
        {
            double? gx1032 = Retrieve.GX_1032(date);
            return gx1032;
        }
        // A cleaner way of checking if a value is null or 0
        private static bool IsValueNullOr0(double? value)
        {
            if (value == null || value == 0) { return true; }
            else { return false; }
        }
    }
}
