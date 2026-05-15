namespace CodingInterviewImplementations.Sorting
{
    /// <summary>
    /// Non-comparison integer sorts. Both algorithms are stable when input order is preserved
    /// among equal keys (radix sort below is stable; counting sort writes from buckets and is
    /// stable with respect to value identity since equal ints are indistinguishable).
    /// </summary>
    public static class IntegerSort
    {
        /// <summary>
        /// Maximum value range (<c>max - min + 1</c>) <see cref="CountingSort(Span{int})"/> will accept.
        /// Counting sort allocates an int[] of this size, so a guard is needed to avoid runaway memory.
        /// </summary>
        public const long MaxCountingSortRange = 1 << 24;

        /// <summary>
        /// Sorts <paramref name="data"/> in place using counting sort. O(n + k) time and O(k) memory
        /// where k is <c>max(data) - min(data) + 1</c>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value range exceeds <see cref="MaxCountingSortRange"/>.</exception>
        public static void CountingSort(Span<int> data)
        {
            if (data.Length < 2)
            {
                return;
            }

            int min = data[0];
            int max = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                int v = data[i];
                if (v < min)
                {
                    min = v;
                }
                else if (v > max)
                {
                    max = v;
                }
            }

            long range = (long)max - min + 1;
            if (range > MaxCountingSortRange)
            {
                throw new InvalidOperationException(
                    $"Counting sort range {range:N0} exceeds the maximum allowed range of {MaxCountingSortRange:N0}. " +
                    $"Use {nameof(RadixSort)} for sparse or wide-range integer data.");
            }

            int[] counts = new int[range];
            for (int i = 0; i < data.Length; i++)
            {
                counts[data[i] - min]++;
            }

            int idx = 0;
            for (int v = 0; v < counts.Length; v++)
            {
                int c = counts[v];
                for (int k = 0; k < c; k++)
                {
                    data[idx++] = v + min;
                }
            }
        }

        /// <inheritdoc cref="CountingSort(Span{int})"/>
        public static void CountingSort(int[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            CountingSort(data.AsSpan());
        }

        /// <summary>
        /// Sorts <paramref name="data"/> in place using LSD radix sort (8-bit passes, four passes total).
        /// O(n) time, O(n) auxiliary memory. Handles the entire <see cref="int"/> range by offsetting
        /// signed values into an unsigned key.
        /// </summary>
        public static void RadixSort(Span<int> data)
        {
            if (data.Length < 2)
            {
                return;
            }

            int min = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] < min)
                {
                    min = data[i];
                }
            }

            // Map signed -> unsigned by shifting so min becomes 0. The full int range maps cleanly
            // into uint (since (long)int.MaxValue - (long)int.MinValue == uint.MaxValue).
            long offset = -(long)min;
            uint[] keys = new uint[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                keys[i] = (uint)(data[i] + offset);
            }

            const int BitsPerPass = 8;
            const int BucketCount = 1 << BitsPerPass;
            const int BucketMask = BucketCount - 1;

            uint[] aux = new uint[data.Length];
            int[] count = new int[BucketCount];

            for (int shift = 0; shift < 32; shift += BitsPerPass)
            {
                Array.Clear(count);

                for (int i = 0; i < keys.Length; i++)
                {
                    count[(int)((keys[i] >> shift) & BucketMask)]++;
                }
                for (int i = 1; i < BucketCount; i++)
                {
                    count[i] += count[i - 1];
                }
                // Stable scatter: iterate in reverse so equal-keyed elements keep their relative order.
                for (int i = keys.Length - 1; i >= 0; i--)
                {
                    int bucket = (int)((keys[i] >> shift) & BucketMask);
                    aux[--count[bucket]] = keys[i];
                }

                (keys, aux) = (aux, keys);
            }

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (int)((long)keys[i] - offset);
            }
        }

        /// <inheritdoc cref="RadixSort(Span{int})"/>
        public static void RadixSort(int[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            RadixSort(data.AsSpan());
        }
    }
}
