using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astronomy
{
    class DMatrix
    {
        public decimal[,] M;
    
        public int rows;
        public int columns;

        //-------------------------------------------------------------------------
        //      DMatrix constructor
        //-------------------------------------------------------------------------
        public DMatrix(int n, int m)
        {
            M = new decimal[n, m];

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

        public static DMatrix operator +(DMatrix a, DMatrix b)
        {
            DMatrix c = new DMatrix(a.rows, a.columns);

            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.columns; j++)
                    c.M[i, j]= a.M[i, j] + b.M[i, j];
            }

            return c;
        }

        public static DMatrix operator -(DMatrix a, DMatrix b)
        {
            DMatrix c = new DMatrix(a.rows, a.columns);

            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.columns; j++)
                    c.M[i, j] = a.M[i, j] - b.M[i, j];
            }

            return c;
        }


        public static DMatrix operator *(DMatrix a, decimal lambda)
        {
            DMatrix c = new DMatrix(a.rows, a.columns);

            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.columns; j++)
                    c.M[i, j] = a.M[i, j]*lambda;
            }

            return c;
        }
    }
}
