namespace CodingInterviewImplementations
{
    public static class StringSolutions
    {
        public static KeyValuePair<char, int> FindMaxOccurringCharacterLinq(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default;
            }

            var charFrequency = new Dictionary<char, int>();

            foreach (char c in input)
            {
                if (charFrequency.TryGetValue(c, out int value))
                {
                    charFrequency[c] = ++value;
                }
                else
                {
                    charFrequency[c] = 1;
                }
            }
            return charFrequency.OrderByDescending(pair => pair.Value).First();
        }
    }
}
