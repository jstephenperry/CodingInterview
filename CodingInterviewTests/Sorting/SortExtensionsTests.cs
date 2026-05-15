namespace CodingInterviewImplementations.Sorting.Tests
{
    [TestFixture]
    [TestOf(typeof(SortExtensions))]
    public class SortExtensionsTests
    {
        [Test]
        public void BubbleSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2 };
            data.BubbleSort();
            Assert.That(data, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void InsertionSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2 };
            data.InsertionSort();
            Assert.That(data, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void SelectionSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2 };
            data.SelectionSort();
            Assert.That(data, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void ShellSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2 };
            data.ShellSort();
            Assert.That(data, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void MergeSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2 };
            data.MergeSort();
            Assert.That(data, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void QuickSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2 };
            data.QuickSort();
            Assert.That(data, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void HeapSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2 };
            data.HeapSort();
            Assert.That(data, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void Sort_Extension_ReturnsDiagnostics([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = SortingTestData.Random(256, seed: 99);
            int[] expected = SortingTestData.BaselineSorted(data);

            SortDiagnostics diag = data.Sort(algorithm);

            Assert.Multiple(() =>
            {
                Assert.That(data, Is.EqualTo(expected));
                Assert.That(diag.Algorithm, Is.EqualTo(algorithm.ToString()));
                Assert.That(diag.ItemCount, Is.EqualTo(256));
            });
        }

        [Test]
        public void Sorted_Extension_LeavesSourceUntouched([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            var source = new List<int> { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5 };
            var snapshot = source.ToList();

            int[] sorted = source.Sorted(algorithm);

            Assert.Multiple(() =>
            {
                Assert.That(sorted, Is.EqualTo(SortingTestData.BaselineSorted(source.ToArray())));
                Assert.That(source, Is.EqualTo(snapshot), "Source enumerable must not be mutated.");
            });
        }

        [Test]
        public void Sorted_Extension_AcceptsLazyEnumerable()
        {
            IEnumerable<int> source = Enumerable.Range(0, 100).Select(i => 99 - i);
            int[] sorted = source.Sorted(SortAlgorithm.Quick);
            Assert.That(sorted, Is.EqualTo(Enumerable.Range(0, 100).ToArray()));
        }

        [Test]
        public void Sorted_Extension_NullSource_Throws()
        {
            IEnumerable<int>? source = null;
            Assert.That(() => source!.Sorted(SortAlgorithm.Quick), Throws.ArgumentNullException);
        }

        [Test]
        public void CountingSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2, -1, 0 };
            data.CountingSort();
            Assert.That(data, Is.EqualTo(new[] { -1, 0, 1, 2, 3 }));
        }

        [Test]
        public void RadixSort_Extension_SortsArrayInPlace()
        {
            int[] data = { 3, 1, 2, -1, 0 };
            data.RadixSort();
            Assert.That(data, Is.EqualTo(new[] { -1, 0, 1, 2, 3 }));
        }
    }
}
