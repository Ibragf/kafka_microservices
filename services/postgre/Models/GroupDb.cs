using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace postgre.Models
{
    public class GroupDb
    {
        [JsonPropertyName("id")]
        [JsonProperty(propertyName: "id")]
        public string Id { get; set; }

        [JsonPropertyName("speciality_fk")]
        [JsonProperty(propertyName: "speciality_fk")]
        public string SpecialityFK { get; set; }
    }
}
