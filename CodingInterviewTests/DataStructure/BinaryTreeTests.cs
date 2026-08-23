using CodingInterviewImplementations.DataStructure;

namespace CodingInterviewImplementations.Tests.DataStructure
{
    /// <summary>
    /// Tests for <see cref="BinaryTree{T}"/>, which previously had no coverage at all despite
    /// holding the only non-trivial pointer manipulation in the repository.
    /// </summary>
    public class BinaryTreeTests
    {
        [Fact]
        public void ANewTreeIsEmpty()
        {
            var tree = new BinaryTree<int>();

            Assert.Multiple(
                () => Assert.Null(tree.Root),
                () => Assert.Equal(0, tree.Count),
                () => Assert.Empty(tree.InOrder()),
                () => Assert.False(tree.Contains(1)),
                () => Assert.False(tree.Remove(1)));
        }

        [Fact]
        public void Add_StoresValuesInSortedOrder()
        {
            BinaryTree<int> tree = TreeOf(50, 30, 70, 20, 40, 60, 80);

            Assert.Multiple(
                () => Assert.Equal(new[] { 20, 30, 40, 50, 60, 70, 80 }, tree.InOrder()),
                () => Assert.Equal(7, tree.Count));
        }

        [Fact]
        public void Add_PlacesDuplicatesInTheRightSubtree()
        {
            BinaryTree<int> tree = TreeOf(5, 5, 5);

            Assert.Multiple(
                () => Assert.Equal(3, tree.Count),
                () => Assert.Equal(new[] { 5, 5, 5 }, tree.InOrder()));
        }

        [Fact]
        public void Contains_FindsEveryStoredValueAndNothingElse()
        {
            BinaryTree<int> tree = TreeOf(50, 30, 70, 20, 40, 60, 80);

            Assert.Multiple(
                () => Assert.All(
                    new[] { 50, 30, 70, 20, 40, 60, 80 },
                    value => Assert.True(tree.Contains(value), $"{value} should be present")),
                () => Assert.All(
                    new[] { 0, 25, 45, 65, 99 },
                    value => Assert.False(tree.Contains(value), $"{value} should be absent")));
        }

        [Fact]
        public void Remove_ALeaf()
        {
            BinaryTree<int> tree = TreeOf(50, 30, 70, 20);

            Assert.Multiple(
                () => Assert.True(tree.Remove(20)),
                () => Assert.Equal(new[] { 30, 50, 70 }, tree.InOrder()),
                () => Assert.Equal(3, tree.Count));
        }

        [Fact]
        public void Remove_ANodeWithOnlyALeftChild()
        {
            BinaryTree<int> tree = TreeOf(50, 30, 20);

            Assert.Multiple(
                () => Assert.True(tree.Remove(30)),
                () => Assert.Equal(new[] { 20, 50 }, tree.InOrder()),
                () => Assert.Equal(2, tree.Count));
        }

        [Fact]
        public void Remove_ANodeWithOnlyARightChild()
        {
            BinaryTree<int> tree = TreeOf(50, 30, 40);

            Assert.Multiple(
                () => Assert.True(tree.Remove(30)),
                () => Assert.Equal(new[] { 40, 50 }, tree.InOrder()),
                () => Assert.Equal(2, tree.Count));
        }

        [Fact]
        public void Remove_ANodeWithTwoChildren_PromotesTheInOrderSuccessor()
        {
            BinaryTree<int> tree = TreeOf(50, 30, 70, 60, 80);

            Assert.Multiple(
                () => Assert.True(tree.Remove(70)),
                () => Assert.Equal(new[] { 30, 50, 60, 80 }, tree.InOrder()),
                () => Assert.Equal(4, tree.Count));
        }

        [Fact]
        public void Remove_TheRoot()
        {
            BinaryTree<int> tree = TreeOf(50, 30, 70, 20, 40, 60, 80);

            Assert.Multiple(
                () => Assert.True(tree.Remove(50)),
                () => Assert.False(tree.Contains(50)),
                () => Assert.Equal(new[] { 20, 30, 40, 60, 70, 80 }, tree.InOrder()),
                () => Assert.Equal(6, tree.Count));
        }

        [Fact]
        public void Remove_TheOnlyNode_EmptiesTheTree()
        {
            BinaryTree<int> tree = TreeOf(1);

            Assert.Multiple(
                () => Assert.True(tree.Remove(1)),
                () => Assert.Null(tree.Root),
                () => Assert.Equal(0, tree.Count));
        }

        [Fact]
        public void Remove_AnAbsentValue_ChangesNothing()
        {
            BinaryTree<int> tree = TreeOf(50, 30, 70);

            Assert.Multiple(
                () => Assert.False(tree.Remove(99)),
                () => Assert.Equal(3, tree.Count),
                () => Assert.Equal(new[] { 30, 50, 70 }, tree.InOrder()));
        }

        [Fact]
        public void Remove_ADuplicate_TakesOutExactlyOneCopy()
        {
            // Removing a node with two children promotes its successor and then unlinks that
            // successor. Counting that inner unlink as a second removal would corrupt Count.
            BinaryTree<int> tree = TreeOf(50, 30, 70, 70, 70);

            Assert.Multiple(
                () => Assert.True(tree.Remove(70)),
                () => Assert.Equal(4, tree.Count),
                () => Assert.Equal(new[] { 30, 50, 70, 70 }, tree.InOrder()));
        }

        [Fact]
        public void RemovingEveryValueLeavesAnEmptyTree()
        {
            int[] values = [50, 30, 70, 20, 40, 60, 80, 35, 45, 10];
            BinaryTree<int> tree = TreeOf(values);

            Assert.All(values, value => Assert.True(tree.Remove(value), $"failed to remove {value}"));

            Assert.Multiple(
                () => Assert.Equal(0, tree.Count),
                () => Assert.Null(tree.Root),
                () => Assert.Empty(tree.InOrder()));
        }

        [Fact]
        public void InOrder_AgreesWithASortedCopyOfTheInput()
        {
            int[] values = [.. Enumerable.Range(0, 200).Select(i => (i * 61) % 199)];
            BinaryTree<int> tree = TreeOf(values);

            Assert.Equal(values.Order(), tree.InOrder());
        }

        [Fact]
        public void AddAndInOrder_DoNotOverflowOnADegenerateTree()
        {
            // Sorted input degenerates this unbalanced tree into a linked list, so any recursive
            // insert or traversal would blow the stack. Both must be iterative.
            BinaryTree<int> tree = TreeOf([.. Enumerable.Range(0, 20_000)]);

            Assert.Equal(20_000, tree.InOrder().Count());
        }

        [Fact]
        public void WorksWithStrings()
        {
            BinaryTree<string> tree = new();
            foreach (string value in new[] { "pear", "apple", "quince", "banana" })
            {
                tree.Add(value);
            }

            Assert.Multiple(
                () => Assert.Equal(new[] { "apple", "banana", "pear", "quince" }, tree.InOrder()),
                () => Assert.True(tree.Contains("banana")),
                () => Assert.False(tree.Contains("cherry")));
        }

        private static BinaryTree<int> TreeOf(params int[] values)
        {
            var tree = new BinaryTree<int>();
            foreach (int value in values)
            {
                tree.Add(value);
            }
            return tree;
        }
    }
}
