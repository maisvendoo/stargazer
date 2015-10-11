using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astronomy
{
    public class DMath
    {
        public const decimal PI = 3.1415926535897932384626433832795m;
        public const decimal RAD = PI / 180.0m;

        private const decimal ERROR = 1e-16m; 
        


        //---------------------------------------------------------------------
        //      y = |x|
        //---------------------------------------------------------------------
        public static decimal abs(decimal x)
        {
            if (x >= 0)
                return x;
            else
                return -x;
        }

        //---------------------------------------------------------------------
        //      y = sign x
        //---------------------------------------------------------------------
        public static decimal sign(decimal x)
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

        //---------------------------------------------------------------------
        //      y = sin x
        //---------------------------------------------------------------------
        public static decimal sin(decimal x)
        {
            decimal sum = x;
            decimal eps = ERROR;

            decimal p = x;
            int n = 2;

            while (abs(p) >= eps)
            {
                p = -p * x * x / (n * (n + 1));
                sum += p;
                n += 2;
            }

            return sum;
        }

        //---------------------------------------------------------------------
        //      y = cos x
        //---------------------------------------------------------------------
        public static decimal cos(decimal x)
        {
            decimal sum = 1;
            decimal eps = ERROR;

            decimal p = 1;
            int n = 2;

            while (abs(p) >= eps)
            {
                p = -p * x * x / (n * (n - 1));
                sum += p;
                n += 2;
            }

            return sum;
        }

        //---------------------------------------------------------------------
        //      y = tg x
        //---------------------------------------------------------------------
        public static decimal tan(decimal x)
        {
            return sin(x) / cos(x);
        }

        //---------------------------------------------------------------------
        //      y = exp x
        //---------------------------------------------------------------------
        public static decimal exp(decimal x)
        {
            decimal sum = 1 + x;
            decimal eps = ERROR;

            decimal p = x;
            int n = 2;

            while (abs(p) >= eps)
            {
                p = p * x / n;
                sum += p;
                n += 1;
            }

            return sum;
        }

        //---------------------------------------------------------------------
        //            ___
        //      y = \/ x
        //
        //---------------------------------------------------------------------
        public static decimal sqrt(decimal a)
        {
            decimal eps = ERROR;
            decimal xn_1 = 1.0m;
            decimal xn = xn_1;

            do
            {
                xn_1 = xn;
                xn = (xn_1 * xn_1 + a) / 2 / xn_1;
            } while (abs(xn - xn_1) >= eps);

            return xn_1;
        }
        
        //---------------------------------------------------------------------
        //      y = ln x
        //---------------------------------------------------------------------
        public static decimal log(decimal a)
        {
            decimal eps = ERROR;
            decimal xn_1 = 0;
            decimal xn = xn_1;

            do
            {
                xn_1 = xn;
                xn = xn_1 - 1 + a/exp(xn_1);
            } while (abs(xn - xn_1) >= eps);

            return xn_1;
        }

        //---------------------------------------------------------------------
        //      y = arcsin x
        //---------------------------------------------------------------------
        public static decimal asin(decimal a)
        {
            decimal eps = ERROR;
            decimal xn_1 = 0;
            decimal xn = xn_1;

            do
            {
                xn_1 = xn;
                xn = xn_1  - (sin(xn_1) - a)/cos(xn_1);
            } while (abs(xn - xn_1) >= eps);

            return xn_1;
        }

        //---------------------------------------------------------------------
        //      y = arccos x
        //---------------------------------------------------------------------
        public static decimal acos(decimal a)
        {
            decimal eps = ERROR;
            decimal xn_1 = 1.0m;
            decimal xn = xn_1;

            do
            {
                xn_1 = xn;
                xn = xn_1 + (cos(xn_1) - a) / sin(xn_1);
            } while (abs(xn - xn_1) >= eps);

            return xn_1;
        }

        //---------------------------------------------------------------------
        //      y = a^x
        //---------------------------------------------------------------------
        public static decimal pow(decimal a, decimal x)
        {
            return exp(x * log(a));
        }

        //---------------------------------------------------------------------
        //      y = sh x
        //---------------------------------------------------------------------
        public static decimal sinh(decimal x)
        {
            decimal sum = x;
            decimal eps = ERROR;

            decimal p = x;
            int n = 2;

            while (abs(p) >= eps)
            {
                p = p * x * x / (n * (n + 1));
                sum += p;
                n += 2;
            }

            return sum;            
        }

        //---------------------------------------------------------------------
        //      y = ch x
        //---------------------------------------------------------------------
        public static decimal cosh(decimal x)
        {
            decimal sum = 1;
            decimal eps = ERROR;

            decimal p = 1;
            int n = 2;

            while (abs(p) >= eps)
            {
                p = p * x * x / (n * (n - 1));
                sum += p;
                n += 2;
            }

            return sum;
        }

        //---------------------------------------------------------------------
        //      y = th x
        //---------------------------------------------------------------------
        public static decimal tanh(decimal x)
        {
            return sinh(x) / cosh(x);
        }

        //---------------------------------------------------------------------
        //      Angle calculation (x) from sin x and cos x (x in [0..2Pi])
        //---------------------------------------------------------------------
        public static decimal arg(decimal sin_x, decimal cos_x)
        {
            if (sin_x >= 0)
                return acos(cos_x);
            else
                return 2 * PI - acos(cos_x);
        }

        //-----------------------------------------------------------
        //      Truncate integer mumber of periods (2Pi)
        //-----------------------------------------------------------
        public static decimal Trunc2PiN(decimal x)
        {
            int N = Convert.ToInt32(Decimal.Truncate(abs(x) / 2 / DMath.PI));

            return (abs(x) - N * 2 * DMath.PI)*sign(x);
        }
    }
}
