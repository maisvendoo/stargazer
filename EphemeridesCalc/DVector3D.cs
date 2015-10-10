using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    public class DVector3D
    {
        public decimal x;
        public decimal y;
        public decimal z;

        public DVector3D(decimal x, decimal y, decimal z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static DVector3D operator +(DVector3D a, DVector3D b)
        {
            return new DVector3D(a.x + b.x,
                                 a.y + b.y,
                                 a.z + a.z);
        }

        public static DVector3D operator -(DVector3D a, DVector3D b)
        {
            return new DVector3D(a.x - b.x,
                                 a.y - b.y,
                                 a.z - a.z);
        }

        public static decimal operator *(DVector3D a, DVector3D b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static DVector3D operator &(DVector3D a, DVector3D b)
        {
            return new DVector3D(a.y * b.z - a.z * b.y,
                                 a.z * b.x - a.x * b.z,
                                 a.x * b.y - a.y * b.x);
        }

        public decimal lenght()
        {
            return DMath.sqrt(x * x + y * y + z * z);
        }

        public DVector3D Mult(decimal lambda)
        {
            return new DVector3D(x * lambda, y * lambda, z * lambda);
        }

        public DVector3D Div(decimal lambda)
        {
            return new DVector3D(x / lambda, y / lambda, z / lambda);
        }

        public DVector3D ort()
        {
            if (lenght() == 0)
                return new DVector3D(0m, 0m, 0m);
            else
                return Div(lenght());
        }

        public decimal angle(DVector3D a)
        {
            return (this * a) / this.lenght() / a.lenght();
        }
    }
}
