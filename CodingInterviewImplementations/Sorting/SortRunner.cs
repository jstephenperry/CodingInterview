using System.Diagnostics;

namespace CodingInterviewImplementations.Sorting
{
    /// <summary>
    /// Runs a sorting algorithm while recording timing and memory diagnostics.
    /// </summary>
    public static class SortRunner
    {
        /// <summary>
        /// Sorts <paramref name="data"/> in place with the requested <paramref name="algorithm"/>
        /// and returns timing/memory diagnostics for the sort.
        /// </summary>
        public static SortDiagnostics Run<T>(SortAlgorithm algorithm, T[] data, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            return RunInternal(algorithm.ToString(), data.Length, () => Dispatch(algorithm, data, comparer));
        }

        /// <summary>
        /// Sorts <paramref name="data"/> in place using <see cref="IntegerSort.CountingSort(int[])"/> and returns diagnostics.
        /// </summary>
        public static SortDiagnostics RunCountingSort(int[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            return RunInternal(nameof(IntegerSort.CountingSort), data.Length, () => IntegerSort.CountingSort(data));
        }

        /// <summary>
        /// Sorts <paramref name="data"/> in place using <see cref="IntegerSort.RadixSort(int[])"/> and returns diagnostics.
        /// </summary>
        public static SortDiagnostics RunRadixSort(int[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            return RunInternal(nameof(IntegerSort.RadixSort), data.Length, () => IntegerSort.RadixSort(data));
        }

        private static void Dispatch<T>(SortAlgorithm algorithm, T[] data, IComparer<T>? comparer)
        {
            switch (algorithm)
            {
                case SortAlgorithm.Bubble:
                    Sorter.BubbleSort(data, comparer);
                    break;
                case SortAlgorithm.Insertion:
                    Sorter.InsertionSort(data, comparer);
                    break;
                case SortAlgorithm.Selection:
                    Sorter.SelectionSort(data, comparer);
                    break;
                case SortAlgorithm.Shell:
                    Sorter.ShellSort(data, comparer);
                    break;
                case SortAlgorithm.Merge:
                    Sorter.MergeSort(data, comparer);
                    break;
                case SortAlgorithm.Quick:
                    Sorter.QuickSort(data, comparer);
                    break;
                case SortAlgorithm.Heap:
                    Sorter.HeapSort(data, comparer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, "Unknown sorting algorithm.");
            }
        }

        private static SortDiagnostics RunInternal(string algorithmName, int itemCount, Action sort)
        {
            // Settle the heap before measuring so previous test allocations don't leak into the delta.
            // Two collections plus a finalizer wait is a common idiom for getting to a stable baseline.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long allocStart = GC.GetAllocatedBytesForCurrentThread();
            long heapStart = GC.GetTotalMemory(forceFullCollection: false);
            int gen0Start = GC.CollectionCount(0);
            int gen1Start = GC.CollectionCount(1);
            int gen2Start = GC.CollectionCount(2);

            Stopwatch stopwatch = Stopwatch.StartNew();
            sort();
            stopwatch.Stop();

            long allocEnd = GC.GetAllocatedBytesForCurrentThread();
            long heapEnd = GC.GetTotalMemory(forceFullCollection: false);

            return new SortDiagnostics(
                Algorithm: algorithmName,
                ItemCount: itemCount,
                Elapsed: stopwatch.Elapsed,
                AllocatedBytes: allocEnd - allocStart,
                ManagedHeapDeltaBytes: heapEnd - heapStart,
                Gen0Collections: GC.CollectionCount(0) - gen0Start,
                Gen1Collections: GC.CollectionCount(1) - gen1Start,
                Gen2Collections: GC.CollectionCount(2) - gen2Start);
        }
    }
}
