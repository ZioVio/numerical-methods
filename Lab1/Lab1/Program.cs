using System;

using UnaryFunc = System.Func<double, double>;

namespace Lab1
{
    class Lab1
    {

        private static double EPSILON = 1e-7;
        private static int EXIT_METHOD_CODE = -1;

        private static double[] algebraicEquationCoeficcients =
        {
            18, 84, -225, -811, 565, 842, -437, -62
        };

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("\nSelect method\n");
                Console.WriteLine("1. Simplified Newthon Method");
                Console.WriteLine("2. Bisection method");
                Console.WriteLine("3. Simple iterations method");
                Console.WriteLine("4. Lobachevsky method");
                int method = SafeReadInt();
                if (method == EXIT_METHOD_CODE)
                {
                    break;
                }
                if (method == int.MinValue || method < 1 || method > 4)
                {
                    Console.WriteLine("Imvalid method");
                    continue;
                }
                Console.WriteLine("Enter left bound (ignore for Lobachevsky method)");
                double a = SafeReadDouble();
                Console.WriteLine("Enter right bound (ignore for Lobachevsky method)");
                double b = SafeReadDouble();
                if ((Double.IsNaN(a) || Double.IsNaN(b)) && method != 4)
                {
                    Console.WriteLine("Invalid bouds");
                    continue;
                }

                switch (method)
                {
                    case 1: OnSimplifiedNewthonMethod(f1, a, b); break;
                    case 2: OnBisectionMethod(f2, a, b); break;
                    case 3: OnSimpleIterationsMehtod(f2, a, b); break;
                    case 4: OnLobachevskyMethod(algebraicEquationCoeficcients); break;
                }
            } while (true);
        }

        private static double f1(double x)
        {
            return Math.Pow(Math.Cos(x), 3) + Math.Pow(x, 3) * Math.Exp(x) - Math.Pow(x, 6) - 35;
        }

        private static double f2(double x)
        {
            return Math.Pow(x, 3) * Math.Cosh(x) + Math.PI - 9 * Math.PI * x;
        }

        private static double polinom(double x)
        {
            double result = 0;
            for (int i = 0; i < algebraicEquationCoeficcients.Length; i++)
            {
                double a = algebraicEquationCoeficcients[i];
                result += a * Math.Pow(x, algebraicEquationCoeficcients.Length - i - 1);
            }

            return result;
        }

        private static void OnSimplifiedNewthonMethod(UnaryFunc f, double a, double b)
        {
            if (!CanFindRoot(f, a, b))
            {
                return;
            }

            UnaryFunc df2 = GetDerivative(f, 2);

            bool aIsMoving = df2(a) * f(a) > 0;
            bool bIsMoving = df2(b) * f(b) > 0;

            bool rootsExist = aIsMoving != bIsMoving;
            
            if (!rootsExist)
            {
                Console.WriteLine($"No roots exist from {a} to {b}");
                return;
            }

            double x0 = aIsMoving ? a : b;

            double xN = x0;
            double xNext;

            UnaryFunc df = GetDerivative(f);
            double df_x0 = df(x0);

            int i = 0;
            do
            {
                i++;

                xNext = xN - f(xN) / df_x0;

                Console.WriteLine($"Xn+1: {xNext} iteration:{i}");

                if(Math.Abs(xNext - xN) <= EPSILON)
                {
                    break;
                }

                xN = xNext;
            } while (true);

            Console.WriteLine($"Result: {xNext}");
        }

        private static void OnBisectionMethod(UnaryFunc f, double a, double b)
        {
           
            if (!CanFindRoot(f, a, b))
            {
                return;
            }

            double m;

            int i = 0;
            do
            {
                i++;
                m = (a + b) / 2.0d;
                double f_l = f(a);
                double f_r = f(b);
                double f_m = f(m);
                Console.WriteLine($"a: {a}  b: {b}  root: {m} iteration:{i}");
                if (f_l * f_m > 0)
                {
                    a = m;
                } else if (f_r * f_m > 0)
                {
                    b = m;
                }
            } while (Math.Abs(a - b) > EPSILON);

            Console.WriteLine($"Result: {m}");
        }

        private static void OnSimpleIterationsMehtod(UnaryFunc f, double a, double b)
        {
            if (!CanFindRoot(f, a, b))
            {
                return;
            }


            UnaryFunc df = GetDerivative(f);
            var minMax = GetMinMax(df, a, b);

            double alpha = minMax.Item1;
            double gamma = minMax.Item2;

            double lambda = 2 / (alpha + gamma);
            double q = Math.Max(Math.Abs(1 - alpha * lambda), Math.Abs(1 - gamma * lambda));

            Func<double, double, double, bool> stopCriteria = GetSimpleIterationsStopCriteria(q);

            UnaryFunc phi = (x) => x - lambda * f(x);

            double xN = (a + b) / 2;
            double xNext;

            int i = 0;
            do
            {
                i++;

                xNext = phi(xN);

                Console.WriteLine($"Xn+1: {xNext} xn: {xN} iteration: {i}");

                if (stopCriteria(xNext, xN, q))
                {
                    break;
                }

                xN = xNext;
            } while (true);

            Console.WriteLine($"Result: {xNext}");

        }


        public static bool LobachevskyStopCriteria(double[] oldKoefs, double[] newKoefs, double presicion) 
        {
            for (int i = 0; i < oldKoefs.Length; i++)
            {
                double oldCoefSquared = Math.Pow(oldKoefs[i], 2);
                if (Math.Abs(oldCoefSquared - newKoefs[i]) > presicion)
                {
                    return false;
                }
            }
            return true;
        }

        private static double[] normalizeKoefs(double[] array, double normalizationDivider = 100)
        {
            double[] res = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                res[i] = array[i] / normalizationDivider;
            }
            return res;
        }

        private static Action<UnaryFunc, double, double> SelectMethodToSpecifyRootsFterLobachevsky()
        {
            Console.WriteLine("\nSelect method to specify roots after lobachevkiy\n");
            Console.WriteLine("1. Simplified Newthon Method");
            Console.WriteLine("2. Bisection method");
            Console.WriteLine("3. Simple iterations method");
            int method = SafeReadInt();
            switch (method)
            {
                case 1: return OnSimplifiedNewthonMethod;
                case 2: return OnBisectionMethod;
                case 3: return OnSimpleIterationsMehtod;
                default: return null;
            }
        }
        private static void OnLobachevskyMethod(double[] koefs)
        {
            Console.WriteLine("Enter presicion");
            double presicion = SafeReadDouble();
            if (Double.IsNaN(presicion))
            {
                Console.WriteLine("Invalid presicion");
                return;
            }

            var specifyRootsFunc = SelectMethodToSpecifyRootsFterLobachevsky();
            if (specifyRootsFunc == null)
            {
                Console.WriteLine("Select valid method next time");
                return;
            }

            int p = 0;

            double[] a = normalizeKoefs(koefs);
            double[] b = new double[a.Length];
            bool shouldStop = false;

            while (!LobachevskyStopCriteria(a, b, presicion))
            {
                p++;
                int n = a.Length - 1;

                for (int k = 0; k <= n; k++)
                {
                    double sum = 0;
                    int sign = -1;
                    for (int j = 1; j <= Math.Min(k, n - k); j++)
                    {
                        sum += sign * a[k - j] * a[k + j];
                        sign = sign < 0 ? 1 : -1;
                    }

                    double nextB_k = a[k] * a[k] + 2 * sum;
                    if (double.IsNaN(nextB_k))
                    {
                        shouldStop = true;
                    }

                    b[k] = nextB_k;
                }

                if (shouldStop)
                {
                    break;
                }

                normalizeKoefs(b).CopyTo(a, 0);
            }

            double[] roots = new double[a.Length - 1];
            double power = Math.Pow(2, -p);

            for (int i = 1; i < a.Length; i++)
            {
                double root = Math.Pow(a[i] / a[i - 1], power);
                Console.WriteLine($"p({root})={polinom(root)}");

                if (Math.Abs(polinom(root)) < 20)
                {
                    roots[i - 1] = root;
                }
                else
                {
                    roots[i - 1] = -root;
                }
            }


            foreach (var root in roots)
            {
                Console.WriteLine($"Root->  {root}");
                specifyRootsFunc(polinom, root - 0.1, root + 0.1);
            }

        }


        private static Func<double, double, double, bool> GetSimpleIterationsStopCriteria(double q)
        {
            Func<double, double, double, bool> simpleCriteria = (xnext, xn, q) =>
            {
                return Math.Abs(xnext - xn) <= EPSILON;
            };

            Func<double, double, double, bool> complexCriteria = (xnext, xn, q) =>
            {
                return Math.Abs(xnext - xn) <= ((1 - q) / q) * EPSILON;
            };

            if (q < 0.5)
            {
                Console.WriteLine($"q = {q}, choosing simple criteria");
                return simpleCriteria;
            } else
            {
                Console.WriteLine($"q = {q}, choosing complex criteria");
                return complexCriteria;
            }
        }


        private static UnaryFunc GetDerivative(UnaryFunc f, int order = 1)
        {
            if (order < 1)
            {
                return f;
            }
            UnaryFunc df = (x) => (f(x + EPSILON) - f(x)) / EPSILON;
            if (order == 1)
            {
                return df;
            }
            return GetDerivative(df, order - 1);
        }

        private static bool IsMonotone(UnaryFunc f, double a, double b)
        {
            bool isRising = f(a + EPSILON) - f(a) > 0;
            // 2 * EPSILON just for speed up
            for (double x = a + EPSILON; x <= b; x += 2 * EPSILON)
            {
                if (f(x + EPSILON) - f(x) > 0 != isRising)
                {
                    return false;
                }
            }
            return true;
        }

        private static Tuple<double, double> GetMinMax(UnaryFunc f, double a, double b)
        {
            double min = f(a);
            double max = min;

            // 2 * EPSILON just for speed up
            for (double x = a + EPSILON; x <= b; x += 2 * EPSILON)
            {
                double f_x = f(x);
                if (f_x > max)
                {
                    max = f_x;
                } 
                if (f_x < min)
                {
                    min = f_x;
                }
            }

            return new Tuple<double, double>(min, max);
        }

        private static bool CanFindRoot(UnaryFunc f, double a, double b)
        {
            if (!IsMonotone(f, a, b))
            {
                Console.WriteLine($"Function is not monotone from a:{a} to b:{b}");
                return false;
            }
            // TODO maybe add this
            //if (f(a) == 0)
            //{
            //    Console.WriteLine($"The root is: {a}");
            //}
            //if (f(b) == 0)
            //{
            //    Console.WriteLine($"The root is: {b}");
            //}

            if (f(a) * f(b) > 0)
            {
                Console.WriteLine($"No roots from a:{a} to b:{b}");
                return false;
            }

            return true;
        }

        private static double SafeReadDouble()
        {
            try
            {
                double result = Convert.ToDouble(Console.ReadLine().Replace('.', ','));
                return result;
            } 
            catch (Exception e)
            {
                return Double.NaN;
            }
        }

        private static int SafeReadInt()
        {
            try
            {
                int result = Convert.ToInt32(Console.ReadLine());
                return result;
            }
            catch (Exception e)
            {
                return int.MinValue;
            }
        }
    }
}
