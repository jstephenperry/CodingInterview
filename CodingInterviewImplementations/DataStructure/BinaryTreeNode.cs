namespace CodingInterviewImplementations.DataStructure
{
    /// <summary>
    /// A single node in a <see cref="BinaryTree{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of value held by the node.</typeparam>
    public class BinaryTreeNode<T>
    {
        /// <summary>
        /// The value held by this node.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// The subtree holding values that sort before <see cref="Value"/>, or null.
        /// </summary>
        public BinaryTreeNode<T>? Left { get; set; }

        /// <summary>
        /// The subtree holding values that sort at or after <see cref="Value"/>, or null.
        /// </summary>
        public BinaryTreeNode<T>? Right { get; set; }

        /// <summary>
        /// Creates a leaf node.
        /// </summary>
        public BinaryTreeNode(T value)
        {
            Value = value;
        }
    }
}
