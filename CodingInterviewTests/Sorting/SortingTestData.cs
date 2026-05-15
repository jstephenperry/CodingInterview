namespace CodingInterviewImplementations.Sorting.Tests
{
    /// <summary>
    /// Deterministic input generators and shared algorithm sets used by the sorting test fixtures.
    /// </summary>
    internal static class SortingTestData
    {
        /// <summary>Quadratic-complexity algorithms get a smaller scale so each test stays inside the second-budget.</summary>
        public const int QuadraticScale = 10_000;

        /// <summary>O(n log n) algorithms can comfortably handle a much larger payload.</summary>
        public const int SubquadraticScale = 200_000;

        /// <summary>Integer-specific O(n)/O(n+k) sorts run at the larger scale too.</summary>
        public const int LinearIntegerScale = 500_000;

        public static readonly SortAlgorithm[] AllAlgorithms = Enum.GetValues<SortAlgorithm>();

        public static readonly SortAlgorithm[] QuadraticAlgorithms =
        {
            SortAlgorithm.Bubble,
            SortAlgorithm.Insertion,
            SortAlgorithm.Selection,
        };

        public static readonly SortAlgorithm[] SubquadraticAlgorithms =
        {
            SortAlgorithm.Shell,
            SortAlgorithm.Merge,
            SortAlgorithm.Quick,
            SortAlgorithm.Heap,
        };

        public static int[] Random(int count, int seed, int min = int.MinValue, int max = int.MaxValue)
        {
            var rng = new Random(seed);
            int[] arr = new int[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = rng.Next(min, max);
            }
            return arr;
        }

        public static int[] Ascending(int count)
        {
            int[] arr = new int[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = i;
            }
            return arr;
        }

        public static int[] Descending(int count)
        {
            int[] arr = new int[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = count - i - 1;
            }
            return arr;
        }

        public static int[] AllEqual(int count, int value = 7)
        {
            int[] arr = new int[count];
            Array.Fill(arr, value);
            return arr;
        }

        /// <summary>Returns a sorted copy via the framework's tested implementation for assertion baselines.</summary>
        public static int[] BaselineSorted(int[] input)
        {
            int[] copy = (int[])input.Clone();
            Array.Sort(copy);
            return copy;
        }
    }
}
