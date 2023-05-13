using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mental_Health_Chatbot.Database
{
    public class adminDatabase
    {
        // ATTENTION!!: Please change the file location if open with a new device, I don't know why
        private static string LoadConnectionString()
        {
            return "Data Source=E:\\document\\Ong Cong Kin\\Degree\\2024 - 04\\Progamming Elective II (2)\\assg1\\Mental Health Chatbot\\Mental Health Chatbot\\Database\\myDatabase.db;Version=3;New=false;Compress=True;";
        }

        public static List<string> getUnknownKeywords()
        {
            List<string> result = new List<string>();

            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string loadTableQuery = "SELECT * FROM KEYWORDS_UNKNOWN;";

                using (SQLiteCommand command = new SQLiteCommand(loadTableQuery, connection))
                {
                    //command.Parameters.AddWithValue("@keyword", keyword);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string keyword = reader.GetString(0);
                            result.Add(keyword);
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        public static void addNonRelatedKeyword(string keyword, int type)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string tableName = "";

                switch (type)
                {
                    case 1:
                        tableName = "KEYWORDS_LEAVE";
                        break;

                    default:
                        tableName = "KEYWORDS_NOMEANING";
                        break;
                }

                string addSQL = "INSERT INTO " + tableName + " (key) VALUES ('" + keyword + "');";

                using (SQLiteCommand command = new SQLiteCommand(addSQL, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine("Insert into " + tableName);
                    Console.WriteLine("Rows affected = " + rowsAffected);
                }

                string dropSQL = "DELETE FROM KEYWORDS_UNKNOWN WHERE key = '" + keyword + "');";
                using (SQLiteCommand command = new SQLiteCommand(addSQL, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine("Drop from KEYWORDS_UNKNOWN");
                    Console.WriteLine("Rows affected = " + rowsAffected);
                }

                connection.Close();
            }
        }

        public static void addRelatedKeyword (string keyword, int table, int type)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string tableName = "";
                string columnName = "";

                switch (table)
                {
                    case 1:
                        tableName = "KEYWORDS_QUESTIONTYPE";
                        columnName = "type";
                        break;

                    case 2:
                        tableName = "KEYWORDS_MH";
                        columnName = "MH_ID";
                        break;
                }

                if(!string.IsNullOrEmpty(tableName))
                {
                    string addSQL = "INSERT INTO " + tableName + " (key, " + columnName + ") VALUES ('" + keyword + "', " + type + ");";
                    using (SQLiteCommand command = new SQLiteCommand(addSQL, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine("Insert into " + tableName);
                        Console.WriteLine("Rows affected = " + rowsAffected);
                    }

                    string dropSQL = "DELETE FROM KEYWORDS_UNKNOWN WHERE key = '" + keyword + "');";
                    using (SQLiteCommand command = new SQLiteCommand(addSQL, connection))
                    {
                        int rowsAffected = command.ExecuteNonQuery();

                        Console.WriteLine("Drop from KEYWORDS_UNKNOWN");
                        Console.WriteLine("Rows affected = " + rowsAffected);
                    }
                }

                connection.Close();
            }
        }
    }
}
