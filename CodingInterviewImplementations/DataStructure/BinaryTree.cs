namespace CodingInterviewImplementations.DataStructure
{
    public class BinaryTree<T>
    {
        public BinaryTreeNode<T> Root { get; set; }

        public BinaryTree()
        {
            Root = null;
        }

        public void Add(T value)
        {
            if (Root == null)
            {
                Root = new BinaryTreeNode<T>(value);
            }
            else
            {
                BinaryTree<T>.Add(value, Root);
            }
        }

        private static void Add(T value, BinaryTreeNode<T> node)
        {
            if (Comparer<T>.Default.Compare(value, node.Value) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new BinaryTreeNode<T>(value);
                }
                else
                {
                    BinaryTree<T>.Add(value, node.Left);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new BinaryTreeNode<T>(value);
                }
                else
                {
                    BinaryTree<T>.Add(value, node.Right);
                }
            }
        }

        public void Remove(T value)
        {
            Root = BinaryTree<T>.Remove(value, Root);
        }

        private static BinaryTreeNode<T> Remove(T value, BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                return null;
            }

            if (Comparer<T>.Default.Compare(value, node.Value) < 0)
            {
                node.Left = BinaryTree<T>.Remove(value, node.Left);
            }
            else if (Comparer<T>.Default.Compare(value, node.Value) > 0)
            {
                node.Right = BinaryTree<T>.Remove(value, node.Right);
            }
            else
            {
                if (node.Left == null)
                {
                    return node.Right;
                }
                else if (node.Right == null)
                {
                    return node.Left;
                }

                node.Value = BinaryTree<T>.MinValue(node.Right);
                node.Right = BinaryTree<T>.Remove(node.Value, node.Right);
            }

            return node;
        }

        private static T MinValue(BinaryTreeNode<T> node)
        {
            T minv = node.Value;

            while (node.Left != null)
            {
                minv = node.Left.Value;
                node = node.Left;
            }

            return minv;
        }

        public bool Contains(T value)
        {
            return BinaryTree<T>.Contains(value, Root);
        }

        private static bool Contains(T value, BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                return false;
            }

            if (Comparer<T>.Default.Compare(value, node.Value) == 0)
            {
                return true;
            }

            if (Comparer<T>.Default.Compare(value, node.Value) < 0)
            {
                return BinaryTree<T>.Contains(value, node.Left);
            }
            else
            {
                return BinaryTree<T>.Contains(value, node.Right);
            }
        }
    }
}
