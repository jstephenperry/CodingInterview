namespace CodingInterviewImplementations.Sorting.Tests
{
    [TestFixture]
    [TestOf(typeof(SortDiagnostics))]
    public class SortDiagnosticsTests
    {
        [Test]
        public void Diagnostics_CapturesItemCountAndAlgorithm()
        {
            int[] data = SortingTestData.Random(1024, seed: 5);
            SortDiagnostics diag = SortRunner.Run(SortAlgorithm.Quick, data);

            Assert.Multiple(() =>
            {
                Assert.That(diag.Algorithm, Is.EqualTo(nameof(SortAlgorithm.Quick)));
                Assert.That(diag.ItemCount, Is.EqualTo(1024));
            });
        }

        [Test]
        public void Diagnostics_ElapsedIsPositive_ForNonTrivialInput()
        {
            int[] data = SortingTestData.Random(50_000, seed: 1);
            SortDiagnostics diag = SortRunner.Run(SortAlgorithm.Merge, data);
            Assert.That(diag.Elapsed, Is.GreaterThan(TimeSpan.Zero));
        }

        [Test]
        public void Diagnostics_InPlaceSorts_AllocateLittle()
        {
            // Heap sort is in-place; the only managed allocation inside should be Comparer<T>.Default
            // (a single cached singleton, not per-call). Allow a small slack for JIT / dispatch overhead.
            int[] data = SortingTestData.Random(50_000, seed: 11);
            SortDiagnostics diag = SortRunner.Run(SortAlgorithm.Heap, data);

            Assert.That(diag.AllocatedBytes, Is.LessThan(64 * 1024),
                $"Heap sort should allocate very little, got {diag.AllocatedBytes} bytes.");
        }

        [Test]
        public void Diagnostics_MergeSort_AllocatesBuffer()
        {
            // Merge sort allocates an auxiliary T[] of length n. For ints (4 bytes), n=50_000 ⇒ ≥ 200_000 B.
            int[] data = SortingTestData.Random(50_000, seed: 12);
            SortDiagnostics diag = SortRunner.Run(SortAlgorithm.Merge, data);

            Assert.That(diag.AllocatedBytes, Is.GreaterThanOrEqualTo(data.Length * sizeof(int)),
                $"Merge sort should allocate at least n*sizeof(int) bytes, got {diag.AllocatedBytes}.");
        }

        [Test]
        public void Diagnostics_ToString_IsSingleLineWithKeyFields()
        {
            var diag = new SortDiagnostics(
                Algorithm: "Quick",
                ItemCount: 1000,
                Elapsed: TimeSpan.FromMilliseconds(12.345),
                AllocatedBytes: 4096,
                ManagedHeapDeltaBytes: 0,
                Gen0Collections: 0,
                Gen1Collections: 0,
                Gen2Collections: 0);

            string text = diag.ToString();

            Assert.Multiple(() =>
            {
                Assert.That(text, Does.Contain("Quick"));
                Assert.That(text, Does.Contain("1,000"));
                Assert.That(text, Does.Contain("12.345"));
                Assert.That(text, Does.Contain("4,096"));
                Assert.That(text, Does.Not.Contain('\n'));
            });
        }

        [Test]
        public void Diagnostics_ItemsPerSecond_IsPositive_ForRealisticRun()
        {
            int[] data = SortingTestData.Random(50_000, seed: 13);
            SortDiagnostics diag = SortRunner.Run(SortAlgorithm.Quick, data);
            Assert.That(diag.ItemsPerSecond, Is.GreaterThan(0));
        }

        [Test]
        public void Diagnostics_ItemsPerSecond_IsZero_ForZeroElapsed()
        {
            var diag = new SortDiagnostics("Stub", 100, TimeSpan.Zero, 0, 0, 0, 0, 0);
            Assert.That(diag.ItemsPerSecond, Is.EqualTo(0d));
        }
    }
}
