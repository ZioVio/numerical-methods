using System;
namespace Lab2
{
    public abstract class Method
    {
        protected double[,] A;
        protected double[] B;

        public readonly string Name;

        protected int N;

        public Method(double[,] A, double[] B, string name) {
            this.A = A;
            this.B = B;
            this.N = A.GetLength(0);
            this.Name = name;
        }
        public abstract double[] Solve();
    }
}
