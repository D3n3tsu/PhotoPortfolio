using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoPortfolio.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        [HttpGet("photographers/")]
        public Task<JsonResult> GetPhotographers()
        {
            string result = JsonConvert.SerializeObject(
                _repository.GetAllPhotographers(),
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            return Task.FromResult(Json(result));
        }

        // GET api/photographers/5
        [HttpGet("photographers/{id}")]
        public Task<JsonResult> GetPhotographer(int id)
        {
            string result = JsonConvert.SerializeObject(
                _repository.GetPhotographer(id),
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            return Task.FromResult(Json(result));
        }

        // POST api/
        [HttpPost("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
