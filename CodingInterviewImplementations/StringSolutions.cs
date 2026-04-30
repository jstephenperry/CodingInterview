using System.Text;

namespace CodingInterviewImplementations
{
    public static class StringSolutions
    {
        /// <summary>
        /// Returns the longest palindromic substring of <paramref name="input"/> using Manacher's algorithm in O(n) time.
        /// </summary>
        /// <param name="input">Input string. Must not be null.</param>
        /// <returns>The longest palindromic substring, or the empty string if <paramref name="input"/> is empty.</returns>
        public static string GetLongestPalindromeSubstring(string input)
        {
            ArgumentNullException.ThrowIfNull(input);
            if (input.Length == 0)
            {
                return string.Empty;
            }

            // Transform "abc" into "^#a#b#c#$" so every palindrome (odd or even length)
            // sits centered on some index in the transformed string.
            var sb = new StringBuilder(2 * input.Length + 3);
            sb.Append('^');
            foreach (char ch in input)
            {
                sb.Append('#').Append(ch);
            }
            sb.Append("#$");

            string s = sb.ToString();
            int[] p = new int[s.Length];
            int center = 0, right = 0;
            int bestCenter = 0, bestLength = 0;

            for (int i = 1; i < s.Length - 1; i++)
            {
                int mirror = 2 * center - i;
                p[i] = right > i ? Math.Min(right - i, p[mirror]) : 0;

                while (s[i + 1 + p[i]] == s[i - 1 - p[i]])
                {
                    p[i]++;
                }

                if (i + p[i] > right)
                {
                    center = i;
                    right = i + p[i];
                }

                if (p[i] > bestLength)
                {
                    bestLength = p[i];
                    bestCenter = i;
                }
            }

            return input.Substring((bestCenter - 1 - bestLength) / 2, bestLength);
        }

        /// <summary>
        /// Determines whether two strings are anagrams using a character-frequency dictionary in O(n) time.
        /// Comparison is ordinal and case-sensitive; whitespace and punctuation are treated as ordinary characters.
        /// </summary>
        public static bool IsAnagramWithDictionaryFrequency(string input1, string input2)
        {
            ArgumentNullException.ThrowIfNull(input1);
            ArgumentNullException.ThrowIfNull(input2);

            if (input1.Length != input2.Length)
            {
                return false;
            }

            var counts = new Dictionary<char, int>(input1.Length);
            foreach (char c in input1)
            {
                counts.TryGetValue(c, out int n);
                counts[c] = n + 1;
            }

            foreach (char c in input2)
            {
                if (!counts.TryGetValue(c, out int n) || n == 0)
                {
                    return false;
                }
                counts[c] = n - 1;
            }

            return true;
        }

        /// <summary>
        /// Determines whether two strings are anagrams by sorting both as char arrays in O(n log n) time.
        /// Comparison is ordinal and case-sensitive.
        /// </summary>
        public static bool IsAnagramWithSorting(string input1, string input2)
        {
            ArgumentNullException.ThrowIfNull(input1);
            ArgumentNullException.ThrowIfNull(input2);

            if (input1.Length != input2.Length)
            {
                return false;
            }

            char[] a = input1.ToCharArray();
            char[] b = input2.ToCharArray();
            Array.Sort(a);
            Array.Sort(b);
            return a.AsSpan().SequenceEqual(b);
        }

        /// <summary>
        /// Determines whether two strings are anagrams using LINQ ordering in O(n log n) time.
        /// Allocates more than <see cref="IsAnagramWithSorting"/>; provided for comparison.
        /// </summary>
        public static bool IsAnagramWithLinqSorting(string input1, string input2)
        {
            ArgumentNullException.ThrowIfNull(input1);
            ArgumentNullException.ThrowIfNull(input2);

            if (input1.Length != input2.Length)
            {
                return false;
            }

            return input1.OrderBy(c => c).SequenceEqual(input2.OrderBy(c => c));
        }

        /// <summary>
        /// Returns the most frequently occurring character in <paramref name="input"/> together with its count,
        /// or <see langword="null"/> if the string is empty. Ties are broken by first occurrence.
        /// </summary>
        public static (char Character, int Count)? FindMaxOccurringCharacter(string input)
        {
            ArgumentNullException.ThrowIfNull(input);
            if (input.Length == 0)
            {
                return null;
            }

            return input
                .GroupBy(c => c)
                .Select(g => (Character: g.Key, Count: g.Count()))
                .OrderByDescending(t => t.Count)
                .First();
        }

        /// <summary>
        /// Returns the most frequently occurring whitespace-delimited word in <paramref name="text"/> together with its count,
        /// or <see langword="null"/> if no words are present. Comparison is ordinal and case-sensitive.
        /// </summary>
        public static (string Word, int Count)? FindMaxOccurringWord(string text)
        {
            ArgumentNullException.ThrowIfNull(text);

            string[] words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                return null;
            }

            return words
                .GroupBy(w => w, StringComparer.Ordinal)
                .Select(g => (Word: g.Key, Count: g.Count()))
                .OrderByDescending(t => t.Count)
                .First();
        }

        /// <summary>
        /// Counts whitespace-delimited words in <paramref name="input"/>. Returns 0 for null or all-whitespace input.
        /// </summary>
        public static int GetWordCount(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            return input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>
        /// Parses a Roman numeral (uppercase, characters I, V, X, L, C, D, M) into an integer.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the input is empty or contains characters outside the Roman alphabet.</exception>
        public static int RomanNumeralToInteger(string romanNumber)
        {
            ArgumentNullException.ThrowIfNull(romanNumber);
            if (romanNumber.Length == 0)
            {
                throw new ArgumentException("Roman numeral must not be empty.", nameof(romanNumber));
            }

            int result = 0;
            int previousValue = 0;
            for (int i = romanNumber.Length - 1; i >= 0; i--)
            {
                int value = romanNumber[i] switch
                {
                    'I' => 1,
                    'V' => 5,
                    'X' => 10,
                    'L' => 50,
                    'C' => 100,
                    'D' => 500,
                    'M' => 1000,
                    _ => throw new ArgumentException($"Invalid Roman numeral character '{romanNumber[i]}'.", nameof(romanNumber)),
                };

                // Subtractive when a smaller numeral precedes a larger one (e.g. IV, IX, CM).
                result += value < previousValue ? -value : value;
                previousValue = value;
            }

            return result;
        }

        /// <summary>
        /// Counts ASCII vowels and consonants in <paramref name="input"/>. The letter 'y' is treated as a consonant.
        /// </summary>
        public static LetterCounts CountVowelsAndConsonants(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            int vowels = 0, consonants = 0;
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    continue;
                }
                switch (char.ToLowerInvariant(c))
                {
                    case 'a':
                    case 'e':
                    case 'i':
                    case 'o':
                    case 'u':
                        vowels++;
                        break;
                    default:
                        consonants++;
                        break;
                }
            }

            return new LetterCounts(vowels, consonants);
        }

        /// <summary>
        /// Returns an uppercase, unseparated hex representation of <paramref name="bytes"/>
        /// (e.g. <c>{0xDE, 0xAD}</c> → <c>"DEAD"</c>).
        /// </summary>
        public static string BytesToHex(byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(bytes);
            return Convert.ToHexString(bytes);
        }

        /// <summary>
        /// Returns <paramref name="input"/> with every occurrence of <paramref name="c"/> removed, using LINQ.
        /// </summary>
        public static string RemoveCharacterFromStringLinq(string input, char c)
        {
            ArgumentNullException.ThrowIfNull(input);
            return new string(input.Where(ch => ch != c).ToArray());
        }

        /// <summary>
        /// Returns <paramref name="input"/> with every occurrence of <paramref name="c"/> removed, using <see cref="StringBuilder"/>.
        /// Faster and lower-allocation than <see cref="RemoveCharacterFromStringLinq"/> for long inputs.
        /// </summary>
        public static string RemoveCharacterFromString(string input, char c)
        {
            ArgumentNullException.ThrowIfNull(input);

            var sb = new StringBuilder(input.Length);
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

    /// <summary>Vowel and consonant counts for an input string.</summary>
    public readonly record struct LetterCounts(int Vowels, int Consonants);
}
