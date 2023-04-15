using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace postgre.Models
{
    public class Student
    {
        [JsonPropertyName("id")]
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty(propertyName: "name")]
        public string Name { get; set; }

        [JsonPropertyName("surname")]
        [JsonProperty(propertyName: "surname")]
        public string Surname { get; set; }

        [JsonPropertyName("group_fk")]
        [JsonProperty(propertyName: "group_fk")]
        public string Group { get; set; }
    }
}
