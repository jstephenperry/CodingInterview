namespace CodingInterviewImplementations.Mathematical
{
    public static class MathOperations
    {
        public static int GreatestCommonDivisor(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static int LeastCommonMultiple(int a, int b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }

            // Divide before multiplying to reduce overflow risk.
            return Math.Abs(a / GreatestCommonDivisor(a, b) * b);
        }

        public static double Derivative(Func<double, double> f, double x)
        {
            const double h = 1e-5;
            return (f(x + h) - f(x - h)) / (2 * h);
        }
    }
}
