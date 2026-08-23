namespace CodingInterviewImplementations.Tests
{
    public class DiceUtilityTests
    {
        public static TheoryData<int> StandardDice => new() { 4, 6, 8, 10, 12, 20, 100 };

        [Theory]
        [MemberData(nameof(StandardDice))]
        public void RollDie_ProducesEveryFace(int sides)
        {
            // The regression test for the original defect: RollDie delegated to an exclusive-upper-
            // bound helper as if it were inclusive, so the highest face never came up. A d6 rolled
            // only 1 through 5 and the old "is the result within 1..6" assertion never noticed.
            var observed = new HashSet<int>();
            int rolls = sides * 200;

            for (int i = 0; i < rolls; i++)
            {
                observed.Add(DiceUtility.RollDie(sides));
            }

            Assert.Multiple(
                () => Assert.True(observed.Contains(sides), $"a d{sides} never rolled its highest face"),
                () => Assert.True(observed.Contains(1), $"a d{sides} never rolled a 1"),
                () => Assert.True(observed.Count == sides, $"a d{sides} produced {observed.Count} of {sides} faces"),
                () => Assert.Equal(sides, observed.Max()),
                () => Assert.Equal(1, observed.Min()));
        }

        [Fact]
        public void RollDie_IsApproximatelyFair()
        {
            const int Rolls = 120_000;
            int[] counts = new int[7];

            for (int i = 0; i < Rolls; i++)
            {
                counts[DiceUtility.RollDie(6)]++;
            }

            // Expected 20,000 per face, standard deviation ~129. A 2,000 window is over 15 sigma.
            Assert.Multiple(
                [.. Enumerable.Range(1, 6).Select(face => (Action)(() => Assert.True(
                    counts[face] >= 18_000 && counts[face] <= 22_000,
                    $"face {face} came up {counts[face]} times")))]);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void RollDie_WithoutAPositiveFaceCount_Throws(int sides)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DiceUtility.RollDie(sides));
        }

        [Fact]
        public void RollDie_WithOneSide_AlwaysReturnsOne()
        {
            for (int i = 0; i < 100; i++)
            {
                Assert.Equal(1, DiceUtility.RollDie(1));
            }
        }

        [Theory]
        [MemberData(nameof(StandardDice))]
        public void RollDice_SumsWithinTheAchievableRange(int sides)
        {
            const int Count = 3;
            const int Modifier = 2;

            for (int i = 0; i < 500; i++)
            {
                Assert.InRange(DiceUtility.RollDice(sides, Count), Count, sides * Count);
                Assert.InRange(
                    DiceUtility.RollDice(sides, Count, Modifier),
                    Count + Modifier,
                    (sides * Count) + Modifier);
            }
        }

        [Fact]
        public void RollDice_ReachesBothExtremesOfTheSum()
        {
            // Two d2 can total 2 or 4; both ends must be achievable.
            var observed = new HashSet<int>();
            for (int i = 0; i < 5_000; i++)
            {
                observed.Add(DiceUtility.RollDice(2, 2));
            }

            TestSupport.EquivalentTo(new[] { 2, 3, 4 }, observed);
        }

        [Fact]
        public void RollDice_WithNoDice_ReturnsZero()
        {
            Assert.Multiple(
                () => Assert.Equal(0, DiceUtility.RollDice(6, 0)),
                () => Assert.Equal(3, DiceUtility.RollDice(6, 0, 3)));
        }

        [Fact]
        public void RollDice_WithANegativeCount_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DiceUtility.RollDice(6, -1));
        }
    }
}
