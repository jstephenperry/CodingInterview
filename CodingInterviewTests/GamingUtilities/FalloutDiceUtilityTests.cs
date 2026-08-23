using CodingInterviewImplementations.GamingUtilities;

namespace CodingInterviewImplementations.Tests.GamingUtilities
{
    public class FalloutDiceUtilityTests
    {
        public static TheoryData<int, string> HitLocationCases => new()
        {
            { 1, "Head" },
            { 2, "Head" },
            { 3, "Torso" },
            { 8, "Torso" },
            { 9, "Left Arm" },
            { 11, "Left Arm" },
            { 12, "Right Arm" },
            { 14, "Right Arm" },
            { 15, "Left Leg" },
            { 17, "Left Leg" },
            { 18, "Right Leg" },
            { 20, "Right Leg" }
        };

        public static TheoryData<int> OutOfRangeRolls => new() { 0, 21, -1 };

        [Fact]
        public void HitLocationTable_CoversEveryFaceOfTheDie()
        {
            Assert.Multiple(
                () => Assert.Equal(
                    FalloutDiceUtility.HitLocationDieSides,
                    FalloutDiceUtility.Fallout2D20HitLocationDictionary.Count),
                () => Assert.All(
                    Enumerable.Range(1, FalloutDiceUtility.HitLocationDieSides),
                    roll => Assert.True(
                        FalloutDiceUtility.Fallout2D20HitLocationDictionary.ContainsKey(roll),
                        $"no hit location mapped for a roll of {roll}")));
        }

        [Theory]
        [MemberData(nameof(HitLocationCases))]
        public void HitLocationTable_MatchesThePublishedSpread(int roll, string expected)
        {
            Assert.Equal(expected, FalloutDiceUtility.GetHitLocation(roll));
        }

        [Fact]
        public void HitLocationTable_HasTheExpectedNumberOfFacesPerLocation()
        {
            Dictionary<string, int> facesPerLocation = FalloutDiceUtility.Fallout2D20HitLocationDictionary
                .GroupBy(pair => pair.Value)
                .ToDictionary(group => group.Key, group => group.Count());

            Assert.Multiple(
                () => Assert.Equal(2, facesPerLocation["Head"]),
                () => Assert.Equal(6, facesPerLocation["Torso"]),
                () => Assert.Equal(3, facesPerLocation["Left Arm"]),
                () => Assert.Equal(3, facesPerLocation["Right Arm"]),
                () => Assert.Equal(3, facesPerLocation["Left Leg"]),
                () => Assert.Equal(3, facesPerLocation["Right Leg"]));
        }

        [Theory]
        [MemberData(nameof(OutOfRangeRolls))]
        public void GetHitLocation_OutsideTheDieRange_Throws(int roll)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => FalloutDiceUtility.GetHitLocation(roll));
        }

        [Fact]
        public void RollHitLocation_EventuallyProducesEveryLocation()
        {
            var observed = new HashSet<string>();
            for (int i = 0; i < 5_000; i++)
            {
                observed.Add(FalloutDiceUtility.RollHitLocation());
            }

            TestSupport.EquivalentTo(
                new[] { "Head", "Torso", "Left Arm", "Right Arm", "Left Leg", "Right Leg" },
                observed);
        }

        [Fact]
        public void RollHitLocation_HitsTheHeadAtRoughlyTheRightRate()
        {
            // Two of twenty faces are the head, so about a tenth of rolls. This is the test that
            // would catch a d20 that could not roll a 20, since Right Leg would be under-represented.
            const int Rolls = 60_000;
            int head = 0, rightLeg = 0;

            for (int i = 0; i < Rolls; i++)
            {
                switch (FalloutDiceUtility.RollHitLocation())
                {
                    case "Head": head++; break;
                    case "Right Leg": rightLeg++; break;
                    default: break;
                }
            }

            Assert.Multiple(
                () => Assert.True(head is >= 5_400 and <= 6_600, $"head hits: {head}, expected about 6,000"),
                () => Assert.True(rightLeg is >= 8_100 and <= 9_900, $"right leg hits: {rightLeg}, expected about 9,000"));
        }
    }
}
