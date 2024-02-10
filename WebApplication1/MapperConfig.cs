using AutoMapper;
using WebApplication1.Models;
using WebApplication1.Models.Dto;

namespace MagicVilla_VillaAPI
{
    public class MapperConfig: Profile
    {
        public MapperConfig() { 
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
        }
    }
}
