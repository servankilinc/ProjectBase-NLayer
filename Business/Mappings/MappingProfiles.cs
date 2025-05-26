using AutoMapper;
using Model.Dtos;
using Model.Dtos.Blog_;
using Model.Entities;

namespace Business.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // CreateMap<source, dest>

        #region Blog
        CreateMap<Blog, BlogBasicResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? $"{src.Author.Name} {src.Author.LastName}".Trim() : ""))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ReverseMap();

        CreateMap<Blog, BlogDetailResponseDto>().ReverseMap();
        #endregion

        CreateMap<Blog, BlogBasicResponseDto>().ReverseMap();
        CreateMap<Dto_UserCreate, User>().ReverseMap();
        CreateMap<BlogCreateDto, Blog>().ReverseMap();
    }
}
