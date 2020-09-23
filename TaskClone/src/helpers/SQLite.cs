using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskClone.src.helpers
{
    public class LogTask {
        public string app { get; set; }
    }
    public class SQLite
    {
        private int ExecuteWrite(string query, Dictionary<string, object> args) {
            int numRowsAffected = 0;
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=C:\TaskClone\db.db"))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        foreach (var pair in args)
                        {
                            cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                        }
                        numRowsAffected = cmd.ExecuteNonQuery();
                    }
                }
                
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return numRowsAffected;

        }

        public int storeLog(string app) {
            const string query = "INSERT INTO logTask(app) VALUES(@app)";

            var args = new Dictionary<string, object> {
                {"@app", app}
            };
            return ExecuteWrite(query, args);
        }

        public List<string> getStoreLog() {
            List<string> apps = new List<string>();
            const string query = "SELECT * FROM logTask";
            using (SQLiteConnection connection = new SQLiteConnection(@"Data Source=C:\TaskClone\db.db")) {
                connection.Open();
                using (SQLiteCommand cmd = connection.CreateCommand()) {
                    cmd.CommandText = query;
                    cmd.CommandType = CommandType.Text;
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) {
                        apps.Add(Convert.ToString(reader["app"]));
                    }
                }
            }

            return apps;
        }
    }


}
