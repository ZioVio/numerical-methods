using System;
namespace Lab2
{
    public static class MatrixUtils
    {
        public static int numberPrintingLength = 15;
        public static double[,] Multiply(double[,] A, double[,] B)
        {
            int ARows = A.GetLength(0);
            int ACols = A.GetLength(1);
            int BRows = B.GetLength(0);
            int BCols = B.GetLength(1);
            if (ACols != BRows)
            {
                throw new InvalidOperationException("Cannot multiply matrises");
            }
            double[,] res = new double[ARows, BCols];
            for (int i = 0; i < ARows; i++)
            {
                for (int j = 0; j < BCols; j++)
                {
                    double val = 0;
                    val = 0;
                    for (int k = 0; k < ACols; k++)
                    {
                        val += A[i, k] * B[k, j];
                    }
                    res[i, j] = val;
                }
            }
            return res;
        }

        public static void PrintMatrix(double[,] m, string name = null)
        {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);

            if (!String.IsNullOrEmpty(name))
            {
                Console.WriteLine($"Matrix - {name} ".PadRight(numberPrintingLength * cols, '~'));
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    double val = m[i, j];
                    string cell = val.ToString("0.#####").PadRight(numberPrintingLength, ' ');
                    Console.Write(cell);
                }
                Console.WriteLine();
            }
            Console.WriteLine("".PadRight(numberPrintingLength * cols, '~'));
            Console.WriteLine();
        }

        public static void PrintMatrix(double[] arr, string name = null)
        {
            PrintMatrix(ToMatrix(arr), name);
        }

        public static double[,] ToMatrix(double[] arr)
        {
            var matrix = new double[1, arr.GetLength(0)];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                matrix[0, i] = arr[i];
            }
            return matrix;
        }

        public static double[,] CopyMatrix(double[,] src)
        {
            return CreateMatrix(src.GetLength(0), src.GetLength(1), (i, j) => src[i, j]);
        }
        public static double[,] Transpose(double[,] m)
        {
            int newRows = m.GetLength(1);
            int newCols = m.GetLength(0);
            double[,] res = new double[newRows, newCols];

            for (int i = 0; i < newCols; i++)
            {
                for (int j = 0; j < newRows; j++)
                {
                    res[j, i] = m[i, j];
                }
            }

            return res;
        }

        public static double[,] Transpose(double[] arr)
        {
            return Transpose(ToMatrix(arr));
        }

        public static void PrintMatrix(double[] arr)
        {
            PrintMatrix(ToMatrix(arr));
        }

        public static double[,] CreateMatrix(int rows, int cols)
        {
            return new double[rows, cols];
        }


        public static double[,] CreateMatrix(int rows, int cols, double fill = 0)
        {
            return CreateMatrix(rows, cols, (_i, _j) => fill);
        }

        public static double[,] CreateMatrix(int rows, int cols, Func<int, int, double> placementFunc)
        {
            double[,] res = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    res[i, j] = placementFunc(i, j);
                }
            }
            return res;
        }

        public static double[] ReversePath(double[,] m, double[] b, bool fromTop = true)
        {
            int n = m.GetLength(0);
            double[] res = new double[n];
            if (fromTop)
            {
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < i; j++)
                    {
                        sum += res[j] * m[i, j];
                    }
                    res[i] = (b[i] - sum) / m[i, i];
                }
            }
            else
            {
                for (int i = n - 1; i >= 0; i--)
                {
                    double sum = 0;
                    for (int j = n - 1; j > i; j--)
                    {
                        sum += res[j] * m[i, j];
                    }

                    res[i] = (b[i] - sum) / m[i, i];
                }
            }
            return res;
        }
    }
}
