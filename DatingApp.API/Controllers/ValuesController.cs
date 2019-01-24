using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.ApI.Data;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.ApI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 //   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public readonly IAuthRepository Repo ;

        public ValuesController(ApplicationDbContext db, IAuthRepository repo)
        {
            _db = db;
            Repo = repo;
            new Seed(db,repo).SeedUsers();
        }
        // GET api/values
        [HttpGet]
        public async Task<ActionResult>  Get()
        {   
            var values =await _db.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var value = await _db.Values.FirstOrDefaultAsync(a => a.Id == id); ;
            if (value is null)
                return NotFound();
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
