using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace postgre.Models
{
    public class Course
    {
        [JsonProperty(propertyName: "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonProperty(propertyName: "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonProperty(propertyName: "department_fk")]
        [JsonPropertyName("department_fk")]
        public int Department { get; set; }
    }
}
