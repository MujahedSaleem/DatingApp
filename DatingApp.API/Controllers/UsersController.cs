using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using DatingApp.API.HelpersAndExtentions;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController, Authorize(AuthenticationSchemes = "Bearer")]

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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams param)
        {
            param.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var users = await repo.Users(param);
            var userToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(userToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailsDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserForUpdatesDto userForUpdates)
        {
            if (!(id.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            var userFromRepo = await repo.GetUser(id);

            _mapper.Map(userForUpdates, userFromRepo);
            if (await repo.SaveAll())
            {
                return NoContent();
            }
            throw new Exception($"updateing user {User.Identity.Name} faild on server");

        }

        [HttpPost("{id}/like/{recipientId}")]

        public async Task<IActionResult> LikeUser(string id, string recipientId)
        {
            if (!(id.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }

            var like = await repo.GetLike(id, recipientId);
            if (like != null)
            {
                repo.Delete<Like>(like);
                if (await repo.SaveAll())
                {
                    return new JsonResult(JsonConvert.SerializeObject(new String("You  disLike")));
                }
                return BadRequest("Faild to like usre");
            }
            if (await repo.GetUser(recipientId) == null)
            {
                return NotFound();
            }
            like = new Like()
            {
                LikerId = id,
                LikeeId = recipientId
            };
            repo.Add<Like>(like);

            if (await repo.SaveAll())
            {
                return new JsonResult(JsonConvert.SerializeObject(new String("You Like")));
            }
            return BadRequest("Faild to like usre");
        }


    }

}
