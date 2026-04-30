namespace CodingInterviewImplementations.Tests
{
    [TestFixture]
    [TestOf(typeof(StackSolutions))]
    public class StackSolutionsTests
    {
        [Test]
        public void IsBalancedBracketsTest_EqualsExpectedReturnValue()
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

        [Test]
        public void InvertStackTest_ReversesOrder()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            StackSolutions.InvertStack(stack);

            Assert.Multiple(() =>
            {
                Assert.That(stack.Pop(), Is.EqualTo(1));
                Assert.That(stack.Pop(), Is.EqualTo(2));
                Assert.That(stack.Pop(), Is.EqualTo(3));
            });
        }
    }
}