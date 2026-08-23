using System.Collections.Frozen;

namespace CodingInterviewImplementations.GamingUtilities
{
    /// <summary>
    /// Dice helpers specific to the Fallout 2d20 tabletop system.
    /// </summary>
    public static class FalloutDiceUtility
    {
        /// <summary>
        /// The number of faces on the hit location die.
        /// </summary>
        public const int HitLocationDieSides = 20;

        /// <summary>
        /// Maps each face of a d20 to the body part it targets.
        /// </summary>
        /// <remarks>
        /// Exposed as a read-only frozen map so that callers cannot mutate the shared table. The
        /// previous <see cref="Dictionary{TKey, TValue}"/> field was writable by anyone holding a
        /// reference to it.
        /// </remarks>
        public static readonly FrozenDictionary<int, string> Fallout2D20HitLocationDictionary =
            new Dictionary<int, string>
            {
                { 1, "Head" }, { 2, "Head" },
                { 3, "Torso" }, { 4, "Torso" }, { 5, "Torso" }, { 6, "Torso" }, { 7, "Torso" }, { 8, "Torso" },
                { 9, "Left Arm" }, { 10, "Left Arm" }, { 11, "Left Arm" },
                { 12, "Right Arm" }, { 13, "Right Arm" }, { 14, "Right Arm" },
                { 15, "Left Leg" }, { 16, "Left Leg" }, { 17, "Left Leg" },
                { 18, "Right Leg" }, { 19, "Right Leg" }, { 20, "Right Leg" }
            }.ToFrozenDictionary();

        /// <summary>
        /// Looks up the body part struck by a given d20 result.
        /// </summary>
        /// <param name="roll">A d20 result in [1, 20].</param>
        /// <returns>The name of the body part struck.</returns>
        public static string GetHitLocation(int roll)
        {
            if (!Fallout2D20HitLocationDictionary.TryGetValue(roll, out string? location))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(roll), roll, $"A hit location roll must be in [1, {HitLocationDieSides}].");
            }

            return location;
        }

        /// <summary>
        /// Rolls the hit location die and returns the body part struck.
        /// </summary>
        public static string RollHitLocation()
        {
            return GetHitLocation(DiceUtility.RollDie(HitLocationDieSides));
        }
    }
}
