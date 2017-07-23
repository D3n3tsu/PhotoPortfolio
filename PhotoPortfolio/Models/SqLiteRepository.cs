using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PhotoPortfolio.Models
{
    public class SqLiteRepository : IRepository
    {

        public async Task CreatePhotographer(string name, DateTime birthDate)
        {
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO photographers(name, birth_date) "
                        + "VAlUES (@name, @birth_date)";
                    cmd.Parameters.Add("@name", SqliteType.Text);
                    cmd.Parameters["@name"].Value = name;
                    cmd.Parameters.Add("@birth_date", SqliteType.Integer);
                    cmd.Parameters["@birth_date"].Value = birthDate;

                    try
                    {
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(CreatePhotographer)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

        public async Task DeletePhotographer(int id)
        {
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM photographers "
                        + "WHERE id = @id";
                    cmd.Parameters.Add("@id", SqliteType.Integer);
                    cmd.Parameters["@id"].Value = id;

                    try
                    {
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch(SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(DeletePhotographer)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

        public async Task<IEnumerable<Photographer>> GetAllPhotographers()
        {
            List<Photographer> photographers = new List<Photographer>();
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name, birth_date FROM photographers";

                    try
                    {
                        await con.OpenAsync();
                        SqliteDataReader dr = await cmd.ExecuteReaderAsync();
                        while(await dr.ReadAsync())
                        {
                            photographers.Add(new Photographer {
                                Id = int.Parse(dr["id"].ToString()),
                                Name = dr["name"].ToString(),
                                BirthDate = DateTime.Parse(dr["birth_date"].ToString())
                            });
                        }
                        
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(GetAllPhotographers)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return photographers;
            }
        }

        public async Task<Photographer> GetPhotographer(int id)
        {
            Photographer photographer = new Photographer();
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name, birth_date FROM photographers "
                        + "WHERE id = @id";
                    cmd.Parameters.Add("@id", SqliteType.Integer);
                    cmd.Parameters["@id"].Value = id;

                    try
                    {
                        await con.OpenAsync();
                        SqliteDataReader dr = await cmd.ExecuteReaderAsync();
                        bool hasPhotographer = await dr.ReadAsync();
                        if (hasPhotographer)
                        {
                            photographer = new Photographer
                            {
                                Id = int.Parse(dr["id"].ToString()),
                                Name = dr["name"].ToString(),
                                BirthDate = DateTime.Parse(dr["birth_date"].ToString())
                            };
                        }
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(GetPhotographer)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return photographer;
            }
        }

        public async Task UpdatePhotographer(Photographer photographer)
        {
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE photographers SET name = @name, birth_date = @birth_date "
                        + "WHERE id = @id";
                    cmd.Parameters.AddWithValue("@name", photographer.Name);
                    cmd.Parameters.AddWithValue("@birth_date", photographer.BirthDate);
                    
                    try
                    {
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(UpdatePhotographer)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
    }
}
