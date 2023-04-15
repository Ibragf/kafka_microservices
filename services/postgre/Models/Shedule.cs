using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace postgre.Models
{
    public class Schedule
    {
        [JsonProperty(propertyName:"id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonProperty(propertyName: "group_fk")]
        [JsonPropertyName("group_fk")]
        public string GroupFK { get; set; }

        [JsonProperty(propertyName: "lesson_fk")]
        [JsonPropertyName("lesson_fk")]
        public int LessonFK { get; set; }

        [JsonProperty(propertyName: "time")]
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
}
