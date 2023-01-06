using System;
using PublicHoliday;
using PortlandPublishedCalculator;
using PortlandPublishedCalculator.DatabaseQueries;
using PortlandPublishedCalculator.Dates;
using PortlandPublishedCalculator.Calculations;
using PortlandPublishedCalculator.Utility;
using System.Text;
#pragma warning disable CS8321 // Local function is declared but never used.

///----------------- notes on program -----------------///

// This program will calculate the Portland Published Wholesale prices for each of the grades that will be uploaded in the y_published_wholesale table of the database.
// The grades that are currently being calculated are: diesel_cif_nwe, fame-10, unleaded_cif_nwe, ethanol_eur_cbm, jet_cif_nwe, propane_cif_nwe

///----------------- notes on program -----------------///


// Calculates and uploads the Portland Published Wholesale prices for every working day inbetween and including the 'startDate' and 'endDate' variables.
// Good for testing purposes only. Not intended to be used for daily automation of the program.
static void BetweenTwoDates()
{
    //yy - mm - dd format:
    DateOnly startDate = new(2023, 01, 05);
    DateOnly endDate = new(2023, 01, 05);

    DateOnly date = Date.WorkingDayCheck(startDate);
    while (date <= endDate)
    {
        //// Calculates the Portland Diesel CIF NWE for a given date. 
        //double? diesel_price = Calculations.Portland_Diesel_CIF_NWE(date, false);
        //if (diesel_price.HasValue)
        //{
        //    Console.WriteLine("The Portland Diesel Price for " + date + " is " + diesel_price);
        //    // UploadToDB.YPublishedWholesale(date, diesel_price, "diesel"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, diesel_price, "diesel"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland FAME-10 price for a given date.
        //double? portland_fame_price = Calculations.Portland_FAME_minus_ten(date);
        //if (portland_fame_price.HasValue)
        //{
        //    Console.WriteLine("The Portland FAME-10 Price for " + date + " is " + portland_fame_price);
        //    // UploadToDB.YPublishedWholesale(date, portland_fame_price, "fame_10"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, portland_fame_price, "fame_10"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland FAME-10 price for a given date.
        //double? portland_fame0_price = Calculations.Portland_FAME_Zero(date);
        //if (portland_fame0_price.HasValue)
        //{
        //    Console.WriteLine("The Portland FAME0 Price for " + date + " is " + portland_fame0_price);
        //    // UploadToDB.YPublishedWholesale(date, portland_fame0_price, "fame0"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, portland_fame0_price, "fame0"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland Unleaded CIF NWE price for a given date.
        //double? portland_unleaded_petrol = Calculations.Portland_Unleaded_CIF_NWE(date);
        //if (portland_unleaded_petrol.HasValue)
        //{
        //    Console.WriteLine("The Portland Unleaded CIF NWE Price for " + date + " is " + portland_unleaded_petrol);
        //    //UploadToDB.YPublishedWholesale(date, portland_unleaded_petrol, "petrol"); //Uploads the price to the database.
        //    //UploadToSiteground.YPublishedWholesale(date, portland_unleaded_petrol, "petrol"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland Ethanol EUR CBM price for a given date.
        //double? portland_ethanol_eur_cbm = Calculations.Portland_Ethanol_EUR_CBM(date);
        //if (portland_ethanol_eur_cbm.HasValue)
        //{
        //    Console.WriteLine("The Portland Ethanol EUR CBM Price for " + date + " is " + portland_ethanol_eur_cbm);
        //    // UploadToDB.YPublishedWholesale(date, portland_ethanol_eur_cbm, "ethanol"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, portland_ethanol_eur_cbm, "ethanol"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland Jet CIF NWE price for a given date.
        //double? portland_jet_cif_nwe = Calculations.Portland_Jet_CIF_NWE(date);
        //if (portland_jet_cif_nwe.HasValue)
        //{
        //    Console.WriteLine("The Portland Jet CIF NWE Price for " + date + " is " + portland_jet_cif_nwe);
        //    // UploadToDB.YPublishedWholesale(date, portland_jet_cif_nwe, "jet"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, portland_jet_cif_nwe, "jet"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland Propane CIF NWE price for a given date.
        //double? portland_propane_cif_nwe = Calculations.Portland_Propane_CIF_NWE(date);
        //if (portland_propane_cif_nwe.HasValue)
        //{
        //    Console.WriteLine("The Portland Propane CIF NWE Price for " + date + " is " + portland_propane_cif_nwe);
        //    // UploadToDB.YPublishedWholesale(date, portland_propane_cif_nwe, "propane"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, portland_propane_cif_nwe, "propane"); //Uploads the price to the siteground database.
        //}

        // Calculates the Portland HVO FRB price for a given date.
        double? portland_hvo_frb = Calculations.TEMPORARY_Portland_HVO_FRB(date);
        if (portland_hvo_frb.HasValue)
        {
            Console.WriteLine("The Portland HVO FRB Price for " + date + " is " + portland_hvo_frb);
            // UploadToDB.YPublishedWholesale(date, portland_hvo_frb, "hvo_frb"); //Uploads the price to the database.
            // UploadToSiteground.YPublishedWholesale(date, portland_hvo_frb, "hvo_frb"); //Uploads the price to the siteground database.
        }

        //// Calculates the Portland HVO CIF NWE price for a given date.
        //double? portland_hvo_cif_nwe = Calculations.Portland_HVO_CIF_NWE(date);
        //if (portland_hvo_cif_nwe.HasValue)
        //{
        //    Console.WriteLine("The Portland HVO CIF NWE Price for " + date + " is " + portland_hvo_cif_nwe);
        //    // UploadToDB.YPublishedWholesale(date, portland_hvo_cif_nwe, "hvo_cif_nwe"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, portland_hvo_cif_nwe, "hvo_cif_nwe"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland Diesel FRB price for a given date.
        //double? portland_diesel_frb = Calculations.Portland_Diesel_FRB(date);
        //if (portland_diesel_frb.HasValue)
        //{
        //    Console.WriteLine("The Portland Diesel FRB Price for " + date + " is " + portland_diesel_frb);
        //    //UploadToDB.YPublishedWholesale(date, portland_diesel_frb, "diesel_frb"); //Uploads the price to the database.
        //    //UploadToSiteground.YPublishedWholesale(date, portland_diesel_frb, "diesel_frb"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland Gasoil price for a given date.
        //double? portland_gasoil = Calculations.Portland_Gasoil1percent_CIF_NWE(date);
        //if (portland_gasoil.HasValue)
        //{
        //    Console.WriteLine("The Portland Gasoil Price for " + date + " is " + portland_gasoil);
        //    //UploadToDB.YPublishedWholesale(date, portland_gasoil, "gasoil"); //Uploads the price to the database.
        //    //UploadToSiteground.YPublishedWholesale(date, portland_gasoil, "gasoil"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland Fueloil price for a given date.
        //double? portland_fueloil = Calculations.Portland_FuelOil3point5_FRB(date);
        //if (portland_fueloil.HasValue)
        //{
        //    Console.WriteLine("The Portland Fueloil Price for " + date + " is " + portland_fueloil);
        //    //UploadToDB.YPublishedWholesale(date, portland_fueloil, "fueloil"); //Uploads the price to the database.
        //    //UploadToSiteground.YPublishedWholesale(date, portland_fueloil, "fueloil"); //Uploads the price to the siteground database.
        //}

        //// Calculates the Portland MFO price for a given date.
        //double? portland_mfo = Calculations.Portland_MFO0point5_FRB(date);
        //if (portland_mfo.HasValue)
        //{
        //    Console.WriteLine("The Portland MFO Price for " + date + " is " + portland_mfo);
        //    // UploadToDB.YPublishedWholesale(date, portland_mfo, "mfo"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, portland_mfo, "mfo"); //Uploads the price to the siteground database.
        //}

        //// Calculates the NYH Diesel price for a given date.
        //double? portland_nyh_diesel = Calculations.Portland_NYH_Diesel(date);
        //if (portland_nyh_diesel.HasValue)
        //{
        //    Console.WriteLine("The Portland NYH Diesel Price for " + date + " is " + portland_nyh_diesel);
        //    // UploadToDB.YPublishedWholesale(date, portland_nyh_diesel, "nyh_diesel"); //Uploads the price to the database.
        //    // UploadToSiteground.YPublishedWholesale(date, portland_nyh_diesel, "nyh_diesel"); //Uploads the price to the siteground database.
        //}

        date = Date.NextWorkingDay(date); //Goes to the following working day. 
    }
}

// Calculates and uploads the previous working days Portland Published Wholesale prices for each of the grades.
// To be used for daily running of the program. 
static void CalculatePrices()
{

    DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
    DateOnly previousWorkingDay = Date.PreviousWorkingDay(currentDate);

    #region ARGUS CHECKS
    // ARGUS CHECK
    // Because Argus emails have been causing issues lately, we have put a check here to make sure that Argus data exists for the day we are trying to generate a price for.
    // If there is missing Argus data, then the program will stop running completely and be manually uploaded. 
    
    if (!ArgusCheck.DoesArgusDataForDateExist(previousWorkingDay))
    {
        // Send email
        ArgusCheck.SendArgusCheckEmail(@$"<html> 
                <body>
                <p>The Portland Published Wholesale program has stopped because there is missing Argus data for {previousWorkingDay} in the database.</p>
                <p>Essential data is missing from the <strong>y_argusomr_biofuels_newer</strong> table, such as FAME-10.</p>
                <p>The program will have to be run manually again once the Argus data is in the database.</p>
                </body>
                </html>");

        Environment.Exit(0);
    }

    #endregion

    StringBuilderPlusConsole.WriteLine("The Portland Published Prices for " + previousWorkingDay + " are:");
    StringBuilderPlusConsole.WriteLineSBOnly("<hr>");

    GradesEnum.Grade[] grades = GradesEnum.GetAllGradesAsArray();
    bool ErrorChecker = false;
    foreach (var grade in grades)
    {
        string gradename = GradesEnum.GetGradeName(grade);
        double? price = Calculated.Price(grade, previousWorkingDay);
        if (price.HasValue)
        {
            StringBuilderPlusConsole.GradeAppend(gradename, price);
            UploadToDB.YPublishedWholesale(previousWorkingDay, price, grade.ToString()); // Uploads the price to the database.
            UploadToSiteground.YPublishedWholesale(previousWorkingDay, price, grade.ToString()); // Uploads the price to the siteground database. 
        } else { StringBuilderPlusConsole.WriteLine("<b>" + gradename + ": </b>" + "ERROR - Cannot generate a value."); ErrorChecker = true; }
    }

    // Sends an email of all the generated prices that have been added to the StringBuilder string.
    Email.SendEmail(ErrorChecker);
}

// Calls the production method to begin running the program when the .exe is opened. 
CalculatePrices();

//// For testing purposes
//BetweenTwoDates();
