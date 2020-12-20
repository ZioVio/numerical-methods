using System;
using System.Collections.Generic;

namespace Lab2
{
    class Program
    {
        public static double[,] matrix1 = new double[,] {
           { 61, -82, 35, -48, -40, -43, -50, -1, -92 },
           { -39, -99,-75,-75, 26, -1, 42, 21, 10 },
           { 85, -88, 76, 48, -8, 90, 94,-86,-24 },
           { 28, 8,-31,-29, 94, 98,-95, 96, 50 },
           { 86,-37, 12, 14,-99,-26, 33, 17, 88 },
           { 24,-30,-51,-57, 14,-53, 58, 21,-92 },
           { 7, -9,-43, 69,-82, 56,-30,100,-44 },
           { 24, 37, 98,-77,-55, 16, 41, 12, 46 },
           { 81, 79, 8,-70, 65,-76,-39,-82,-54 },
        };

        public static double[] b1 = new double[] {
            67,
            94,
            -83,
            76,
            -93,
            -79,
            -18,
            94,
            -77
        };

        public static double[,] matrix2 = new double[,] {
            {268,  74, -71, 11,-104, 16, -33, 64, 7, 11},
            {74,   375, -118, 230, -51, -212, 180, 204, 163, -35 },
            {-71,  -118, 400, -122, -81, 72, -201, 110, 20, 333,},
            {11,   230, -122, 239, -47, -40, 189, 188, 57, -69,},
            {-104, -51, -81, -47, 325, -53, -13, -222, -27, -118},
            {16,   -212, 72, -40, -53, 335, -64, -60, -110, 73,},
            {-33,  180, -201, 189, -13, -64, 325, 47, -8, -84},
            {64,   204, 110, 188, -222, -60, 47, 446, 123, 123,},
            {7,    163, 20, 57, -27, -110, -8, 123, 379, 65, },
            {11,   -35, 333, -69, -118, 73, -84, 123, 65, 378, }
        };
        public static double[] b2 = new double[] {
            420,
            19,
            384,
            83,
            108,
            264,
            80,
            14,
            -215,
            394
        };
        // LU with 1's in L - DONE
        // TR-метод обертань - DONE без детерминанта и А^-1
        // метод квадратних коренів - метод холецького
        // прямий метод?
        // метод прогонки
        // прямий метод?
        static void Main(string[] args)
        {
            double[,] sampleMatrix = { { 7, -2, 1 },
                        { 14, -7, -3 },
                        { -7, 11, 18 } };
            double[] sampleB = new double[] { 12, 17, 5 };

            var LU = new LUMethod(matrix1, b1);
            var TR = new TRMethod(matrix1, b1);
            var Holetsky = new HoletskyMethod(matrix2, b2);

            var methods = new Method[] { 
                // LU, 
                // TR, 
                Holetsky 
            };

            foreach (var method in methods)
            {
                // todo add method name to class
                Console.WriteLine($"Solving with ---------{method.Name}--------- method");
                var result = method.Solve();
                MatrixUtils.PrintMatrix(MatrixUtils.Transpose(result), "Result");
            }
        }
    }
}
