namespace CodingInterviewImplementations.DataStructure
{
    /// <summary>
    /// An unbalanced binary search tree.
    /// </summary>
    /// <typeparam name="T">
    /// The element type. Constrained to <see cref="IComparable{T}"/> so that an incomparable type is
    /// rejected at compile time rather than throwing from <see cref="Comparer{T}.Default"/> on the
    /// first insert.
    /// </typeparam>
    /// <remarks>
    /// This is deliberately a plain BST with no rebalancing, so sorted input degenerates into a
    /// linked list and operations become O(n). Use <see cref="SortedSet{T}"/> when balanced
    /// behaviour matters.
    /// <para>
    /// Every operation is iterative. Recursive versions were bounded by the height of the tree, and
    /// because the tree is unbalanced that height equals the element count for sorted input: adding
    /// twenty thousand ascending values overflowed the stack.
    /// </para>
    /// </remarks>
    public class BinaryTree<T> where T : IComparable<T>
    {
        /// <summary>
        /// The root of the tree, or null when the tree is empty.
        /// </summary>
        public BinaryTreeNode<T>? Root { get; set; }

        /// <summary>
        /// The number of values currently in the tree.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Inserts a value. Duplicates are permitted and are placed in the right subtree.
        /// </summary>
        public void Add(T value)
        {
            var node = new BinaryTreeNode<T>(value);
            Count++;

            if (Root == null)
            {
                Root = node;
                return;
            }

            BinaryTreeNode<T> current = Root;
            while (true)
            {
                if (Compare(value, current.Value) < 0)
                {
                    if (current.Left == null)
                    {
                        current.Left = node;
                        return;
                    }

                    current = current.Left;
                }
                else
                {
                    if (current.Right == null)
                    {
                        current.Right = node;
                        return;
                    }

                    current = current.Right;
                }
            }
        }

        /// <summary>
        /// Removes one occurrence of a value.
        /// </summary>
        /// <returns>True if a matching value was found and removed.</returns>
        public bool Remove(T value)
        {
            BinaryTreeNode<T>? parent = null;
            BinaryTreeNode<T>? node = Root;

            while (node != null)
            {
                int comparison = Compare(value, node.Value);
                if (comparison == 0)
                {
                    break;
                }

                parent = node;
                node = comparison < 0 ? node.Left : node.Right;
            }

            if (node == null)
            {
                return false;
            }

            if (node.Left != null && node.Right != null)
            {
                // Two children: overwrite this node with its in-order successor, then fall through
                // to unlink the successor, which by definition has no left child.
                BinaryTreeNode<T> successorParent = node;
                BinaryTreeNode<T> successor = node.Right;

                while (successor.Left != null)
                {
                    successorParent = successor;
                    successor = successor.Left;
                }

                node.Value = successor.Value;
                node = successor;
                parent = successorParent;
            }

            // node now has at most one child, so it can be spliced out directly.
            BinaryTreeNode<T>? child = node.Left ?? node.Right;

            if (parent == null)
            {
                Root = child;
            }
            else if (ReferenceEquals(parent.Left, node))
            {
                parent.Left = child;
            }
            else
            {
                parent.Right = child;
            }

            Count--;
            return true;
        }

        /// <summary>
        /// Determines whether the tree holds a value.
        /// </summary>
        public bool Contains(T value)
        {
            BinaryTreeNode<T>? node = Root;

            while (node != null)
            {
                // One comparison per node; the previous version compared twice at every step.
                int comparison = Compare(value, node.Value);

                if (comparison == 0)
                {
                    return true;
                }

                node = comparison < 0 ? node.Left : node.Right;
            }

            return false;
        }

        /// <summary>
        /// Walks the tree in order, yielding values from smallest to largest.
        /// </summary>
        public IEnumerable<T> InOrder()
        {
            // Explicit stack rather than recursion so a degenerate tree cannot overflow.
            var stack = new Stack<BinaryTreeNode<T>>();
            BinaryTreeNode<T>? node = Root;

            while (node != null || stack.Count > 0)
            {
                while (node != null)
                {
                    stack.Push(node);
                    node = node.Left;
                }

                node = stack.Pop();
                yield return node.Value;
                node = node.Right;
            }
        }

        private static int Compare(T left, T right)
        {
            return Comparer<T>.Default.Compare(left, right);
        }
    }
}
