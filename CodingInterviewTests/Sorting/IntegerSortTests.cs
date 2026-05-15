namespace CodingInterviewImplementations.Sorting.Tests
{
    [TestFixture]
    [TestOf(typeof(IntegerSort))]
    public class IntegerSortTests
    {
        [Test]
        public void CountingSort_EmptyArray_NoOp()
        {
            int[] data = Array.Empty<int>();
            IntegerSort.CountingSort(data);
            Assert.That(data, Is.Empty);
        }

        [Test]
        public void CountingSort_SingleElement_NoOp()
        {
            int[] data = { 17 };
            IntegerSort.CountingSort(data);
            Assert.That(data, Is.EqualTo(new[] { 17 }));
        }

        [Test]
        public void CountingSort_SmallInput_SortsCorrectly()
        {
            int[] data = { 5, -1, 3, 9, 0, 5, 2, -7, 100, 5 };
            int[] expected = { -7, -1, 0, 2, 3, 5, 5, 5, 9, 100 };
            IntegerSort.CountingSort(data);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void CountingSort_AllNegative_SortsCorrectly()
        {
            int[] data = { -3, -10, -1, -7, -3, -5 };
            int[] expected = { -10, -7, -5, -3, -3, -1 };
            IntegerSort.CountingSort(data);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void CountingSort_ModerateScale_DenseRange_SortsCorrectly()
        {
            int[] data = SortingTestData.Random(SortingTestData.LinearIntegerScale, seed: 7, min: -10_000, max: 10_000);
            int[] expected = SortingTestData.BaselineSorted(data);

            SortDiagnostics diag = SortRunner.RunCountingSort(data);

            Assert.Multiple(() =>
            {
                Assert.That(data, Is.EqualTo(expected));
                Assert.That(diag.Elapsed, Is.GreaterThan(TimeSpan.Zero));
                Assert.That(diag.AllocatedBytes, Is.GreaterThan(0), "Counting sort allocates a count array.");
            });

            TestContext.WriteLine(diag.ToString());
        }

        [Test]
        public void CountingSort_RangeTooLarge_Throws()
        {
            int[] data = { int.MinValue, int.MaxValue };
            Assert.That(() => IntegerSort.CountingSort(data), Throws.InvalidOperationException);
        }

        [Test]
        public void RadixSort_EmptyArray_NoOp()
        {
            int[] data = Array.Empty<int>();
            IntegerSort.RadixSort(data);
            Assert.That(data, Is.Empty);
        }

        [Test]
        public void RadixSort_SmallInput_SortsCorrectly()
        {
            int[] data = { 5, -1, 3, 9, 0, 5, 2, -7, 100, 5 };
            int[] expected = { -7, -1, 0, 2, 3, 5, 5, 5, 9, 100 };
            IntegerSort.RadixSort(data);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void RadixSort_FullIntRange_SortsCorrectly()
        {
            // Spans both signed extremes to ensure offset/unsigned-key handling is correct.
            int[] data = { int.MaxValue, 0, int.MinValue, -1, 1, int.MinValue + 1, int.MaxValue - 1 };
            int[] expected = SortingTestData.BaselineSorted(data);
            IntegerSort.RadixSort(data);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void RadixSort_ModerateScale_WideRange_SortsCorrectly()
        {
            int[] data = SortingTestData.Random(SortingTestData.LinearIntegerScale, seed: 31);
            int[] expected = SortingTestData.BaselineSorted(data);

            SortDiagnostics diag = SortRunner.RunRadixSort(data);

            Assert.Multiple(() =>
            {
                Assert.That(data, Is.EqualTo(expected));
                Assert.That(diag.Elapsed, Is.GreaterThan(TimeSpan.Zero));
                Assert.That(diag.AllocatedBytes, Is.GreaterThan(0), "Radix sort allocates auxiliary buffers.");
            });

            TestContext.WriteLine(diag.ToString());
        }

        [Test]
        public void RadixSort_AllEqual_RemainsEqual()
        {
            int[] data = SortingTestData.AllEqual(8192, value: -123);
            IntegerSort.RadixSort(data);
            Assert.That(data, Is.EqualTo(SortingTestData.AllEqual(8192, value: -123)));
        }

        [Test]
        public void CountingSort_NullArray_Throws()
        {
            int[]? data = null;
            Assert.That(() => IntegerSort.CountingSort(data!), Throws.ArgumentNullException);
        }

        [Test]
        public void RadixSort_NullArray_Throws()
        {
            int[]? data = null;
            Assert.That(() => IntegerSort.RadixSort(data!), Throws.ArgumentNullException);
        }
    }
}
