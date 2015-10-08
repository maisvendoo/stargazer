public class SquareMatrix
{
    public double[,] create(int n)
    {
        double[,] m = new double[n, n];

        for (int i = 0; i < n; i++)
            for (int j = 0; i < n; j++)
                m[i, j] = 0;

        return m;
    }

    public double[,] Add(double[,] a, double[,] b)
    {
        if (a.Rank != b.Rank)
            return null;

        double[,] result = new double[a.Rank,a.Rank];

        for (int i = 0; i < a.Rank; i++)
            for (int j = 0; i < a.Rank; j++)
                result[i, j] = a[i,j] + b[i,j];
 
        return result;
    }

    public double[,] Sub(double[,] a, double[,] b)
    {
        if (a.Rank != b.Rank)
            return null;

        double[,] result = new double[a.Rank, a.Rank];

        for (int i = 0; i < a.Rank; i++)
            for (int j = 0; i < a.Rank; j++)
                result[i, j] = a[i, j] - b[i, j];

        return result;
    }
}