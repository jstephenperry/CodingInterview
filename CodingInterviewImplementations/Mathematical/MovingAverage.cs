namespace CodingInterviewImplementations.Mathematical
{
    /// <summary>
    /// Simple moving averages over a sequence of integers.
    /// </summary>
    /// <remarks>
    /// All four overloads share one sliding-window core. The window total is carried between
    /// positions rather than recomputed, which makes the work O(n) instead of O(n * window).
    /// </remarks>
    public static class MovingAverage
    {
        private const int DefaultWindow = 3;

        /// <summary>
        /// Calculates the moving average of three elements at a time.
        /// </summary>
        public static double[] CalculateMovingAverage(int[] arr)
        {
            return CalculateMovingAverage(arr, DefaultWindow);
        }

        /// <summary>
        /// Calculates the moving average of three elements at a time.
        /// </summary>
        public static List<double> CalculateMovingAverage(List<int> arr)
        {
            return CalculateMovingAverage(arr, DefaultWindow);
        }

        /// <summary>
        /// Calculates the moving average of <paramref name="n"/> elements at a time.
        /// </summary>
        /// <param name="arr">The source values.</param>
        /// <param name="n">The window size. Must be at least one.</param>
        /// <returns>
        /// One average per window position, or an empty array when the source is shorter than the
        /// window. Previously a source shorter than the window threw <see cref="OverflowException"/>
        /// from a negative array length.
        /// </returns>
        public static double[] CalculateMovingAverage(int[] arr, int n)
        {
            ArgumentNullException.ThrowIfNull(arr);
            return Calculate(arr, n);
        }

        /// <summary>
        /// Calculates the moving average of <paramref name="n"/> elements at a time.
        /// </summary>
        /// <param name="arr">The source values.</param>
        /// <param name="n">The window size. Must be at least one.</param>
        /// <returns>
        /// One average per window position, or an empty list when the source is shorter than the
        /// window.
        /// </returns>
        public static List<double> CalculateMovingAverage(List<int> arr, int n)
        {
            ArgumentNullException.ThrowIfNull(arr);
            return [.. Calculate(System.Runtime.InteropServices.CollectionsMarshal.AsSpan(arr), n)];
        }

        private static double[] Calculate(ReadOnlySpan<int> values, int n)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);

            if (values.Length < n)
            {
                return [];
            }

            double[] result = new double[values.Length - n + 1];

            // Sum widened to long so that a window of large values cannot overflow mid-accumulation.
            long windowTotal = 0;
            for (int i = 0; i < n; i++)
            {
                windowTotal += values[i];
            }
            result[0] = (double)windowTotal / n;

            for (int i = n; i < values.Length; i++)
            {
                windowTotal += values[i] - values[i - n];
                result[i - n + 1] = (double)windowTotal / n;
            }

            return result;
        }
    }
}
