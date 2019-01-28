using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.HelpersAndExtentions;
using DatingApp.API.Models;

namespace DatingApp.API.MapperProfiles
{
    public class ListUserProfile : Profile
    {
        public ListUserProfile()
        {
            CreateMap<User, UserForListDto>()
            .ForMember(dest => dest.PhotosUrl, opt =>
            {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            }).ForMember(dest => dest.Age, opt =>
            {
                opt.MapFrom((src, dest) => src.DateOfBirth.Age());
            }).ForMember(dest => dest.Created, opt =>
            {
                opt.MapFrom((src, dest) => src.Created.Date);
            }).ReverseMap();
            CreateMap<User, UserForDetailsDto>().ForMember(dest => dest.PhotosUrl, opt =>
            {
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            }).ForMember(dest => dest.Age, opt =>
            {
                opt.MapFrom((src, dest) => src.DateOfBirth.Age());
            }).ForMember(dest => dest.Created, opt =>
            {
                opt.MapFrom((src, dest) => src.Created);
            }).ReverseMap();
            CreateMap<Photo, UserPhotoDto>().ReverseMap();

            CreateMap<User, UserForUpdatesDto>().ReverseMap();
            CreateMap<Photo, PhotoForReturnDto>().ReverseMap();
            CreateMap<Photo, photoForCreationDto>().ReverseMap();
            CreateMap<User, UserForRegisterDto>()
            .ForMember(dest => dest.UserName, opt =>
            {
                opt.MapFrom(src => src.Name);
            })
            .ForMember(dest => dest.gender, opt =>
            {
                opt.MapFrom(src => src.gander);
            }).ReverseMap();
            
        }

    }
}