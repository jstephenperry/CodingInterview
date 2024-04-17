namespace CodingInterviewImplementations
{
    public static class StringSolutions
    {
        public static Dictionary<char, int> FindMaxOccurringCharacterLinq(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input is null or empty.");
                return new Dictionary<char, int>();
            }

            return input.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
