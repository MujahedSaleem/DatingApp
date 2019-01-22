using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]

    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository _repo, IMapper mapper)
        {
            _mapper = mapper;
            repo = _repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
             var users = await repo.Users();
            var userToReturn =_mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(userToReturn);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await repo.GetUser(id);
            var userToReturn =_mapper.Map<UserForDetailsDto>(user);
            return Ok(userToReturn);
        }
    }
}