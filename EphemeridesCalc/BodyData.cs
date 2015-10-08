public struct TOrbitData
{
    public double  a;            // Orbit semimajor axis (m)
    public double  e;            // Orbit eccentricity
    public double  i;            // Orbit inclination (deg)
    public double  Omega;        // Longitude of the ascending node (deg)
    public double  omega;        // Periapsys argument (deg)
    public double  M0;           // Middle anomaly (rad)
    public double  t0;           // Epoch time (sec)
    public double  period;       // Orbital period
    public double  RefRadius;    // Reference body radius (m)
}

public struct TBodyState
{
    public double  theta;    // True anomaly
    public double  E;        // Eccentric anomaly
    public double  r;        // Radius-vector
    public double  h;        // Altitude
    public double  beta;     // Ecliptic latitude
    public double  lambda;   // Ecliptic longtitude    
}

public struct TBodyData
{
    public TOrbitData orbit;
    public string name;
    public int id;
    public string refBody;
    public double  mass;
    public double  radius;
    public double  gravParameter;
    public double  rotationPeriod;
}