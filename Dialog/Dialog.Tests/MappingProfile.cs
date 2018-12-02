using AutoMapper;
using Dialog.Models.Blog;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;

namespace Tests
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostSummaryViewModel>()
                .ForPath(e => e.CommmentsCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForPath(e => e.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
                .ReverseMap();

            CreateMap<Post, RecentBlogViewModel>()
                .ForPath(e => e.CommmentsCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForPath(e => e.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
                .ReverseMap();

            CreateMap<Post, PostViewModel>()
                .ForPath(e => e.Author.Id, opt => opt.MapFrom(src => src.Author.Id))
                .ForPath(e => e.Author.Name, opt => opt.MapFrom(src => src.Author.UserName))
                .ReverseMap();
        }
    }
}