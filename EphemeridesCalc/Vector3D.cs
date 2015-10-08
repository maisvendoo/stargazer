using System;

//-------------------------------------------------------------------
//
//-------------------------------------------------------------------
public struct Vector3D
{
    public double x;
    public double y;
    public double z;

    public Vector3D(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3D Add(Vector3D a, Vector3D b)
    {
        Vector3D result = new Vector3D(0, 0, 0);

        result.x = a.x + b.x;
        result.y = a.y + b.y;
        result.z = a.z + b.z;

        return result;
    }

    public static Vector3D operator +(Vector3D a, Vector3D b)
    {
        Vector3D result = new Vector3D(0, 0, 0);

        result.x = a.x + b.x;
        result.y = a.y + b.y;
        result.z = a.z + b.z;

        return result;
    }

    public Vector3D Sub(Vector3D a, Vector3D b)
    {
        Vector3D result = new Vector3D(0, 0, 0);

        result.x = a.x - b.x;
        result.y = a.y - b.y;
        result.z = a.z - b.z;

        return result;
    }

    public static Vector3D operator -(Vector3D a, Vector3D b)
    {
        Vector3D result = new Vector3D(0, 0, 0);

        result.x = a.x - b.x;
        result.y = a.y - b.y;
        result.z = a.z - b.z;

        return result;
    }

    public double lenght()
    {
        return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
    }

    public Vector3D get_ort()
    {
        Vector3D ort = new Vector3D(0, 0, 0);

        double len = this.lenght();

        if (len != 0)
        {
            ort.x = this.x / len;
            ort.y = this.y / len;
            ort.z = this.z / len;
        }

        return ort;
    }

    public double DotProduct(Vector3D a, Vector3D b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public double GetAngle(Vector3D a, Vector3D b)
    {
        return Math.Acos( DotProduct(a, b)/a.lenght()/b.lenght() );
    }

    public Vector3D Mul(Vector3D a, double lambda)
    {
        Vector3D result = new Vector3D(0, 0, 0);

        result.x = lambda * a.x;
        result.y = lambda * a.y;
        result.z = lambda * a.z;

        return result;
    }

    public Vector3D Div(Vector3D a, double lambda)
    {
        Vector3D result = new Vector3D(0, 0, 0);

        result.x =  a.x / lambda;
        result.y =  a.y / lambda;
        result.z =  a.z / lambda;

        return result;
    }

    public Vector3D CrossProduct(Vector3D a, Vector3D b)
    {
        Vector3D c = new Vector3D(0, 0, 0);

        c.x = a.y * b.z - a.z * b.y;
        c.y = a.z * b.x - a.x * b.z;
        c.z = a.x * b.y - a.y * b.z;

        return c;
    }

    public static Vector3D operator &(Vector3D a, Vector3D b)
    {
        Vector3D c = new Vector3D(0, 0, 0);

        c.x = a.y * b.z - a.z * b.y;
        c.y = a.z * b.x - a.x * b.z;
        c.z = a.x * b.y - a.y * b.z;

        return c;
    }
}

