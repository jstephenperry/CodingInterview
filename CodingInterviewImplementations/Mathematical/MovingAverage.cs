namespace CodingInterviewImplementations.Mathematical
{
    public class MovingAverage
    {
        /// <summary>
        /// Calculate the moving average of three elements at a time in an array
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double[] CalculateMovingAverage(int[] arr)
        {
            double[] result = new double[arr.Length - 2];
            for (int i = 0; i < arr.Length - 2; i++)
            {
                result[i] = (arr[i] + arr[i + 1] + arr[i + 2]) / 3.0;
            }
            return result;
        }

        /// <summary>
        /// Calculate the moving average of three elements at a time in a list
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static List<double> CalculateMovingAverage(List<int> arr)
        {
            List<double> result = new();
            for (int i = 0; i < arr.Count - 2; i++)
            {
                result.Add((arr[i] + arr[i + 1] + arr[i + 2]) / 3.0);
            }
            return result;
        }

        /// <summary>
        /// Calculate the moving average of n elements at a time in an array
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double[] CalculateMovingAverage(int[] arr, int n)
        {
            double[] result = new double[arr.Length - n + 1];
            for (int i = 0; i < arr.Length - n + 1; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += arr[i + j];
                }
                result[i] = sum / n;
            }
            return result;
        }

        /// <summary>
        /// Calculate the moving average of n elements at a time in a list
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<double> CalculateMovingAverage(List<int> arr, int n)
        {
            List<double> result = new();
            for (int i = 0; i < arr.Count - n + 1; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += arr[i + j];
                }
                result.Add(sum / n);
            }
            return result;
        }
    }
}
