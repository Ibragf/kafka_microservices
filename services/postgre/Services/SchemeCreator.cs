using Npgsql;
using System.Data.Common;

namespace postgre.Services
{
    public class SchemeCreator : IDisposable
    {
        public const string TABLE_INSTITUTES = "institutes";
        public const string TABLE_DEPARTMENTS = "departments";
        public const string TABLE_SPECIALITIES = "specialities";
        public const string TABLE_COURSES = "courses";
        public const string TABLE_STUDENTS = "students";
        public const string TABLE_GROUPS = "groups";
        public const string TABLE_VISITS = "visits";
        public const string TABLE_LESSONS = "lessons";
        public const string TABLE_SCHEDULE = "schedule";

        private readonly IConfiguration configuration;
        private readonly ILogger logger;
        private readonly DbConnectionProvider _connectionProvider;
        public SchemeCreator(IConfiguration configuration, ILogger<SchemeCreator> logger, DbConnectionProvider connectionProvider)
        {
            this.configuration = configuration;
            this.logger = logger;
            _connectionProvider = connectionProvider;
        }
        public void OpenConnection()
        {
            _connectionProvider.OpenConnection();
        }
        public void CreateScheme()
        {
            logger.LogInformation("CREATING database scheme");
            CreateInstitutes();
            CreateDepartaments();
            CreateSpecialities();
            CreateCourses();
            CreateGroups();
            CreateStudents();
            CreateLessons();
            CreateShedule();
            CreateVisits();

            _connectionProvider.Dispose();
            logger.LogInformation("CREATED database scheme");
        }
        private void CreateGroups()
        {
            _connectionProvider.Execute(@$"CREATE TABLE {TABLE_GROUPS}
                                        (
                                            id VARCHAR(12) PRIMARY KEY,
                                            speciality_fk INT NOT NULL,
                                            FOREIGN KEY (speciality_fk) REFERENCES {TABLE_SPECIALITIES} (id)
                                        );");
        }
        private void CreateStudents()
        {
            _connectionProvider.Execute(@$"CREATE TABLE {TABLE_STUDENTS}
                                        (
                                            id VARCHAR(8) PRIMARY KEY,
                                            name VARCHAR(20) NOT NULL,
                                            surname VARCHAR(20) NOT NULL, 
                                            group_fk VARCHAR(12) NOT NULL, 
                                            FOREIGN KEY (group_fk) REFERENCES {TABLE_GROUPS} (id)
                                        );");
        }
        private void CreateInstitutes()
        {
            _connectionProvider.Execute($@"CREATE TABLE {TABLE_INSTITUTES}
                                            (
                                                id SERIAL PRIMARY KEY,
                                                name VARCHAR(60) NOT NULL
                                            );");
        }
        private void CreateDepartaments()
        {
            _connectionProvider.Execute($@"CREATE TABLE {TABLE_DEPARTMENTS}
                                        (
                                            id SERIAL PRIMARY KEY,
                                            name VARCHAR(10) NOT NULL,
                                            institute_fk INT NOT NULL,
                                            FOREIGN KEY (institute_fk) REFERENCES {TABLE_INSTITUTES} (id)
                                        );");
        }
        private void CreateSpecialities()
        {
            _connectionProvider.Execute($@"CREATE TABLE {TABLE_SPECIALITIES}
                                        (
                                            id SERIAL PRIMARY KEY,
                                            name VARCHAR(8) NOT NULL,
                                            department_fk INT NOT NULL,
                                            FOREIGN KEY (department_fk) REFERENCES {TABLE_DEPARTMENTS} (id)
                                        );");
        }
        private void CreateCourses()
        {
            _connectionProvider.Execute($@"CREATE TABLE {TABLE_COURSES}
                                        (
                                            id SERIAL PRIMARY KEY,
                                            name VARCHAR(150) NOT NULL,
                                            department_fk INT NOT NULL,
                                            FOREIGN KEY (department_fk) REFERENCES {TABLE_DEPARTMENTS} (id)
                                        );");
        }
        private void CreateLessons()
        {
            _connectionProvider.Execute($@"CREATE TABLE {TABLE_LESSONS} (id SERIAL PRIMARY KEY, type VARCHAR(20) NOT NULL, course_fk INT NOT NULL, name VARCHAR(50) NOT NULL, FOREIGN KEY (course_fk) REFERENCES {TABLE_COURSES} (id));");
        }
        private void CreateShedule()
        {
            _connectionProvider.Execute($@"CREATE TABLE {TABLE_SCHEDULE}
                                        (
                                            id SERIAL, 
                                            group_fk VARCHAR(12) REFERENCES {TABLE_GROUPS} (id) NOT NULL, 
                                            lesson_fk INT REFERENCES {TABLE_LESSONS} (id) NOT NULL, 
                                            time TIMESTAMP NOT NULL 
                                        ); --PARTITION BY RANGE (time);");
        }
        private void CreateTablePartition(string partitionName, DateTime timeFrom, DateTime timeTo)
        {
            _connectionProvider.Execute($@"CREATE TABLE {partitionName} 
                                           PARTITION OF {TABLE_SCHEDULE} FOR VALUES FROM ('{timeFrom}') TO ('{timeTo}');");

        }
        private void CreateVisits()
        {
            _connectionProvider.Execute($@"CREATE TABLE {TABLE_VISITS}(id SERIAL PRIMARY KEY, visited boolean NOT NULL, student_fk VARCHAR(8) NOT NULL, schedule_fk INT NOT NULL, FOREIGN KEY (student_fk) REFERENCES {TABLE_STUDENTS} (id));");
        }
        public void Dispose()
        {
            _connectionProvider.Dispose();
        }
    }
}
