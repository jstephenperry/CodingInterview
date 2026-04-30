namespace CodingInterviewImplementations.Mathematical
{
    public static class MathOperations
    {
        /// <summary>
        /// Returns the greatest common divisor of <paramref name="a"/> and <paramref name="b"/> (Euclidean algorithm).
        /// By convention <c>gcd(0, 0) == 0</c>.
        /// </summary>
        public static int GreatestCommonDivisor(int a, int b)
        {
            // int.MinValue has no positive representation as int; promote to long for safe abs().
            long la = Math.Abs((long)a);
            long lb = Math.Abs((long)b);

            while (lb != 0)
            {
                (la, lb) = (lb, la % lb);
            }

            return (int)la;
        }

        /// <summary>
        /// Returns the least common multiple of <paramref name="a"/> and <paramref name="b"/>.
        /// Returns 0 if either argument is 0. Throws on overflow.
        /// </summary>
        /// <exception cref="OverflowException">The result does not fit in <see cref="int"/>.</exception>
        public static int LeastCommonMultiple(int a, int b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }

            // Divide before multiplying to reduce overflow risk; checked block surfaces the rest.
            return checked(Math.Abs(a / GreatestCommonDivisor(a, b) * b));
        }

        /// <summary>
        /// Numerically approximates the derivative of <paramref name="f"/> at <paramref name="x"/>
        /// using the central-difference formula.
        /// </summary>
        /// <param name="f">A real-valued function.</param>
        /// <param name="x">The point at which to evaluate <c>f'</c>.</param>
        /// <param name="h">Step size; defaults to <c>1e-5</c>, which balances truncation and round-off error for typical inputs.</param>
        public static double Derivative(Func<double, double> f, double x, double h = 1e-5)
        {
            ArgumentNullException.ThrowIfNull(f);
            if (h <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(h), "Step size must be positive.");
            }

            return (f(x + h) - f(x - h)) / (2 * h);
        }
    }
}
