using AutoMapper;
using Model.Dtos.Blog_;
using Model.Dtos.BlogComment_;
using Model.Dtos.BlogLike_;
using Model.Dtos.Category_;
using Model.Dtos.User_;
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
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != default ? $"{src.Author.Name} {src.Author.LastName}".Trim() : default))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != default ? src.Category.Name : default))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ReverseMap();

        CreateMap<Blog, BlogDetailResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(src => src.Author != default ? src.Author.Name : default))
            .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Author != default ? src.Author.LastName : default))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != default ? src.Category.Name : default))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.CommentList, opt => opt.MapFrom(src => src.BlogComments))
            .ReverseMap();

        CreateMap<Blog, BlogLikesResponseDto>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.UserList, opt => opt.MapFrom(src => src.BlogLikes != default ? src.BlogLikes.Select(x => x.User) : default))
            .ReverseMap();

        CreateMap<BlogCreateDto, Blog>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ReverseMap();

        CreateMap<BlogUpdateDto, Blog>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ReverseMap();
        #endregion

        #region BlogComment
        CreateMap<BlogComment, BlogCommentBasicResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ReverseMap();

        CreateMap<BlogComment, BlogCommentDetailResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User != default ? src.User.Name : default))
            .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User != default ? src.User.LastName : default))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ReverseMap();

        CreateMap<BlogCommentCreateDto, BlogComment>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ReverseMap();

        CreateMap<BlogCommentUpdateDto, BlogComment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ReverseMap();
        #endregion

        #region BlogLike
        CreateMap<BlogLike, BlogLikeResponseDto>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User != default ? src.User.Name : default))
            .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User != default ? src.User.LastName : default))
            .ReverseMap();

        CreateMap<BlogLikeCreateDto, BlogLike>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ReverseMap();

        CreateMap<BlogLikeDeleteDto, BlogLike>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ReverseMap();
        #endregion

        #region Category
        CreateMap<Category, CategoryResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();

        CreateMap<Category, CategoryBlogsResponseDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.BlogList, opt => opt.MapFrom(src => src.Blogs))
            .ReverseMap();

        CreateMap<CategoryCreateDto, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();

        CreateMap<CategoryUpdateDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
        #endregion

        #region User
        CreateMap<User, UserBasicResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();

        CreateMap<User, UserDetailResponseDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Addres, opt => opt.MapFrom(src => src.Addres))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ReverseMap();

        CreateMap<User, UserBlogsResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.BlogList, opt => opt.MapFrom(src => src.Blogs))
            .ReverseMap();

        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Addres, opt => opt.MapFrom(src => src.Addres))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ReverseMap();

        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Addres, opt => opt.MapFrom(src => src.Addres))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ReverseMap();
        #endregion
    }
}
