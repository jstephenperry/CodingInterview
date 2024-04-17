namespace CodingInterviewImplementations.Tests
{
    [TestFixture]
    [TestOf(typeof(StringSolutions))]
    public class StringSolutionsTests
    {
        [Test]
        public void FindMaxOccurringCharacterLinqTest()
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
    }
}