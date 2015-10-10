using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    public class Lambert
    {
        private static double  r1;
        private static double  r2;
        private static double  s;
        private static double  tau;
        
        //---------------------------------------------------------------------
        //      Get orbit by two positions x1 = r(t1), x2 = r(t2)
        //      r = r(t) - body radius-vector
        //---------------------------------------------------------------------
        public static bool get_orbit(Vector3D x1, 
                                     Vector3D x2, 
                                     double  t1, 
                                     double  t2, 
                                     double  mu, 
                                     ref Orbit orbit)
        {
            // Radius-vectors lenght calculation
            r1 = x1.lenght();
            r2 = x2.lenght();

            // Thue anomalies difference calculation
            double  dV = x1.angle(x2);

            // Distance calculation
            Vector3D dx = x1 - x2;
            s = dx.lenght();            

            // Lambert theorem parameter 
            tau = Math.Sqrt(mu) * (t2 - t1);

            // Newton solver input data
            double [] x = new double [3]{dV, dV, r1};
            double  eps = 1e-5;
            double [] err = new double [3] { eps, eps, eps };

            // Solve Lambert equations
            if (!EQs.newton_solver(f, Jacoby, err, ref x))
                return false;
            
            orbit.a = x[2];
            
            // Eccentricity calculation
            double  L = (x[0] - x[1])/2;
            double  R = (r1 + r2) / 4 / orbit.a;
            double  P = (r2 - r1) / 2 / orbit.a / Math.Sin(L);
            double  Q = (1 - 2 * R) / Math.Cos(L);
            
            orbit.e = Math.Sqrt(P * P + Q * Q);

            // Eccentric anomalies calculation
            double  sin_K = P / orbit.e;
            double  cos_K = Q / orbit.e;

            double  K = math.arg(sin_K, cos_K);         

            double  E1 = K - L;
            double  E2 = K + L;

            // Mean anomalies calculation
            double  M1 = E1 - orbit.e * Math.Sin(E1);
            double  M2 = E2 - orbit.e * Math.Sin(E2);

            // Mean anomaly at epoch calculation
            double  n = (M2 - M1) / (t2 - t1);
            double  n_test = Math.Sqrt(mu / orbit.a) / orbit.a;

            orbit.M0 = math.Trunc2PiN(M1 - n * t1);

            if (orbit.M0 < 0)
                orbit.M0 = 2 * Math.PI + orbit.M0;

            // True anomalies calculation
            double  sin_V1 = Math.Sqrt(1 - orbit.e * orbit.e) * Math.Sin(E1) / (1 - orbit.e * Math.Cos(E1));
            double  cos_V1 = (Math.Cos(E1) - orbit.e) / (1 - orbit.e * Math.Cos(E1));
            double  V1 = math.arg(sin_V1, cos_V1);

            double  V1_deg = V1 / math.RAD;

            double  sin_V2 = Math.Sqrt(1 - orbit.e * orbit.e) * Math.Sin(E2) / (1 - orbit.e * Math.Cos(E2));
            double  cos_V2 = (Math.Cos(E2) - orbit.e) / (1 - orbit.e * Math.Cos(E2));
            double  V2 = math.arg(sin_V2, cos_V2);

            double  V2_deg = V2 / math.RAD;

            // Orbit orientation calculation (high accuracity!!!)
            
            // Radius-vectors orts
            DVector3D ex1 = new DVector3D(Convert.ToDecimal(x1.ort().x), Convert.ToDecimal(x1.ort().y), Convert.ToDecimal(x1.ort().z));
            DVector3D ex2 = new DVector3D(Convert.ToDecimal(x2.ort().x), Convert.ToDecimal(x2.ort().y), Convert.ToDecimal(x2.ort().z));

            // Orbit plane normal
            DVector3D en = (ex1 & ex2).ort();           

            // Orbit inclination
            decimal i = DMath.acos(en.z);
            orbit.i = Convert.ToDouble(i) / math.RAD;

            // Accenting node longtitude
            decimal  sin_Omega = en.x / DMath.sin(i);
            decimal  cos_Omega = -en.y / DMath.sin(i);

            decimal Omega = DMath.arg(sin_Omega, cos_Omega);

            orbit.Omega = Convert.ToDouble(Omega) / math.RAD;

            // Argument of latitude
            decimal sin_u = ex1.z / DMath.sin(i);
            decimal cos_u = (ex1.x + sin_Omega * en.z * sin_u) / cos_Omega;

            decimal u = DMath.arg(sin_u, cos_u);

            // Argument of periapsis
            orbit.omega = (Convert.ToDouble(u) - V1) / math.RAD;            

            return true;
        }

        
        //---------------------------------------------------------------------
        //      Jacoby matrix of Lambert equation system
        //---------------------------------------------------------------------
        private static Matrix Jacoby(double [] x)
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


        //---------------------------------------------------------------------
        //      Lambert equation system
        //---------------------------------------------------------------------
        private static double [] f(double [] x)
        {
            double [] y = new double [x.Length];

            y[0] = x[0] - Math.Sin(x[0]) - x[1] + Math.Sin(x[1]) - tau * Math.Pow(x[2], -1.5);
            y[1] = r1 + r2 + s - 2 * x[2] * (1 - Math.Cos(x[0]));
            y[2] = r1 + r2 - s - 2 * x[2] * (1 - Math.Cos(x[1]));

            return y;
        }
    }
}
