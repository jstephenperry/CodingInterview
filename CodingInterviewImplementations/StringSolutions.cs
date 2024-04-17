namespace CodingInterviewImplementations
{
    public static class StringSolutions
    {
        public static bool IsAnagramWithDictionaryFrequency(string input1, string input2)
        {
            if (string.IsNullOrEmpty(input1) || string.IsNullOrEmpty(input2))
            {
                return false;
            }

            if (input1.Length != input2.Length)
            {
                return false;
            }

            var charFrequency = new Dictionary<char, int>();

            foreach (char c in input1)
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

            foreach (char c in input2)
            {
                if (charFrequency.TryGetValue(c, out int value))
                {
                    if (value == 0)
                    {
                        return false;
                    }
                    charFrequency[c] = --value;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsAnagramWithSorting(string input1, string input2)
        {
            if (string.IsNullOrEmpty(input1) || string.IsNullOrEmpty(input2))
            {
                return false;
            }

            if (input1.Length != input2.Length)
            {
                return false;
            }

            var input1Array = input1.ToCharArray();
            var input2Array = input2.ToCharArray();

            Array.Sort(input1Array);
            Array.Sort(input2Array);

            return input1Array.SequenceEqual(input2Array);
        }

        public static bool IsAnagramWithLinqSorting(string input1, string input2)
        {
            if (string.IsNullOrEmpty(input1) || string.IsNullOrEmpty(input2))
            {
                return false;
            }

            if (input1.Length != input2.Length)
            {
                return false;
            }

            return input1.OrderBy(c => c).SequenceEqual(input2.OrderBy(c => c));
        }

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
