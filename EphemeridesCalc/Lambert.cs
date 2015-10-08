using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    public class Lambert
    {
        private double r1;
        private double r2;
        private double s;
        private double tau;
        
        public TOrbitData get_orbit(Vector3D x1, Vector3D x2, double dt, double mu)
        {
            TOrbitData orbit = new TOrbitData();
            Vector3D vec = new Vector3D();

            r1 = x1.lenght();
            r2 = x2.lenght();

            Vector3D dx = x1 - x2;
            s = dx.lenght();

            Vector3D N = x1 & x2;
            Vector3D n = N.get_ort();
            double len = n.lenght();

            orbit.i = Math.Asin(Math.Sqrt(n.x*n.x + n.y*n.y));

            tau = Math.Sqrt(mu) * dt;

            double[] x = new double[3]{Math.PI/2, Math.PI/2, (r1 + r2)/2};
            double eps = 1e-2;
            double[] err = new double[3] { eps, eps, eps };

            EQs solver = new EQs();

            bool flag = solver.newton_solver(f, Jacoby, err, ref x);

            orbit.a = x[2];
            double dE2 = (x[0] - x[1])/2;

            double R = (r1 + r2) / 4 / orbit.a;
            double P = (r2 - r1) / 2 / orbit.a / Math.Sin(dE2);
            double Q = (1 - 2 * R) / Math.Cos(dE2);

            orbit.e = Math.Sqrt(P * P + Q * Q);

            return orbit;
        }

        private Matrix Jacoby(double[] x)
        {
            Matrix J = new Matrix(x.Length, x.Length);

            J.M[0, 0] =  1 - Math.Cos(x[0]);
            J.M[0, 1] = -1 + Math.Cos(x[1]);
            J.M[0, 2] = 3 * tau * Math.Pow(x[2], -2.5) / 2;

            J.M[1, 0] = -2 * Math.Sin(x[0]);
            J.M[1, 1] = 0;
            J.M[1, 2] = -2*(1 - Math.Cos(x[0]));

            J.M[2, 0] = 0;
            J.M[2, 1] = -2 * x[2] * Math.Sin(x[1]);
            J.M[2, 2] = -2 * (1 - Math.Cos(x[1]));

            return J;
        }

        private double[] f(double[] x)
        {
            double[] y = new double[x.Length];

            y[0] = x[0] - Math.Sin(x[0]) - x[1] + Math.Sin(x[1]) - tau * Math.Pow(x[2], -1.5);
            y[1] = r1 + r2 + s - 2 * x[2] * (1 - Math.Cos(x[0]));
            y[2] = r1 + r2 - s - 2 * x[2] * (1 - Math.Cos(x[1]));

            return y;
        }
    }
}
