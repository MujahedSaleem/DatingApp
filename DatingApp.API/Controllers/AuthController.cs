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

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private IConfiguration Configuration;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            Configuration = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {


            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            if (_repo.userExistsAsync(userForRegisterDto.UserName))
            {
                return BadRequest("User Name Already Exsist");
            }
            var User = new User()
            {
                Name = userForRegisterDto.UserName,
                Email = userForRegisterDto.UserName

            };
            var createdUser = await _repo.Register(user: User, Password: userForRegisterDto.Password);
            if (createdUser.GetType() == typeof(User))
            {
            return StatusCode(201);
            }
            return StatusCode(204);

        }
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserForLogInDto userForLogInDto)
        {
            

            userForLogInDto.UserName = userForLogInDto.UserName.ToLower();
            if (!_repo.userExistsAsync(userForLogInDto.UserName))
            {
                return Unauthorized();
            }

            var createdUser = await _repo.Login(userName: userForLogInDto.UserName, Password: userForLogInDto.Password);
            if (createdUser is null)
            {
                return Unauthorized();
            }
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,createdUser.Id.ToString()),

                new Claim(ClaimTypes.Name,createdUser.Name.ToString())

            };
            var Securitykey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(Configuration.GetSection("AppSettings:Token").Value));

            var signinCredentials = new SigningCredentials(Securitykey, SecurityAlgorithms.HmacSha256Signature);

            var tokeOptions = new JwtSecurityToken(
                         issuer: "http://localhost:5000",
                         audience: "http://localhost:5000",
                         claims: claims,
                         expires: DateTime.Now.AddMinutes(5),
                         signingCredentials: signinCredentials
                     );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return Ok(new { Token = tokenString });

        }
    }
}