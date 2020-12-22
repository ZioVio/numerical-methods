using System;
using System.Collections.Generic;

namespace Lab2
{
    public class TomasMethod : Method
    {
        public TomasMethod(double[,] A, double[] B) : base(A, B, "Tomas")
        { }

        public override double[] Solve()
        {
            List<double> a, b, c;
            FindABC(out a, out b, out c);

            MatrixUtils.PrintMatrix(MatrixUtils.Transpose(a.ToArray()), "A");
            MatrixUtils.PrintMatrix(MatrixUtils.Transpose(b.ToArray()), "B");
            MatrixUtils.PrintMatrix(MatrixUtils.Transpose(c.ToArray()), "C");

            double[] beta, alpha;
            FindAlphaBeta(a, b, c, out beta, out alpha);

            MatrixUtils.PrintMatrix(MatrixUtils.Transpose(beta), "Beta");
            MatrixUtils.PrintMatrix(MatrixUtils.Transpose(alpha), "Alpha");

            double[] X = GetX(beta, alpha);
            return X;
        }

        private void FindAlphaBeta(List<double> a, List<double> b, List<double> c, out double[] beta, out double[] alpha)
        {
            beta = new double[N];
            beta[0] = c[0] / b[0];

            alpha = new double[N];
            alpha[0] = -B[0] / b[0];

            for (int i = 1; i < N; i++)
            {
                beta[i] = c[i] / (b[i] - a[i] * beta[i - 1]);
                alpha[i] = (a[i] * alpha[i - 1] - B[i]) / (b[i] - a[i] * beta[i - 1]);
            }
        }

        private void FindABC(out List<double> a, out List<double> b, out List<double> c)
        {
            a = new List<double>();
            b = new List<double>();
            c = new List<double>();
            a.Add(0);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j + 1)
                        a.Add(A[i, j]);
                    if (i == j)
                        b.Add(-A[i, j]);
                    if (i == j - 1)
                        c.Add(A[i, j]);
                }
            }
            c.Add(0);
        }

        private double[] GetX(double[] P, double[] Q) {
            double[] X = new double[N];
            
            X[N - 1] = Q[N - 1];

            for (int i = N - 2; i >= 0; i--)
            {
                X[i] = (P[i] * X[i + 1]) + Q[i];
            }

            return X;
        }
        
    }
}