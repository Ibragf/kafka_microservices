using Newtonsoft.Json;
using postgre.Models;
using System.Text;

namespace postgre.Services
{
    public class DataFiller
    {
        public readonly DateTime START = new DateTime(2022,2,4);
        public readonly TimeSpan DELTA_WEEK = new TimeSpan(7, 0, 0, 0);
        public readonly TimeSpan DELTA_DAY = new TimeSpan(1, 0, 0, 0);
        public const int ALL_WEEKS = 32;

        private readonly List<Student> _students = new List<Student>(); 
        private readonly List<string> _groups = new List<string> { "БСБО-01-19", "БСБО-02-19", "БСБО-03-19", "БИСО-01-20", "БИСО-03-20", "БИСО-02-20", "МГЕР-01-19", "МГЕР-02-19", "МГЕР-03-19", "БИСО-01-19", "БИСО-06-20" };
        private readonly List<string> _lessons = new List<string> { "Программирование", "Базы данных", "Мат. анализ", "Философия", "Безопасность ИС", "Методы системной инженерии", "Криптография" };

        private readonly DbConnectionProvider _connectionProvider;
        private readonly DataHelper _dataHelper;
        private readonly ILogger<DataFiller> _logger;

        private List<Speciality> specialities;
        private List<Course> courses;
        public DataFiller(DbConnectionProvider connectionProvider, DataHelper dataHelper, ILogger<DataFiller> logger)
        {
            _connectionProvider = connectionProvider;
            _dataHelper = dataHelper;
            _logger = logger;
            connectionProvider.OpenConnection();
        }

        public void FillScheme()
        {
            _logger.LogInformation("START filling database");
            GenerateStudents();
            var institutes = LoadInstitutes();

            var instNumber = 1;
            foreach (var institute in institutes)
            {
                InsertInstitute(institute.name);
                var depNumber = 1;
                foreach (var departmen in institute.department)
                {
                    InsertDepartment(departmen.name, instNumber);
                    foreach (var speciality in departmen.specs)
                    {
                        InsertSpeciality(speciality.name, depNumber);
                    }
                    foreach (var course in departmen.courses)
                    {
                        InsertCourse(course.name, depNumber);
                    }
                    depNumber++;
                }

                instNumber++;
            }

            _logger.LogInformation("FILLED institutes, deps, specs, courses");
            specialities = _dataHelper.GetSpecialities();
            courses = _dataHelper.GetCourses();

            Random r = new Random();

            foreach (var group in _groups)
            {
                InsertGroup(group, specialities[r.Next(0, specialities.Count-1)].Id);
            }
            foreach(var student in _students)
            {
                InsertStudent(student.Id, student.Name, student.Surname, student.Group);
            }
            foreach (var lesson in _lessons)
            {
                InsertLesson(lesson, ShAdd(0.5) ? "Лекция" : "Практика", courses[r.Next(0, courses.Count-1)].Id);
            }

            var lessons = _dataHelper.GetLessons();
            foreach (var group in _groups)
            {
                if (!ShAdd(0.3)) continue;
                FillShedule(group, lessons);
            }

            var shedule = _dataHelper.GetSchedule();
            foreach(var shed in shedule)
            {
                FillVisits(shed);
            }

            _logger.LogInformation("FILLED database");
        }


        private string GenerateStudentId()
        {
            string letters = "АБВГДЕЖЗИКЛМНОПРСТУФХЦЧШЩЫЭЮЯ";
            string numbers = "1234567890";

            Random r = new Random();
            string let = String.Empty;
            string num = String.Empty;
            for (int i = 0; i < 2; i++)
            {
                let += letters[r.Next(0, letters.Length-1)];
            }
            for (int i = 0; i < 4; i++)
            {
                num += numbers[r.Next(0, numbers.Length-1)];
            }

            return "19" + let + num;
        }

        private void GenerateStudents()
        {
            var names = new List<string>();
            var surnames = new List<string>();
            using (var reader = new StreamReader("./data/names.txt"))
            {
                string name = reader.ReadLine()!;
                while (name != null)
                {
                    names.Add(name);

                    name = reader.ReadLine()!;
                }
            }
            using (var reader = new StreamReader("data/surnames.txt"))
            {
                string surname = reader.ReadLine()!;
                while (surname != null)
                {
                    surnames.Add(surname);

                    surname = reader.ReadLine()!;
                }
            }

            Random r = new Random();
            foreach (var group in _groups)
            {
                for (int i = 0; i < r.Next(20, 31); i++)
                {
                    var id = GenerateStudentId();
                    var student = new Student
                    {
                        Id = id,
                        Name = names[r.Next(0, names.Count-1)],
                        Surname = surnames[r.Next(0, surnames.Count-1)],
                        Group = group,
                    };

                    _students.Add(student);
                }
            }
        }

        private bool ShGroup()
        {
            Random r = new Random();
            return r.NextDouble() < 0.8;
        }

        private bool ShAdd(double value)
        {
            Random r = new Random();
            return r.NextDouble() < value;
        }

        private void InsertGroup(string group, int spec) => _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_GROUPS}(id, speciality_fk) VALUES ('{group}', '{spec}');");
        private void InsertInstitute(string name) => _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_INSTITUTES}(name) VALUES ('{name}');");
        private void InsertDepartment(string name, int institute) => _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_DEPARTMENTS}(name, institute_fk) VALUES ('{name}', {institute});");
        private void InsertSpeciality(string name, int department) => _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_SPECIALITIES}(name, department_fk) VALUES ('{name}', {department});");
        private void InsertCourse(string name, int department) => _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_COURSES}(name, department_fk) VALUES ('{name}', {department});");
        private void InsertStudent(string studentId, string studentName, string studentSurname, string group) =>
            _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_STUDENTS}(id, name, surname, group_fk) VALUES ('{studentId}', '{studentName}', '{studentSurname}', '{group}');");
        private void InsertLesson(string name, string type, int courseId) => _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_LESSONS}(name, type, course_fk) VALUES ('{name}', '{type}', '{courseId}');");
        private void InsertSchedule(string group, int lesson, DateTime datetime) => _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_SCHEDULE}(group_fk, lesson_fk, time) VALUES('{group}', {lesson}, '{datetime}');");
        private void InsertVisit(int schedule_fk, string student, bool visited) => _connectionProvider.Execute($"INSERT INTO {SchemeCreator.TABLE_VISITS}(schedule_fk, student_fk, visited) VALUES('{schedule_fk}', '{student}', {visited});");

        private void FillDay(DateTime day, string group, List<Lesson> lessons)
        {
            var lessonTimes = new List<TimeSpan>()
            {
                new TimeSpan(9, 0, 0),
                new TimeSpan(10, 30, 0),
                new TimeSpan(12, 30, 0),
                new TimeSpan(14, 20, 0),
                new TimeSpan(16, 20, 0),
                new TimeSpan(18, 0, 0)
            };

            Random r = new Random();
            foreach (var lessonTime in lessonTimes)
            {
                if (!ShAdd(0.37)) continue;

                var lesson = lessons[r.Next(0, lessons.Count)];
                InsertSchedule(group, lesson.Id, day.Add(lessonTime));
            }
        }

        private void FillWeek(DateTime week, string group, List<Lesson> lessons)
        {
            var current = week;

            for (int i = 0; i < 6; i++)
            {
                FillDay(current, group, lessons);
                current = current + DELTA_DAY;
            }
        }

        private void FillShedule(string group, List<Lesson> lessons)
        {
            var current = START;

            for (int i = 0; i < ALL_WEEKS; i++)
            {
                var selectionLessons = new List<Lesson>();
                Random r = new Random();
                for (int j = 0; j < 5; j++)
                {
                    selectionLessons.Add(lessons[r.Next(0, lessons.Count)]);
                }

                FillWeek(current, group, selectionLessons);

                current = current + DELTA_WEEK;
            }
        }

        private void FillVisits(Schedule schedule)
        {
            var filterStudents = _students.Where(student => student.Group == schedule.GroupFK);
            foreach (var student in filterStudents)
            {
                InsertVisit(schedule.Id, student.Id, ShAdd(0.8));
            }
        }

        private string GetJsonString(string path)
        {
            StringBuilder sb = new StringBuilder();
            using (var reader = new StreamReader(path))
            {
                string line;
                while(!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    sb.Append(line);
                }
            }

            return sb.ToString();
        }
        public List<Institute> LoadInstitutes()
        {
            var listData = new List<string>() { "./data/institute_1.json", "./data/institute_2.json", "./data/institute_3.json" };
            List<Institute> institutes = new List<Institute>();

            foreach (var path in listData)
            {
                var jsonString = GetJsonString(path);
                var institute = JsonConvert.DeserializeObject<Institute>(jsonString);
                institutes.Add(institute!);
            }

            return institutes;
        }
    }
}
