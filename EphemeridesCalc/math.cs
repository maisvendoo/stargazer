using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astronomy
{
    class  math
    {
        public const double RAD = Math.PI / 180.0;

        public static double sign(double x)
        {
            if (x == 0)
                return 0;
            else
            {
                if (x > 0)
                    return 1;
                else
                    return -1;
            }
        }
        
        public static double  arg(double  sin_x, double  cos_x)
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

        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        public static double Trunc2PiN(double x)
        {
            int N = Convert.ToInt32(Math.Truncate(Math.Abs(x) / 2 / Math.PI));

            return (Math.Abs(x) - N * 2 * Math.PI)*sign(x);
        }

        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        public static double Atanh(double x)
        {
            return 0.5 * Math.Log((1 + x) / (1 - x));
        }
    }    
}


