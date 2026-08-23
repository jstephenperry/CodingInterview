using System.Globalization;

namespace CodingInterviewImplementations.Tests
{
    public class StringSolutionsTests
    {
        public static TheoryData<string?, KeyValuePair<char, int>> MaxOccurringCharacterCases => new()
        {
            { "", default },                                        // empty input
            { null, default },                                      // null input
            { "a", new('a', 1) },                                   // single character
            { "aa", new('a', 2) },                                  // one repeated character
            { "ab", new('a', 1) },                                  // tie resolves to the first
            { "aab", new('a', 2) },                                 // clear winner first
            { "abb", new('b', 2) },                                 // clear winner last
            { "aabb", new('a', 2) },                                // two-way tie
            { "aaabbbccc", new('a', 3) },                           // three-way tie
            { "aaabbbcccdddeeefffggghhhiiijjjjjkkklllmmnnnooo", new('j', 5) },  // winner in the middle
            { "aaabbbcccdddeeefffggghhhiiijjjkkklllmmnnnoooo", new('o', 4) },   // winner at the end
            { "Aa", new('A', 1) },                                  // case sensitive
            { "  x", new(' ', 2) }                                  // whitespace counts
        };

        public static TheoryData<string?, string?, bool> AnagramCases => new()
        {
            { "a", "a", true },
            { "cat", "act", true },
            { "aide", "idea", true },
            { "melon", "lemon", true },
            { "", "", true },               // two empty strings are anagrams
            { "aabb", "bbaa", true },
            { "a", "b", false },
            { "abc", "def", false },
            { "def", "efg", false },
            { "aab", "abb", false },        // same characters, different multiplicities
            { "abc", "abcd", false },       // different lengths
            { "Cat", "act", false },        // case sensitive
            { null, "abc", false },
            { "abc", null, false },
            { null, null, false }
        };

        public static TheoryData<string, string> PalindromeCases => new()
        {
            { "aaaabbaa", "aabbaa" },
            { "abc", "a" },
            { "", "" },
            { "a", "a" },
            { "aa", "aa" },                                 // even length palindrome
            { "aba", "aba" },                               // odd length palindrome
            { "babad", "bab" },
            { "cbbd", "bb" },
            { "forgeeksskeegfor", "geeksskeeg" },
            { "abacdfgdcaba", "aba" }                       // first of several equal-length palindromes
        };

        public static TheoryData<string?, int> RomanNumeralCases => new()
        {
            { "III", 3 },
            { "IV", 4 },
            { "IX", 9 },
            { "LVIII", 58 },
            { "MCMXCIV", 1994 },
            { "MMMCMXCIX", 3999 },
            { "I", 1 },
            { "MMXXIV", 2024 },
            { "", 0 },
            { null, 0 }
        };

        public static TheoryData<string> InvalidRomanNumerals =>
        [
            "hello",
            "MCM!",
            "iv",       // lower case is not accepted
            "123"
        ];

        public static TheoryData<string?, int> WordCountCases => new()
        {
            { null, 0 },
            { "", 0 },
            { "   ", 0 },
            { "one", 1 },
            { "one two three", 3 },
            { "  leading and trailing  ", 3 },
            { "double  spaced  words", 3 }
        };

        public static TheoryData<string, char, string> RemoveCharacterCases => new()
        {
            { "hello world", 'l', "heo word" },
            { "aaa", 'a', "" },
            { "abc", 'z', "abc" },
            { "", 'a', "" }
        };

        [Theory]
        [MemberData(nameof(MaxOccurringCharacterCases))]
        public void FindMaxOccurringCharacterLinq_EqualsExpectedReturnValue(
            string? input, KeyValuePair<char, int> expected)
        {
            Assert.Equal(expected, StringSolutions.FindMaxOccurringCharacterLinq(input));
        }

        [Theory]
        [MemberData(nameof(AnagramCases))]
        public void IsAnagram_AllThreeImplementationsAgree(string? s1, string? s2, bool expected)
        {
            // Differential testing: the three implementations must not only be right, they must
            // never disagree with each other.
            Assert.Multiple(
                () => Assert.Equal(expected, StringSolutions.IsAnagramWithDictionaryFrequency(s1, s2)),
                () => Assert.Equal(expected, StringSolutions.IsAnagramWithSorting(s1, s2)),
                () => Assert.Equal(expected, StringSolutions.IsAnagramWithLinqSorting(s1, s2)));
        }

        [Theory]
        [MemberData(nameof(PalindromeCases))]
        public void GetLongestPalindromeSubstring_EqualsExpectedReturnValue(string input, string expected)
        {
            Assert.Equal(expected, StringSolutions.GetLongestPalindromeSubstring(input));
        }

        [Fact]
        public void GetLongestPalindromeSubstring_ResultIsAlwaysAPalindromeAndASubstring()
        {
            // A property check rather than a fixed expectation: whatever comes back must actually be
            // a palindrome and must actually appear in the input.
            string[] inputs = ["", "a", "abcde", "aabbaa", "racecarx", "xyzzyx", "abbaccabba"];

            Assert.All(inputs, input =>
            {
                string result = StringSolutions.GetLongestPalindromeSubstring(input);
                string reversed = new([.. result.Reverse()]);

                Assert.Contains(result, input, StringComparison.Ordinal);
                Assert.Equal(result, reversed);
            });
        }

        [Fact]
        public void GetLongestPalindromeSubstring_WithANullInput_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => StringSolutions.GetLongestPalindromeSubstring(null!));
        }

        [Theory]
        [MemberData(nameof(RomanNumeralCases))]
        public void RomanNumeralToInteger_EqualsExpectedReturnValue(string? input, int expected)
        {
            Assert.Equal(expected, StringSolutions.RomanNumeralToInteger(input));
        }

        [Theory]
        [MemberData(nameof(InvalidRomanNumerals))]
        public void RomanNumeralToInteger_WithAnInvalidCharacter_Throws(string input)
        {
            // This used to silently skip unknown characters, so "hello" quietly returned 0 and was
            // indistinguishable from an empty input.
            Assert.Throws<FormatException>(() => StringSolutions.RomanNumeralToInteger(input));
        }

        [Fact]
        public void FindMaxOccurringWordLinq_EqualsExpectedReturnValue()
        {
            Assert.Multiple(
                () => Assert.Equal(
                    new KeyValuePair<string, int>("c", 5),
                    StringSolutions.FindMaxOccurringWordLinq(["a a b b b c c c c c"])),
                () => Assert.Equal(
                    new KeyValuePair<string, int>("the", 3),
                    StringSolutions.FindMaxOccurringWordLinq(["the cat", "the hat", "the end"])),
                () => Assert.Equal(
                    new KeyValuePair<string, int>("one", 1),
                    StringSolutions.FindMaxOccurringWordLinq(["one two"])));
        }

        [Fact]
        public void FindMaxOccurringWordLinq_SplitsOnAllWhitespace()
        {
            Assert.Equal(
                new KeyValuePair<string, int>("x", 3),
                StringSolutions.FindMaxOccurringWordLinq(["x\ty\nx  x"]));
        }

        [Fact]
        public void FindMaxOccurringWordLinq_WithNoWords_ReturnsTheDefault()
        {
            Assert.Multiple(
                () => Assert.Equal(default, StringSolutions.FindMaxOccurringWordLinq(null)),
                () => Assert.Equal(default, StringSolutions.FindMaxOccurringWordLinq([])),
                () => Assert.Equal(default, StringSolutions.FindMaxOccurringWordLinq(["   "])));
        }

        [Theory]
        [MemberData(nameof(WordCountCases))]
        public void GetWordCount_EqualsExpectedReturnValue(string? input, int expected)
        {
            Assert.Equal(expected, StringSolutions.GetWordCount(input));
        }

        [Fact]
        public void GetWordCount_CountsAcrossAllWhitespace()
        {
            // Splitting on the space character alone returned 2 for this input instead of 4.
            Assert.Equal(4, StringSolutions.GetWordCount("one\ttwo\nthree\r\nfour"));
        }

        [Fact]
        public void FindVowelsAndConsonants_CountsLettersOnly()
        {
            Dictionary<string, int>? result = StringSolutions.FindVowelsAndConsonants("Hello, World! 123");

            Assert.NotNull(result);
            Assert.Multiple(
                () => Assert.Equal(3, result["Vowels"]),         // e, o, o
                () => Assert.Equal(7, result["Consonants"]));    // H, l, l, W, r, l, d
        }

        [Fact]
        public void FindVowelsAndConsonants_IsCaseInsensitiveAndCultureInvariant()
        {
            // char.ToLower is culture sensitive: under a Turkish culture it maps 'I' to a dotless
            // 'i', which is not in the vowel set, so the count silently changed with the thread
            // culture. ToLowerInvariant fixes that; this test pins it.
            CultureInfo original = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
                Dictionary<string, int>? result = StringSolutions.FindVowelsAndConsonants("AEIOU");

                Assert.NotNull(result);
                Assert.Multiple(
                    () => Assert.Equal(5, result["Vowels"]),
                    () => Assert.Equal(0, result["Consonants"]));
            }
            finally
            {
                CultureInfo.CurrentCulture = original;
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FindVowelsAndConsonants_WithNoInput_ReturnsNull(string? input)
        {
            Assert.Null(StringSolutions.FindVowelsAndConsonants(input));
        }

        [Fact]
        public void ByteArrayToString_ProducesUpperCaseHexWithNoSeparators()
        {
            Assert.Multiple(
                () => Assert.Equal("000FA5FF", StringSolutions.ByteArrayToString([0x00, 0x0F, 0xA5, 0xFF])),
                () => Assert.Equal(string.Empty, StringSolutions.ByteArrayToString([])),
                () => Assert.Equal("DEADBEEF", StringSolutions.ByteArrayToString([0xDE, 0xAD, 0xBE, 0xEF])));
        }

        [Fact]
        public void ByteArrayToString_WithANullArray_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => StringSolutions.ByteArrayToString(null!));
        }

        [Theory]
        [MemberData(nameof(RemoveCharacterCases))]
        public void RemoveCharacterFromString_BothImplementationsAgree(string input, char c, string expected)
        {
            Assert.Multiple(
                () => Assert.Equal(expected, StringSolutions.RemoveCharacterFromString(input, c)),
                () => Assert.Equal(expected, StringSolutions.RemoveCharacterFromStringLinq(input, c)));
        }
    }
}
