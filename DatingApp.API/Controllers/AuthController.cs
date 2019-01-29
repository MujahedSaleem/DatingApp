using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Models;
using DatingApp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System;
using AutoMapper;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private IConfiguration Configuration;

        public readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            Configuration = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {


            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            if (_repo.userExistsAsync(userForRegisterDto.UserName))
            {
                return BadRequest("User Name Already Exsist");
            }
            var User = _mapper.Map<User>(userForRegisterDto);
            User.LastActive = DateTime.Now;
            var userFromRepo = await _repo.Register(user: User, Password: userForRegisterDto.Password);
            if (userFromRepo.GetType() == typeof(User))
            {
                var userToBeReturn = _mapper.Map<UserForDetailsDto>(userFromRepo);
                return CreatedAtRoute(
                    routeName: "GetUser",
                    routeValues: new { id = userFromRepo.Id },
                    value: userToBeReturn);
            }
            return StatusCode(204);

        }
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserForLogInDto userForLogInDto)
        {


            if (!_repo.userExistsAsync(userForLogInDto.UserName))
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.Login(userName: userForLogInDto.UserName, Password: userForLogInDto.Password);
            if (userFromRepo is null)
            {
                return Unauthorized();
            }
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id),

                new Claim(ClaimTypes.Name,userFromRepo.UserName)

            };
            var Securitykey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(Configuration.GetSection("AppSettings:Token").Value));

            var signinCredentials = new SigningCredentials(Securitykey, SecurityAlgorithms.HmacSha256Signature);

            var tokeOptions = new JwtSecurityToken(
                         issuer: "http://localhost:5000",
                         audience: "http://localhost:5000",
                         claims: claims,
                         expires: DateTime.Now.AddDays(1),
                         signingCredentials: signinCredentials
                     );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            var user = _mapper.Map<UserForListDto>(userFromRepo);

            return Ok(new { Token = tokenString, user });

        }
          [HttpPost("logout")]
        public  IActionResult logout( )
        {

            _repo.LogOut();
            
            return Ok();

        }
    
    }
    }