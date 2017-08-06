using PhotoPortfolio.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoPortfolio.Models
{
    public interface IPhotoRepository
    {
        IEnumerable<Thumbnail> GetAllThumbnails(int photographerId);
        Task<bool> UploadPhoto(UploadPhotoViewModel photo);
        Task<Photo> DownloadPhoto(int photoId);
        Task<bool> DeletePhoto(int photoId);
    }
}
