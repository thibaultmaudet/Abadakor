using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Abadakor.Settings
{
    public class Settings
    {
        private Database database;

        private string token;

        private static volatile Settings instance;
        private static readonly object SyncRoot = new object();

        public static Settings Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                lock (SyncRoot)
                    if (instance == null)
                        instance = new Settings();

                return instance;
            }
        }

        public Database Database { get => database; }

        public string Token { get => token; }

        public Settings()
        {
            database = new Database();

            string json = File.ReadAllText("settings.json");

            dynamic settings = JsonConvert.DeserializeObject(json);

            token = settings.Token;

            database.Host = settings.Database.Host;
            database.Name = settings.Database.Name;
            database.User = settings.Database.User;
            database.Password = settings.Database.Password;
        }
    }
}
