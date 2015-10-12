using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astronomy
{
    public struct KDate
    {
        public int year;
        public int day;
        public int hour;
        public int min;
        public int sec;
    }
    
    public class KCalendar
    {
        public const int Days = 426;
        public const int Hours = 6;
        public const int Mins = 60;
        public const int Secs = 60;

        public static double  date_to_sec(int year, int day, int hour, int min, int sec)
        {
            return 9201600.0 * (year - 1) + 21600.0 * (day - 1) + 3600.0 * hour + 60.0 * min + sec;
        }

        public static void sec_to_date(double t, ref KDate date)
        {
            int sYear = Days * Hours * Mins * Secs;
            int sDay = Hours * Mins * Secs;
            int sHour = Mins * Secs;
            int sMin = Secs;

            date.year = Convert.ToInt32(Math.Truncate(t / sYear)) + 1;
            date.day = Convert.ToInt32(Math.Truncate( (t - (date.year-1)*sYear) / sDay)) + 1;
            date.hour = Convert.ToInt32(Math.Truncate((t - (date.year - 1) * sYear - (date.day - 1) * sDay) / sHour));
            date.min = Convert.ToInt32(Math.Truncate((t - (date.year - 1) * sYear - (date.day - 1) * sDay - date.hour * sHour) / sMin));
            date.sec = Convert.ToInt32(Math.Truncate(t - (date.year - 1) * sYear - (date.day - 1) * sDay - date.hour * sHour - date.min * sMin));
        }

        public static void DeltaDate(double t, ref KDate date)
        {
            int sYear = Days * Hours * Mins * Secs;
            int sDay = Hours * Mins * Secs;
            int sHour = Mins * Secs;
            int sMin = Secs;

            date.year = Convert.ToInt32(Math.Truncate(t / sYear));
            date.day = Convert.ToInt32(Math.Truncate((t - (date.year) * sYear) / sDay));
            date.hour = Convert.ToInt32(Math.Truncate((t - (date.year) * sYear - (date.day ) * sDay) / sHour));
            date.min = Convert.ToInt32(Math.Truncate((t - (date.year) * sYear - (date.day) * sDay - date.hour * sHour) / sMin));
            date.sec = Convert.ToInt32(Math.Truncate(t - (date.year) * sYear - (date.day) * sDay - date.hour * sHour - date.min * sMin));
        }
    }
}
