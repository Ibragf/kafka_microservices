using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace postgre.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CourseInst
    {
        public string name { get; set; }
    }

    public class Department
    {
        public string name { get; set; }
        public List<Spec> specs { get; set; }
        public List<CourseInst> courses { get; set; }
    }

    public class Institute
    {
        public string name { get; set; }
        public List<Department> department { get; set; }
    }

    public class Spec
    {
        public string name { get; set; }
    }


}
