using Npgsql;
using postgre.Models;
using System.Data.Common;

namespace postgre.Services
{
    public class DataHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DataHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("postgres");
        }


        public List<Speciality> GetSpecialities()
        {
            var list = new List<Speciality>();  

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string sql = $"SELECT * FROM {SchemeCreator.TABLE_SPECIALITIES}";
            using var cmd = new NpgsqlCommand(sql, connection);
            var reader = cmd.ExecuteReader();
            
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    var speciality = new Speciality();
                    speciality.Id = reader.GetInt32(0);
                    speciality.Name= reader.GetString(1);
                    speciality.Department = reader.GetInt32(2);

                    list.Add(speciality);
                }
            }

            reader.Close();
            cmd.Dispose();

            return list;
        }

        public List<Course> GetCourses()
        {
            var list = new List<Course>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string sql = $"SELECT * FROM {SchemeCreator.TABLE_COURSES}";
            using var cmd = new NpgsqlCommand(sql, connection);
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var course = new Course();
                    course.Id = reader.GetInt32(0);
                    course.Name = reader.GetString(1);
                    course.Department = reader.GetInt32(2);
                    
                    list.Add(course);
                }
            }

            reader.Close();
            cmd.Dispose();

            return list;
        }
        public List<Lesson> GetLessons()
        {
            var list = new List<Lesson>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string sql = $"SELECT * FROM {SchemeCreator.TABLE_LESSONS}";
            using var cmd = new NpgsqlCommand(sql, connection);
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var lesson = new Lesson();  
                    lesson.Id = reader.GetInt32(0);
                    lesson.Type = reader.GetString(1);
                    lesson.CourseFK = reader.GetInt32(2);
                    lesson.Name = reader.GetString(3);

                    list.Add(lesson);
                }
            }

            reader.Close();
            cmd.Dispose();

            return list;
        }

        public List<Schedule> GetSchedule()
        {
            var list = new List<Schedule>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string sql = $"SELECT * FROM {SchemeCreator.TABLE_SCHEDULE}";
            using var cmd = new NpgsqlCommand(sql, connection);
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var sched = new Schedule();
                    sched.Id = reader.GetInt32(0);
                    sched.GroupFK = reader.GetString(1);
                    sched.LessonFK = reader.GetInt32(2);
                    sched.Time = reader.GetDateTime(3);

                    list.Add(sched);
                }
            }
            reader.Close();
            cmd.Dispose();

            return list;
        }
        public List<Student> GetStudents()
        {
            var list = new List<Student>();

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string sql = $"SELECT * FROM {SchemeCreator.TABLE_STUDENTS} LIMIT 15";
            using var cmd = new NpgsqlCommand(sql, connection);
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var student = new Student();
                    student.Id = reader.GetString(0);
                    student.Name = reader.GetString(1);
                    student.Surname = reader.GetString(2);
                    student.Group = reader.GetString(3);

                    list.Add(student);
                }
            }
            reader.Close();
            cmd.Dispose();

            return list;
        }
    }
}
