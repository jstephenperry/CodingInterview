using CodingInterviewImplementations.RandomUtilities;

namespace CodingInterviewImplementations
{
    /// <summary>
    /// Tabletop-style dice rolling on top of <see cref="SecureRandomHelper"/>.
    /// </summary>
    public static class DiceUtility
    {
        /// <summary>Rolls a single fair die with <paramref name="sides"/> sides and returns a value in <c>[1, sides]</c>.</summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="sides"/> &lt; 1.</exception>
        public static int RollDie(int sides)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(sides, 1);
            return SecureRandomHelper.GenerateRandomInt(1, sides);
        }

        /// <summary>
        /// Rolls <paramref name="numberOfDice"/> fair dice with <paramref name="sides"/> sides each and returns the sum.
        /// Returns 0 if <paramref name="numberOfDice"/> is 0.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="sides"/> &lt; 1 or <paramref name="numberOfDice"/> &lt; 0.</exception>
        public static int RollDice(int sides, int numberOfDice)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(sides, 1);
            ArgumentOutOfRangeException.ThrowIfNegative(numberOfDice);

            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                total += SecureRandomHelper.GenerateRandomInt(1, sides);
            }
            return total;
        }

        /// <summary>
        /// Rolls <paramref name="numberOfDice"/>d<paramref name="sides"/> and adds <paramref name="modifier"/>
        /// (the standard tabletop "<c>NdS+M</c>" notation).
        /// </summary>
        public static int RollDice(int sides, int numberOfDice, int modifier) =>
            RollDice(sides, numberOfDice) + modifier;
    }
}
