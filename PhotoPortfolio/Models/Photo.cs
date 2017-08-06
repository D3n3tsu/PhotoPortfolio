using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoPortfolio.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public byte[] Body { get; set; }
        public int PhotographerId { get; set; }
    }
}
