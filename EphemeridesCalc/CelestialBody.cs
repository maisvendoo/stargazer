using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astronomy
{
    //-------------------------------------------------------------------------
    //
    //-------------------------------------------------------------------------
    public struct Orbit
    {
        public double a;            // Orbit semimajor axis (m)
        public double e;            // Orbit eccentricity
        public double i;            // Orbit inclination (deg)
        public double Omega;        // Longitude of the ascending node (deg)
        public double omega;        // Periapsys argument (deg)
        public double M0;           // Middle anomaly (rad)
        public double t0;           // Epoch time (sec)
        public double period;       // Orbital period
        public double RefRadius;    // Reference body radius (m)
    }



    //-------------------------------------------------------------------------
    //
    //-------------------------------------------------------------------------
    public struct OrbitPos
    {
        public double theta;       // True anomaly
        public double E;           // Eccentric anomaly
        public double M;           // Mean anomaly
        public double r;           // Radius-vector;  
        public double refAltitude; // Altitude over reference body surface
    }

    //-------------------------------------------------------------------------
    //
    //-------------------------------------------------------------------------
    public struct BodyData
    {
        public string name;
        public int id;
        public string refBody;
        public int refId;
        public double mass;
        public double radius;
        public double gravParameter;
        public double rotationPeriod;

        public Orbit orbit;        
    }

    public struct EclipticPos
    {
        public double beta;
        public double lambda;
    }    
    
    public class CelestialBody
    {
        private BodyData data;

        private const double KEPLER_EQ_ERROR = 1e-11;
        private const int NEWTON = 0;
        private const int SIMPLE_ITER = 1;

        public void get_data(ref BodyData data)
        {
            data.name = this.data.name;
            data.id = this.data.id;
            data.refBody = this.data.refBody;
            data.mass = this.data.mass;
            data.radius = this.data.radius;
            data.gravParameter = this.data.gravParameter;
            data.rotationPeriod = this.data.rotationPeriod;

            data.orbit.a = this.data.orbit.a;
            data.orbit.e = this.data.orbit.e;
            data.orbit.M0 = this.data.orbit.M0;
            data.orbit.period = this.data.orbit.period;
            data.orbit.t0 = this.data.orbit.t0;
            data.orbit.RefRadius = this.data.orbit.RefRadius;
            data.orbit.omega = this.data.orbit.omega;
            data.orbit.Omega = this.data.orbit.Omega;
            data.orbit.i = this.data.orbit.i;
        }

        public void set_data(BodyData data)
        {
            this.data.name = data.name;
            this.data.id = data.id;
            this.data.refBody = data.refBody;
            this.data.mass = data.mass;
            this.data.radius = data.radius;
            this.data.gravParameter = data.gravParameter;
            this.data.rotationPeriod = data.rotationPeriod;

            this.data.orbit.a = data.orbit.a;
            this.data.orbit.e = data.orbit.e;
            this.data.orbit.M0 = data.orbit.M0;
            this.data.orbit.period = data.orbit.period;
            this.data.orbit.t0 = data.orbit.t0;
            this.data.orbit.RefRadius = data.orbit.RefRadius;
            this.data.orbit.omega = data.orbit.omega;
            this.data.orbit.Omega = data.orbit.Omega;
            this.data.orbit.i = data.orbit.i;
        }        

        public void get_position(double t, ref OrbitPos pos)
        {
            // Middle anomaly calculation
            double n = 2 * Math.PI / data.orbit.period;

            double M = n * (t - data.orbit.t0) + data.orbit.M0;            

            // Eccentric anomaly calculation (Kepler equation solve)
            double E = get_eccentric_anomaly(M, data.orbit.e, NEWTON);

            pos.E = math.Trunc2PiN(E);
            pos.r = data.orbit.a * (1 - data.orbit.e * Math.Cos(E));
            pos.refAltitude = pos.r - data.orbit.RefRadius;

            // True anomaly calculation
            double sin_V = Math.Sqrt(1 - data.orbit.e * data.orbit.e) * Math.Sin(E) / (1 - data.orbit.e * Math.Cos(E));
            double cos_V = (Math.Cos(E) - data.orbit.e) / (1 - data.orbit.e * Math.Cos(E));

            pos.theta = math.arg(sin_V, cos_V);
        }

        //---------------------------------------------------------------------
        //  Kepler equation solver
        //---------------------------------------------------------------------
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



        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public string get_ref_body()
        {
            return data.refBody;
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public void set_refId(int refId)
        {
            data.refId = refId;
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public double get_radius()
        {
            return data.radius;
        }

        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public double get_gravParameter()
        {
            return data.gravParameter;
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public void set_refRadius(double refRadius)
        {
            data.orbit.RefRadius = refRadius;
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public void get_ecliptic_coords(double theta, ref EclipticPos ecoords)
        {
            double Omega = data.orbit.Omega * math.RAD;
            double omega = data.orbit.omega * math.RAD;
            double i = data.orbit.i * math.RAD;
            double V = theta;

            ecoords.beta = Math.Asin(Math.Sin(i) * Math.Sin(omega + V));

            double sin_lambda = (Math.Sin(Omega) * Math.Cos(V + omega) + Math.Cos(Omega) * Math.Cos(i) * Math.Sin(V + omega)) / Math.Cos(ecoords.beta);
            double cos_lambda = (Math.Cos(Omega) * Math.Cos(V + omega) - Math.Sin(Omega) * Math.Cos(i) * Math.Sin(V + omega)) / Math.Cos(ecoords.beta);

            ecoords.lambda = math.arg(sin_lambda, cos_lambda);
        }


        public Vector3D get_cartesian_pos(double theta)
        {
            double r = data.orbit.a * (1 - data.orbit.e * data.orbit.e) / (1 + data.orbit.e * Math.Cos(theta));

            EclipticPos epos = new EclipticPos();

            get_ecliptic_coords(theta, ref epos);

            double x = r * Math.Cos(epos.beta) * Math.Cos(epos.lambda);
            double y = r * Math.Cos(epos.beta) * Math.Sin(epos.lambda); ;
            double z = r * Math.Sin(epos.beta);

            return new Vector3D(x, y, z);
        }        
    }
}
