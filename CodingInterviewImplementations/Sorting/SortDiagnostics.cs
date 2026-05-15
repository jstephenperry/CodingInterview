using System.Globalization;

namespace CodingInterviewImplementations.Sorting
{
    /// <summary>
    /// Captured timing and memory metrics for a single sort invocation.
    /// </summary>
    /// <param name="Algorithm">Human-readable algorithm name.</param>
    /// <param name="ItemCount">Number of elements sorted.</param>
    /// <param name="Elapsed">Wall-clock elapsed time from <see cref="System.Diagnostics.Stopwatch"/>.</param>
    /// <param name="AllocatedBytes">Bytes allocated on the calling thread during the sort, from <see cref="GC.GetAllocatedBytesForCurrentThread"/>.</param>
    /// <param name="ManagedHeapDeltaBytes">Delta of <see cref="GC.GetTotalMemory(bool)"/> across the sort. May be negative if a collection happens to fire mid-sort.</param>
    /// <param name="Gen0Collections">Number of gen-0 collections observed during the sort.</param>
    /// <param name="Gen1Collections">Number of gen-1 collections observed during the sort.</param>
    /// <param name="Gen2Collections">Number of gen-2 collections observed during the sort.</param>
    public readonly record struct SortDiagnostics(
        string Algorithm,
        int ItemCount,
        TimeSpan Elapsed,
        long AllocatedBytes,
        long ManagedHeapDeltaBytes,
        int Gen0Collections,
        int Gen1Collections,
        int Gen2Collections)
    {
        /// <summary>Throughput in items per second, or zero if <see cref="Elapsed"/> is zero.</summary>
        public double ItemsPerSecond =>
            Elapsed > TimeSpan.Zero ? ItemCount / Elapsed.TotalSeconds : 0d;

        /// <summary>Returns a compact, human-readable single-line summary of this diagnostic.</summary>
        public override string ToString()
        {
            CultureInfo c = CultureInfo.InvariantCulture;
            return string.Create(
                c,
                $"{Algorithm}: n={ItemCount.ToString("N0", c)}, " +
                $"elapsed={Elapsed.TotalMilliseconds.ToString("F3", c)}ms, " +
                $"allocated={AllocatedBytes.ToString("N0", c)}B, " +
                $"heapDelta={ManagedHeapDeltaBytes.ToString("N0", c)}B, " +
                $"gc=[{Gen0Collections},{Gen1Collections},{Gen2Collections}], " +
                $"throughput={ItemsPerSecond.ToString("N0", c)}/s");
        }
    }
}
