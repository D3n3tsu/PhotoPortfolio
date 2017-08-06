using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoPortfolio.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using PhotoPortfolio.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PhotoPortfolio.Controllers
{
    [Route("api/")]
    public class ApiController : Controller
    {
        private IPhotoRepository _photoRepository;
        private IPhotographerRepository _photographerRepository;

        public ApiController(IPhotographerRepository photographerRep, IPhotoRepository photoRep)
        {
            _photographerRepository = photographerRep;
            _photoRepository = photoRep;
        }

        // GET: api/photographers
        [HttpGet("photographers")]
        public Task<JsonResult> GetPhotographers()
        {
            return Task.FromResult(Json(_photographerRepository.GetAllPhotographers()));
        }

        // GET api/photographers/5
        [HttpGet("photographers/{id}")]
        public Task<JsonResult> GetPhotographer(int id)
        {
            return Task.FromResult(Json(_photographerRepository.GetPhotographer(id)));
        }

        // POST api/create-photographer
        [HttpPost("create-photographer")]
        public Task<JsonResult> CreatePhotographer([FromBody]CreatePhotographerViewModel photographer)
        {
            return Task.FromResult(Json(
                _photographerRepository.CreatePhotographer(photographer.Name, photographer.BirthDate)
                ));
        }

        // PUT api/update-photographer/5
        [HttpPut("update-photographer/{id}")]
        public Task<JsonResult> UpdatePhotographer(int id,
            [FromBody]CreatePhotographerViewModel photographer)
        {
            return Task.FromResult(Json(
            _photographerRepository.UpdatePhotographer(
                new Photographer
                {
                    Id = id,
                    Name = photographer.Name,
                    BirthDate = photographer.BirthDate
                })
                ));
        }

        // DELETE api/delete-photographer/5
        [HttpDelete("delete-photographer/{id}")]
        public Task<JsonResult> DeletePhotographer(int id)
        {
            return Task.FromResult(Json(
                _photographerRepository.DeletePhotographer(id)
            ));
        }

        // POST api/upload-photo/5
        [HttpPost("upload-photo/{photographerId}")]
        public async Task<JsonResult> UploadPhoto(int photographerId)
        {
            var files = Request.Form.Files;
            byte[] fileData = null;
            bool success = false;
            using (var binaryReader = new BinaryReader(files[0].OpenReadStream()))
            {
                fileData = binaryReader.ReadBytes((int)files[0].OpenReadStream().Length);
                if (fileData != null)
                    success = await _photoRepository.UploadPhoto(
                        new UploadPhotoViewModel
                        {
                            Body = fileData,
                            PhotographerId = photographerId
                        });
            }

            return Json(_photoRepository.GetAllThumbnails(photographerId));
        }

        //GET api/get-thumbnails/2
        [HttpGet("get-thumbnails/{photographerId}")]
        public Task<JsonResult> GetAllThumbnails(int photographerId)
        {
            return Task.FromResult(
                Json(_photoRepository.GetAllThumbnails(photographerId))
                );
        }

        //GET api/download-photo/2
        [HttpGet("download-photo/{id}")]
        public Task<JsonResult> DownloadPhoto(int id)
        {
            return Task.FromResult(
                Json(_photoRepository.DownloadPhoto(id))
                );
        }

        //DELETE api/delete-photo/5
        [HttpDelete("delete-photo/{id}")]
        public Task<JsonResult> DeletePhoto(int id)
        {
            return Task.FromResult(
                Json(_photoRepository.DeletePhoto(id))
                );
        }
    }
}
