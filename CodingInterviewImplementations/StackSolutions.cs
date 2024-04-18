﻿namespace CodingInterviewImplementations
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
    }
}
