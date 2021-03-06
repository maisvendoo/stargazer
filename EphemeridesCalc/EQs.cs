﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astronomy
{
    public class EQs
    {
        public delegate double [] FuncDelegate(double [] x);
        public delegate Matrix JacobyDelegate(double [] x);

        private const int NEWTON_ITER_MAX = 100000;

        //---------------------------------------------------------------------
        //      Gauss method solver
        //---------------------------------------------------------------------
        public static double [] gauss_solver(Matrix A, double [] b)
        {
            int n = A.rows;
            double [] x = new double [n];
            double  c = 0;

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
                double  sum = b[i];

                for (int j = n - 1; j >= i + 1; j--)
                    sum -= A.M[i, j] * x[j];

                x[i] = sum / A.M[i, i];
            }

            return x;
        }



        //---------------------------------------------------------------------
        //      Newton method solver
        //---------------------------------------------------------------------
        public static bool newton_solver(FuncDelegate func, JacobyDelegate J, double [] error, ref double [] x)
        {
            double [] dx;
            double [] f;
            int iter_count = 0;
            bool is_solved = true;
            
            do
            {
                Matrix A = J(x);
                f = func(x);
                double [] b = new double [x.Length];

                for (int i = 0; i < x.Length; i++)
                {
                    b[i] = -f[i];
                }

                dx = gauss_solver(A, b);

                for (int i = 0; i < x.Length; i++)
                    x[i] = x[i] + dx[i];

                f = func(x);                

                iter_count++;

            } while ( (is_error(f, error)) && (iter_count <= NEWTON_ITER_MAX)) ;

            if (iter_count > NEWTON_ITER_MAX)
                is_solved = false;

            return is_solved;
        }



        //---------------------------------------------------------------------
        //      Check Newton solver error
        //---------------------------------------------------------------------
        private static bool is_error(double [] dx, double [] error)
        {
            bool err = false;
            int n = dx.Length;

            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(dx[i]) >= error[i])
                    err = true;
            }

            return err;
        }
    }
}
