namespace CodingInterviewImplementations.Tests
{
    [TestFixture]
    [TestOf(typeof(StringSolutions))]
    public class StringSolutionsTests
    {
        [Test]
        public void FindMaxOccurringCharacterLinqTest_EqualsExpectedReturnValue()
        {
            var testData = new[]
            {
                ("", default(KeyValuePair<char, int>)),
                ("a", new KeyValuePair<char, int>('a', 1)),
                ("aa", new KeyValuePair<char, int>('a', 2)),
                ("ab", new KeyValuePair<char, int>('a', 1)),
                ("aab", new KeyValuePair<char, int>('a', 2)),
                ("abb", new KeyValuePair<char, int>('b', 2)),
                ("aabb", new KeyValuePair<char, int>('a', 2)),
                ("aaabbb", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbccc", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccddd", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeee", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefff", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggg", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggghhh", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggghhhiii", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggghhhiiijjj", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggghhhiiijjjkkk", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggghhhiiijjjkkklll", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggghhhiiijjjkkklllmmm", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggghhhiiijjjkkklllmmnnn", new KeyValuePair<char, int>('a', 3)),
                ("aaabbbcccdddeeefffggghhhiiijjjjjkkklllmmnnnooo", new KeyValuePair<char, int>('j', 5)),
                ("aaabbbcccdddeeefffggghhhiiijjjkkklllmmnnnoooo", new KeyValuePair<char, int>('o', 4))
            };

            foreach (var (input, expected) in testData)
            {
                Assert.That(StringSolutions.FindMaxOccurringCharacterLinq(input), Is.EqualTo(expected));
            }
        }

        [Test]
        [TestCase("a", "a", true)]
        [TestCase("cat", "act", true)]
        [TestCase("aide", "idea", true)]
        [TestCase("melon", "lemon", true)]
        [TestCase("a", "b", false)]
        [TestCase("abc", "def", false)]
        [TestCase("def", "efg", false)]
        [Parallelizable(ParallelScope.All)]
        public void IsAnagramTest_EqualsExpectedReturnValue(string s1, string s2, Boolean expected)
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.IsAnagramWithDictionaryFrequency(s1, s2), Is.EqualTo(expected));
                Assert.That(StringSolutions.IsAnagramWithSorting(s1, s2), Is.EqualTo(expected));
                Assert.That(StringSolutions.IsAnagramWithLinqSorting(s1, s2), Is.EqualTo(expected));
            });
        }

        [Test]
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
        public void GetLongestPalindromeSubstringTest_EqualsExpectedReturnValue(string input, string expected)
        {
            Assert.That(StringSolutions.GetLongestPalindromeSubstring(input), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("III", 3)]
        [TestCase("IV", 4)]
        [TestCase("IX", 9)]
        [TestCase("LVIII", 58)]
        [TestCase("MCMXCIV", 1994)]
        [Parallelizable(ParallelScope.All)]
        public void RomanNumeralToIntegerTest_EqualsExpectedReturnValue(string input, int expected)
        {
            Assert.That(StringSolutions.RomanNumeralToInteger(input), Is.EqualTo(expected));
        }

        [Test]
        public void FindMaxOccurringWordLinqTest()
        {
            var testCases = new[]
            {
                ("a a b b b c c c c c", new KeyValuePair<string, int>("c", 5)),
                ("the quick brown fox", new KeyValuePair<string, int>("the", 1)),
                ("one two two three three three", new KeyValuePair<string, int>("three", 3)),
                ("repeat repeat repeat", new KeyValuePair<string, int>("repeat", 3))
            };

            foreach (var (input, expected) in testCases)
            {
                Assert.That(StringSolutions.FindMaxOccurringWordLinq([.. input.Split(" ")]), Is.EqualTo(expected));
            }
        }

        [Test]
        public void FindMaxOccurringWordLinq_NullOrEmpty_ReturnsDefault()
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.FindMaxOccurringWordLinq(null!), Is.EqualTo(default(KeyValuePair<string, int>)));
                Assert.That(StringSolutions.FindMaxOccurringWordLinq(new List<string>()), Is.EqualTo(default(KeyValuePair<string, int>)));
            });
        }

        [Test]
        public void GetWordCountTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(StringSolutions.GetWordCount(""), Is.EqualTo(0));
                Assert.That(StringSolutions.GetWordCount("hello"), Is.EqualTo(1));
                Assert.That(StringSolutions.GetWordCount("hello world"), Is.EqualTo(2));
                Assert.That(StringSolutions.GetWordCount("  many   spaces   here  "), Is.EqualTo(3));
            });
        }

        [Test]
        public void FindVowelsAndConsonantsTest()
        {
            var result = StringSolutions.FindVowelsAndConsonants("Hello World");
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result!["Vowels"], Is.EqualTo(3));
                Assert.That(result["Consonants"], Is.EqualTo(7));
            });
        }
    }
}