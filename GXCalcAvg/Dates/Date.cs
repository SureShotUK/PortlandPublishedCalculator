using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicHoliday;

namespace PortlandPublishedCalculator.Dates;

public class Date
{
    readonly static UKBankHoliday bh = new();

    // Returns the nearest working day after a given DateOnly date - or if the date is already a working day, returns the same value. 
    public static DateOnly WorkingDayCheck(DateOnly date)
    {
        date = DateOnly.FromDateTime(bh.NextWorkingDay(date.ToDateTime(TimeOnly.Parse("00:00:00"))));
        return date;
    }
    // Returns the next working day which is not the same day as the given DateOnly date. 
    public static DateOnly NextWorkingDay(DateOnly date)
    {
        return DateOnly.FromDateTime(bh.NextWorkingDayNotSameDay(date.ToDateTime(TimeOnly.Parse("00:00:00"))));
    }
    public static DateOnly PreviousWorkingDay(DateOnly date)
    {
        return DateOnly.FromDateTime(bh.PreviousWorkingDayNotSameDay(date.ToDateTime(TimeOnly.Parse("00:00:00"))));
    }
    // Checks if the given date is a rollover day for the ICE LSG contracts - which means that the 'M001' value month changes
    public static bool IsDateIceLSGRolloverDay(DateOnly date)
    {
        // Rollover date = 2 working days prior to 14th of month
        // First find the 14th of the given month. 
        // Check if date is 2 working days before 14th. 

        // Gets the 14th day of the month/year that date is in
        DateOnly dayFourteen = new(date.Year, date.Month, 14);

        DateOnly rollOverDay = DateOnly.FromDateTime(
                bh.PreviousWorkingDayNotSameDay(
                    bh.PreviousWorkingDayNotSameDay(dayFourteen.ToDateTime(TimeOnly.Parse("00:00:00")))
                )
            );

        if (rollOverDay == date)
        {
            return true;
        }
        else { return false; }

    }
    // Checks if given date falls on the last day of the month
    public static bool IsDateLastDayOfMonth(DateOnly date)
    {
        int lastDayOfMonth = DateTime.DaysInMonth(date.Year, date.Month);
        if (lastDayOfMonth == date.Day)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // The following function returns the date in the same format as the delivery column in the prima_forward_price table
    // Which is : " Mmm yyyy " e.g. " Sep 2022 " 
    public static string DeliveryDateFormat(DateOnly date)
    {
        int year = date.Year;
        string month_name = date.ToString("MMM");
        return (month_name + " " + year);
    }
}
