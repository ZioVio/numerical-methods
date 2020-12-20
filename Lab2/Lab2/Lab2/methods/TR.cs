using System;
namespace Lab2
{
    public class TRMethod : Method
    {
        public TRMethod(double[,] A, double[] B) : base(A, B, "TR") { }

        public override double[] Solve()
        {
            Console.WriteLine("Solving with TR method\n");
            ModifyAB();
            MatrixUtils.PrintMatrix(A, "A");
            MatrixUtils.PrintMatrix(B, "B");
            double[] X = MatrixUtils.ReversePath(A, B, false);
            return X;
        }

        private void ModifyAB()
        {
            for (int i = 0; i < N - 1; i++)
            {
                for (int k = i + 1; k < N; k++)
                {
                    double cc = A[i, i] / Math.Sqrt((Math.Pow(A[i, i], 2) + Math.Pow(A[k, i], 2)));
                    double ss = A[k, i] / Math.Sqrt((Math.Pow(A[i, i], 2) + Math.Pow(A[k, i], 2)));

                    for (int j = 0; j < N; j++)
                    {
                        double a1 = A[i, j];
                        double a2 = A[k, j];
                        A[i, j] = cc * a1 + ss * a2;
                        A[k, j] = -ss * a1 + cc * a2;
                    }
                    double b1 = B[i];
                    double b2 = B[k];

                    B[i] = cc * b1 + ss * b2;
                    B[k] = -ss * b1 + cc * b2;
                }
            }
        }
    }
}
