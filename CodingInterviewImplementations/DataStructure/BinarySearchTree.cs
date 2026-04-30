namespace CodingInterviewImplementations.DataStructure
{
    /// <summary>
    /// An unbalanced binary search tree keyed on <typeparamref name="T"/> using
    /// <see cref="Comparer{T}.Default"/>. Duplicates are inserted in the right subtree.
    /// </summary>
    public class BinarySearchTree<T>
    {
        public BinarySearchTreeNode<T>? Root { get; private set; }

        public void Add(T value)
        {
            if (Root == null)
            {
                Root = new BinarySearchTreeNode<T>(value);
                return;
            }

            Add(value, Root);
        }

        private static void Add(T value, BinarySearchTreeNode<T> node)
        {
            if (Comparer<T>.Default.Compare(value, node.Value) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new BinarySearchTreeNode<T>(value);
                }
                else
                {
                    Add(value, node.Left);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new BinarySearchTreeNode<T>(value);
                }
                else
                {
                    Add(value, node.Right);
                }
            }
        }

        public void Remove(T value)
        {
            Root = Remove(value, Root);
        }

        private static BinarySearchTreeNode<T>? Remove(T value, BinarySearchTreeNode<T>? node)
        {
            if (node == null)
            {
                return null;
            }

            int cmp = Comparer<T>.Default.Compare(value, node.Value);
            if (cmp < 0)
            {
                node.Left = Remove(value, node.Left);
            }
            else if (cmp > 0)
            {
                node.Right = Remove(value, node.Right);
            }
            else
            {
                if (node.Left == null)
                {
                    return node.Right;
                }
                if (node.Right == null)
                {
                    return node.Left;
                }

                // Replace with in-order successor (smallest value in right subtree).
                node.Value = MinValue(node.Right);
                node.Right = Remove(node.Value, node.Right);
            }

            return node;
        }

        private static T MinValue(BinarySearchTreeNode<T> node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node.Value;
        }

        public bool Contains(T value) => Contains(value, Root);

        private static bool Contains(T value, BinarySearchTreeNode<T>? node)
        {
            while (node != null)
            {
                int cmp = Comparer<T>.Default.Compare(value, node.Value);
                if (cmp == 0)
                {
                    return true;
                }
                node = cmp < 0 ? node.Left : node.Right;
            }
            return false;
        }
    }
}
