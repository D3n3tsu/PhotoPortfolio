using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotoPortfolio.Models.ViewModels;
using Microsoft.Data.Sqlite;
using System.IO;
using SkiaSharp;

namespace PhotoPortfolio.Models
{
    public class SqLitePhotoRepository : IPhotoRepository
    {
        private List<Thumbnail> thumbnails;

        public SqLitePhotoRepository()
        {
            thumbnails = GetAllThumbnailsFromDB().ToList();
        }

        public IEnumerable<Thumbnail> GetAllThumbnails(int photographerId) => 
            thumbnails.Where(t => t.PhotographerId == photographerId);


        public async Task<bool> DeletePhoto(int photoId)
        {
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM photos "
                        + "WHERE id = @id";
                    cmd.Parameters.Add("@id", SqliteType.Integer);
                    cmd.Parameters["@id"].Value = photoId;
                    int deletedRows = 0;

                    try
                    {
                        await con.OpenAsync();
                        deletedRows = await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(DeletePhoto)} method", ex);
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

        public async Task<Photo> DownloadPhoto(int id)
        {
            Photo photo = new Photo();
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, photo, photographer_id FROM photos "
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
                            photo = new Photo
                            {
                                Id = int.Parse(dr["id"].ToString()),
                                Body = (byte[])dr["photo"],
                                PhotographerId = int.Parse(dr["photographer_id"].ToString())
                            };
                        }
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(DownloadPhoto)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return photo;
            }
        }



        public async Task<bool> UploadPhoto(UploadPhotoViewModel photo)
        {
            byte[] thumb = CreateThumbnail(photo.Body);
            int photosUploaded = 0;
            
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO photos (photo, thumbnail, photographer_id) "
                        + "VALUES (@photo, @thumbnail, @photographer_id)";
                    cmd.Parameters.AddWithValue("@photo", photo.Body);
                    cmd.Parameters.AddWithValue("@thumbnail", thumb);
                    cmd.Parameters.AddWithValue("@photographer_id", photo.PhotographerId);

                    try
                    {
                        await con.OpenAsync();
                        photosUploaded = await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(UploadPhoto)} method" +
                            $" while uploading photo", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, thumbnail, photographer_id FROM photos "
                        + "WHERE thumbnail = @thumbnail";
                    cmd.Parameters.AddWithValue("@thumbnail", thumb);

                    try
                    {
                        await con.OpenAsync();
                        SqliteDataReader dr = await cmd.ExecuteReaderAsync();
                        bool hasPhotographer = await dr.ReadAsync();
                        if (hasPhotographer)
                        {
                            thumbnails.Add(new Thumbnail
                            {
                                Id = int.Parse(dr["id"].ToString()),
                                Body = (byte[])dr["thumbnail"],
                                PhotographerId = int.Parse(dr["photographer_id"].ToString())
                            });
                        }
                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(DownloadPhoto)} method " +
                            $"while getting saved thumbnail", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                if (photosUploaded > 0)
                    return true;
                return false;
            }
        }

        private byte[] CreateThumbnail(byte[] photo)
        {
            using (MemoryStream ms = new MemoryStream(photo))
            {
                using (SKData data = SKData.Create(ms))
                {
                    using (SKImage image = SKImage.FromEncodedData(data))
                    {

                        double multiplicator =
                            image.Height > image.Width
                            ?
                            100 / image.Height
                            :
                            100 / image.Width;

                        using (SKBitmap originalBitmap = SKBitmap.FromImage(image),
                            resizedBitmap = originalBitmap.Resize(
                                new SKImageInfo
                                {
                                    Height = (int)(image.Height * multiplicator),
                                    Width = (int)(image.Width * multiplicator)
                                },
                                SKBitmapResizeMethod.Lanczos3))
                        {
                            return SKImage.FromBitmap(resizedBitmap).Encode().ToArray();
                        }

                    }
                }
            }
        }

        private IEnumerable<Thumbnail> GetAllThumbnailsFromDB()
        {
            List<Thumbnail> thumbnails = new List<Thumbnail>();
            using (SqliteConnection con = new SqliteConnection(
                Startup.Configuration["ConnectionStrings:SQLiteConnection"]))
            {
                using (SqliteCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, thumbnail, photographer_id FROM photos";

                    try
                    {
                        con.Open();
                        SqliteDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            thumbnails.Add(new Thumbnail
                            {
                                Id = int.Parse(dr["id"].ToString()),
                                Body = (byte[])dr["thumbnail"],
                                PhotographerId = int.Parse(dr["photographer_id"].ToString())
                            });
                        }

                    }
                    catch (SqliteException ex)
                    {
                        throw new Exception($"Problem in {nameof(GetAllThumbnailsFromDB)} method", ex);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return thumbnails;
            }
        }
    }
}
