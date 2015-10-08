public class Matrix
{
    public double [,] M;
    
    public int rows;
    public int columns;

    //-------------------------------------------------------------------------
    //      Matrix constructor
    //-------------------------------------------------------------------------
    public Matrix(int n, int m)
    {
        M = new double [n, m];

        rows = n;
        columns = m;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                M[i, j] = 0;
            }
        }
    }

    public static Matrix operator +(Matrix a, Matrix b)
    {
        Matrix c = new Matrix(a.rows, a.columns);

        for (int i = 0; i < a.rows; i++)
        {
            for (int j = 0; j < a.columns; j++)
                c.M[i, j]= a.M[i, j] + b.M[i, j];
        }

        return c;
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        Matrix c = new Matrix(a.rows, a.columns);

        for (int i = 0; i < a.rows; i++)
        {
            for (int j = 0; j < a.columns; j++)
                c.M[i, j] = a.M[i, j] - b.M[i, j];
        }

        return c;
    }


    public static Matrix operator *(Matrix a, double  lambda)
    {
        Matrix c = new Matrix(a.rows, a.columns);

        for (int i = 0; i < a.rows; i++)
        {
            for (int j = 0; j < a.columns; j++)
                c.M[i, j] = a.M[i, j]*lambda;
        }

        return c;
    }
}