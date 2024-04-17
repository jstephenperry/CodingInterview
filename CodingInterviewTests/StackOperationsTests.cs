using CodingInterviewImplementations;

namespace CodingInterviewTests
{
    [TestFixture]
    [TestOf(typeof(StackOperations))]
    public class StackOperationsTests
    {
        [Test]
        public void TestIsBalancedBrackets()
        {
            var testCases = new[]
            {
                ("", true),
                ("()", true),
                ("{}", true),
                ("[]", true),
                ("(]", false),
                ("{]", false),
                ("[)", false),
                ("([)]", false),
                ("{[()]}", true)
            };

            foreach (var (input, expected) in testCases)
            {
                Assert.That(StackOperations.IsBalancedBrackets(input), Is.EqualTo(expected));
            }
        }
    }
}