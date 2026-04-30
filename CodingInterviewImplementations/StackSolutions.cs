namespace CodingInterviewImplementations
{
    public static class StackSolutions
    {
        /// <summary>
        /// Determines if a string of brackets is balanced.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>True if the brackets are balanced</returns>
        public static bool IsBalancedBrackets(string input)
        {
            if (input.Length % 2 != 0)
            {
                return false;
            }

            var brackets = new Dictionary<char, char>
            {
                { '(', ')' },
                { '{', '}' },
                { '[', ']' }
            };

            var stack = new Stack<char>();
            foreach (var c in input)
            {
                if (brackets.ContainsKey(c))
                {
                    stack.Push(c);
                }
                else if (stack.Count == 0 || brackets[stack.Pop()] != c)
                {
                    return false;
                }
            }
            return stack.Count == 0;
        }

        /// <summary>
        /// Inverts a stack using recursion.
        /// </summary>
        /// <param name="stack"></param>
        public static void InvertStack(Stack<int> stack)
        {
            if (stack.Count == 0)
            {
                return;
            }

            int temp = stack.Pop();
            InvertStack(stack);
            InsertAtBottom(stack, temp);
        }

        /// <summary>
        /// Inserts a value at the bottom of a stack.
        /// </summary>
        /// <param name="stack"></param>
        /// <param name="value"></param>
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
