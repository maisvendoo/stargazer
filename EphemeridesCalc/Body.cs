using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    public struct TEclipticCoords
    {
        public double  beta;
        public double  lambda;
    }
    
    public class CBody
    {
        private const double  KEPLER_EQ_ERROR = 1e-11;
        
        private const double  muK = 1.1723328e18;
        public const double  RAD = Math.PI / 180.0;

        private const int NEWTON = 0;
        private const int SIMPLE_ITER = 1;        
        
        //-----------------------------------------------------------
        //  Planet state calculation (position and velocity)
        //-----------------------------------------------------------
        public void get_body_state(TOrbitData orbit_data, 
                                     double  t,
                                     ref TBodyState body_state)
        {
            // Middle anomaly calculation
            double  n = 2 * Math.PI / orbit_data.period;

            double  M = n * (t - orbit_data.t0) + orbit_data.M0;
            
            // Eccentric anomaly calculation (Kepler equation solve)
            double  E = get_eccentric_anomaly(M, orbit_data.e, NEWTON);

            body_state.E = Trunc2PiN(E);

            // Half of true anomaly tangens calculation
            double  tgV2 = Math.Tan(E / 2)*Math.Sqrt((1 + orbit_data.e)/(1 - orbit_data.e));

            // True anomaly calculation
            double  sin_V = Math.Sqrt(1 - orbit_data.e * orbit_data.e) * Math.Sin(E) / (1 - orbit_data.e * Math.Cos(E));
            double  cos_V = (Math.Cos(E) - orbit_data.e) / (1 - orbit_data.e * Math.Cos(E));

            body_state.theta = get_angle(sin_V, cos_V);

            // Planet radius-vector lenght calculation
            body_state.r = orbit_data.a * (1 - orbit_data.e * Math.Cos(E));
            body_state.h = body_state.r - orbit_data.RefRadius;

            // Ecliptic coordinates calculation
            TEclipticCoords ecoords = new TEclipticCoords();

            get_ecliptic_coords(orbit_data, body_state.theta, ref ecoords);

            body_state.beta = ecoords.beta;
            body_state.lambda = ecoords.lambda;            
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        public void get_ecliptic_coords(TOrbitData orbit_data, double  theta, ref TEclipticCoords ecoords)
        {
            double  Omega = orbit_data.Omega * RAD;
            double  omega = orbit_data.omega * RAD;
            double  i = orbit_data.i * RAD;
            double  V = theta;

            ecoords.beta = Math.Asin(sin(i)*sin(omega + V));

            double  sin_lambda = (sin(Omega) * cos(V + omega) + cos(Omega) * cos(i) * sin(V + omega)) / cos(ecoords.beta);
            double  cos_lambda = (cos(Omega) * cos(V + omega) - sin(Omega) * cos(i) * sin(V + omega)) / cos(ecoords.beta);

            ecoords.lambda = get_angle(sin_lambda, cos_lambda);
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        public void get_coords(TOrbitData orbit_data, double  theta, ref Vector3D coords)
        {
            double  p = orbit_data.a * (1 - orbit_data.e * orbit_data.e);
            double  r = p / (1 + orbit_data.e * cos(theta));

            TEclipticCoords ecoords = new TEclipticCoords();

            get_ecliptic_coords(orbit_data, theta, ref ecoords);

            coords.x = r * cos(ecoords.beta) * cos(ecoords.lambda);
            coords.y = r * cos(ecoords.beta) * sin(ecoords.lambda);
            coords.z = r * sin(ecoords.beta);
        }


        //-----------------------------------------------------------
        //  Kepler equation solver
        //-----------------------------------------------------------
        private double  get_eccentric_anomaly(double  M, double  e, int method)
        {
            double  En_1 = M;
            double  En = M;
            double  eps = KEPLER_EQ_ERROR;

            do
            {
                En_1 = En;

                switch (method)
                {
                    case SIMPLE_ITER:
                    {
                        En = M + eps * Math.Sin(En_1);
                        break;
                    }

                    case NEWTON:
                    {
                        En = En_1 - (En_1 - e * Math.Sin(En_1) - M) / (1 - e * Math.Cos(En_1));
                        break;
                    }
                }

            } while (Math.Abs(En - En_1) >= eps);

            return En;
        }



        //-----------------------------------------------------------
        //  Get angle by sinus and cosinus
        //-----------------------------------------------------------
        private double  get_angle(double  sin_x, double  cos_x)
        {
            double  x = 0;

            if (sin_x >= 0)
            {
                x = Math.Acos(cos_x);
            }
            else
            {
                x = 2 * Math.PI - Math.Acos(cos_x);
            }

            return x;
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private double  Trunc2PiN(double  x)
        {
            int N = Convert.ToInt32(Math.Truncate(x / 2 / Math.PI));

            return x - N*2*Math.PI;
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private double  sin(double  x)
        {
            return Math.Sin(x);
        }

        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private double  cos(double  x)
        {
            return Math.Cos(x);
        }

        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private double  tg(double  x)
        {
            return Math.Tan(x);
        }
    }
}
