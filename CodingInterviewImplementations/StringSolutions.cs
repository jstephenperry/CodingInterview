using System.Text;

namespace CodingInterviewImplementations
{
    public static class StringSolutions
    {
        /// <summary>
        /// Finds the longest palindrome substring from an input string using Manacher's algorithm.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The palindrome substring</returns>
        public static string GetLongestPalindromeSubstring(string input)
        {
            StringBuilder sb = new("^");
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

        /// <summary>
        /// Determines if two strings are anagrams of each other using a dictionary to store character frequency.
        /// </summary>
        /// <param name="input1">The first input string</param>
        /// <param name="input2">The second input string</param>
        /// <returns>True if the parameter strings are anagrams</returns>
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

        /// <summary>
        /// Determines if two strings are anagrams of each other using sorting.
        /// </summary>
        /// <param name="input1">The first input string</param>
        /// <param name="input2">The second input string</param>
        /// <returns>True if the parameter strings are anagrams</returns>
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

        /// <summary>
        /// Determines if two strings are anagrams of each other using LINQ sorting.
        /// </summary>
        /// <param name="input1">The first input string</param>
        /// <param name="input2">The second input string</param>
        /// <returns>True if the parameter strings are anagrams</returns>
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

        /// <summary>
        /// Finds the most frequently occurring character in a string.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The key-value pair of the most frequently occurring character and its occurrence count</returns>
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

        /// <summary>
        /// Finds the most frequently occurring word in a list of strings.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static KeyValuePair<string, int> FindMaxOccurringWordLinq(List<string> input)
        {
            if (input == null || input.Count == 0)
            {
                return default;
            }

            var wordFrequency = input.SelectMany(s => s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());

            return wordFrequency.OrderByDescending(pair => pair.Value).First();
        }

        /// <summary>
        /// Finds the total number of words in a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int GetWordCount(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            return input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>
        /// Converts  a Roman numeral to an integer.
        /// </summary>
        /// <param name="romanNumber">The Roman numeral string</param>
        /// <returns>The integer value of the Roman numeral</returns>
        public static int RomanNumeralToInteger(string romanNumber)
        {
            if (string.IsNullOrEmpty(romanNumber))
            {
                return 0;
            }

            var romanNumberToInteger = new Dictionary<char, int>
            {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 }
            };

            int result = 0;
            int previousValue = 0;

            foreach (char c in romanNumber)
            {
                if (romanNumberToInteger.TryGetValue(c, out int value))
                {
                    result += value;
                    if (previousValue < value)
                    {
                        result -= 2 * previousValue;
                    }
                    previousValue = value;
                }
            }

            return result;
        }

        /// <summary>
        /// Counts the number of vowels and consonants in a string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Dictionary<string, int>? FindVowelsAndConsonants(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default;
            }

            var vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u' };
            var vowelsAndConsonants = new Dictionary<string, int>
            {
                { "Vowels", 0 },
                { "Consonants", 0 }
            };

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    if (vowels.Contains(char.ToLower(c)))
                    {
                        vowelsAndConsonants["Vowels"]++;
                    }
                    else
                    {
                        vowelsAndConsonants["Consonants"]++;
                    }
                }
            }

            return vowelsAndConsonants;
        }

        /// <summary>
        /// Converts a byte array to a string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        /// <summary>
        /// Removes a character from a string using LINQ.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string RemoveCharacterFromStringLinq(string input, char c)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return new string(input.Where(ch => ch != c).ToArray());
        }

        /// <summary>
        /// Removes a character from a string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string RemoveCharacterFromString(string input, char c)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            StringBuilder sb = new();
            foreach (char ch in input)
            {
                if (ch != c)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString();
        }
    }
}
