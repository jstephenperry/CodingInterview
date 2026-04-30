using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingInterviewImplementations
{
    public static class JsonUtils
    {
        /// <summary>
        /// Returns <see langword="true"/> when <paramref name="input"/> is a syntactically valid JSON document
        /// (object, array, or any other JSON value).
        /// </summary>
        public static bool IsValidJson(string? input)
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

        /// <summary>
        /// Evaluates <paramref name="jsonPath"/> against <paramref name="jsonString"/> and returns the matched value
        /// as a CLR object (primitive, <see cref="JArray"/>, or <see cref="JObject"/>), or <see langword="null"/> when
        /// the path does not match.
        /// </summary>
        /// <exception cref="ArgumentNullException">Either argument is <see langword="null"/>.</exception>
        /// <exception cref="JsonReaderException"><paramref name="jsonString"/> is not valid JSON.</exception>
        public static object? GetJsonValueByJsonPath(string jsonString, string jsonPath)
        {
            ArgumentNullException.ThrowIfNull(jsonString);
            ArgumentNullException.ThrowIfNull(jsonPath);

            // JToken.Parse handles object, array, and primitive roots — JObject.Parse rejects non-object roots.
            JToken root = JToken.Parse(jsonString);
            return root.SelectToken(jsonPath)?.ToObject<object>();
        }
    }
}
