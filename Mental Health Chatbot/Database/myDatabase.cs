using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mental_Health_Chatbot.Database
{
    public class myDatabase
    {

        // ATTENTION!!: Please change the file location if open with a new device, I don't know why
        private static string LoadConnectionString()
        {
            return "Data Source=E:\\document\\Ong Cong Kin\\Degree\\2024 - 04\\Progamming Elective II (2)\\assg1\\Mental Health Chatbot\\Mental Health Chatbot\\Database\\myDatabase.db;Version=3;New=false;Compress=True;";
        }

        // testing purpose
        public static void LoadMentalHealth()
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string loadTableQuery = "SELECT * FROM MENTAL_HEALTH;";

                using (SQLiteCommand command = new SQLiteCommand(loadTableQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(1);
                            string name = reader.GetString(0);
                            Console.WriteLine("Id = " + id + "\nname = " + name);
                        }
                    }
                }

                connection.Close();
            }
        }

        // question type, return -1 = no data, return 0 = one keyword has many type, please give more information
        // or using list, situation 1: no data, check another keyword database
        // situation 2: 1 data, correct question type
        // situation 3: 2 data, need more keywords
        // accumulate the frequency of each keyword and compare
        // if have two max, please give more information
        // the mental health type need to find a max
        // if no question type, just return default answer; else just like situation above
        // if have a max, no need to check again
        // return id for each keyword, then use the id to retrieve, another function
        // function 1: check question type keyword
        // function 2: check mental health type keyword
        // function 3: retrieve keyword

        public static List<int> GetQuestionType(string keyword)
        {
            List<int> result = new List<int>();

            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string loadTableQuery = "SELECT type FROM KEYWORDS_QUESTIONTYPE WHERE key = '" + keyword + "';";

                using (SQLiteCommand command = new SQLiteCommand(loadTableQuery, connection))
                {
                    //command.Parameters.AddWithValue("@keyword", keyword);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            result.Add(id);
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        public static List<int> GetMentalHealthType(string keyword)
        {
            List<int> result = new List<int>();

            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string loadTableQuery = "SELECT MH_ID FROM KEYWORDS_MH WHERE key = '" + keyword + "';";

                using (SQLiteCommand command = new SQLiteCommand(loadTableQuery, connection))
                {
                    //command.Parameters.AddWithValue("@keyword", keyword);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            result.Add(id);
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        public static string GetReply(int MH_ID, int QT_ID)
        {
            string result = "";

            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string QType = "";
                switch (QT_ID)
                {
                    case 1:
                        QType = "MH_DESCRIPTION";
                        break;

                    case 2:
                        QType = "MH_EFFECT";
                        break;

                    case 3:
                        QType = "MH_COPING";
                        break;

                    case 4:
                        QType = "MH_SELFCARE";
                        break; 

                    default:
                        QType = "MH_DEFAULT";
                        break;
                }

                string loadTableQuery = "SELECT " + QType + " FROM MENTAL_HEALTH WHERE MH_ID = " + MH_ID + ";";

                using (SQLiteCommand command = new SQLiteCommand(loadTableQuery, connection))
                {
                    //command.Parameters.AddWithValue("@QType", QType);
                    //command.Parameters.AddWithValue("@id", MH_ID);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        public static bool isLeaving(string keyword)
        {
            bool result = false;

            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string loadTableQuery = "SELECT * FROM KEYWORDS_LEAVE WHERE key = '" + keyword + "';";

                using(SQLiteCommand command = new SQLiteCommand(loadTableQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = true;
                        }
                    }
                }
                connection.Close();
            }

            return result;
        }

        public static bool isNoMeaning(string keyword)
        {
            bool result = false;

            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string loadTableQuery = "SELECT * FROM KEYWORDS_NOMEANING WHERE key = '" + keyword + "';";
                using (SQLiteCommand command = new SQLiteCommand(loadTableQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = true;
                        }
                    }
                }
                connection.Close();
            }
            return result;
        }
        
        public static void addUnknownKeyword(string keyword)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                connection.Open();

                string sql = "INSERT INTO KEYWORDS_UNKNOWN (key) VALUES ('" + keyword + "');";

                using(SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }



    }
}
