using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace CodingInterviewImplementations
{
    /// <summary>
    /// JSON parsing and querying helpers.
    /// </summary>
    public static class JsonUtils
    {
        /// <summary>
        /// Determines whether a string is a single, complete, well-formed JSON document.
        /// </summary>
        /// <param name="input">The candidate JSON text.</param>
        /// <returns>True if the text parses as JSON.</returns>
        /// <remarks>
        /// This was previously a bracket-balance check, which both accepted non-JSON such as
        /// "hello world" and rejected valid JSON whose strings contained braces. Parsing is the only
        /// way to answer the question the method name asks. <see cref="JsonDocument"/> is strict by
        /// default: comments, trailing commas, unquoted property names and trailing content are all
        /// rejected.
        /// </remarks>
        public static bool IsValidJson(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            try
            {
                using JsonDocument document = JsonDocument.Parse(input);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        /// <summary>
        /// Reads a single value out of a JSON document using a JSONPath expression.
        /// </summary>
        /// <param name="jsonString">The JSON document.</param>
        /// <param name="jsonPath">A JSONPath expression, for example "$.address.city".</param>
        /// <returns>The matched value, or null when the path matches nothing or matches JSON null.</returns>
        /// <exception cref="ArgumentException">The document or the path is null or blank.</exception>
        public static object? GetJsonValueByJsonPath(string jsonString, string jsonPath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(jsonString);
            ArgumentException.ThrowIfNullOrWhiteSpace(jsonPath);

            // JToken rather than JObject so documents with an array at the root are supported.
            JToken json = JToken.Parse(jsonString);
            return json.SelectToken(jsonPath)?.ToObject<object>();
        }
    }
}
