using CodingInterviewImplementations.Tests;

namespace CodingInterviewImplementations.Mathematical.Tests
{
    public class MovingAverageTests
    {
        [Fact]
        public void CalculateMovingAverage_OverAnArray()
        {
            Assert.Multiple(
                () => Assert.Equal(
                    new double[] { 2, 3, 4, 5 },
                    MovingAverage.CalculateMovingAverage(new int[] { 1, 2, 3, 4, 5, 6 })),
                () => Assert.Equal(
                    new double[] { 1.5, 2.5, 3.5, 4.5, 5.5 },
                    MovingAverage.CalculateMovingAverage(new int[] { 1, 2, 3, 4, 5, 6 }, 2)));
        }

        [Fact]
        public void CalculateMovingAverage_OverAList()
        {
            Assert.Multiple(
                () => Assert.Equal(
                    new List<double> { 2, 3, 4, 5 },
                    MovingAverage.CalculateMovingAverage(new List<int> { 1, 2, 3, 4, 5, 6 })),
                () => Assert.Equal(
                    new List<double> { 1.5, 2.5, 3.5, 4.5, 5.5 },
                    MovingAverage.CalculateMovingAverage(new List<int> { 1, 2, 3, 4, 5, 6 }, 2)));
        }

        [Fact]
        public void CalculateMovingAverage_WithAWindowOfOne_ReturnsTheInput()
        {
            Assert.Equal(
                new double[] { 4, 8, 15 },
                MovingAverage.CalculateMovingAverage(new int[] { 4, 8, 15 }, 1));
        }

        [Fact]
        public void CalculateMovingAverage_WithAWindowSpanningTheWholeInput_ReturnsOneValue()
        {
            Assert.Equal(
                new double[] { 2 },
                MovingAverage.CalculateMovingAverage(new int[] { 1, 2, 3 }, 3));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void CalculateMovingAverage_WithInputShorterThanTheWindow_ReturnsEmpty(int length)
        {
            // The previous implementation sized the result array as length - 2 and threw
            // OverflowException from a negative array length for any input shorter than three.
            int[] shortInput = Enumerable.Range(1, length).ToArray();

            Assert.Multiple(
                () => Assert.Empty(MovingAverage.CalculateMovingAverage(shortInput)),
                () => Assert.Empty(MovingAverage.CalculateMovingAverage(shortInput.ToList())));
        }

        [Fact]
        public void CalculateMovingAverage_DoesNotOverflowOnLargeValues()
        {
            // A window of large ints overflows if the running total is accumulated as an int.
            int[] input = new[] { int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue };

            TestSupport.EqualWithin(
                new double[] { int.MaxValue, int.MaxValue },
                MovingAverage.CalculateMovingAverage(input, 3),
                1e-6);
        }

        [Fact]
        public void CalculateMovingAverage_HandlesNegativeValues()
        {
            Assert.Equal(
                new double[] { 0, 3 },
                MovingAverage.CalculateMovingAverage(new int[] { -3, 0, 3, 6 }, 3));
        }

        [Fact]
        public void CalculateMovingAverage_MatchesADirectRecomputationOverALongInput()
        {
            // Guards the sliding-window optimisation against drift: the incremental total must
            // agree with summing each window from scratch.
            int[] input = Enumerable.Range(0, 500).Select(i => ((i * 37) % 101) - 50).ToArray();
            const int Window = 7;

            double[] actual = MovingAverage.CalculateMovingAverage(input, Window);
            double[] expected = Enumerable
                .Range(0, input.Length - Window + 1)
                .Select(i => input.Skip(i).Take(Window).Average())
                .ToArray();

            TestSupport.EqualWithin(expected, actual, 1e-9);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CalculateMovingAverage_WithANonPositiveWindow_Throws(int window)
        {
            Assert.Multiple(
                () => Assert.Throws<ArgumentOutOfRangeException>(
                    () => MovingAverage.CalculateMovingAverage(new int[] { 1, 2, 3 }, window)),
                () => Assert.Throws<ArgumentOutOfRangeException>(
                    () => MovingAverage.CalculateMovingAverage(new List<int> { 1, 2, 3 }, window)));
        }

        [Fact]
        public void CalculateMovingAverage_WithANullSource_Throws()
        {
            Assert.Multiple(
                () => Assert.Throws<ArgumentNullException>(
                    () => MovingAverage.CalculateMovingAverage((int[])null!)),
                () => Assert.Throws<ArgumentNullException>(
                    () => MovingAverage.CalculateMovingAverage((List<int>)null!)));
        }
    }
}
