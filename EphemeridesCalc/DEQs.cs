using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EphemeridesCalc
{
    class DEQs
    {
        public delegate decimal[] FuncDelegate(decimal[] x);
        public delegate DMatrix JacobyDelegate(decimal[] x);

        private const int NEWTON_ITER_MAX = 100000;

        //---------------------------------------------------------------------
        //      Gauss method solver
        //---------------------------------------------------------------------
        public static decimal[] gauss_solver(DMatrix A, decimal[] b)
        {
            int n = A.rows;
            decimal[] x = new decimal[n];
            decimal c = 0;

            // Forward (A to uptriangle form)
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i; j < n - 1; j++)
                {
                    c = A.M[j + 1, i] / A.M[i, i];

                    for (int k = 0; k < n; k++)
                        A.M[j + 1, k] = A.M[j + 1, k] - c * A.M[i, k];

                    b[j + 1] = b[j + 1] - c * b[i];
                }
            }

            // Backward (roots found)
            for (int i = n - 1; i >= 0; i--)
            {
                decimal sum = b[i];

                for (int j = n - 1; j >= i + 1; j--)
                    sum -= A.M[i, j] * x[j];

                x[i] = sum / A.M[i, i];
            }

            return x;
        }



        //---------------------------------------------------------------------
        //      Newton method solver
        //---------------------------------------------------------------------
        public static bool newton_solver(FuncDelegate func, JacobyDelegate J, decimal[] error, ref decimal[] x)
        {
            decimal[] dx;
            decimal[] f;
            int iter_count = 0;
            bool is_solved = true;

            do
            {
                DMatrix A = J(x);
                f = func(x);
                decimal[] b = new decimal[x.Length];

                for (int i = 0; i < x.Length; i++)
                {
                    b[i] = -f[i];
                }

                dx = gauss_solver(A, b);

                for (int i = 0; i < x.Length; i++)
                    x[i] = x[i] + dx[i];

                f = func(x);

                iter_count++;

            } while ((is_error(f, error)) && (iter_count <= NEWTON_ITER_MAX));

            if (iter_count > NEWTON_ITER_MAX)
                is_solved = false;

            return is_solved;
        }



        //---------------------------------------------------------------------
        //      Check Newton solver error
        //---------------------------------------------------------------------
        private static bool is_error(decimal[] dx, decimal[] error)
        {
            bool err = false;
            int n = dx.Length;

            for (int i = 0; i < n; i++)
            {
                if (DMath.abs(dx[i]) >= error[i])
                    err = true;
            }

            return err;
        }
    }
}
