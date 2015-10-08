using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    class math
    {
        public double Arg(double sin_x, double cos_x)
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
    }    
}


