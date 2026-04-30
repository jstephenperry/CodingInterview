namespace CodingInterviewImplementations.Mathematical
{
    public static class MovingAverage
    {
        private const int DefaultWindow = 3;

        /// <summary>Computes a 3-element simple moving average over <paramref name="values"/>.</summary>
        public static double[] CalculateMovingAverage(int[] values) =>
            CalculateMovingAverage(values, DefaultWindow);

        /// <summary>Computes a 3-element simple moving average over <paramref name="values"/>.</summary>
        public static List<double> CalculateMovingAverage(List<int> values) =>
            CalculateMovingAverage(values, DefaultWindow);

        /// <summary>
        /// Computes a simple moving average with window size <paramref name="windowSize"/> over <paramref name="values"/>.
        /// Output length is <c>values.Length - windowSize + 1</c>; an empty array is returned when the input is shorter than the window.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="windowSize"/> is non-positive.</exception>
        public static double[] CalculateMovingAverage(int[] values, int windowSize)
        {
            ArgumentNullException.ThrowIfNull(values);
            return CalculateMovingAverageCore((IReadOnlyList<int>)values, windowSize);
        }

        /// <summary>
        /// Computes a simple moving average with window size <paramref name="windowSize"/> over <paramref name="values"/>.
        /// </summary>
        public static List<double> CalculateMovingAverage(List<int> values, int windowSize)
        {
            ArgumentNullException.ThrowIfNull(values);
            double[] result = CalculateMovingAverageCore(values, windowSize);
            return new List<double>(result);
        }

        private static double[] CalculateMovingAverageCore(IReadOnlyList<int> values, int windowSize)
        {
            if (windowSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be positive.");
            }
            if (values.Count < windowSize)
            {
                return Array.Empty<double>();
            }

            double[] result = new double[values.Count - windowSize + 1];
            long sum = 0;
            for (int i = 0; i < windowSize; i++)
            {
                sum += values[i];
            }
            result[0] = (double)sum / windowSize;
            for (int i = 1; i < result.Length; i++)
            {
                sum += values[i + windowSize - 1] - values[i - 1];
                result[i] = (double)sum / windowSize;
            }
            return result;
        }
    }
}
