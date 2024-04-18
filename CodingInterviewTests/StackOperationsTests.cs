namespace CodingInterviewImplementations.Tests
{
    [TestFixture]
    [TestOf(typeof(StackSolutions))]
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
                Assert.That(StackSolutions.IsBalancedBrackets(input), Is.EqualTo(expected));
            }
        }
    }
}