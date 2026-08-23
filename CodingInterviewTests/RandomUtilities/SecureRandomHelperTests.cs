using CodingInterviewImplementations.Tests;

namespace CodingInterviewImplementations.RandomUtilities.Tests
{
    /// <summary>
    /// Tests for <see cref="SecureRandomHelper"/>.
    /// </summary>
    /// <remarks>
    /// Range assertions alone cannot validate a generator: the original off-by-one that stopped a d6
    /// rolling a 6 lived strictly inside the asserted range. Every generator here is therefore
    /// checked for reachable bounds, an excluded upper bound, and rough uniformity.
    /// <para>
    /// Sample sizes are chosen so the tolerance sits more than ten standard deviations from the
    /// expected count. These tests are statistical but not flaky.
    /// </para>
    /// </remarks>
    public class SecureRandomHelperTests
    {
        private const int UniformitySamples = 100_000;

        public static TheoryData<int> ByteLengths => new() { 0, 1, 16, 1024 };

        [Theory]
        [MemberData(nameof(ByteLengths))]
        public void GenerateRandomBytes_ReturnsRequestedLength(int length)
        {
            Assert.Equal(length, SecureRandomHelper.GenerateRandomBytes(length).Length);
        }

        [Fact]
        public void GenerateRandomBytes_WithNegativeLength_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SecureRandomHelper.GenerateRandomBytes(-1));
        }

        [Fact]
        public void GenerateRandomBytes_DoesNotReturnAConstantBuffer()
        {
            byte[] first = SecureRandomHelper.GenerateRandomBytes(32);
            byte[] second = SecureRandomHelper.GenerateRandomBytes(32);

            Assert.Multiple(
                () => Assert.NotEqual(second, first),
                () => Assert.Contains(first, b => b != 0));
        }

        [Fact]
        public void GenerateRandomShort_StaysInRangeAndReachesBothEnds()
        {
            var observed = new HashSet<short>();
            for (int i = 0; i < 10_000; i++)
            {
                observed.Add(SecureRandomHelper.GenerateRandomShort(1, 7));
            }

            Assert.Multiple(
                () => Assert.All(
                    new short[] { 1, 2, 3, 4, 5, 6 },
                    value => Assert.True(observed.Contains(value), $"{value} was never generated")),
                () => Assert.DoesNotContain((short)7, observed));
        }

        [Fact]
        public void GenerateRandomShort_HandlesNegativeRanges()
        {
            for (int i = 0; i < 1_000; i++)
            {
                Assert.InRange(SecureRandomHelper.GenerateRandomShort(-10, -5), -10, -6);
            }
        }

        [Fact]
        public void GenerateRandomInt_StaysInRangeAndReachesBothEnds()
        {
            var observed = new HashSet<int>();
            for (int i = 0; i < 10_000; i++)
            {
                observed.Add(SecureRandomHelper.GenerateRandomInt(1, 7));
            }

            Assert.Multiple(
                () => Assert.All(
                    new[] { 1, 2, 3, 4, 5, 6 },
                    value => Assert.True(observed.Contains(value), $"{value} was never generated")),
                () => Assert.DoesNotContain(7, observed));
        }

        [Fact]
        public void GenerateRandomInt_IsApproximatelyUniform()
        {
            AssertApproximatelyUniform(() => SecureRandomHelper.GenerateRandomInt(0, 5), buckets: 5);
        }

        [Fact]
        public void GenerateRandomInt_SpanningTheWholeDomain_DoesNotThrow()
        {
            for (int i = 0; i < 1_000; i++)
            {
                Assert.InRange(
                    SecureRandomHelper.GenerateRandomInt(int.MinValue, int.MaxValue),
                    int.MinValue,
                    int.MaxValue - 1);
            }
        }

        [Fact]
        public void GenerateRandomLong_StaysInRangeAndReachesBothEnds()
        {
            var observed = new HashSet<long>();
            for (int i = 0; i < 10_000; i++)
            {
                observed.Add(SecureRandomHelper.GenerateRandomLong(1, 7));
            }

            Assert.Multiple(
                () => Assert.All(
                    new long[] { 1, 2, 3, 4, 5, 6 },
                    value => Assert.True(observed.Contains(value), $"{value} was never generated")),
                () => Assert.DoesNotContain(7L, observed));
        }

        [Fact]
        public void GenerateRandomLong_IsApproximatelyUniform()
        {
            AssertApproximatelyUniform(() => (int)SecureRandomHelper.GenerateRandomLong(0, 5), buckets: 5);
        }

        [Fact]
        public void GenerateRandomLong_SpanningTheWholeDomain_DoesNotOverflow()
        {
            // The range size does not fit in a signed long, so the implementation must do the
            // subtraction unsigned.
            for (int i = 0; i < 1_000; i++)
            {
                Assert.InRange(
                    SecureRandomHelper.GenerateRandomLong(long.MinValue, long.MaxValue),
                    long.MinValue,
                    long.MaxValue - 1);
            }
        }

        [Fact]
        public void GenerateRandomFloat_IsAlwaysFiniteAndInRange()
        {
            // Reinterpreting random bytes as a float produced NaN or infinity roughly once every
            // 2,000 draws, which made any range assertion intermittently fail.
            for (int i = 0; i < 50_000; i++)
            {
                float value = SecureRandomHelper.GenerateRandomFloat(1f, 6f);

                Assert.True(float.IsFinite(value), $"generated a non-finite float: {value}");
                TestSupport.InHalfOpenRange(value, 1f, 6f);
            }
        }

        [Fact]
        public void GenerateRandomFloat_IsApproximatelyUniform()
        {
            AssertApproximatelyUniform(() => (int)SecureRandomHelper.GenerateRandomFloat(0f, 5f), buckets: 5);
        }

        [Fact]
        public void GenerateRandomDouble_IsAlwaysFiniteAndInRange()
        {
            for (int i = 0; i < 50_000; i++)
            {
                double value = SecureRandomHelper.GenerateRandomDouble(1d, 6d);

                Assert.True(double.IsFinite(value), $"generated a non-finite double: {value}");
                TestSupport.InHalfOpenRange(value, 1d, 6d);
            }
        }

        [Fact]
        public void GenerateRandomDouble_IsApproximatelyUniform()
        {
            // The previous implementation put roughly 60% of all draws in the first fifth of the
            // range. This is the assertion that catches that.
            AssertApproximatelyUniform(() => (int)SecureRandomHelper.GenerateRandomDouble(0d, 5d), buckets: 5);
        }

        [Fact]
        public void GenerateRandomDouble_HandlesNegativeRanges()
        {
            for (int i = 0; i < 10_000; i++)
            {
                TestSupport.InHalfOpenRange(SecureRandomHelper.GenerateRandomDouble(-3.5, -1.5), -3.5, -1.5);
            }
        }

        [Fact]
        public void GenerateRandomBool_ReturnsBothValuesAboutEqually()
        {
            int trueCount = 0;
            for (int i = 0; i < UniformitySamples; i++)
            {
                if (SecureRandomHelper.GenerateRandomBool())
                {
                    trueCount++;
                }
            }

            // Expected 50,000, standard deviation ~158. A 2,500 window is ~15 sigma.
            Assert.InRange(trueCount, 47_500, 52_500);
        }

        [Fact]
        public void GenerateRandomShort_WithMinEqualToMax_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SecureRandomHelper.GenerateRandomShort(5, 5));
        }

        [Fact]
        public void GenerateRandomInt_WithMinAboveMax_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SecureRandomHelper.GenerateRandomInt(5, 4));
        }

        [Fact]
        public void GenerateRandomLong_WithMinEqualToMax_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SecureRandomHelper.GenerateRandomLong(5, 5));
        }

        [Fact]
        public void GenerateRandomFloat_WithMinAboveMax_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SecureRandomHelper.GenerateRandomFloat(5f, 4f));
        }

        [Fact]
        public void GenerateRandomDouble_WithMinEqualToMax_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => SecureRandomHelper.GenerateRandomDouble(5d, 5d));
        }

        [Fact]
        public void GenerateRandomDouble_WithANonFiniteBound_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => SecureRandomHelper.GenerateRandomDouble(double.NaN, 1d));
        }

        [Fact]
        public void GenerateRandomDouble_WithATooWideRange_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => SecureRandomHelper.GenerateRandomDouble(double.MinValue, double.MaxValue));
        }

        /// <summary>
        /// Asserts that a bucket-producing generator spreads its output roughly evenly.
        /// </summary>
        private static void AssertApproximatelyUniform(Func<int> generateBucket, int buckets)
        {
            int[] counts = new int[buckets];

            for (int i = 0; i < UniformitySamples; i++)
            {
                int bucket = generateBucket();
                Assert.InRange(bucket, 0, buckets - 1);
                counts[bucket]++;
            }

            int expected = UniformitySamples / buckets;
            int tolerance = expected / 10;

            Assert.Multiple(
                [.. Enumerable.Range(0, buckets).Select(i => (Action)(() => Assert.True(
                    counts[i] >= expected - tolerance && counts[i] <= expected + tolerance,
                    $"bucket {i} held {counts[i]} of {UniformitySamples} draws, expected about {expected}")))]);
        }
    }
}
