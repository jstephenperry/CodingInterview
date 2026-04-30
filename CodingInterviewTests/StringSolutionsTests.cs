namespace CodingInterviewImplementations.Tests
{
    [TestFixture]
    [TestOf(typeof(StringSolutions))]
    public class StringSolutionsTests
    {
        [Test]
        public void FindMaxOccurringCharacter_ReturnsExpected()
        {
            var testData = new (string Input, (char, int)? Expected)[]
            {
                ("", null),
                ("a", ('a', 1)),
                ("aa", ('a', 2)),
                ("ab", ('a', 1)),
                ("aab", ('a', 2)),
                ("abb", ('b', 2)),
                ("aabb", ('a', 2)),
                ("aaabbb", ('a', 3)),
                ("aaabbbcccdddeeefffggghhhiiijjjjjkkklllmmnnnooo", ('j', 5)),
                ("aaabbbcccdddeeefffggghhhiiijjjkkklllmmnnnoooo", ('o', 4))
            };

            foreach (var (input, expected) in testData)
            {
                Assert.That(StringSolutions.FindMaxOccurringCharacter(input), Is.EqualTo(expected));
            }
        }

        [Test]
        [TestCase("a", "a", true)]
        [TestCase("cat", "act", true)]
        [TestCase("aide", "idea", true)]
        [TestCase("melon", "lemon", true)]
        [TestCase("", "", true)]
        [TestCase("a", "b", false)]
        [TestCase("abc", "def", false)]
        [TestCase("def", "efg", false)]
        [Parallelizable(ParallelScope.All)]
        public void IsAnagram_AllImplementationsAgree(string s1, string s2, bool expected)
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.IsAnagramWithDictionaryFrequency(s1, s2), Is.EqualTo(expected));
                Assert.That(StringSolutions.IsAnagramWithSorting(s1, s2), Is.EqualTo(expected));
                Assert.That(StringSolutions.IsAnagramWithLinqSorting(s1, s2), Is.EqualTo(expected));
            });
        }

        [Test]
        public void IsAnagram_NullInputs_Throws()
        {
            Assert.Multiple(() =>
            {
                Assert.That(() => StringSolutions.IsAnagramWithDictionaryFrequency(null!, "a"), Throws.ArgumentNullException);
                Assert.That(() => StringSolutions.IsAnagramWithSorting("a", null!), Throws.ArgumentNullException);
                Assert.That(() => StringSolutions.IsAnagramWithLinqSorting(null!, null!), Throws.ArgumentNullException);
            });
        }

        [Test]
        [TestCase("", "")]
        [TestCase("aaaabbaa", "aabbaa")]
        [TestCase("abc", "a")]
        [TestCase("a", "a")]
        [TestCase("racecar", "racecar")]
        [TestCase("abba", "abba")]
        [TestCase("babad", "bab")]
        [TestCase("cbbd", "bb")]
        [TestCase("aaaaa", "aaaaa")]
        [TestCase("xyzracecarxyz", "racecar")]
        [Parallelizable(ParallelScope.All)]
        public void GetLongestPalindromeSubstring_ReturnsExpected(string input, string expected)
        {
            Assert.That(StringSolutions.GetLongestPalindromeSubstring(input), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("I", 1)]
        [TestCase("III", 3)]
        [TestCase("IV", 4)]
        [TestCase("IX", 9)]
        [TestCase("LVIII", 58)]
        [TestCase("MCMXCIV", 1994)]
        [TestCase("MMMCMXCIX", 3999)]
        [Parallelizable(ParallelScope.All)]
        public void RomanNumeralToInteger_ReturnsExpected(string input, int expected)
        {
            Assert.That(StringSolutions.RomanNumeralToInteger(input), Is.EqualTo(expected));
        }

        [Test]
        public void RomanNumeralToInteger_RejectsInvalidInput()
        {
            Assert.Multiple(() =>
            {
                Assert.That(() => StringSolutions.RomanNumeralToInteger(""), Throws.ArgumentException);
                Assert.That(() => StringSolutions.RomanNumeralToInteger("iv"), Throws.ArgumentException);
                Assert.That(() => StringSolutions.RomanNumeralToInteger("IIA"), Throws.ArgumentException);
                Assert.That(() => StringSolutions.RomanNumeralToInteger(null!), Throws.ArgumentNullException);
            });
        }

        [Test]
        public void FindMaxOccurringWord_ReturnsExpected()
        {
            var cases = new (string Text, (string, int)? Expected)[]
            {
                ("a a b b b c c c c c", ("c", 5)),
                ("the quick brown fox", ("the", 1)),
                ("one two two three three three", ("three", 3)),
                ("repeat repeat repeat", ("repeat", 3)),
                ("  many   spaces   here  ", ("many", 1)),
            };

            foreach (var (text, expected) in cases)
            {
                Assert.That(StringSolutions.FindMaxOccurringWord(text), Is.EqualTo(expected));
            }
        }

        [Test]
        public void FindMaxOccurringWord_EmptyText_ReturnsNull()
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.FindMaxOccurringWord(""), Is.Null);
                Assert.That(StringSolutions.FindMaxOccurringWord("   \t\n  "), Is.Null);
                Assert.That(() => StringSolutions.FindMaxOccurringWord(null!), Throws.ArgumentNullException);
            });
        }

        [Test]
        public void GetWordCount_ReturnsExpected()
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.GetWordCount(null), Is.EqualTo(0));
                Assert.That(StringSolutions.GetWordCount(""), Is.EqualTo(0));
                Assert.That(StringSolutions.GetWordCount("   "), Is.EqualTo(0));
                Assert.That(StringSolutions.GetWordCount("hello"), Is.EqualTo(1));
                Assert.That(StringSolutions.GetWordCount("hello world"), Is.EqualTo(2));
                Assert.That(StringSolutions.GetWordCount("  many   spaces   here  "), Is.EqualTo(3));
                Assert.That(StringSolutions.GetWordCount("tab\tseparated\nlines"), Is.EqualTo(3));
            });
        }

        [Test]
        public void CountVowelsAndConsonants_ReturnsExpected()
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.CountVowelsAndConsonants("Hello World"), Is.EqualTo(new LetterCounts(3, 7)));
                Assert.That(StringSolutions.CountVowelsAndConsonants(""), Is.EqualTo(new LetterCounts(0, 0)));
                Assert.That(StringSolutions.CountVowelsAndConsonants("123!?"), Is.EqualTo(new LetterCounts(0, 0)));
                Assert.That(StringSolutions.CountVowelsAndConsonants("AEIOU"), Is.EqualTo(new LetterCounts(5, 0)));
            });
        }

        [Test]
        public void BytesToHex_ReturnsUppercaseUnseparatedHex()
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.BytesToHex(Array.Empty<byte>()), Is.EqualTo(""));
                Assert.That(StringSolutions.BytesToHex(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF }), Is.EqualTo("DEADBEEF"));
                Assert.That(StringSolutions.BytesToHex(new byte[] { 0x00, 0x0F }), Is.EqualTo("000F"));
            });
        }

        [Test]
        public void RemoveCharacterFromString_BothImplementationsAgree()
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.RemoveCharacterFromString("hello world", 'l'), Is.EqualTo("heo word"));
                Assert.That(StringSolutions.RemoveCharacterFromStringLinq("hello world", 'l'), Is.EqualTo("heo word"));
                Assert.That(StringSolutions.RemoveCharacterFromString("", 'x'), Is.EqualTo(""));
                Assert.That(StringSolutions.RemoveCharacterFromStringLinq("", 'x'), Is.EqualTo(""));
                Assert.That(StringSolutions.RemoveCharacterFromString("abc", 'z'), Is.EqualTo("abc"));
            });
        }
    }
}
