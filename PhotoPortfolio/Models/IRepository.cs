using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoPortfolio.Models
{
    public interface IRepository
    {
        Task<IEnumerable<Photographer>> GetAllPhotographers();
        Task CreatePhotographer(string name, DateTime birthDate);
        Task<Photographer> GetPhotographer(int id);
        Task UpdatePhotographer(Photographer photographer);
        Task DeletePhotographer(int id);
    }
}
