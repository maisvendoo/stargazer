using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    public class CCalendar
    {
        public const int KERBOL = 0;
        public const int SOLAR = 1;

        private int system;

        private int year;
        private int day;
        private int hour;
        private int min;
        private int sec;
                
        public double date_to_sec(int year, int day, int hour, int min, int sec)
        {
            return 9201600.0 * (year - 1) + 21600.0 * (day - 1) + 3600.0 * hour + 60.0 * min + sec;
        }
    }
}
