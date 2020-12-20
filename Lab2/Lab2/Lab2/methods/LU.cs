using System;
namespace Lab2
{
    public class LUMethod : Method
    {
        private double[,] L;
        private double[,] U;
        private double[] Y;

        public LUMethod(double[,] A, double[] B) : base(A, B, "LU") { }
        public override double[] Solve()
        {
            var LUTuple = GetLU();

            L = LUTuple.Item1;
            MatrixUtils.PrintMatrix(L, "L");

            U = LUTuple.Item2;
            MatrixUtils.PrintMatrix(U, "U");

            double detA = GetDeterminant();
            Console.WriteLine($"Determinant A: {detA}");

            double[,] inversedA = GetInversed();
            MatrixUtils.PrintMatrix(inversedA, "Inversed A");

            Y = GetY();
            MatrixUtils.PrintMatrix(MatrixUtils.Transpose(Y), "Y");

            return GetX();
        }

        private double[] GetY()
        {
            return MatrixUtils.ReversePath(L, B, true);
        }

        private double[] GetX()
        {
            return MatrixUtils.ReversePath(U, Y, false);
        }

        private double GetDeterminant()
        {
            double det = 1;
            int n = U.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                det *= U[i, i];
            }
            return det;
        }

        private double[,] GetInversed()
        {
            int n = A.GetLength(0);
            double[,] inversedBase = MatrixUtils.CreateMatrix(n, n, (i, j) => i == j ? 1 : 0);
            double[,] LU = CombineLU(L, U);
            double[,] transposedInversed = MatrixUtils.CreateMatrix(n, n, 1);

            for (int i = 0; i < n; i++)
            {
                double[] y = new double[n];
                for (int j = 0; j < n; j++)
                {
                    double val = 0;
                    for (int k = j - 1; k >= 0; k--)
                    {
                        val += y[k] * LU[j, k];
                    }
                    y[j] = inversedBase[i, j] - val;
                }

                double[] x = new double[n];
                for (int j = n - 1; j >= 0; j--)
                {
                    double sum = 0;
                    for (int k = n - 1; k > j; k--)
                    {
                        sum += LU[j, k] * x[k];
                    }
                    x[j] = ((y[j] - sum) / LU[j, j]);
                }

                for (int j = 0; j < n; j++) {
                    transposedInversed[i, j] = x[j];
                }
            }

            return MatrixUtils.Transpose(transposedInversed);
        }

        private double[,] CombineLU(double[,] l, double[,] u)
        {
            int n = l.GetLength(0);
            double[,] lu = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j < i)
                        lu[i, j] = l[i, j];
                    else
                        lu[i, j] = u[i, j];
                }
            }
            return lu;
        }
        private Tuple<double[,], double[,]> GetLU()
        {
            int n = A.GetLength(0);
            double[,] L = new double[n, n];
            double[,] U = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        sum += (L[i, k] * U[k, j]);
                    }
                    U[i, j] = A[i, j] - sum;
                }

                for (int j = i; j < n; j++)
                {
                    if (i == j)
                    {
                        L[i, i] = 1;
                    }
                    else
                    {
                        double sum = 0;
                        for (int k = 0; k < i; k++)
                        {
                            sum += (L[j, k] * U[k, i]);
                        }
                        L[j, i] = (A[j, i] - sum) / U[i, i];
                    }
                }
            }
            return new Tuple<double[,], double[,]>(L, U);
        }
    }
}