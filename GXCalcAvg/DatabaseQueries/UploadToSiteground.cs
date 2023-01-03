using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortlandPublishedCalculator.Prices;
using PortlandPublishedCalculator.SiteGround;

namespace PortlandPublishedCalculator.DatabaseQueries;

public class UploadToSiteground
{
    // Create database connection
    readonly static SiteGroundContext db = new();
    public static void YPublishedWholesale(DateOnly date, double? price, string grade)
    {
        // If there is already a row of data in the database for the given date, then append that row with the given data.
        if (DoesPublishedDateExist(date))
        {

            SiteGround.YPublishedWholesale existingRow = db.YPublishedWholesales.Where(x => x.PublishedDate == date).First();
            switch (grade)
            {
                case "diesel":
                    existingRow.DieselCifNwe = price;
                    break;
                case "fame_10":
                    existingRow.Fame10 = price;
                    break;
                case "fame0":
                    existingRow.Fame0 = price;
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
                case "hvo_frb":
                    existingRow.HvoFrb = price;
                    break;
                case "hvo_cif_nwe":
                    existingRow.HvoCifNwe = price;
                    break;
                case "diesel_frb":
                    existingRow.DieselFrb = price;
                    break;
                case "gasoil":
                    existingRow.Gasoil01CifNwe = price;
                    break;
                case "fueloil":
                    existingRow.Fueloil35Frb = price;
                    break;
                case "mfo":
                    existingRow.Mfo05Frb = price;
                    break;

            }
            db.SaveChanges();
        }

        // Else, if there is not a row of data in the database for the given date, then add a new row to the database with the given data.
        else
        {
            SiteGround.YPublishedWholesale ypw = new();
            switch (grade)
            {
                case "diesel":
                    ypw.DieselCifNwe = price;
                    break;
                case "fame_10":
                    ypw.Fame10 = price;
                    break;
                case "fame0":
                    ypw.Fame0 = price;
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
                case "hvo_frb":
                    ypw.HvoFrb = price;
                    break;
                case "hvo_cif_nwe":
                    ypw.HvoCifNwe = price;
                    break;
                case "diesel_frb":
                    ypw.DieselFrb = price;
                    break;
                case "gasoil":
                    ypw.Gasoil01CifNwe = price;
                    break;
                case "fueloil":
                    ypw.Fueloil35Frb = price;
                    break;
                case "mfo":
                    ypw.Mfo05Frb = price;
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
            SiteGround.YPublishedWholesale? rowForDate = db.YPublishedWholesales.Where(x => x.PublishedDate == date).FirstOrDefault();
            if (rowForDate != null) { return true; }

            else { return false; }
        }
        catch
        {
            return false;
        }
    }
}
