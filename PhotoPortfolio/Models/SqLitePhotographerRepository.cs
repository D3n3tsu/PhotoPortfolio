using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoPortfolio.Models.ViewModels;

namespace PhotoPortfolio.Models
{
    public class SqLitePhotographerRepository : IPhotographerRepository
    {
        private IEnumerable<Photographer> photographers;

        public SqLitePhotographerRepository()
        {
            photographers = GetAllPhotographersFromDB();
        }

        public IEnumerable<Photographer> GetAllPhotographers()
            => photographers;

        public async Task<Photographer> CreatePhotographer(string name, DateTime birthDate)
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
                        throw new Exception($"Problem in {nameof(CreatePhotographer)} method" +
                            $"while saving photographer", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }

                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name, birth_date FROM photographers " +
                        "WHERE name = @name";
                    cmd.Parameters.AddWithValue("@name", name);
                    Photographer savedPhotographer = null;

                    try
                    {
                        await con.OpenAsync();
                        SqliteDataReader dr = await cmd.ExecuteReaderAsync();
                        bool hasSaved = await dr.ReadAsync();
                        if(hasSaved){
                            int savedId = int.Parse(dr["id"].ToString());
                            string savedName = dr["name"].ToString();
                            DateTime savedBirthDate = DateTime.Parse(dr["birth_date"].ToString());
                            savedPhotographer = new Photographer
                            {
                                Id = savedId,
                                Name = savedName,
                                BirthDate = savedBirthDate
                            };
                        };
                    }
                    catch(SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(CreatePhotographer)} method" +
                            $"while getting saved photographer", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                    return savedPhotographer;
                }
            }
        }

        public async Task<bool> DeletePhotographer(int id)
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
                    int deletedRows = 0;

                    try
                    {
                        await con.OpenAsync();
                        deletedRows = await cmd.ExecuteNonQueryAsync();
                    }
                    catch(SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(DeletePhotographer)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }

                    if (deletedRows > 0)
                        return true;
                    return false;
                }
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

        public async Task<Photographer> UpdatePhotographer(Photographer photographer)
        {
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE photographers SET name = @name, birth_date = @birth_date "
                        + "WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", photographer.Id);
                    cmd.Parameters.AddWithValue("@name", photographer.Name);
                    cmd.Parameters.AddWithValue("@birth_date", photographer.BirthDate);
                    
                    try
                    {
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(UpdatePhotographer)} method" +
                            $"while updating photographer", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }

                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM photographers WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", photographer.Id);
                    Photographer updatedPhotographer = null;

                    try
                    {
                        await con.OpenAsync();
                        var reader = await cmd.ExecuteReaderAsync();
                        await reader.ReadAsync();
                        updatedPhotographer = new Photographer
                        {
                            Id = int.Parse((string)reader["id"]),
                            Name = (string)reader["name"],
                            BirthDate = DateTime.Parse((string)reader["birth_date"])
                        };
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(UpdatePhotographer)} method" +
                            $"while getting updated photographer", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                    return updatedPhotographer;
                }
            }
        }

        private IEnumerable<Photographer> GetAllPhotographersFromDB()
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
                        con.Open();
                        SqliteDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            photographers.Add(new Photographer
                            {
                                Id = int.Parse(dr["id"].ToString()),
                                Name = dr["name"].ToString(),
                                BirthDate = DateTime.Parse(dr["birth_date"].ToString())
                            });
                        }

                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(GetAllPhotographersFromDB)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return photographers;
            }
        }
    }
}
