using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheVilleSkill.Models.Eventful
{
    public class TimePeriod
    {
        // Date used for the date argument in the /events/search queries to the Eventful API.
        // https://api.eventful.com/docs/events/search
        // Limit this list of results to a date range, specified by label or exact range. Currently supported labels include: 
        // "All", "Future", "Past", "Today", "Last Week", "This Week", "Next week", and months by name, e.g. "October". Exact
        // ranges can be specified the form 'YYYYMMDD00-YYYYMMDD00', for example '2012042500-2012042700'; the last two digits
        // of each date in this format are ignored.

        public static string All => "All";

        public static string Future => "Future";

        public static string Past => "Past";        

        public static string Today => "Today";

        public static string ThisWeek => "This+Week";

        public static string NextWeek => "Next+Week";

        public static string January => "January";

        public static string February => "February";

        public static string March => "March";

        public static string April => "April";

        public static string May => "May";

        public static string June => "June";

        public static string July => "July";

        public static string August => "August";

        public static string September => "September";

        public static string October => "October";

        public static string November => "November";

        public static string December => "December";

        public static string DateRange(DateTime fromDate, DateTime toDate)
        {
            string strFromDate = $"{fromDate.Year}{fromDate.Month:D2}{fromDate.Day:D2}";
            string strToDate = $"{toDate.Year}{toDate.Month:D2}{toDate.Day:D2}";
            string range = $"{strFromDate}-{strToDate}";
            return range;
        }

        public static string NextXDays(int days, DateTime today) => DateRange(today, today.AddDays(days));

        public static string OnDate(DateTime date) => DateRange(date, date);
    }
}
