namespace CodingInterviewImplementations.Mathematical
{
    public static class MathOperations
    {
        public static int GreatestCommonDivisor(int a, int b)
        {
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
            return a * b / GreatestCommonDivisor(a, b);
        }

        public static double Derivative(Func<double, double> f, double x)
        {
            const double h = 0.0001;
            return (f(x + h) - f(x)) / h;
        }
    }
}
