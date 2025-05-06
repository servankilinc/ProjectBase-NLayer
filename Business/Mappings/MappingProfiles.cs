using AutoMapper;
using Model.Dtos;
using Model.Entities;

namespace Business.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // CreateMap<source, dest>

        CreateMap<Blog, BlogBasicResponseDto>().ReverseMap();
        CreateMap<Dto_UserCreate, User>().ReverseMap();
        CreateMap<BlogCreateDto, Blog>().ReverseMap();
    }
}
