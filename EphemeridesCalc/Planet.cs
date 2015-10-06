using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    public class CPlanet
    {
        private const double KEPLER_EQ_ERROR = 1e-11;
        
        private const double muK = 1.1723328e18;
        private const double RAD = Math.PI / 180.0;

        private const int NEWTON = 0;
        private const int SIMPLE_ITER = 1;

        private int kepler_solver_method;
        
        //-----------------------------------------------------------
        //  Planet state calculation (position and velocity)
        //-----------------------------------------------------------
        public void get_planet_state(TOrbitData orbit_data, 
                                     double t,
                                     ref TPlanetState planet_state)
        {
            // Middle anomaly calculation
            double n = Math.Sqrt(muK / orbit_data.a) / orbit_data.a;

            double M = n * (t - orbit_data.t0) + orbit_data.M0;
            
            // Eccentric anomaly calculation (Kepler equation solve)
            double E = get_eccentric_anomaly(M, orbit_data.e, NEWTON);

            // Half of true anomaly tangens calculation
            double tgV2 = Math.Tan(E / 2)*Math.Sqrt((1 + orbit_data.e)/(1 - orbit_data.e));

            // True anomaly calculation
            double sin_V = Math.Sqrt(1 - orbit_data.e * orbit_data.e) * Math.Sin(E) / (1 - orbit_data.e * Math.Cos(E));
            double cos_V = (Math.Cos(E) - orbit_data.e) / (1 - orbit_data.e * Math.Cos(E));

            planet_state.theta = get_angle(sin_V, cos_V);

            // Planet radius-vector lenght calculation
            planet_state.r = orbit_data.a * (1 - orbit_data.e * Math.Cos(E));
            planet_state.h = planet_state.r - 261600000.0;

            // Ecliptic coordinates calculation
            planet_state.beta = Math.Asin(Math.Sin(orbit_data.i * RAD) * Math.Sin(orbit_data.omega*RAD + planet_state.theta));

            double Omega = orbit_data.Omega * RAD;
            double omega = orbit_data.omega * RAD;
            double i = orbit_data.i * RAD;
            double V = planet_state.theta;

            double sin_lambda = (sin(Omega) * cos(V + omega) + cos(Omega) * cos(i) * sin(V + omega)) / cos(planet_state.beta);
            double cos_lambda = (cos(Omega) * cos(V + omega) - sin(Omega) * cos(i) * sin(V + omega)) / cos(planet_state.beta);

            planet_state.lambda = get_angle(sin_lambda, cos_lambda);
        }



        //-----------------------------------------------------------
        //  Kepler equation solver
        //-----------------------------------------------------------
        private double get_eccentric_anomaly(double M, double e, int method)
        {
            double En_1 = M;
            double En = M;
            double eps = KEPLER_EQ_ERROR;

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
        private double get_angle(double sin_x, double cos_x)
        {
            double x = 0;

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
        private double Trunc2PiN(double x)
        {
            int N = Convert.ToInt32(Math.Truncate(x / 2 / Math.PI));

            return x - N*2*Math.PI;
        }



        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private double sin(double x)
        {
            return Math.Sin(x);
        }

        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private double cos(double x)
        {
            return Math.Cos(x);
        }

        //-----------------------------------------------------------
        //
        //-----------------------------------------------------------
        private double tg(double x)
        {
            return Math.Tan(x);
        }
    }
}
