namespace CodingInterviewImplementations.Tests
{
    public class StackOperationsTests
    {
        /// <summary>
        /// Cases carry a comment describing what each one exercises, so a failure is readable
        /// alongside the raw argument values xUnit reports.
        /// </summary>
        public static TheoryData<string, bool> BracketCases => new()
        {
            { "", true },                       // empty string
            { "()", true },                     // parentheses
            { "{}", true },                     // braces
            { "[]", true },                     // square brackets
            { "{[()]}", true },                 // nested, all three kinds
            { "()[]{}", true },                 // sequential pairs
            { "((()))", true },                 // deeply nested
            { "(a + b) * [c]", true },          // brackets around other characters
            { "no brackets at all", true },     // no brackets
            { "(]", false },                    // mismatched paren and square
            { "{]", false },                    // mismatched brace and square
            { "[)", false },                    // mismatched square and paren
            { "([)]", false },                  // interleaved rather than nested
            { "(", false },                     // unclosed opening
            { ")", false },                     // unmatched closing
            { "())", false },                   // one closing too many
            { "(()", false },                   // one opening too many
            { "(a", false }                     // unclosed opening with other characters
        };

        [Theory]
        [MemberData(nameof(BracketCases))]
        public void IsBalancedBrackets_EqualsExpectedReturnValue(string input, bool expected)
        {
            Assert.Equal(expected, StackSolutions.IsBalancedBrackets(input));
        }

        [Fact]
        public void IsBalancedBrackets_WithANullInput_ReturnsTrue()
        {
            Assert.True(StackSolutions.IsBalancedBrackets(null));
        }

        [Fact]
        public void InvertStack_ReversesTheOrder()
        {
            // Pushed 1..5, so popping yields 5,4,3,2,1. After inverting it must yield 1..5.
            var stack = new Stack<int>([1, 2, 3, 4, 5]);

            StackSolutions.InvertStack(stack);

            Assert.Equal(new[] { 1, 2, 3, 4, 5 }, stack.ToArray());
        }

        [Fact]
        public void InvertStack_PreservesTheCount()
        {
            var stack = new Stack<int>([.. Enumerable.Range(0, 100)]);

            StackSolutions.InvertStack(stack);

            Assert.Equal(100, stack.Count);
        }

        [Fact]
        public void InvertStack_AppliedTwice_RestoresTheOriginalOrder()
        {
            int[] values = [3, 1, 4, 1, 5, 9, 2, 6];
            var stack = new Stack<int>(values);
            int[] before = stack.ToArray();

            StackSolutions.InvertStack(stack);
            StackSolutions.InvertStack(stack);

            Assert.Equal(before, stack.ToArray());
        }

        [Fact]
        public void InvertStack_OnASingleElementStack_IsANoOp()
        {
            var stack = new Stack<int>([7]);

            StackSolutions.InvertStack(stack);

            Assert.Equal(new[] { 7 }, stack.ToArray());
        }

        [Fact]
        public void InvertStack_OnAnEmptyStack_IsANoOp()
        {
            var stack = new Stack<int>();

            StackSolutions.InvertStack(stack);

            Assert.Empty(stack);
        }

        [Fact]
        public void InvertStack_WithANullStack_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => StackSolutions.InvertStack(null!));
        }
    }
}
