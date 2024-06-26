﻿namespace CodingInterviewImplementations.Tests
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
               ("a a b b b c c c c c", new KeyValuePair<string, int>("c", 5))
            };

            foreach (var (input, expected) in testCases)
            {
                Assert.That(StringSolutions.FindMaxOccurringWordLinq([.. input.Split(" ")]), Is.EqualTo(expected));
            }
        }
    }
}