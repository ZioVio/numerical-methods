using System;

namespace Lab2
{
    public class HoletskyMethod : Method
    {

        public HoletskyMethod(double[,] A, double[] B) : base(A, B, "Holetskyi") { 
        }
        public override double[] Solve()
        {
            double[,] U = GetU();
            MatrixUtils.PrintMatrix(U, "U");
            double[,] UTransposed = MatrixUtils.Transpose(MatrixUtils.CopyMatrix(U));
            MatrixUtils.PrintMatrix(UTransposed, "UTransposed");

            double ADeterminant = GetDeterminant(U);
            Console.WriteLine($"Determinant: {ADeterminant}\n");

            double[,] AInverted = GetInverted(U, UTransposed);
            MatrixUtils.PrintMatrix(AInverted, "Inverted A");

            // UT * y = b  
            double[] Y = MatrixUtils.ReversePath(UTransposed, B, true);
            MatrixUtils.PrintMatrix(MatrixUtils.Transpose(Y), "Y");
            // U * X = y
            double[] X = MatrixUtils.ReversePath(U, Y, false);
            return X;
        }

        private double[,] GetU()
        {
            double[,] U = MatrixUtils.CreateMatrix(N, N);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                        double sum = 0;
                        for (int k = 0; k <= i - 1; k++)
                        {
                            sum += Math.Pow(U[k, i], 2);
                        }
                        U[i, j] = Math.Sqrt(A[i, i] - sum);
                    }
                    else if (j > i)
                    {
                        double sum = 0;
                        for (int k = 0; k <= i - 1; k++)
                        {
                            sum += U[k, i] * U[k, j];
                        }
                        U[i, j] = (A[i, j] - sum) / U[i, i];
                    }
                    else if (j < i)
                    {
                        U[i, j] = 0;
                    }
                }
            }
            return U;
        }

        private double GetDeterminant(double[,] U) {
            double det = 1;
            for (int i = 0; i < N; i++)
            {
                det *= Math.Pow(U[i, i], 2);
            }
            return det;
        } 

        private double[,] GetInverted(double[,] U, double[,] Ut) {
            double[,] invertedBase = MatrixUtils.CreateMatrix(N, N, (i, j) => i == j ? 1 : 0);
            double[,] transposedInversed = MatrixUtils.CreateMatrix(N, N, 1);

            for (int i = 0; i < N; i++)
            {
                double[] ort = new double[N];
                for (int j = 0; j < N; j++) {
                    ort[j] = invertedBase[i, j];
                }
                // Ut * Y = ort
                double[] Y = MatrixUtils.ReversePath(Ut, ort, true);

                // U * X = Y
                double[] X = MatrixUtils.ReversePath(U, Y, false);

                for (int j = 0; j < N; j++) {
                    transposedInversed[i, j] = X[j];
                }
            }

            return MatrixUtils.Transpose(transposedInversed);
        }
    }
}
