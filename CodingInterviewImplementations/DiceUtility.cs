namespace CodingInterviewImplementations
{
    public static class DiceUtility
    {
        public static int RollDie(int sides)
        {
            return RandomUtilities.SecureRandomHelper.GenerateRandomInt(1, sides);
        }

        public static int RollDice(int sides, int numberOfDice)
        {
            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                total += RollDie(sides);
            }
            return total;
        }

        public static int RollDice(int sides, int numberOfDice, int modifier)
        {
            return RollDice(sides, numberOfDice) + modifier;
        }
    }
}
