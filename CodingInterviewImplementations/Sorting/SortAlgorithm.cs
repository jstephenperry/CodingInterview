namespace CodingInterviewImplementations.Sorting
{
    /// <summary>
    /// Comparison-based sorting algorithms exposed by <see cref="Sorter"/> and selectable
    /// through <see cref="SortRunner.Run{T}(SortAlgorithm, T[], System.Collections.Generic.IComparer{T}?)"/>.
    /// </summary>
    public enum SortAlgorithm
    {
        /// <summary>Bubble sort — stable, O(n²) worst/average, O(n) best (early-exit when no swaps).</summary>
        Bubble,

        /// <summary>Insertion sort — stable, O(n²) worst, O(n) on nearly-sorted input.</summary>
        Insertion,

        /// <summary>Selection sort — unstable, O(n²) regardless of input order.</summary>
        Selection,

        /// <summary>Shell sort — unstable, ~O(n log² n) with the Ciura gap sequence.</summary>
        Shell,

        /// <summary>Top-down merge sort — stable, O(n log n) worst-case, O(n) auxiliary memory.</summary>
        Merge,

        /// <summary>Introspective-style quick sort — unstable, O(n log n) average, in-place with median-of-three pivot and small-range insertion-sort cutoff.</summary>
        Quick,

        /// <summary>Heap sort — unstable, O(n log n) worst-case, in-place.</summary>
        Heap,
    }
}
