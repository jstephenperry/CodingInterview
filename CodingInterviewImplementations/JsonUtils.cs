using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingInterviewImplementations
{
    public static class JsonUtils
    {
        public static bool IsValidJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            try
            {
                JToken.Parse(input);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        public static object? GetJsonValueByJsonPath(string jsonString, string jsonPath)
        {
            JObject json = JObject.Parse(jsonString);
            return json.SelectToken(jsonPath)?.ToObject<object>() ?? null;
        }
    }
}
