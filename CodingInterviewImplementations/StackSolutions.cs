namespace CodingInterviewImplementations
{
    public static class StackSolutions
    {
        /// <summary>
        /// Returns <see langword="true"/> when every opener in <paramref name="input"/>
        /// (one of <c>(</c>, <c>[</c>, <c>{</c>) has a matching closer in the correct order,
        /// and the string contains no other characters.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> is <see langword="null"/>.</exception>
        public static bool IsBalancedBrackets(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length % 2 != 0)
            {
                return false;
            }

            var stack = new Stack<char>(input.Length / 2);
            foreach (char c in input)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                        stack.Push(c);
                        break;
                    case ')':
                        if (stack.Count == 0 || stack.Pop() != '(')
                        {
                            return false;
                        }
                        break;
                    case ']':
                        if (stack.Count == 0 || stack.Pop() != '[')
                        {
                            return false;
                        }
                        break;
                    case '}':
                        if (stack.Count == 0 || stack.Pop() != '{')
                        {
                            return false;
                        }
                        break;
                    default:
                        return false;
                }
            }
            return stack.Count == 0;
        }

        /// <summary>
        /// Reverses <paramref name="stack"/> in place using only recursion (no auxiliary collection).
        /// Runs in O(n²) time and O(n) recursion depth — appropriate for small stacks only.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="stack"/> is <see langword="null"/>.</exception>
        public static void InvertStack<T>(Stack<T> stack)
        {
            ArgumentNullException.ThrowIfNull(stack);

            if (stack.Count == 0)
            {
                return;
            }

            T top = stack.Pop();
            InvertStack(stack);
            InsertAtBottom(stack, top);
        }

        private static void InsertAtBottom<T>(Stack<T> stack, T value)
        {
            if (stack.Count == 0)
            {
                stack.Push(value);
                return;
            }

            T top = stack.Pop();
            InsertAtBottom(stack, value);
            stack.Push(top);
        }
    }
}
