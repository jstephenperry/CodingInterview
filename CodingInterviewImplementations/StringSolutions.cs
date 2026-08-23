using System.Text;

namespace CodingInterviewImplementations
{
    /// <summary>
    /// String manipulation and analysis problems.
    /// </summary>
    public static class StringSolutions
    {
        private static readonly char[] Vowels = ['a', 'e', 'i', 'o', 'u'];

        /// <summary>
        /// Maps each Roman digit to its value. Static so the table is not rebuilt on every call.
        /// </summary>
        private static readonly Dictionary<char, int> RomanDigitValues = new()
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 }
        };

        /// <summary>
        /// Finds the longest palindromic substring using Manacher's algorithm.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The longest palindromic substring, or an empty string when the input is empty.</returns>
        public static string GetLongestPalindromeSubstring(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                return string.Empty;
            }

            // Interleave separators so even- and odd-length palindromes are handled uniformly, and
            // bookend with sentinels so the expansion loop cannot run off either end.
            StringBuilder sb = new("^");
            foreach (char chr in input)
            {
                sb.Append('#');
                sb.Append(chr);
            }
            sb.Append("#$");

            string s = sb.ToString();
            int[] p = new int[s.Length];
            int center = 0, rightEdge = 0;
            int bestLength = 0, bestCenter = 0;

            for (int i = 1; i < s.Length - 1; i++)
            {
                int mirror = (2 * center) - i;
                p[i] = rightEdge > i ? Math.Min(rightEdge - i, p[mirror]) : 0;

                while (s[i + 1 + p[i]] == s[i - 1 - p[i]])
                {
                    p[i]++;
                }

                if (i + p[i] > rightEdge)
                {
                    center = i;
                    rightEdge = i + p[i];
                }

                // Track the best as we go; the original made a second full pass to find it.
                if (p[i] > bestLength)
                {
                    bestLength = p[i];
                    bestCenter = i;
                }
            }

            return input.Substring((bestCenter - 1 - bestLength) / 2, bestLength);
        }

        /// <summary>
        /// Determines whether two strings are anagrams by comparing character frequencies.
        /// </summary>
        /// <returns>True if the strings hold the same characters with the same multiplicities.</returns>
        /// <remarks>Two empty strings are anagrams of each other; a null input is not.</remarks>
        public static bool IsAnagramWithDictionaryFrequency(string? input1, string? input2)
        {
            if (!AreComparableAnagramCandidates(input1, input2))
            {
                return false;
            }

            var charFrequency = new Dictionary<char, int>();

            foreach (char c in input1!)
            {
                charFrequency[c] = charFrequency.GetValueOrDefault(c) + 1;
            }

            foreach (char c in input2!)
            {
                if (!charFrequency.TryGetValue(c, out int count) || count == 0)
                {
                    return false;
                }

                charFrequency[c] = count - 1;
            }

            return true;
        }

        /// <summary>
        /// Determines whether two strings are anagrams by sorting both.
        /// </summary>
        public static bool IsAnagramWithSorting(string? input1, string? input2)
        {
            if (!AreComparableAnagramCandidates(input1, input2))
            {
                return false;
            }

            char[] input1Array = input1!.ToCharArray();
            char[] input2Array = input2!.ToCharArray();

            Array.Sort(input1Array);
            Array.Sort(input2Array);

            return input1Array.AsSpan().SequenceEqual(input2Array);
        }

        /// <summary>
        /// Determines whether two strings are anagrams by sorting both with LINQ.
        /// </summary>
        public static bool IsAnagramWithLinqSorting(string? input1, string? input2)
        {
            if (!AreComparableAnagramCandidates(input1, input2))
            {
                return false;
            }

            return input1!.Order().SequenceEqual(input2!.Order());
        }

        /// <summary>
        /// Rejects inputs that cannot be anagrams before any counting or sorting work is done.
        /// </summary>
        private static bool AreComparableAnagramCandidates(string? input1, string? input2)
        {
            return input1 != null && input2 != null && input1.Length == input2.Length;
        }

        /// <summary>
        /// Finds the most frequently occurring character in a string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// The most frequent character and its count. Ties are broken in favour of the character
        /// that appears first in the input. Returns the default pair for a null or empty input.
        /// </returns>
        public static KeyValuePair<char, int> FindMaxOccurringCharacterLinq(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default;
            }

            var charFrequency = new Dictionary<char, int>();
            foreach (char c in input)
            {
                charFrequency[c] = charFrequency.GetValueOrDefault(c) + 1;
            }

            int maxCount = charFrequency.Values.Max();

            // Scan the input rather than the dictionary so ties resolve to the first character in
            // the string. Dictionary enumeration order is an implementation detail, not a contract
            // worth depending on for a deterministic result.
            foreach (char c in input)
            {
                if (charFrequency[c] == maxCount)
                {
                    return new KeyValuePair<char, int>(c, maxCount);
                }
            }

            return default;
        }

        /// <summary>
        /// Finds the most frequently occurring whitespace-delimited word across a list of strings.
        /// </summary>
        /// <param name="input">The strings to scan.</param>
        /// <returns>
        /// The most frequent word and its count. Ties are broken in favour of the word that appears
        /// first. Returns the default pair when the input is null, empty, or holds no words.
        /// </returns>
        public static KeyValuePair<string, int> FindMaxOccurringWordLinq(List<string>? input)
        {
            if (input == null || input.Count == 0)
            {
                return default;
            }

            List<string> words = [.. input
                .Where(s => s != null)
                .SelectMany(s => s.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))];

            if (words.Count == 0)
            {
                return default;
            }

            var wordFrequency = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (string word in words)
            {
                wordFrequency[word] = wordFrequency.GetValueOrDefault(word) + 1;
            }

            int maxCount = wordFrequency.Values.Max();

            foreach (string word in words)
            {
                if (wordFrequency[word] == maxCount)
                {
                    return new KeyValuePair<string, int>(word, maxCount);
                }
            }

            return default;
        }

        /// <summary>
        /// Counts the whitespace-delimited words in a string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The number of words.</returns>
        /// <remarks>
        /// Splits on all whitespace, not just the space character, and counts by scanning rather
        /// than by allocating an array of every word only to read its length.
        /// </remarks>
        public static int GetWordCount(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0;
            }

            int count = 0;
            bool inWord = false;

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    inWord = false;
                }
                else if (!inWord)
                {
                    inWord = true;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Converts a Roman numeral to an integer.
        /// </summary>
        /// <param name="romanNumber">The Roman numeral, in upper case.</param>
        /// <returns>The integer value, or zero when the input is null or empty.</returns>
        /// <exception cref="FormatException">
        /// The input holds a character that is not a Roman digit. The previous version silently
        /// ignored unknown characters, so "hello" returned 0 rather than being rejected.
        /// </exception>
        public static int RomanNumeralToInteger(string? romanNumber)
        {
            if (string.IsNullOrEmpty(romanNumber))
            {
                return 0;
            }

            int result = 0;
            int previousValue = 0;

            foreach (char c in romanNumber)
            {
                if (!RomanDigitValues.TryGetValue(c, out int value))
                {
                    throw new FormatException($"'{c}' is not a Roman numeral digit.");
                }

                result += value;

                // A smaller digit before a larger one is subtractive, and it has already been added
                // once, so remove it twice.
                if (previousValue < value)
                {
                    result -= 2 * previousValue;
                }

                previousValue = value;
            }

            return result;
        }

        /// <summary>
        /// Counts the vowels and consonants in a string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The counts, or null when the input is null or empty.</returns>
        /// <remarks>
        /// Case folding is invariant. <see cref="char.ToLower(char)"/> is culture sensitive, so under
        /// a Turkish culture it maps 'I' to a dotless 'i' and the vowel test misses it.
        /// </remarks>
        public static Dictionary<string, int>? FindVowelsAndConsonants(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var vowelsAndConsonants = new Dictionary<string, int>
            {
                { "Vowels", 0 },
                { "Consonants", 0 }
            };

            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    continue;
                }

                bool isVowel = Array.IndexOf(Vowels, char.ToLowerInvariant(c)) >= 0;
                vowelsAndConsonants[isVowel ? "Vowels" : "Consonants"]++;
            }

            return vowelsAndConsonants;
        }

        /// <summary>
        /// Converts a byte array to its upper-case hexadecimal representation.
        /// </summary>
        /// <remarks>
        /// <see cref="Convert.ToHexString(byte[])"/> does this in one pass. The previous
        /// BitConverter-then-Replace form built a dash-separated string and then allocated a second
        /// string to strip the dashes back out.
        /// </remarks>
        public static string ByteArrayToString(byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(bytes);
            return Convert.ToHexString(bytes);
        }

        /// <summary>
        /// Removes every occurrence of a character from a string using LINQ.
        /// </summary>
        public static string RemoveCharacterFromStringLinq(string input, char c)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return new string([.. input.Where(ch => ch != c)]);
        }

        /// <summary>
        /// Removes every occurrence of a character from a string.
        /// </summary>
        public static string RemoveCharacterFromString(string input, char c)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // Sized up front: the result can never be longer than the input.
            StringBuilder sb = new(input.Length);
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
