using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Giovanni.Services.Database
{
    public class DBConnection
    {
        private DBConnection() { }

        public string Server { get; set; } = "localhost";
        public string DatabaseName { get; set; } = "bot";
        public string UserName { get; set; } = "root";
        public string Password { get; set; } = "root";

        private MySqlConnection Connection { get; set; }

        private static DBConnection _instance = null;

        public static DBConnection Instance()
        {
            return _instance ??= new DBConnection();
        }

        public MySqlConnection Open()
        {
            Connection ??= new MySqlConnection(
                $"Server={Server}; database={DatabaseName}; UID={UserName}; password={Password}");

            Connection.Open();

            return Connection;
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}