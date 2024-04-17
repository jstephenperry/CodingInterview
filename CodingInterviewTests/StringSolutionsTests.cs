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
                ("", new Dictionary<char, int>()),
                ("a", new Dictionary<char, int> { { 'a', 1 } }),
                ("aa", new Dictionary<char, int> { { 'a', 2 } }),
                ("ab", new Dictionary<char, int> { { 'a', 1 }, { 'b', 1 } }),
                ("aab", new Dictionary<char, int> { { 'a', 2 }, { 'b', 1 } }),
                ("aabb", new Dictionary<char, int> { { 'a', 2 }, { 'b', 2 } }),
                ("aabbcc", new Dictionary<char, int> { { 'a', 2 }, { 'b', 2 }, { 'c', 2 } }),
                ("aabbccdd", new Dictionary<char, int> { { 'a', 2 }, { 'b', 2 }, { 'c', 2 }, { 'd', 2 } }),
                ("aabbccdde", new Dictionary<char, int> { { 'a', 2 }, { 'b', 2 }, { 'c', 2 }, { 'd', 2 }, { 'e', 1 } }),
                ("aabbccddeeff", new Dictionary<char, int> { { 'a', 2 }, { 'b', 2 }, { 'c', 2 }, { 'd', 2 }, { 'e', 2 }, { 'f', 2 } }),
                ("aabbccddeeffgg", new Dictionary<char, int> { { 'a', 2 }, { 'b', 2 }, { 'c', 2 }, { 'd', 2 }, { 'e', 2 }, { 'f', 2 }, { 'g', 2 } }),
                ("aabbccddeeffgghh", new Dictionary<char, int> { { 'a', 2 }, { 'b', 2 }, { 'c', 2 }, { 'd', 2 }, { 'e', 2 }, { 'f', 2 }, { 'g', 2 }, { 'h', 2 } })
            };

            foreach (var (input, expected) in testData)
            {
                Assert.That(StringSolutions.FindMaxOccurringCharacterLinq(input), Is.EqualTo(expected));
            }
        }
    }
}