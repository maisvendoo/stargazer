using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astronomy
{
    public struct Transfer
    {
        public Orbit orbit;
        public double arivTime;
        public double depTime;
        public KDate arivDate;
        public KDate depDate;
        public double destLambda;
        public double transTime;        
    }

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
            double  eps = 1e-3;
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



        //----------------------------------------------------------------------
        //
        //----------------------------------------------------------------------
        public static double get_dest_theta(CelestialBody body, double lambda)
        {
            BodyData data = new BodyData();

            body.get_data(ref data);

            double i = data.orbit.i * math.RAD;
            double Omega = data.orbit.Omega * math.RAD;
            

            double cos_u = sqrt(pow(cos(i), 0.2e1) * (0.2e1 * pow(cos(lambda), 0.2e1) * pow(cos(Omega), 0.2e1) - 
                 0.2e1 * cos(Omega) * cos(lambda) * sin(lambda) * sin(Omega) - 
                 pow(cos(lambda), 0.2e1) - pow(cos(Omega), 0.2e1) + 0.1e1) / (pow(cos(lambda), 0.4e1) * pow(cos(i), 0.2e1) + 
                 0.2e1 * pow(cos(lambda), 0.2e1) * pow(cos(i), 0.2e1) * pow(cos(Omega), 0.2e1) + 
                 pow(cos(i), 0.2e1) * pow(cos(Omega), 0.4e1) - pow(cos(lambda), 0.4e1) - 
                 0.2e1 * pow(cos(lambda), 0.2e1) * pow(cos(i), 0.2e1) - 0.2e1 * cos(Omega) * cos(lambda) * sin(lambda) * sin(Omega) - 
                 0.2e1 * pow(cos(i), 0.2e1) * pow(cos(Omega), 0.2e1) - pow(cos(Omega), 0.4e1) + pow(cos(lambda), 0.2e1) + pow(cos(i), 0.2e1) + 
                 pow(cos(Omega), 0.2e1))) * (cos(lambda) * cos(Omega) + sin(lambda) * sin(Omega));

            double sin_u = -sqrt(pow(cos(i), 0.2e1) * (0.2e1 * pow(cos(lambda), 0.2e1) * pow(cos(Omega), 0.2e1) - 
                   0.2e1 * cos(Omega) * cos(lambda) * sin(lambda) * sin(Omega) - pow(cos(lambda), 0.2e1) - 
                   pow(cos(Omega), 0.2e1) + 0.1e1) / (pow(cos(lambda), 0.4e1) * pow(cos(i), 0.2e1) + 
                   0.2e1 * pow(cos(lambda), 0.2e1) * pow(cos(i), 0.2e1) * pow(cos(Omega), 0.2e1) + 
                   pow(cos(i), 0.2e1) * pow(cos(Omega), 0.4e1) - pow(cos(lambda), 0.4e1) - 
                   0.2e1 * pow(cos(lambda), 0.2e1) * pow(cos(i), 0.2e1) - 0.2e1 * cos(Omega) * cos(lambda) * sin(lambda) * sin(Omega) - 
                   0.2e1 * pow(cos(i), 0.2e1) * pow(cos(Omega), 0.2e1) - pow(cos(Omega), 0.4e1) + 
                   pow(cos(lambda), 0.2e1) + pow(cos(i), 0.2e1) + 
                   pow(cos(Omega), 0.2e1))) * (cos(lambda) * sin(Omega) - cos(Omega) * sin(lambda)) / cos(i);


            double u = math.arg(sin_u, cos_u);

            return u - data.orbit.omega*math.RAD;
        }

        private static double sin(double x) { return Math.Sin(x); }
        private static double cos(double x) { return Math.Cos(x); }
        private static double pow(double x, double a) { return Math.Pow(x, a); }
        private static double sqrt(double x) { return Math.Sqrt(x); }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        private static double g(CelestialBody body, double destLambda, double theta)
        {
            EclipticPos epos = new EclipticPos();

            body.get_ecliptic_coords(theta, ref epos);

            return destLambda - epos.lambda;
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        private static double dgdV(CelestialBody body, double destLambda, double theta)
        {
            double h = 1e-3;
            double g0 = g(body, destLambda, theta);
            double g1 = g(body, destLambda, theta + h);

            return (g1 - g0)/h;
        }



        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public static double get_transfer_orbit(double t,
                                                CelestialBody arivBody,
                                                CelestialBody destBody,
                                                double phi, 
                                                ref Orbit orbit,
                                                ref double destLambda)
        {
            if (arivBody.get_ref_body() != destBody.get_ref_body())
                return -1;
            
            OrbitPos pos = new OrbitPos();
            EclipticPos epos = new EclipticPos();            

            arivBody.get_position(t, ref pos);
            arivBody.get_ecliptic_coords(pos.theta, ref epos);
            destLambda = epos.lambda + Math.PI - phi;

            destLambda = math.Trunc2PiN(destLambda);

            double destTheta = get_dest_theta(destBody, destLambda);

            Vector3D x1 = arivBody.get_cartesian_pos(pos.theta);
            Vector3D x2 = destBody.get_cartesian_pos(destTheta);

            double u = 0;
            get_transfer_orientation(x1, x2, ref orbit.i, ref orbit.Omega, ref u); 

            double r1 = x1.lenght();
            double r2 = x2.lenght();            

            orbit.omega = u / math.RAD;

            BodyData data = new BodyData();

            destBody.get_data(ref data);

            double transTime = 0;
            double mu = 4 * Math.PI * Math.PI * Math.Pow(data.orbit.a, 3) / data.orbit.period / data.orbit.period;
            double E = 0;
            double theta = 0;
            
            
            theta = x1.angle(x2);
            orbit.e = (r2 - r1) / (r1 - r2 * Math.Cos(theta));

            if ( (orbit.e > - 1) && (orbit.e < 1) )
            {
                orbit.a = r1 / (1 - orbit.e);

                double n = Math.Sqrt(mu / orbit.a) / orbit.a;
                double tgE2 = Math.Sqrt((1 - orbit.e) / (1 + orbit.e)) * Math.Tan(theta / 2);
                E = 2 * Math.Atan(tgE2);
                double M = E - orbit.e * Math.Sin(E);
                transTime = M / n;
            }
            
            if (orbit.e == 1)
            {
                orbit.a = 2*r1;
                transTime = r1*Math.Sqrt(2*r1/mu)*(Math.Tan(theta/2) + Math.Pow(Math.Tan(theta/2), 3)/3); 
            }

            if (orbit.e > 1)
            {
                orbit.a = r1 / (orbit.e - 1);

                double n = Math.Sqrt(mu / orbit.a) / orbit.a;
                double thE2 = Math.Sqrt((orbit.e - 1) / (orbit.e + 1)) * Math.Tan(theta / 2);
                double H = Math.Log((1 + thE2) / (1 - thE2));
                double M = orbit.e * Math.Sinh(H) - H;
                transTime = M / n;
            }

            return transTime;
        }

        private static void get_transfer_orientation(Vector3D x1, Vector3D x2, 
                                                       ref double i,
                                                       ref double Omega,
                                                       ref double u)
        {
            // Radius-vectors orts
            DVector3D ex1 = new DVector3D(Convert.ToDecimal(x1.ort().x), Convert.ToDecimal(x1.ort().y), Convert.ToDecimal(x1.ort().z));
            DVector3D ex2 = new DVector3D(Convert.ToDecimal(x2.ort().x), Convert.ToDecimal(x2.ort().y), Convert.ToDecimal(x2.ort().z));

            // Orbit plane normal
            DVector3D en = (ex1 & ex2).ort();

            // Orbit inclination
            decimal inc = DMath.acos(en.z);
            i = Convert.ToDouble(inc) / math.RAD;

            // Accenting node longtitude
            decimal sin_Omega = en.x / DMath.sin(inc);
            decimal cos_Omega = -en.y / DMath.sin(inc);

            decimal Omg = DMath.arg(sin_Omega, cos_Omega);

            Omega = Convert.ToDouble(Omg) / math.RAD;

            // Argument of latitude
            decimal sin_u = ex1.z / DMath.sin(inc);
            decimal cos_u = (ex1.x + sin_Omega * en.z * sin_u) / cos_Omega;

            u = Convert.ToDouble(DMath.arg(Decimal.Round(sin_u, 4), Decimal.Round(cos_u, 4)));
        }

        
        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public static double Phi(double t,
                                     CelestialBody arivBody,
                                     CelestialBody destBody,
                                     double phi)
        {
            Orbit orbit = new Orbit();

            double destLambda = 0;
            double transTime = get_transfer_orbit(t, arivBody, destBody, phi, ref orbit, ref destLambda);

            OrbitPos pos = new OrbitPos();
            EclipticPos epos = new EclipticPos();

            destBody.get_position(t + transTime, ref pos);
            destBody.get_ecliptic_coords(pos.theta, ref epos);
            
            return get_phase(epos.lambda - destLambda);
        }



        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public static double get_phase(double angle)
        {
            if (Math.Abs(angle) <= Math.PI)
            {
                return angle;
            }
            else
                return angle - 2 * Math.PI*Math.Sign(angle);
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public static double dDLmabdadt(double t,
                                     CelestialBody arivBody,
                                     CelestialBody destBody,
                                     double psi)
        {
            double h = 1.0*21600.0;

            double dL0 = Phi(t, arivBody, destBody, psi);
            double dL1 = Phi(t + h, arivBody, destBody, psi);

            return (dL1 - dL0)/h;
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public static bool get_transfer_date(double t0,
                                             double t1,
                                             CelestialBody arivBody,
                                             CelestialBody destBody,
                                             double psi,
                                             ref Transfer trans)
        {
            double t = t0;
            double dL0;
            double dL1;
            double eps = 1e-8;
            double dt = 21600.0;

            bool ready = false;            

            do
            {
                dL0 = Phi(t, arivBody, destBody, psi);
                t += dt;
                dL1 = Phi(t, arivBody, destBody, psi);

                if ((dL0 * dL1 < 0) && (Math.Abs(dL1) < 10.0 * math.RAD))
                    ready = true;
                else
                    ready = false;

            } while ( (!ready) && (t <= t1) );

            if (t > t1)
                return false;
            
            double ta = t - dt;
            double tb = t;
            double tc = 0;

            do
            {
                tc = (ta + tb) / 2;

                dL0 = Phi(ta, arivBody, destBody, psi);
                dL1 = Phi(tc, arivBody, destBody, psi);

                if (dL0 * dL1 >= 0)
                    ta = tc;
                else
                    tb = tc;

            } while (Math.Abs(dL1) >= eps);

            trans.arivTime = tc;
            KCalendar.sec_to_date(tc, ref trans.arivDate);
            
            trans.transTime = get_transfer_orbit(tc, arivBody, destBody, psi, ref trans.orbit, ref trans.destLambda);

            trans.depTime = tc + trans.transTime;
            KCalendar.sec_to_date(trans.depTime, ref trans.depDate);

            return true;
        }
    }
}
