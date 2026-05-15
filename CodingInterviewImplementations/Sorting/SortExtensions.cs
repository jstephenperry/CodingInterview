namespace CodingInterviewImplementations.Sorting
{
    /// <summary>
    /// Extension-method front for <see cref="Sorter"/> and <see cref="IntegerSort"/>.
    /// In-place overloads operate on the supplied array; <c>*Sorted</c> overloads accept
    /// any <see cref="IEnumerable{T}"/> and return a freshly-allocated sorted copy.
    /// </summary>
    public static class SortExtensions
    {
        /// <summary>In-place bubble sort over <paramref name="data"/>.</summary>
        public static void BubbleSort<T>(this T[] data, IComparer<T>? comparer = null) =>
            Sorter.BubbleSort(data, comparer);

        /// <summary>In-place insertion sort over <paramref name="data"/>.</summary>
        public static void InsertionSort<T>(this T[] data, IComparer<T>? comparer = null) =>
            Sorter.InsertionSort(data, comparer);

        /// <summary>In-place selection sort over <paramref name="data"/>.</summary>
        public static void SelectionSort<T>(this T[] data, IComparer<T>? comparer = null) =>
            Sorter.SelectionSort(data, comparer);

        /// <summary>In-place shell sort over <paramref name="data"/>.</summary>
        public static void ShellSort<T>(this T[] data, IComparer<T>? comparer = null) =>
            Sorter.ShellSort(data, comparer);

        /// <summary>In-place merge sort over <paramref name="data"/>.</summary>
        public static void MergeSort<T>(this T[] data, IComparer<T>? comparer = null) =>
            Sorter.MergeSort(data, comparer);

        /// <summary>In-place quick sort over <paramref name="data"/>.</summary>
        public static void QuickSort<T>(this T[] data, IComparer<T>? comparer = null) =>
            Sorter.QuickSort(data, comparer);

        /// <summary>In-place heap sort over <paramref name="data"/>.</summary>
        public static void HeapSort<T>(this T[] data, IComparer<T>? comparer = null) =>
            Sorter.HeapSort(data, comparer);

        /// <summary>
        /// In-place sort over <paramref name="data"/> using the chosen <paramref name="algorithm"/>,
        /// returning timing/memory diagnostics for the sort.
        /// </summary>
        public static SortDiagnostics Sort<T>(this T[] data, SortAlgorithm algorithm, IComparer<T>? comparer = null) =>
            SortRunner.Run(algorithm, data, comparer);

        /// <summary>Returns a freshly-allocated array of <paramref name="source"/> sorted with <paramref name="algorithm"/>.</summary>
        public static T[] Sorted<T>(this IEnumerable<T> source, SortAlgorithm algorithm, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(source);
            T[] copy = source.ToArray();
            SortRunner.Run(algorithm, copy, comparer);
            return copy;
        }

        /// <summary>In-place counting sort over <paramref name="data"/>.</summary>
        public static void CountingSort(this int[] data) => IntegerSort.CountingSort(data);

        /// <summary>In-place radix sort over <paramref name="data"/>.</summary>
        public static void RadixSort(this int[] data) => IntegerSort.RadixSort(data);
    }
}
