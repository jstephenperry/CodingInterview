namespace CodingInterviewImplementations.Tests
{
    public class JsonUtilsTests
    {
        private const string Json = @"{
            ""firstName"": ""John"",
            ""lastName"": ""Smith"",
            ""isAlive"": true,
            ""age"": 27,
            ""address"": {
                ""streetAddress"": ""21 2nd Street"",
                ""city"": ""New York"",
                ""state"": ""NY"",
                ""postalCode"": ""10021-3100""
            },
            ""phoneNumbers"": [
                {
                    ""type"": ""home"",
                    ""number"": ""212 555-1234""
                },
                {
                    ""type"": ""office"",
                    ""number"": ""646 555-4567""
                }
            ],
            ""children"": [],
            ""spouse"": null
        }";

        public static TheoryData<string> ValidJson =>
        [
            "{}",                       // empty object
            "[]",                       // empty array
            "[1, 2, 3]",                // array at the root
            "123",                      // bare number
            "true",                     // bare boolean
            "null",                     // bare null
            @"""a string""",            // bare string
            @"{""a"": ""}""}",          // brace inside a string literal
            @"{""a"": ""[[[""}",        // brackets inside a string literal
            @"{""a"": ""a \"" quote""}" // escaped quote inside a string literal
        ];

        public static TheoryData<string?> InvalidJson =>
        [
            // The cast disambiguates the collection-expression element: a bare null could bind to
            // either TheoryData.Add(string?) or TheoryData.Add(TheoryDataRow<string?>).
            (string?)null,              // null input
            "",                         // empty input
            "   ",                      // whitespace only
            "hello world",              // plain prose
            "{,,,}",                    // balanced braces but not JSON
            "()",                       // parentheses are not JSON
            @"{""a"": 1",               // truncated object
            @"{""a"": 1,}",             // trailing comma
            "{a: 1}",                   // unquoted property name
            "{} {}",                    // trailing content after the document
            @"{""a"": 1} garbage",      // trailing garbage
            @"{""a"": ""unterminated}"  // unterminated string
        ];

        public static TheoryData<string?, string?> BlankArguments => new()
        {
            { null, "$.a" },
            { "", "$.a" },
            { "   ", "$.a" },
            { @"{""a"": 1}", null },
            { @"{""a"": 1}", "" }
        };

        [Fact]
        public void IsValidJson_AcceptsAWellFormedDocument()
        {
            Assert.True(JsonUtils.IsValidJson(Json));
        }

        [Theory]
        [MemberData(nameof(ValidJson))]
        public void IsValidJson_AcceptsValidJson(string input)
        {
            // The brace-inside-a-string cases are the ones the old bracket-counting version rejected.
            Assert.True(JsonUtils.IsValidJson(input), $"expected valid JSON: {input}");
        }

        [Theory]
        [MemberData(nameof(InvalidJson))]
        public void IsValidJson_RejectsInvalidJson(string? input)
        {
            // "hello world" and "{,,,}" both passed the old bracket-balance implementation.
            Assert.False(JsonUtils.IsValidJson(input), $"expected invalid JSON: {input}");
        }

        [Fact]
        public void GetJsonValueByJsonPath_ReadsValuesAtEveryDepth()
        {
            Assert.Multiple(
                () => Assert.Equal("John", JsonUtils.GetJsonValueByJsonPath(Json, "$.firstName")),
                () => Assert.Equal("Smith", JsonUtils.GetJsonValueByJsonPath(Json, "$.lastName")),
                () => Assert.Equal("21 2nd Street", JsonUtils.GetJsonValueByJsonPath(Json, "$.address.streetAddress")),
                () => Assert.Equal("New York", JsonUtils.GetJsonValueByJsonPath(Json, "$.address.city")),
                () => Assert.Equal("NY", JsonUtils.GetJsonValueByJsonPath(Json, "$.address.state")),
                () => Assert.Equal("10021-3100", JsonUtils.GetJsonValueByJsonPath(Json, "$.address.postalCode")),
                () => Assert.Equal("212 555-1234", JsonUtils.GetJsonValueByJsonPath(Json, "$.phoneNumbers[0].number")),
                () => Assert.Equal("646 555-4567", JsonUtils.GetJsonValueByJsonPath(Json, "$.phoneNumbers[1].number")),
                () => Assert.NotNull(JsonUtils.GetJsonValueByJsonPath(Json, "$.children")));
        }

        [Fact]
        public void GetJsonValueByJsonPath_ReturnsNullForMissingPathsAndJsonNull()
        {
            Assert.Multiple(
                () => Assert.Null(JsonUtils.GetJsonValueByJsonPath(Json, "$.spouse")),
                () => Assert.Null(JsonUtils.GetJsonValueByJsonPath(Json, "$.middleName")),
                () => Assert.Null(JsonUtils.GetJsonValueByJsonPath(Json, "$.preferredName")),
                () => Assert.Null(JsonUtils.GetJsonValueByJsonPath(Json, "$.phoneNumbers[2]")));
        }

        [Fact]
        public void GetJsonValueByJsonPath_SupportsAnArrayAtTheRoot()
        {
            // JObject.Parse could not handle this; JToken.Parse can.
            Assert.Equal(7L, JsonUtils.GetJsonValueByJsonPath(@"[{""id"": 7}]", "$[0].id"));
        }

        [Theory]
        [MemberData(nameof(BlankArguments))]
        public void GetJsonValueByJsonPath_RejectsBlankArguments(string? json, string? path)
        {
            Assert.ThrowsAny<ArgumentException>(() => JsonUtils.GetJsonValueByJsonPath(json!, path!));
        }
    }
}
