using PhotoPortfolio.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoPortfolio.Models
{
    public interface IPhotographerRepository
    {
        IEnumerable<Photographer> GetAllPhotographers();
        Task<Photographer> CreatePhotographer(string name, DateTime birthDate);
        Task<Photographer> GetPhotographer(int id);
        Task<Photographer> UpdatePhotographer(Photographer photographer);
        Task<bool> DeletePhotographer(int id);


    }
}
