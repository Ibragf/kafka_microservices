using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace mongo.Models
{
    public class Course
    {
        public int id { get; set; }
        public string name { get; set; }
        public int department_fk { get; set; }
    }

    public class Department
    {
        public int id { get; set; }
        public string name { get; set; }
        public int institute_fk { get; set; }   
        public List<Spec> specs { get; set; }
        public List<Course> courses { get; set; }
    }

    public class Institute
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Department> departments { get; set; }
    }

    public class Spec
    {
        public int id { get; set; }
        public string name { get; set; }
        public int department_fk { get; set; }
    }
}
