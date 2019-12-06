using Abadakor.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Abadakor
{
    public class Database
    {
        public static Settings.Settings Settings => Abadakor.Settings.Settings.Instance;
        
        private MySqlConnection mySqlConnection;

        private static volatile Database instance;
        private static readonly object SyncRoot = new object();

        public static Database Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                lock (SyncRoot)
                    if (instance == null)
                        instance = new Database();

                return instance;
            }
        }
        
        public void Open()
        {
            string connectionString = "SERVER=" + Settings.Database.Host + ";DATABASE=" + Settings.Database.Name + ";UID=" + Settings.Database.User + ";PASSWORD=" + Settings.Database.Password + ";";

            mySqlConnection = new MySqlConnection(connectionString);

            try
            {
                mySqlConnection.Open();
                
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Connecté à la base de données");
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                Console.WriteLine("Erreur : " + ex.Message);
            }
        }

        public List<User> GetUsers()
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                List<User> users = new List<User>();

                command.CommandText = "SELECT * FROM Users";

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    users.Add(new User() { Id = dataReader.GetString(dataReader.GetOrdinal("id")), FirstName = dataReader.GetString(dataReader.GetOrdinal("firstName")), Name = dataReader.GetString(dataReader.GetOrdinal("lastName")) });

                dataReader.Close();

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                return default;
            }
        }

        public List<User> GetUsers(string firstName, string lastName)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                List<User> users = new List<User>();

                command.CommandText = "SELECT * FROM Users WHERE firstName LIKE @firstName AND lastName LIKE @lastName";
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    users.Add(new User() { Id = dataReader.GetString(dataReader.GetOrdinal("id")), FirstName = dataReader.GetString(dataReader.GetOrdinal("firstName")), Name = dataReader.GetString(dataReader.GetOrdinal("lastName")) });

                dataReader.Close();

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                return default;
            }
        }

        public User GetUser(string id)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            User user = null;

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                command.CommandText = "SELECT * FROM Users WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    user = new User() { Id = dataReader.GetString(dataReader.GetOrdinal("id")), FirstName = dataReader.GetString(dataReader.GetOrdinal("firstName")), Name = dataReader.GetString(dataReader.GetOrdinal("lastName")) };

                dataReader.Close();

                return user;
            }
            catch(Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);
                return default;
            }
        }

        public bool AddUser(string id, string firstName, string lastName)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                command.CommandText = "INSERT INTO Users (id, firstName, lastName, timestamp) VALUES (@id, @firstName, @lastName, @timestamp)";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@timestamp", DateTime.Now);

                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                return false;
            }
        }

        public List<Course> GetCourses()
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                List<Course> courses = new List<Course>();

                command.CommandText = "SELECT * FROM Courses";

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    courses.Add(new Course() { Id = dataReader.GetInt32(dataReader.GetOrdinal("id")), Caption = dataReader.GetString(dataReader.GetOrdinal("caption")) });

                dataReader.Close();

                return courses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                return default;
            }
        }

        public List<Course> GetCourses(string userId)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                List<Course> courses = new List<Course>();

                command.CommandText = "SELECT Courses.caption AS Caption, Courses.id AS CourseID, assoc_coursesusers.state FROM assoc_coursesusers, Courses WHERE assoc_coursesusers.id_course = Courses.id AND assoc_coursesusers.id_user = @userId";
                command.Parameters.AddWithValue("@userId", userId);

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    courses.Add(new Course() { Id = dataReader.GetInt32(dataReader.GetOrdinal("CourseID")), Caption = dataReader.GetString(dataReader.GetOrdinal("Caption")), State = dataReader.GetInt32(dataReader.GetOrdinal("state")) });

                dataReader.Close();

                return courses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                return default;
            }
        }

        public Course GetCourse(string caption)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            Course course = null;

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                command.CommandText = "SELECT * FROM Courses WHERE caption = @caption";
                command.Parameters.AddWithValue("@caption", caption);

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    course = new Course() { Id = dataReader.GetInt32(dataReader.GetOrdinal("id")), Caption = dataReader.GetString(dataReader.GetOrdinal("caption")) };

                dataReader.Close();

                return course;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);
                return default;
            }
        }

        public Course GetCourse(int id)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            Course course = null;

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                command.CommandText = "SELECT * FROM Courses WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    course = new Course() { Id = dataReader.GetInt32(dataReader.GetOrdinal("id")), Caption = dataReader.GetString(dataReader.GetOrdinal("caption")) };

                dataReader.Close();

                return course;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);
                return default;
            }
        }

        public bool AddCourse(string caption)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                command.CommandText = "INSERT INTO Courses (caption) VALUES (@caption)";
                command.Parameters.AddWithValue("@caption", caption);

                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                return false;
            }
        }

        public bool CreateAssociation(string userId, int courseId)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                command.CommandText = "INSERT INTO assoc_coursesusers (id_user,id_course,state) VALUES (@userId, @courseId, 0);";
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@courseId", courseId);

                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                return false;
            }
        }

        public bool UpdateState(string userId, int courseId, int state)
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open) Open();

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                command.CommandText = "UPDATE assoc_coursesusers SET state = @state WHERE assoc_coursesusers.id_user = @userId AND assoc_coursesusers.id_course = @courseId";
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@courseId", courseId);
                command.Parameters.AddWithValue("@state", state);

                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                return false;
            }
        }
    }
}
