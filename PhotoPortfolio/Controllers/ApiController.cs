using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoPortfolio.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using PhotoPortfolio.Models.ViewModels;


namespace PhotoPortfolio.Controllers
{
    [Route("api/")]
    public class ApiController : Controller
    {
        private IRepository _repository;

        public ApiController(IRepository repo)
        {
            _repository = repo;
        }

        // GET: api/photographers
        [HttpGet("photographers")]
        public Task<JsonResult> GetPhotographers()
        {
            return Task.FromResult(Json(_repository.GetAllPhotographers()));
        }

        // GET api/photographers/5
        [HttpGet("photographers/{id}")]
        public Task<JsonResult> GetPhotographer(int id)
        {
            return Task.FromResult(Json(_repository.GetPhotographer(id)));
        }

        // POST api/create-photographer
        [HttpPost("create-photographer")]
        public Task<JsonResult> Post([FromBody]CreatePhotographerViewModel photographer)
        {
            _repository.CreatePhotographer(photographer.Name, photographer.BirthDate);
            
            return Task.FromResult(Json(_repository.GetAllPhotographers()));
        }

        // PUT api/5
        [HttpPut("update-photographer/{id}")]
        public void Put(int id, [FromBody]CreatePhotographerViewModel photographer)
        {
            _repository.UpdatePhotographer(
                new Photographer
                {
                    Id = id,
                    Name = photographer.Name,
                    BirthDate = photographer.BirthDate
                });
        }

        // DELETE api/5
        [HttpDelete("delete-photographer/{id}")]
        public void Delete(int id)
        {
            _repository.DeletePhotographer(id);
        }
    }
}
