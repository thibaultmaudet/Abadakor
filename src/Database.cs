using Abadakor.Models;
using Abadakor.Settings;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abadakor
{
    public class Database
    {
        public static Settings.Settings Settings => Abadakor.Settings.Settings.Instance;

        private bool isOpen;

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

        public bool IsOpen { get => isOpen; }

        public void Open()
        {
            string connectionString = "SERVER=" + Settings.Database.Host + ";DATABASE=" + Settings.Database.Name + ";UID=" + Settings.Database.User + ";PASSWORD=" + Settings.Database.Password + ";";

            mySqlConnection = new MySqlConnection(connectionString);

            try
            {
                mySqlConnection.Open();

                isOpen = true;

                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "Connecté à la base de données");
            }
            catch (Exception ex)
            {
                isOpen = false;

                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + ex.Message);

                Console.WriteLine("Erreur : " + ex.Message);
            }
        }

        public List<User> GetUsers()
        {
            if (!isOpen) return default;

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                List<User> users = new List<User>();

                command.CommandText = "SELECT * FROM Users";

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    users.Add(new User() { FirstName = dataReader.GetString(dataReader.GetOrdinal("firstName")), Name = dataReader.GetString(dataReader.GetOrdinal("lastName")) });

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
            if (!isOpen) return default;

            User user = null;

            MySqlCommand command = mySqlConnection.CreateCommand();

            try
            {
                command.CommandText = "SELECT * FROM Users WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                    user = new User() { FirstName = dataReader.GetString(dataReader.GetOrdinal("firstName")), Name = dataReader.GetString(dataReader.GetOrdinal("lastName")) };

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
            if (!isOpen) return false;

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
    }
}
