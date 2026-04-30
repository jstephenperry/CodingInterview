namespace CodingInterviewImplementations.Mathematical
{
    public class MovingAverage
    {
        /// <summary>
        /// Calculate the moving average of three elements at a time in an array
        /// </summary>
        public static double[] CalculateMovingAverage(int[] arr)
        {
            return CalculateMovingAverage(arr, 3);
        }

        /// <summary>
        /// Calculate the moving average of three elements at a time in a list
        /// </summary>
        public static List<double> CalculateMovingAverage(List<int> arr)
        {
            return CalculateMovingAverage(arr, 3);
        }

        /// <summary>
        /// Calculate the moving average of n elements at a time in an array
        /// </summary>
        public static double[] CalculateMovingAverage(int[] arr, int n)
        {
            if (arr == null) throw new ArgumentNullException(nameof(arr));
            if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (arr.Length < n) return Array.Empty<double>();

            double[] result = new double[arr.Length - n + 1];
            long sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += arr[i];
            }
            result[0] = (double)sum / n;
            for (int i = 1; i < result.Length; i++)
            {
                sum += arr[i + n - 1] - arr[i - 1];
                result[i] = (double)sum / n;
            }
            return result;
        }

        /// <summary>
        /// Calculate the moving average of n elements at a time in a list
        /// </summary>
        public static List<double> CalculateMovingAverage(List<int> arr, int n)
        {
            if (arr == null) throw new ArgumentNullException(nameof(arr));
            return CalculateMovingAverage(arr.ToArray(), n).ToList();
        }
    }
}
