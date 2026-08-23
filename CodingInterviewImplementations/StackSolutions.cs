namespace CodingInterviewImplementations
{
    /// <summary>
    /// Problems whose natural solution is a stack.
    /// </summary>
    public static class StackSolutions
    {
        private static readonly Dictionary<char, char> ClosingByOpening = new()
        {
            { '(', ')' },
            { '{', '}' },
            { '[', ']' }
        };

        /// <summary>
        /// Determines whether the brackets in a string are balanced and correctly nested.
        /// </summary>
        /// <param name="input">The input string. Characters that are not brackets are ignored.</param>
        /// <returns>True if every opening bracket has a matching closing bracket in the right order.</returns>
        /// <remarks>
        /// Non-bracket characters are skipped rather than treated as failures, so "(a + b) * [c]" is
        /// balanced. The lookup table is static: the previous version rebuilt it on every call.
        /// </remarks>
        public static bool IsBalancedBrackets(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }

            var stack = new Stack<char>();

            foreach (char c in input)
            {
                if (ClosingByOpening.TryGetValue(c, out char expectedClosing))
                {
                    stack.Push(expectedClosing);
                }
                else if (c is ')' or '}' or ']')
                {
                    if (stack.Count == 0 || stack.Pop() != c)
                    {
                        return false;
                    }
                }
            }

            return stack.Count == 0;
        }

        /// <summary>
        /// Reverses a stack in place using only recursion and the stack itself.
        /// </summary>
        /// <param name="stack">The stack to reverse.</param>
        /// <remarks>
        /// Recursion depth is proportional to the size of the stack, so this is a demonstration of
        /// the technique rather than something to run on a large stack.
        /// </remarks>
        public static void InvertStack(Stack<int> stack)
        {
            ArgumentNullException.ThrowIfNull(stack);

            if (stack.Count == 0)
            {
                return;
            }

            int temp = stack.Pop();
            InvertStack(stack);
            InsertAtBottom(stack, temp);
        }

        private static void InsertAtBottom(Stack<int> stack, int value)
        {
            if (stack.Count == 0)
            {
                stack.Push(value);
                return;
            }

            int temp = stack.Pop();
            InsertAtBottom(stack, value);
            stack.Push(temp);
        }
    }
}
