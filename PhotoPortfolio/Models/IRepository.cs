using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoPortfolio.Models
{
    public interface IRepository
    {
        void CreatePhotographer(string name, DateTime birthDate);
        Photographer GetPhotographer(int id);
        void UpdatePhotographer(Photographer photographer);
        void DeletePhotographer(int id);
    }
}
