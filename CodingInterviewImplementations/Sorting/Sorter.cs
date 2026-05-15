namespace CodingInterviewImplementations.Sorting
{
    /// <summary>
    /// Generic comparison-based sorting algorithms operating in place over <see cref="Span{T}"/>.
    /// Each method also has an array overload for convenience.
    /// Pass an <see cref="IComparer{T}"/> to use a custom ordering; <see langword="null"/> uses <see cref="Comparer{T}.Default"/>.
    /// </summary>
    public static class Sorter
    {
        // Below this partition size, quick-sort hands off to insertion sort, which has lower
        // constant factors on tiny ranges.
        private const int InsertionSortThreshold = 16;

        // Ciura's empirically-derived gap sequence for shell sort, extended downward to 1.
        // Truncated at 701 — enough for inputs well into the tens of millions; the inner loop
        // skips gaps that are >= n.
        private static readonly int[] ShellGaps = { 701, 301, 132, 57, 23, 10, 4, 1 };

        /// <summary>Sorts <paramref name="data"/> in place using bubble sort.</summary>
        public static void BubbleSort<T>(Span<T> data, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            int n = data.Length;
            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;
                int end = n - 1 - i;
                for (int j = 0; j < end; j++)
                {
                    if (comparer.Compare(data[j], data[j + 1]) > 0)
                    {
                        (data[j], data[j + 1]) = (data[j + 1], data[j]);
                        swapped = true;
                    }
                }
                if (!swapped)
                {
                    return;
                }
            }
        }

        /// <inheritdoc cref="BubbleSort{T}(Span{T}, IComparer{T}?)"/>
        public static void BubbleSort<T>(T[] data, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            BubbleSort(data.AsSpan(), comparer);
        }

        /// <summary>Sorts <paramref name="data"/> in place using insertion sort.</summary>
        public static void InsertionSort<T>(Span<T> data, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            for (int i = 1; i < data.Length; i++)
            {
                T key = data[i];
                int j = i - 1;
                while (j >= 0 && comparer.Compare(data[j], key) > 0)
                {
                    data[j + 1] = data[j];
                    j--;
                }
                data[j + 1] = key;
            }
        }

        /// <inheritdoc cref="InsertionSort{T}(Span{T}, IComparer{T}?)"/>
        public static void InsertionSort<T>(T[] data, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            InsertionSort(data.AsSpan(), comparer);
        }

        /// <summary>Sorts <paramref name="data"/> in place using selection sort.</summary>
        public static void SelectionSort<T>(Span<T> data, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            int n = data.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (comparer.Compare(data[j], data[minIdx]) < 0)
                    {
                        minIdx = j;
                    }
                }
                if (minIdx != i)
                {
                    (data[i], data[minIdx]) = (data[minIdx], data[i]);
                }
            }
        }

        /// <inheritdoc cref="SelectionSort{T}(Span{T}, IComparer{T}?)"/>
        public static void SelectionSort<T>(T[] data, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            SelectionSort(data.AsSpan(), comparer);
        }

        /// <summary>Sorts <paramref name="data"/> in place using shell sort with Ciura's gap sequence.</summary>
        public static void ShellSort<T>(Span<T> data, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            int n = data.Length;
            foreach (int gap in ShellGaps)
            {
                if (gap >= n)
                {
                    continue;
                }
                for (int i = gap; i < n; i++)
                {
                    T temp = data[i];
                    int j = i;
                    while (j >= gap && comparer.Compare(data[j - gap], temp) > 0)
                    {
                        data[j] = data[j - gap];
                        j -= gap;
                    }
                    data[j] = temp;
                }
            }
        }

        /// <inheritdoc cref="ShellSort{T}(Span{T}, IComparer{T}?)"/>
        public static void ShellSort<T>(T[] data, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            ShellSort(data.AsSpan(), comparer);
        }

        /// <summary>Sorts <paramref name="data"/> in place using stable top-down merge sort. Allocates a single auxiliary buffer of length <c>data.Length</c>.</summary>
        public static void MergeSort<T>(Span<T> data, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            if (data.Length < 2)
            {
                return;
            }

            T[] buffer = new T[data.Length];
            MergeSortInternal(data, buffer, 0, data.Length - 1, comparer);
        }

        /// <inheritdoc cref="MergeSort{T}(Span{T}, IComparer{T}?)"/>
        public static void MergeSort<T>(T[] data, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            MergeSort(data.AsSpan(), comparer);
        }

        private static void MergeSortInternal<T>(Span<T> data, T[] buffer, int left, int right, IComparer<T> comparer)
        {
            // Cut over to insertion sort for tiny ranges — fewer recursive frames, better caches.
            if (right - left + 1 <= InsertionSortThreshold)
            {
                InsertionSortRange(data, left, right, comparer);
                return;
            }

            int mid = left + ((right - left) >> 1);
            MergeSortInternal(data, buffer, left, mid, comparer);
            MergeSortInternal(data, buffer, mid + 1, right, comparer);

            // Skip the merge entirely if the two runs are already in order.
            if (comparer.Compare(data[mid], data[mid + 1]) <= 0)
            {
                return;
            }

            Merge(data, buffer, left, mid, right, comparer);
        }

        private static void Merge<T>(Span<T> data, T[] buffer, int left, int mid, int right, IComparer<T> comparer)
        {
            int i = left, j = mid + 1, k = left;
            while (i <= mid && j <= right)
            {
                if (comparer.Compare(data[i], data[j]) <= 0)
                {
                    buffer[k++] = data[i++];
                }
                else
                {
                    buffer[k++] = data[j++];
                }
            }
            while (i <= mid)
            {
                buffer[k++] = data[i++];
            }
            while (j <= right)
            {
                buffer[k++] = data[j++];
            }
            for (int idx = left; idx <= right; idx++)
            {
                data[idx] = buffer[idx];
            }
        }

        /// <summary>
        /// Sorts <paramref name="data"/> in place using quicksort with a median-of-three pivot,
        /// small-range insertion-sort cutoff, and tail-recursion elimination on the larger partition.
        /// </summary>
        public static void QuickSort<T>(Span<T> data, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            if (data.Length < 2)
            {
                return;
            }
            QuickSortInternal(data, 0, data.Length - 1, comparer);
        }

        /// <inheritdoc cref="QuickSort{T}(Span{T}, IComparer{T}?)"/>
        public static void QuickSort<T>(T[] data, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            QuickSort(data.AsSpan(), comparer);
        }

        private static void QuickSortInternal<T>(Span<T> data, int left, int right, IComparer<T> comparer)
        {
            while (left < right)
            {
                if (right - left + 1 <= InsertionSortThreshold)
                {
                    InsertionSortRange(data, left, right, comparer);
                    return;
                }

                int pivotIdx = MedianOfThree(data, left, right, comparer);
                (data[pivotIdx], data[right]) = (data[right], data[pivotIdx]);
                T pivot = data[right];

                int store = left;
                for (int j = left; j < right; j++)
                {
                    if (comparer.Compare(data[j], pivot) <= 0)
                    {
                        (data[store], data[j]) = (data[j], data[store]);
                        store++;
                    }
                }
                (data[store], data[right]) = (data[right], data[store]);

                // Recurse on the smaller partition; iterate on the larger.
                // This bounds the recursion depth at O(log n) regardless of pivot quality.
                if (store - left < right - store)
                {
                    QuickSortInternal(data, left, store - 1, comparer);
                    left = store + 1;
                }
                else
                {
                    QuickSortInternal(data, store + 1, right, comparer);
                    right = store - 1;
                }
            }
        }

        private static int MedianOfThree<T>(Span<T> data, int left, int right, IComparer<T> comparer)
        {
            int mid = left + ((right - left) >> 1);
            if (comparer.Compare(data[left], data[mid]) > 0)
            {
                (data[left], data[mid]) = (data[mid], data[left]);
            }
            if (comparer.Compare(data[left], data[right]) > 0)
            {
                (data[left], data[right]) = (data[right], data[left]);
            }
            if (comparer.Compare(data[mid], data[right]) > 0)
            {
                (data[mid], data[right]) = (data[right], data[mid]);
            }
            return mid;
        }

        private static void InsertionSortRange<T>(Span<T> data, int left, int right, IComparer<T> comparer)
        {
            for (int i = left + 1; i <= right; i++)
            {
                T key = data[i];
                int j = i - 1;
                while (j >= left && comparer.Compare(data[j], key) > 0)
                {
                    data[j + 1] = data[j];
                    j--;
                }
                data[j + 1] = key;
            }
        }

        /// <summary>Sorts <paramref name="data"/> in place using heap sort (in-place binary max-heap).</summary>
        public static void HeapSort<T>(Span<T> data, IComparer<T>? comparer = null)
        {
            comparer ??= Comparer<T>.Default;
            int n = data.Length;
            for (int i = (n / 2) - 1; i >= 0; i--)
            {
                SiftDown(data, i, n, comparer);
            }
            for (int i = n - 1; i > 0; i--)
            {
                (data[0], data[i]) = (data[i], data[0]);
                SiftDown(data, 0, i, comparer);
            }
        }

        /// <inheritdoc cref="HeapSort{T}(Span{T}, IComparer{T}?)"/>
        public static void HeapSort<T>(T[] data, IComparer<T>? comparer = null)
        {
            ArgumentNullException.ThrowIfNull(data);
            HeapSort(data.AsSpan(), comparer);
        }

        private static void SiftDown<T>(Span<T> data, int start, int end, IComparer<T> comparer)
        {
            int root = start;
            while ((2 * root) + 1 < end)
            {
                int child = (2 * root) + 1;
                if (child + 1 < end && comparer.Compare(data[child], data[child + 1]) < 0)
                {
                    child++;
                }
                if (comparer.Compare(data[root], data[child]) >= 0)
                {
                    return;
                }
                (data[root], data[child]) = (data[child], data[root]);
                root = child;
            }
        }
    }
}
