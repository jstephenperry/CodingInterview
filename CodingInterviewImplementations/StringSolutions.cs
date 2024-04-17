using System.Text;

namespace CodingInterviewImplementations
{
    public static class StringSolutions
    {
        /// <summary>
        /// Returns the longest palindrome substring from an input string using Manacher's algorithm.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetLongestPalindromeSubstring(string input)
        {
            StringBuilder sb = new StringBuilder("^");
            foreach (var chr in input)
            {
                sb.Append("#");
                sb.Append(chr);
            }
            sb.Append("#$");

            string s = sb.ToString();
            int[] p = new int[s.Length];
            int c = 0, r = 0;
            for (int i = 1; i < s.Length - 1; i++)
            {
                int iMirror = 2 * c - i;
                p[i] = r > i ? Math.Min(r - i, p[iMirror]) : 0;

                while (s[i + 1 + p[i]] == s[i - 1 - p[i]])
                {
                    p[i]++;
                }

                if (i + p[i] > r)
                {
                    c = i;
                    r = i + p[i];
                }
            }

            int length = 0;
            int centerIndex = 0;
            for (int i = 1; i < s.Length - 1; i++)
            {
                if (p[i] > length)
                {
                    length = p[i];
                    centerIndex = i;
                }
            }
            return input.Substring((centerIndex - 1 - length) / 2, length);
        }

        public static bool IsAnagramWithDictionaryFrequency(string input1, string input2)
        {
            if (string.IsNullOrEmpty(input1) || string.IsNullOrEmpty(input2))
            {
                return false;
            }

            if (input1.Length != input2.Length)
            {
                return false;
            }

            var charFrequency = new Dictionary<char, int>();

            foreach (char c in input1)
            {
                if (charFrequency.TryGetValue(c, out int value))
                {
                    charFrequency[c] = ++value;
                }
                else
                {
                    charFrequency[c] = 1;
                }
            }

            foreach (char c in input2)
            {
                if (charFrequency.TryGetValue(c, out int value))
                {
                    if (value == 0)
                    {
                        return false;
                    }
                    charFrequency[c] = --value;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsAnagramWithSorting(string input1, string input2)
        {
            if (string.IsNullOrEmpty(input1) || string.IsNullOrEmpty(input2))
            {
                return false;
            }

            if (input1.Length != input2.Length)
            {
                return false;
            }

            var input1Array = input1.ToCharArray();
            var input2Array = input2.ToCharArray();

            Array.Sort(input1Array);
            Array.Sort(input2Array);

            return input1Array.SequenceEqual(input2Array);
        }

        public static bool IsAnagramWithLinqSorting(string input1, string input2)
        {
            if (string.IsNullOrEmpty(input1) || string.IsNullOrEmpty(input2))
            {
                return false;
            }

            if (input1.Length != input2.Length)
            {
                return false;
            }

            return input1.OrderBy(c => c).SequenceEqual(input2.OrderBy(c => c));
        }

        public static KeyValuePair<char, int> FindMaxOccurringCharacterLinq(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default;
            }

            var charFrequency = new Dictionary<char, int>();

            foreach (char c in input)
            {
                if (charFrequency.TryGetValue(c, out int value))
                {
                    charFrequency[c] = ++value;
                }
                else
                {
                    charFrequency[c] = 1;
                }
            }
            return charFrequency.OrderByDescending(pair => pair.Value).First();
        }
    }
}
