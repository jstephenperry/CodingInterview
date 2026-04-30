namespace CodingInterviewImplementations.DataStructure
{
    /// <summary>A single node in a <see cref="BinarySearchTree{T}"/>.</summary>
    public sealed class BinarySearchTreeNode<T>
    {
        public T Value { get; set; }
        public BinarySearchTreeNode<T>? Left { get; set; }
        public BinarySearchTreeNode<T>? Right { get; set; }

        public BinarySearchTreeNode(T value)
        {
            Value = value;
        }
    }
}
