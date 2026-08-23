namespace CodingInterviewImplementations.Mathematical
{
    /// <summary>
    /// Small numeric helpers.
    /// </summary>
    public static class MathOperations
    {
        /// <summary>
        /// Computes the greatest common divisor using the Euclidean algorithm.
        /// </summary>
        /// <returns>The non-negative greatest common divisor, or zero when both inputs are zero.</returns>
        /// <remarks>
        /// Operates on <see cref="long"/> internally so that <see cref="int.MinValue"/> can be
        /// negated without overflowing.
        /// </remarks>
        public static int GreatestCommonDivisor(int a, int b)
        {
            long x = Math.Abs((long)a);
            long y = Math.Abs((long)b);

            while (y != 0)
            {
                (x, y) = (y, x % y);
            }

            return (int)x;
        }

        /// <summary>
        /// Computes the least common multiple.
        /// </summary>
        /// <returns>The non-negative least common multiple, or zero when either input is zero.</returns>
        /// <remarks>
        /// Divides before multiplying and returns a <see cref="long"/>. The previous
        /// <c>a * b / gcd</c> form overflowed silently: LeastCommonMultiple(1000000, 999999)
        /// returned -728379968 instead of 999999000000.
        /// </remarks>
        public static long LeastCommonMultiple(int a, int b)
        {
            if (a == 0 || b == 0)
            {
                // The only common multiple of zero and anything else is zero, and the GCD is zero
                // when both are zero, so guard before dividing.
                return 0;
            }

            long gcd = GreatestCommonDivisor(a, b);
            return Math.Abs((long)a / gcd * b);
        }

        /// <summary>
        /// Approximates the first derivative of a function at a point.
        /// </summary>
        /// <param name="f">The function to differentiate.</param>
        /// <param name="x">The point at which to evaluate the derivative.</param>
        /// <returns>The approximate slope of <paramref name="f"/> at <paramref name="x"/>.</returns>
        /// <remarks>
        /// Uses a central difference, whose error is O(h^2), rather than the forward difference used
        /// previously, whose error is O(h). The step is scaled to the magnitude of
        /// <paramref name="x"/> so the approximation does not collapse for large inputs, and the
        /// effective step is recovered from the rounded endpoints to avoid subtraction error.
        /// </remarks>
        public static double Derivative(Func<double, double> f, double x)
        {
            ArgumentNullException.ThrowIfNull(f);

            // Cube root of machine epsilon is the step that balances truncation against rounding
            // error for a central difference.
            const double CubeRootEpsilon = 6.055454452393343e-6;

            double h = CubeRootEpsilon * Math.Max(Math.Abs(x), 1.0);
            double high = x + h;
            double low = x - h;

            return (f(high) - f(low)) / (high - low);
        }
    }
}
