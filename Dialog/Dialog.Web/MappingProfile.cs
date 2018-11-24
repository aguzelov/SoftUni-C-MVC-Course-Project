using AutoMapper;
using Dialog.Models.Blog;
using Dialog.Web.Areas.Blog.Models;

namespace Dialog.Web
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

            //CreateMap<Order, EventWithTicketsCountViewModel>()
            //    .ForMember(
            //        e => e.Name,
            //        opt => opt.MapFrom(src => src.Event.Name)
            //    )
            //    .ForMember(
            //        e => e.End,
            //        opt => opt.MapFrom(src => src.Event.End)
            //    )
            //    .ForMember(
            //        e => e.Start,
            //        opt => opt.MapFrom(src => src.Event.Start)
            //    ).ReverseMap();

            //CreateMap<Order, DisplayOrderViewModel>()
            //    .ForPath(
            //    e => e.EventName,
            //    opt => opt.MapFrom(src => src.Event.Name)
            //    )
            //    .ForPath(
            //    e => e.Customer,
            //    opt => opt.MapFrom(src => src.Customer.UserName)
            //    ).ReverseMap();
        }
    }
}