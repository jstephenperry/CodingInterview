namespace CodingInterviewImplementations.Sorting.Tests
{
    [TestFixture]
    [TestOf(typeof(Sorter))]
    public class SorterTests
    {
        [Test]
        public void EmptyArray_RemainsEmpty([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = Array.Empty<int>();
            SortRunner.Run(algorithm, data);
            Assert.That(data, Is.Empty);
        }

        [Test]
        public void SingleElement_Unchanged([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = { 42 };
            SortRunner.Run(algorithm, data);
            Assert.That(data, Is.EqualTo(new[] { 42 }));
        }

        [Test]
        public void TwoElements_Sorted([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = { 9, 1 };
            SortRunner.Run(algorithm, data);
            Assert.That(data, Is.EqualTo(new[] { 1, 9 }));
        }

        [Test]
        public void SmallKnownInput_SortedCorrectly([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = { 5, -1, 3, 9, 0, 5, 2, -7, 100, 5 };
            int[] expected = { -7, -1, 0, 2, 3, 5, 5, 5, 9, 100 };
            SortRunner.Run(algorithm, data);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void AllEqualValues_RemainsEqual([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = SortingTestData.AllEqual(1024, value: -3);
            SortRunner.Run(algorithm, data);
            Assert.That(data, Is.EqualTo(SortingTestData.AllEqual(1024, value: -3)));
        }

        [Test]
        public void AscendingInput_RemainsSorted([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = SortingTestData.Ascending(2048);
            int[] expected = SortingTestData.Ascending(2048);
            SortRunner.Run(algorithm, data);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void DescendingInput_BecomesSorted([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = SortingTestData.Descending(2048);
            int[] expected = SortingTestData.Ascending(2048);
            SortRunner.Run(algorithm, data);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void CustomComparer_ReverseOrder([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = { 1, 4, 2, 8, 5, 7, 3, 6 };
            int[] expected = { 8, 7, 6, 5, 4, 3, 2, 1 };
            IComparer<int> reverse = Comparer<int>.Create((a, b) => b.CompareTo(a));
            SortRunner.Run(algorithm, data, reverse);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void StringInput_SortedOrdinally([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            string[] data = { "banana", "apple", "cherry", "date", "apple", "blueberry" };
            string[] expected = { "apple", "apple", "banana", "blueberry", "cherry", "date" };
            SortRunner.Run(algorithm, data, StringComparer.Ordinal);
            Assert.That(data, Is.EqualTo(expected));
        }

        [Test]
        public void QuadraticAlgorithm_ModerateScale_SortsCorrectly(
            [ValueSource(typeof(SortingTestData), nameof(SortingTestData.QuadraticAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = SortingTestData.Random(SortingTestData.QuadraticScale, seed: 42);
            int[] expected = SortingTestData.BaselineSorted(data);

            SortDiagnostics diag = SortRunner.Run(algorithm, data);

            Assert.Multiple(() =>
            {
                Assert.That(data, Is.EqualTo(expected));
                Assert.That(diag.ItemCount, Is.EqualTo(SortingTestData.QuadraticScale));
                Assert.That(diag.Elapsed, Is.GreaterThan(TimeSpan.Zero));
                Assert.That(diag.AllocatedBytes, Is.GreaterThanOrEqualTo(0));
                Assert.That(diag.Gen0Collections, Is.GreaterThanOrEqualTo(0));
            });

            TestContext.WriteLine(diag.ToString());
        }

        [Test]
        public void SubquadraticAlgorithm_ModerateScale_SortsCorrectly(
            [ValueSource(typeof(SortingTestData), nameof(SortingTestData.SubquadraticAlgorithms))] SortAlgorithm algorithm)
        {
            int[] data = SortingTestData.Random(SortingTestData.SubquadraticScale, seed: 1337);
            int[] expected = SortingTestData.BaselineSorted(data);

            SortDiagnostics diag = SortRunner.Run(algorithm, data);

            Assert.Multiple(() =>
            {
                Assert.That(data, Is.EqualTo(expected));
                Assert.That(diag.ItemCount, Is.EqualTo(SortingTestData.SubquadraticScale));
                Assert.That(diag.Elapsed, Is.GreaterThan(TimeSpan.Zero));
            });

            TestContext.WriteLine(diag.ToString());
        }

        [Test]
        public void SubquadraticAlgorithm_AdversarialSortedInput_StillSubsecond(
            [ValueSource(typeof(SortingTestData), nameof(SortingTestData.SubquadraticAlgorithms))] SortAlgorithm algorithm)
        {
            // Ascending input is the classic worst case for naive quicksort; median-of-three
            // pivot selection should keep us well-behaved here.
            int[] data = SortingTestData.Ascending(SortingTestData.SubquadraticScale);
            int[] expected = (int[])data.Clone();

            SortDiagnostics diag = SortRunner.Run(algorithm, data);

            Assert.Multiple(() =>
            {
                Assert.That(data, Is.EqualTo(expected));
                Assert.That(diag.Elapsed, Is.LessThan(TimeSpan.FromSeconds(5)),
                    $"Algorithm {algorithm} should not degrade catastrophically on sorted input.");
            });

            TestContext.WriteLine(diag.ToString());
        }

        [Test]
        public void MergeSort_IsStable_ForEqualKeys()
        {
            // Pair items so the sort can be observed reordering equal first-components.
            // Use a record carrying a payload tag — after sort, identical keys must keep input order.
            var items = new (int Key, int Tag)[]
            {
                (3, 0), (1, 1), (3, 2), (2, 3), (1, 4), (3, 5), (2, 6), (1, 7),
            };
            var expected = new (int Key, int Tag)[]
            {
                (1, 1), (1, 4), (1, 7), (2, 3), (2, 6), (3, 0), (3, 2), (3, 5),
            };
            IComparer<(int Key, int Tag)> byKey = Comparer<(int, int)>.Create((a, b) => a.Item1.CompareTo(b.Item1));

            Sorter.MergeSort(items, byKey);

            Assert.That(items, Is.EqualTo(expected));
        }

        [Test]
        public void NullArray_Throws([ValueSource(typeof(SortingTestData), nameof(SortingTestData.AllAlgorithms))] SortAlgorithm algorithm)
        {
            int[]? data = null;
            Assert.That(() => SortRunner.Run(algorithm, data!), Throws.ArgumentNullException);
        }
    }
}
