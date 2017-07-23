using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoPortfolio.Infrastructure
{
    public class DatabaseCreation
    {
        public void EnsureTableCreation()
        {
            using(SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using(SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "CREATE table IF NOT EXISTS photographers "
                        + "(id INTEGER PRIMARY KEY NOT NULL, "
                        + "name TEXT NOT NULL, "
                        + "birth_date TEXT NOT NULL)";
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch(SqliteException ex)
                    {
                        throw new Exception("Problem while table creation " + ex);
                    }
                }
            }
        }
    }
}
