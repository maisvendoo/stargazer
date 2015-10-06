public struct TOrbitData
{
    public double a;        // Orbit semimajor axis (m)
    public double e;        // Orbit eccentricity
    public double i;        // Orbit inclination (deg)
    public double Omega;    // Longitude of the ascending node (deg)
    public double omega;    // Periapsys argument (deg)
    public double M0;       // Middle anomaly (rad)
    public double t0;       // Epoch time (sec)    
}

public struct TPlanetState
{
    public double theta;    // True anomaly
    public double r;        // Radius-vector
    public double beta;     // Ecliptic latitude
    public double lambda;   // Ecliptic longtitude
}