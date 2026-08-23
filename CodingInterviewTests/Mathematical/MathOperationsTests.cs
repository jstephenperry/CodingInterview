namespace CodingInterviewImplementations.Mathematical.Tests
{
    public class MathOperationsTests
    {
        public static TheoryData<int, int, int> GreatestCommonDivisorCases => new()
        {
            { 12, 8, 4 },
            { 8, 12, 4 },
            { 17, 5, 1 },
            { 270, 192, 6 },
            { 7, 7, 7 },
            { 0, 5, 5 },       // gcd with zero is the other operand
            { 5, 0, 5 },
            { 0, 0, 0 },       // both zero has no positive divisor
            { -12, 8, 4 },     // sign is ignored
            { 12, -8, 4 },
            { -12, -8, 4 }
        };

        public static TheoryData<int, int, long> LeastCommonMultipleCases => new()
        {
            { 4, 6, 12L },
            { 21, 6, 42L },
            { 7, 7, 7L },
            { 1, 13, 13L },
            { 0, 5, 0L },      // zero has no positive multiple in common
            { 5, 0, 0L },
            { 0, 0, 0L },
            { -4, 6, 12L }     // result is always non-negative
        };

        public static TheoryData<double, double> XSquaredDerivativeCases => new()
        {
            { 3.0, 6.0 },
            { 0.0, 0.0 },
            { -2.5, -5.0 }
        };

        [Theory]
        [MemberData(nameof(GreatestCommonDivisorCases))]
        public void GreatestCommonDivisor_ReturnsTheExpectedValue(int a, int b, int expected)
        {
            Assert.Equal(expected, MathOperations.GreatestCommonDivisor(a, b));
        }

        [Fact]
        public void GreatestCommonDivisor_HandlesIntMinValue()
        {
            // Negating int.MinValue overflows, so the absolute value has to be taken in a wider type.
            Assert.Equal(2, MathOperations.GreatestCommonDivisor(int.MinValue, 2));
        }

        [Theory]
        [MemberData(nameof(LeastCommonMultipleCases))]
        public void LeastCommonMultiple_ReturnsTheExpectedValue(int a, int b, long expected)
        {
            Assert.Equal(expected, MathOperations.LeastCommonMultiple(a, b));
        }

        [Fact]
        public void LeastCommonMultiple_DoesNotOverflow()
        {
            // The regression test for a * b / gcd: this returned -728379968 before the fix, because
            // the product overflowed int before the division could bring it back into range.
            Assert.Equal(999_999_000_000L, MathOperations.LeastCommonMultiple(1_000_000, 999_999));
        }

        [Fact]
        public void LeastCommonMultiple_WithBothInputsZero_DoesNotDivideByZero()
        {
            // The GCD of zero and zero is zero, so the naive formula divides by zero here.
            Assert.Equal(0L, MathOperations.LeastCommonMultiple(0, 0));
        }

        [Theory]
        [MemberData(nameof(XSquaredDerivativeCases))]
        public void Derivative_OfXSquared_IsTwiceX(double x, double expected)
        {
            Assert.Equal(expected, MathOperations.Derivative(v => v * v, x), 1e-6);
        }

        [Fact]
        public void Derivative_OfALineIsItsSlope()
        {
            Assert.Equal(3, MathOperations.Derivative(v => (3 * v) + 1, 42), 1e-6);
        }

        [Fact]
        public void Derivative_OfSineIsCosine()
        {
            Assert.Equal(Math.Cos(1.0), MathOperations.Derivative(Math.Sin, 1.0), 1e-8);
        }

        [Fact]
        public void Derivative_StaysAccurateAtLargeArguments()
        {
            // A fixed step of 1e-4 is lost in the floating point noise once x is large, so the step
            // has to scale with the magnitude of x. The forward difference this replaced returned a
            // visibly wrong slope here.
            const double X = 1e8;
            double expected = 2 * X;

            // 0.0001 percent of the expected value, expressed as an absolute tolerance.
            Assert.Equal(expected, MathOperations.Derivative(v => v * v, X), Math.Abs(expected) * 1e-6);
        }

        [Fact]
        public void Derivative_IsMoreAccurateThanAForwardDifference()
        {
            // Central difference error is O(h^2) against O(h) for the forward difference. On a
            // curved function the improvement should be visible, not marginal.
            const double X = 2.0;
            const double H = 0.0001;

            static double F(double v) => v * v * v;
            double exact = 3 * X * X;

            double forwardDifference = (F(X + H) - F(X)) / H;
            double centralError = Math.Abs(MathOperations.Derivative(F, X) - exact);
            double forwardError = Math.Abs(forwardDifference - exact);

            Assert.True(
                centralError < forwardError / 100,
                $"central error {centralError} was not two orders better than forward error {forwardError}");
        }

        [Fact]
        public void Derivative_WithANullFunction_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => MathOperations.Derivative(null!, 1.0));
        }
    }
}
