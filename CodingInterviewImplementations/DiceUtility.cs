using CodingInterviewImplementations.RandomUtilities;

namespace CodingInterviewImplementations
{
    /// <summary>
    /// Tabletop-style dice rolling backed by a cryptographically secure source.
    /// </summary>
    public static class DiceUtility
    {
        /// <summary>
        /// Rolls a single die.
        /// </summary>
        /// <param name="sides">The number of faces on the die. Must be at least one.</param>
        /// <returns>A value in [1, <paramref name="sides"/>], inclusive of both ends.</returns>
        public static int RollDie(int sides)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(sides, 1);

            // SecureRandomHelper's upper bound is exclusive, so a d6 needs an exclusive bound of 7.
            return SecureRandomHelper.GenerateRandomInt(1, sides + 1);
        }

        /// <summary>
        /// Rolls a number of identical dice and sums the results.
        /// </summary>
        /// <param name="sides">The number of faces on each die. Must be at least one.</param>
        /// <param name="numberOfDice">How many dice to roll. Must not be negative.</param>
        /// <returns>The sum of the rolls, or zero when <paramref name="numberOfDice"/> is zero.</returns>
        public static int RollDice(int sides, int numberOfDice)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(numberOfDice);

            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                total += RollDie(sides);
            }
            return total;
        }

        /// <summary>
        /// Rolls a number of identical dice, sums the results and applies a flat modifier.
        /// </summary>
        public static int RollDice(int sides, int numberOfDice, int modifier)
        {
            return RollDice(sides, numberOfDice) + modifier;
        }
    }
}
