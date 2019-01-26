using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.HelpersAndExtentions;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Route("api/users/{userId}/photo")]
    [ApiController, Authorize(AuthenticationSchemes = "Bearer")]
    public class PhotoController : ControllerBase
    {
        public readonly IDatingRepository _Repo;
        public readonly IOptions<CloudinarySettings> _Options;
        public readonly IMapper _Mapper;
        private Cloudinary _cloudinary;

        public PhotoController(IDatingRepository repo,
         IMapper mapper,
          IOptions<CloudinarySettings> options)
        {
            this._Mapper = mapper;
            this._Options = options;
            this._Repo = repo;
            Account account = new Account(
            _Options.Value.Cloudname,
             _Options.Value.APIKey,
            _Options.Value.APISecret);
            _cloudinary = new Cloudinary(account);


        }
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> getPhoto(int id)
        {
            var phtotFromrfepo = await this._Repo.GetPhoto(id);

            var Photo = _Mapper.Map<PhotoForReturnDto>(phtotFromrfepo);

            return Ok(Photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(string userId, [FromForm] photoForCreationDto photoForCreationDto)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            var userFromRepo = await _Repo.GetUser(userId);
            var file = photoForCreationDto.File;

            var uplodeResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uplodeParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().
                        Width(500).
                        Height(500).
                        Crop("fill").
                        Gravity("face")
                    };

                    uplodeResult = _cloudinary.Upload(uplodeParams);

                }

            }
            photoForCreationDto.url = uplodeResult.Uri.ToString();
            photoForCreationDto.PublicID = uplodeResult.PublicId;
            var photo = _Mapper.Map<Photo>(photoForCreationDto);
            if (!userFromRepo.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }
            userFromRepo.Photos.Add(photo);
            if (await _Repo.SaveAll())
            {
                var phototoReturn = _Mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute(
                                   routeName: "GetPhoto",
                                   routeValues: new { id = photo.Id },
                                   value: photo);
            }
            return BadRequest("Could not Uplode Photo");

        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(string userId, int id)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            var userFromRepo = await _Repo.GetUser(userId);
            if (!userFromRepo.Photos.Any(a => a.Id == id))
            {
                return Unauthorized();
            }
            var photoFromRepo = await _Repo.GetPhoto(id);
            if (photoFromRepo.IsMain)
            {
                return BadRequest("The MAin Photo ");
            }

            var CurrentMainPhoto = await _Repo.GetMainPhoto(userId);
            CurrentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;
            if (await _Repo.SaveAll())
                return NoContent();
            return BadRequest("Could not set photo to main");
        }
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeletePhoto(string userId, int id)
        {
            if (!(userId.Equals((User.FindFirst(ClaimTypes.NameIdentifier).Value))))
            {
                return Unauthorized();
            }
            var userFromRepo = await _Repo.GetUser(userId);
            if (!userFromRepo.Photos.Any(a => a.Id == id))
            {
                return Unauthorized();
            }
            var photoFromRepo = await _Repo.GetPhoto(id);
            if (photoFromRepo.IsMain)
            {
                return BadRequest("This is Main Photo ");
            }
            if (photoFromRepo.PublicID != null)
            {
                var delResParams = new DeletionParams(photoFromRepo.PublicID);
                var result = _cloudinary.Destroy(delResParams);
                if (result.Result == "ok")
                    _Repo.Delete<Photo>(photoFromRepo);

                if (await _Repo.SaveAll())
                    return Ok(result.StatusCode);
            }
            else
            {
                _Repo.Delete<Photo>(photoFromRepo);

                if (await _Repo.SaveAll())
                    return Ok();
            }

            return BadRequest("Could not Delete to main");
        }

    }
}