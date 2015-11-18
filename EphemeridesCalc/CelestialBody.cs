using System;

namespace Astronomy
{
    //-------------------------------------------------------------------------
    //      Orbit elemnts
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
    //      Body orbital position
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
    //      Common body data
    //-------------------------------------------------------------------------
    public struct BodyData
    {
        public string name;                 // Body's name
        public int id;                      // Body's id 
        public string refBody;              // Referense body name  
        public int refId;                   // Referense body id
        public double mass;                 // Body's mass
        public double radius;               // Equatorial radius
        public double gravParameter;        // mu = G*M (G = 6.67e-11 - constatnt of gravity, M - body's mass)    
        public double refGravParameter;     // Reference body mu
        public double rotationPeriod;       // Own axis rotation period
        public double sphereOfInfluence;    // SOI radius
        public double initialRotation;      // Initial angle of rotation  (at t = 1y 1d 0h 0m 0s)

        public Orbit orbit;                 // Orbit
    }

    public struct EclipticPos
    {
        public double beta;                 // Ecliptic latitude
        public double lambda;               // Ecliptic longtitude
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
            data.refGravParameter = this.data.refGravParameter;
            data.rotationPeriod = this.data.rotationPeriod;
            data.sphereOfInfluence = this.data.sphereOfInfluence;
            data.initialRotation = this.data.initialRotation;

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

        public void set_data(ref BodyData data)
        {
            this.data.name = data.name;
            this.data.id = data.id;
            this.data.refBody = data.refBody;
            this.data.mass = data.mass;
            this.data.radius = data.radius;
            this.data.gravParameter = data.gravParameter;
            this.data.refGravParameter = data.refGravParameter;
            this.data.rotationPeriod = data.rotationPeriod;
            this.data.sphereOfInfluence = data.sphereOfInfluence;
            this.data.initialRotation = data.initialRotation;

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
            double mu = data.refGravParameter;
            double e = data.orbit.e;

            // Elliptic orbit
            if ((e > -1) && (e < 1))
            {
                // Middle anomaly calculation
                double n = Math.Sqrt(mu / data.orbit.a) / data.orbit.a;
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

            // Parabolic orbit
            if (e == 1)
            {
                // Middle anomaly calculation (a - is periapsis radius!!!)
                double n = Math.Sqrt(mu / 2 / data.orbit.a) / data.orbit.a;
                double M = n * (t - data.orbit.t0) + data.orbit.M0;

                // Barker equation solution
                double x = 12 * M + 4 * Math.Sqrt(9 * M * M + 4);
                double S = 0.5 * Math.Pow(x, 1 / 3) - 2/Math.Pow(x, 1/3);

                double sin_V = 2 * S / (1 + S * S);
                double cos_V = (1 - S * S) / (1 + S * S);

                pos.theta = math.arg(sin_V, cos_V);
                pos.r = 2 * data.orbit.a/(1 + Math.Cos(pos.theta));
                pos.refAltitude = pos.r - data.orbit.RefRadius;
            }

            // Hyperbolic orbit
            if (e > 1)
            {
                // Middle anomaly calculation
                double n = Math.Sqrt(mu / data.orbit.a) / data.orbit.a;
                double M = n * (t - data.orbit.t0) + data.orbit.M0;

                // Eccentric anomaly calculation (Kepler equation solve)
                double E = get_eccentric_anomaly(M, data.orbit.e, NEWTON);

                pos.E = E;
                pos.r = data.orbit.a * (data.orbit.e * Math.Cosh(E) - 1);
                pos.refAltitude = pos.r - data.orbit.RefRadius;

                double sin_V = Math.Sqrt(e * e - 1) * Math.Sinh(E) / (e * Math.Cosh(E) - 1);
                double cos_V = (e - Math.Cosh(E)) / (e * Math.Cosh(E) - 1);

                pos.theta = math.arg(sin_V, cos_V);
            }
        }

        //---------------------------------------------------------------------
        //  Kepler equation solver for elliptic and hyperbolic orbit
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
                            if (e < 1)
                            {
                                En = M + e * Math.Sin(En_1);
                            }
                            else
                            {
                                double x = (En_1 + M) / e;
                                En = Math.Log(Math.Sqrt(x*x + 1) + x);
                            }

                            break;
                        }

                    case NEWTON:
                        {
                            if (e < 1)
                                En = En_1 - (En_1 - e * Math.Sin(En_1) - M) / (1 - e * Math.Cos(En_1));
                            else
                                En = En_1 - (e * Math.Sinh(En_1) - En_1 - M) / (e * Math.Cosh(En_1) - 1);
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
        public void set_refGravParameter(double mu)
        {
            data.refGravParameter = mu;
        }

        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public double get_refGravParameter()
        {
            return data.refGravParameter;
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

        //---------------------------------------------------------------------
        //      Cartesian system position of body
        //---------------------------------------------------------------------
        public Vector3D get_cartesian_pos(double theta)
        {
            double r = 0;
            double e = data.orbit.e;            
            
            if ( (e > -1) && (e < 1) )
                r = data.orbit.a * (1 - e * e) / (1 + e * Math.Cos(theta));

            if (e == 1)
                r = 2 * data.orbit.a / (1 + e * Math.Cos(theta));

            if (e > 1)
                r = data.orbit.a * (e*e - 1) / (1 + e * Math.Cos(theta));

            EclipticPos epos = new EclipticPos();

            get_ecliptic_coords(theta, ref epos);

            double x = r * Math.Cos(epos.beta) * Math.Cos(epos.lambda);
            double y = r * Math.Cos(epos.beta) * Math.Sin(epos.lambda); ;
            double z = r * Math.Sin(epos.beta);

            return new Vector3D(x, y, z);
        }


        //---------------------------------------------------------------------
        //      Get body velocity vector
        //---------------------------------------------------------------------
        public Vector3D get_velocity(double theta)
        {
            double vx = 0;
            double vy = 0;
            double vz = 0;

            double e = data.orbit.e;
            double p = 0;

            if ((e > -1) && (e < 1))
                p = data.orbit.a * (1 - e * e);

            if (e == 1)
                p = 2*data.orbit.a;

            if (e > 1)
                p = data.orbit.a * (e * e - 1);

            double mu = data.refGravParameter;
            double vp = Math.Sqrt(mu / p);
            double vr = e * Math.Sin(theta) * vp;
            double vt = (1 + e*Math.Cos(theta))*vp;

            double i = data.orbit.i * math.RAD;
            double Omega = data.orbit.Omega * math.RAD;
            double omega = data.orbit.omega * math.RAD;

            vx = vr * (Math.Cos(Omega) * Math.Cos(omega + theta) - Math.Sin(Omega) * Math.Cos(i) * Math.Sin(omega + theta)) -
                 vt * (Math.Cos(Omega) * Math.Sin(omega + theta) + Math.Sin(Omega) * Math.Cos(i) * Math.Cos(omega + theta));

            vy = vr * (Math.Sin(Omega) * Math.Cos(omega + theta) + Math.Cos(Omega) * Math.Cos(i) * Math.Sin(omega + theta)) -
                 vt * (Math.Sin(Omega) * Math.Sin(omega + theta) - Math.Cos(Omega) * Math.Cos(i) * Math.Cos(omega + theta));

            vz = Math.Sin(i) * (vr * Math.Sin(omega + theta) + vt * Math.Cos(omega + theta));

            return new Vector3D(vx, vy, vz);
        }


        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public double get_motion_time(double theta0, double theta1)
        {
            double dT = 0;
            double mu = data.refGravParameter;

            double e = data.orbit.e;
            double a = data.orbit.a;

            if ((e > -1) && (e < 1))
            {
                double n = Math.Sqrt(mu / a) / a;
                double E0 = 2 * Math.Atan(Math.Tan(theta0 / 2) * Math.Sqrt((1 - e) / (1 + e)));
                double E1 = 2 * Math.Atan(Math.Tan(theta1 / 2) * Math.Sqrt((1 - e) / (1 + e)));
                double M = E1 - E0 - e * (Math.Sin(E1) - Math.Sin(E0));
                dT = M / n;
            }

            if (e == 1)
            {
                double n = Math.Sqrt(mu / 2 / a) / a;
                double M = Math.Tan(theta1 / 2) - Math.Tan(theta0 / 2) + 
                          (Math.Pow(Math.Tan(theta1 / 2), 3) - Math.Pow(Math.Tan(theta0 / 2), 3)) / 3;
                dT = M / n;
            }

            if (e > 1)
            {
                double n = Math.Sqrt(mu / a) / a;
                double E0 = 2 * math.Atanh(Math.Tan(theta0 / 2) * Math.Sqrt((e - 1) / (1 + e)));
                double E1 = 2 * math.Atanh(Math.Tan(theta1 / 2) * Math.Sqrt((e - 1) / (1 + e)));
                double M = e * (Math.Sinh(E1) - Math.Sinh(E0)) + E0 - E1;
                dT = M / n;
            }

            return dT;
        }

        //---------------------------------------------------------------------
        //
        //---------------------------------------------------------------------
        public double get_rotation_angle(double t)
        {
            double omega = 2 * Math.PI / data.rotationPeriod;

            return math.Trunc2PiN(data.initialRotation * math.RAD + omega * t) / math.RAD;
        }
    }
}
