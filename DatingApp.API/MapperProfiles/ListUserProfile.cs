using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.ExtentionsMethods;
using DatingApp.API.Models;

namespace DatingApp.API.MapperProfiles
{
    public class ListUserProfile:Profile
    {
        public ListUserProfile()
        {
            CreateMap<User,UserForListDto>()
            .ForMember(dest => dest.PhotosUrl,opt => {
                opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url);
            }).ForMember(dest => dest.Age,opt => {
                opt.MapFrom((src,dest) =>src.DateOfBirth.Age());
            }).ForMember(dest => dest.Created, opt=>{
                opt.MapFrom((src,dest)=>src.Created.Date);
            }).ReverseMap();
            CreateMap<User,UserForDetailsDto>() .ForMember(dest => dest.PhotoUrl,opt => {
                opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url);
            }).ForMember(dest => dest.Age,opt => {
                opt.MapFrom((src,dest) =>src.DateOfBirth.Age());
            }).ForMember(dest => dest.Created, opt=>{
                opt.MapFrom((src,dest)=>src.Created.ToString("d/M/yyy"));
            }).ReverseMap();         
            CreateMap<Photo,UserPhotoDto>().ReverseMap();

        }

    }
}