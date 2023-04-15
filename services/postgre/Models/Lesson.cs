using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace postgre.Models
{
    public class Lesson
    {
        [JsonPropertyName("id")]
        [JsonProperty(propertyName: "id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        [JsonProperty(propertyName: "type")]
        public string Type { get; set; }

        [JsonPropertyName("course_fk")]
        [JsonProperty(propertyName: "course_fk")]
        public int CourseFK { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty(propertyName: "name")]
        public string Name { get; set; }
    }
}
