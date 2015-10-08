using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    public class CCalendar
    {
        public const int Days = 426;
        public const int Hours = 6;
        public const int Mins = 60;
        public const int Secs = 60;

        public double  date_to_sec(int year, int day, int hour, int min, int sec)
        {
            return 9201600.0 * (year - 1) + 21600.0 * (day - 1) + 3600.0 * hour + 60.0 * min + sec;
        }
    }
}
