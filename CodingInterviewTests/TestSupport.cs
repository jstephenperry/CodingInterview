namespace CodingInterviewImplementations.Tests
{
    /// <summary>
    /// Assertions xUnit does not ship out of the box.
    /// </summary>
    internal static class TestSupport
    {
        /// <summary>
        /// Asserts that two sequences of doubles agree element by element within a tolerance.
        /// </summary>
        /// <remarks>
        /// xUnit has a tolerance overload for a single double but not for a sequence of them, so
        /// the comparison is done per element here.
        /// </remarks>
        public static void EqualWithin(
            IReadOnlyList<double> expected, IReadOnlyList<double> actual, double tolerance)
        {
            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i], actual[i], tolerance);
            }
        }

        /// <summary>
        /// Asserts that a value falls in [<paramref name="min"/>, <paramref name="max"/>), with the
        /// upper bound excluded.
        /// </summary>
        /// <remarks>
        /// <c>Assert.InRange</c> includes both ends, which cannot express the half-open range that
        /// every generator in <c>SecureRandomHelper</c> produces.
        /// </remarks>
        public static void InHalfOpenRange(double value, double min, double max)
        {
            Assert.True(
                value >= min && value < max,
                $"expected a value in [{min}, {max}) but got {value}");
        }

        /// <summary>
        /// Asserts that two collections hold the same elements, ignoring order.
        /// </summary>
        public static void EquivalentTo<T>(IEnumerable<T> expected, IEnumerable<T> actual)
            where T : IComparable<T>
        {
            Assert.Equal(expected.Order(), actual.Order());
        }
    }
}
