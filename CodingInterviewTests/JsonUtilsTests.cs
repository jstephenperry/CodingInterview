namespace CodingInterviewImplementations.Tests
{
    [TestFixture]
    [TestOf(typeof(JsonUtils))]
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

        [Test]
        public void IsValidJsonTest()
        {
            Assert.That(JsonUtils.IsValidJson(Json), Is.True);
        }

        [Test]
        public void GetJsonValueByJsonPathTest()
        {
            // Valid JSONPath expressions
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.firstName"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.firstName"), Is.EqualTo("John"));
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.lastName"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.lastName"), Is.EqualTo("Smith"));
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.address.streetAddress"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.address.streetAddress"), Is.EqualTo("21 2nd Street"));
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.address.city"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.address.city"), Is.EqualTo("New York"));
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.address.state"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.address.state"), Is.EqualTo("NY"));
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.address.postalCode"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.address.postalCode"), Is.EqualTo("10021-3100"));
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.phoneNumbers[0].number"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.phoneNumbers[0].number"), Is.EqualTo("212 555-1234"));
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.phoneNumbers[1].number"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.phoneNumbers[1].number"), Is.EqualTo("646 555-4567"));
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.children"), Is.Not.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.spouse"), Is.Null);

            // Invalid JSONPath expressions
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.middleName"), Is.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.preferredName"), Is.Null);
            Assert.That(JsonUtils.GetJsonValueByJsonPath(Json, "$.phoneNumbers[2]"), Is.Null);
        }
    }
}