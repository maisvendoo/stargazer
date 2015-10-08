using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    class  math
    {
        public static double  Arg(double  sin_x, double  cos_x)
        {
            if (sin_x >= 0)
            {
                return Math.Acos(cos_x);
            }
            else
            {
                return 2 * Math.PI - Math.Acos(cos_x);
            }
        }


        public static decimal abs(decimal x)
        {
            if (x >= 0)
                return x;
            else
                return -x;
        }


        public static double sin(double x)
        {
            decimal sum = Convert.ToDecimal(x);
            decimal eps = Convert.ToDecimal(1e-16);

            decimal p = Convert.ToDecimal(x);            
            int n = 2;

            while (abs(p) >= eps)
            {
                p = -p * Convert.ToDecimal(x) * Convert.ToDecimal(x) / (n * (n + 1));
                sum += p;
                n += 2;
            }

            return Convert.ToDouble(sum);
        }

        public static double cos(double x)
        {
            decimal sum = 1;
            decimal eps = Convert.ToDecimal(1e-16);

            decimal p = 1;
            int n = 2;

            while (abs(p) >= eps)
            {
                p = -p * Convert.ToDecimal(x) * Convert.ToDecimal(x) / (n * (n - 1));
                sum += p;
                n += 2;
            }

            return Convert.ToDouble(sum);
        }
    }    
}


