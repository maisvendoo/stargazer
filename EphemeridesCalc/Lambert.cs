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
        
        public TOrbitData get_orbit(Vector3D x1, Vector3D x2, double t1, double t2, double mu)
        {
            TOrbitData orbit = new TOrbitData();
            Vector3D vec = new Vector3D();

            math mth = new math();

            r1 = x1.lenght();
            r2 = x2.lenght();

            double dV = x1.GetAngle(x2);

            Vector3D dx = x1 - x2;
            s = dx.lenght();            

            tau = Math.Sqrt(mu) * (t2 - t1);

            double[] x = new double[3]{dV, dV, r1};
            double eps = 1e-5;
            double[] err = new double[3] { eps, eps, eps };

            EQs solver = new EQs();

            bool flag = solver.newton_solver(f, Jacoby, err, ref x);

            orbit.a = x[2];
            double L = (x[0] - x[1])/2;

            double R = (r1 + r2) / 4 / orbit.a;
            double P = (r2 - r1) / 2 / orbit.a / Math.Sin(L);
            double Q = (1 - 2 * R) / Math.Cos(L);

            orbit.e = Math.Sqrt(P * P + Q * Q);

            /*double sin_K = P / orbit.e;
            double cos_K = Q / orbit.e;

            double K = mth.Arg(sin_K, cos_K);*/
            double K = Math.Atan(P / Q);

            double E1 = K - L;
            double E2 = K + L;

            double M1 = E1 - orbit.e * Math.Sin(E1);
            double M2 = E2 - orbit.e * Math.Sin(E2);

            double n = (M2 - M1) / (t2 - t1);
            double n_test = Math.Sqrt(mu / orbit.a) / orbit.a;

            orbit.M0 = M1 - n * t1;            

            double sin_V1 = Math.Sqrt(1 - orbit.e * orbit.e) * Math.Sin(E1) / (1 - orbit.e * Math.Cos(E1));
            double cos_V1 = (Math.Cos(E1) - orbit.e) / (1 - orbit.e * Math.Cos(E1));
            double V1 = mth.Arg(sin_V1, cos_V1);

            double V1_deg = V1 / CBody.RAD;

            double sin_V2 = Math.Sqrt(1 - orbit.e * orbit.e) * Math.Sin(E2) / (1 - orbit.e * Math.Cos(E2));
            double cos_V2 = (Math.Cos(E2) - orbit.e) / (1 - orbit.e * Math.Cos(E2));
            double V2 = mth.Arg(sin_V2, cos_V2);

            double V2_deg = V2 / CBody.RAD;

            Vector3D en = (x1 & x2).get_ort();

            orbit.i = Math.Acos(en.z);

            double i_deg = orbit.i / CBody.RAD;

            double sin_O = en.x / Math.Sin(orbit.i);
            double cos_O = -en.y / Math.Sin(orbit.i);

            orbit.Omega = mth.Arg(sin_O, cos_O);

            double Omega_deg = orbit.Omega / CBody.RAD;

            Vector3D er = x1.get_ort();

            double sin_u = er.z / Math.Sin(orbit.i);
            double cos_u = (er.x + sin_O * en.z * sin_u) / cos_O;

            double u = mth.Arg(sin_u, cos_u);

            orbit.omega = u - V1;

            double omega_deg = orbit.omega / CBody.RAD;

            return orbit;
        }

        private Matrix Jacoby(double[] x)
        {
            Matrix J = new Matrix(x.Length, x.Length);

            J.M[0, 0] =  1 - Math.Cos(x[0]);
            J.M[0, 1] = -1 + Math.Cos(x[1]);
            J.M[0, 2] = 3 * tau * Math.Pow(x[2], -2.5) / 2;

            J.M[1, 0] = -2 *x[2]* Math.Sin(x[0]);
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
