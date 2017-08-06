using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoPortfolio.Models.ViewModels
{
    public class UploadPhotoViewModel
    {
        public byte[] Body { get; set; }
        public int PhotographerId { get; set; }
    }
}
