using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingInterviewImplementations
{
    public static class JsonUtils
    {
        public static bool IsValidJson(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            var stack = new Stack<char>();

            foreach (char c in input)
            {
                if (c == '{' || c == '[' || c == '(')
                {
                    stack.Push(c);
                }
                else if (c == '}' || c == ']' || c == ')')
                {
                    if (stack.Count == 0)
                    {
                        return false;
                    }

                    char openingBracket = stack.Pop();

                    if (c == '}' && openingBracket != '{')
                    {
                        return false;
                    }

                    if (c == ']' && openingBracket != '[')
                    {
                        return false;
                    }

                    if (c == ')' && openingBracket != '(')
                    {
                        return false;
                    }
                }
            }

            return stack.Count == 0;
        }

        public static object? GetJsonValueByJsonPath(string jsonString, string jsonPath)
        {
            JObject json = JObject.Parse(jsonString);
            return json.SelectToken(jsonPath)?.ToObject<object>() ?? null;
        }
    }
}
