﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortlandPublishedCalculator.Prices;

namespace PortlandPublishedCalculator.DatabaseQueries;

public class UploadToDB
{
    // Create database connection
    readonly static PricesContext db = new();
    public static void YPublishedWholesale(DateOnly date, double? price, string grade)
    {
        // If there is already a row of data in the database for the given date, then append that row with the given data.
        if (DoesPublishedDateExist(date))
        {

            YPublishedWholesale existingRow = db.YPublishedWholesales.Where(x => x.PublishedDate == date).First();
            switch (grade)
            {
                case "diesel":
                    existingRow.DieselCifNwe = price;
                    break;
                case "fame":
                    existingRow.Fame10 = price;
                    break;
                case "petrol":
                    existingRow.UnleadedCifNwe = price;
                    break;
                case "ethanol":
                    existingRow.EthanolEurCbm = price;
                    break;
                case "jet":
                    existingRow.JetCifNwe = price;
                    break;
                case "propane":
                    existingRow.PropaneCifNwe = price;
                    break;

            }
            db.SaveChanges();
        }

        // Else, if there is not a row of data in the database for the given date, then add a new row to the database with the given data.
        else
        {
            YPublishedWholesale ypw = new();
            switch (grade)
            {   
                case "diesel":
                    ypw.DieselCifNwe = price;
                    break;
                case "fame":
                    ypw.Fame10 = price;
                    break;
                case "petrol":
                    ypw.UnleadedCifNwe = price;
                    break;
                case "ethanol":
                    ypw.EthanolEurCbm = price;
                    break;
                case "jet":
                    ypw.JetCifNwe = price;
                    break;
                case "propane":
                    ypw.PropaneCifNwe = price;
                    break;
            }
            ypw.PublishedDate = date;
            db.YPublishedWholesales.Add(ypw);
            db.SaveChanges();

        }

    }
    // This function will return true if the given date already has a row of data in the database. 
    public static bool DoesPublishedDateExist(DateOnly date)
    {
        try
        {
            // Checking if the specified date exists in the database
            YPublishedWholesale? rowForDate = db.YPublishedWholesales.Where(x => x.PublishedDate == date).FirstOrDefault();
            if (rowForDate != null) { return true; }

            else { return false; }
        }
        catch
        {
            return false;
        }
    }
}