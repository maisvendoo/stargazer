﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astronomy
{
    public struct Transfer
    {
        public Orbit orbit;
        public double depTime;
        public double arrivTime;
        public KDate depDate;
        public KDate arrivDate;
        public double destLambda;
        public double transTime;        
    }

    public struct DepManuever
    {
        public Orbit orbit;
        public double dv;
        public double h;
        public int turns;
        public double ejectTime;
        public KDate ejectDate;
        public double startTime;
        public KDate startDate;
        public double azimuth;
        public double launchLat;
        public double launchLon;
        public double launchTime;
        public KDate launchDate;
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
        //      Get true anomaly from ecliptic position 
        //      (code generated by Maple 18)
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

        //---------------------------------------------------------------------
        //      Simple trigonometry for previos method 
        //---------------------------------------------------------------------
        private static double sin(double x) { return Math.Sin(x); }
        private static double cos(double x) { return Math.Cos(x); }
        private static double pow(double x, double a) { return Math.Pow(x, a); }
        private static double sqrt(double x) { return Math.Sqrt(x); }
        
       
        //---------------------------------------------------------------------
        //      Get transfer orbit
        //---------------------------------------------------------------------
        public static double get_transfer_orbit(double t,
                                                CelestialBody depBody,
                                                CelestialBody arrivBody,
                                                double phi, 
                                                ref Orbit orbit,
                                                ref double destLambda)
        {
            // Is bodies has same reference body
            if (depBody.get_ref_body() != arrivBody.get_ref_body())
                return -1;
            
            OrbitPos pos = new OrbitPos();
            EclipticPos epos = new EclipticPos();            

            depBody.get_position(t, ref pos);
            depBody.get_ecliptic_coords(pos.theta, ref epos);
            destLambda = epos.lambda + Math.PI - phi;

            destLambda = math.Trunc2PiN(destLambda);

            double destTheta = get_dest_theta(arrivBody, destLambda);

            Vector3D x1 = depBody.get_cartesian_pos(pos.theta);
            Vector3D x2 = arrivBody.get_cartesian_pos(destTheta);

            double u = 0;
            get_transfer_orientation(x1, x2, ref orbit.i, ref orbit.Omega, ref u); 

            double r1 = x1.lenght();
            double r2 = x2.lenght();            

            orbit.omega = u / math.RAD;

            BodyData data = new BodyData();

            arrivBody.get_data(ref data);

            double transTime = 0;
            double mu = depBody.get_refGravParameter();            
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
        public static double LambdaErr(double t,
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
        public static bool get_transfer_date(double t0,
                                             double t1,
                                             CelestialBody depBody,
                                             CelestialBody arrivBody,
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
                dL0 = LambdaErr(t, depBody, arrivBody, psi);
                t += dt;
                dL1 = LambdaErr(t, depBody, arrivBody, psi);

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

                dL0 = LambdaErr(ta, depBody, arrivBody, psi);
                dL1 = LambdaErr(tc, depBody, arrivBody, psi);

                if (dL0 * dL1 >= 0)
                    ta = tc;
                else
                    tb = tc;

            } while (Math.Abs(dL1) >= eps);

            trans.depTime = tc;
            KCalendar.sec_to_date(tc, ref trans.depDate);
            
            trans.transTime = get_transfer_orbit(tc, depBody, arrivBody, psi, ref trans.orbit, ref trans.destLambda);

            trans.arrivTime = tc + trans.transTime;
            KCalendar.sec_to_date(trans.arrivTime, ref trans.arrivDate);

            return true;
        }

        //---------------------------------------------------------------------
        //      Transfer dV calculation
        //---------------------------------------------------------------------
        public static void get_depatrure_manuever(CelestialBody depBody,                                                 
                                               CelestialBody craft,
                                               double t1,                                               
                                               ref DepManuever manuever)
        {
            OrbitPos depPos = new OrbitPos();                     

            depBody.get_position(t1, ref depPos);

            // Calculation SOI departure velocity
            Vector3D Vd = depBody.get_velocity(depPos.theta);
            
            Vector3D Vc = craft.get_velocity(0);

            Vector3D Vro = Vc - Vd;

            double v_ro = Vro.lenght();

            // Depatrure hyperbolic orbit calculation
            Vector3D x1 = depBody.get_cartesian_pos(depPos.theta);

            double u = 0;

            get_transfer_orientation(x1, Vro, ref manuever.orbit.i, ref manuever.orbit.Omega, ref u);

            if (manuever.orbit.i > 90.0)
            {
                manuever.orbit.i = 180.0 - manuever.orbit.i;
                manuever.orbit.Omega += 180.0;

                if (manuever.orbit.Omega > 360)
                    manuever.orbit.Omega -= 360;
            }

            // Eject dV calculation
            BodyData depData = new BodyData();

            depBody.get_data(ref depData);

            double ro = depData.sphereOfInfluence;
            double R = depData.radius;
            double mu = depData.gravParameter;
            double h = manuever.h;

            double v0 = Math.Sqrt(2 * mu * (1 / (R + h) - 1 / ro) + v_ro * v_ro);
            manuever.dv = v0 - Math.Sqrt(mu / (R + h));

            // Time of hyperbolic departure calculation
            double vk = Math.Sqrt(mu / (R + h));
            manuever.orbit.e = (v0 * v0 / vk / vk - 1);
            double p = (R + h) * (1 + manuever.orbit.e);
            double cos_theta = (p / ro - 1) / manuever.orbit.e;
            double theta = Math.Acos(cos_theta);
            manuever.orbit.a = p / (manuever.orbit.e * manuever.orbit.e - 1);
            double n = Math.Sqrt(mu / manuever.orbit.a) / manuever.orbit.a;
            double thH2 = Math.Sqrt((manuever.orbit.e - 1) / (manuever.orbit.e + 1)) * Math.Tan(theta / 2);
            double H = Math.Log((1 + thH2) / (1 - thH2));
            double M = manuever.orbit.e * Math.Sinh(H) - H;
            double dT = M / n;

            manuever.orbit.omega = (Math.Acos(1/manuever.orbit.e) - Math.PI/2) / math.RAD;

            // Low orbit wait time calculation
            double waitTime = manuever.turns * 2 * Math.PI * (R + h) / vk;

            manuever.ejectTime = t1 - dT;
            manuever.startTime = t1 - dT - waitTime;

            KCalendar.sec_to_date(manuever.ejectTime, ref manuever.ejectDate);
            KCalendar.sec_to_date(manuever.startTime, ref manuever.startDate);
        } 
       


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public static bool get_launch_params(CelestialBody depBody,
                                             ref DepManuever manuever)
        {
            BodyData depData = new BodyData();
            depBody.get_data(ref depData);
            double mu = depData.gravParameter;
            double R = depData.radius;
            double r0 = R + manuever.h;           
                        
            double vk = Math.Sqrt(mu / r0);

            double i = manuever.orbit.i * math.RAD;
            double Omega = manuever.orbit.Omega * math.RAD;

            if (i < Math.Abs(manuever.launchLat))
                return false;

            double sin_A = Math.Cos(i)/Math.Cos(manuever.launchLat);
            double A = Math.Asin(sin_A);            
            
            double ny = -Math.Cos(A);
            double nz = sin_A;

            double D = ny * ny + nz * nz * Math.Pow(Math.Sin(manuever.launchLat), 2);
            double sin_d = -(nz * Math.Cos(Omega) * Math.Sin(manuever.launchLat) + ny * Math.Sin(Omega)) * Math.Sin(i) / D;
            double cos_d = (nz * Math.Sin(Omega) * Math.Sin(manuever.launchLat) - ny * Math.Cos(Omega)) * Math.Sin(i) / D;

            double d = math.arg(sin_d, cos_d);
            double rotAngle_ref = d - manuever.launchLon;

            double omega = 2 * Math.PI / depData.rotationPeriod;
            double rotAngle = depBody.get_rotation_angle(manuever.startTime)*math.RAD;

            double dT = 0;

            if (rotAngle >= rotAngle_ref)
            {
                dT = (rotAngle - rotAngle_ref) / omega; 
            }
            else
            {
                dT = 21600 - (rotAngle_ref - rotAngle) / omega;
            }

            manuever.launchTime = manuever.startTime - dT;
            KCalendar.sec_to_date(manuever.launchTime, ref manuever.launchDate);

            // Debug
            rotAngle = depBody.get_rotation_angle(manuever.launchTime)*math.RAD;

            // Relative azimuth calculation
            double vs = omega*R*Math.Cos(manuever.launchLat);
            double sin_Ar = (vk * sin_A - vs) / Math.Sqrt(vk * vk + vs * vs - 2 * vk * vs * sin_A);

            manuever.azimuth = Math.Asin(sin_Ar) / math.RAD;

            return true;
        }
    }
}
