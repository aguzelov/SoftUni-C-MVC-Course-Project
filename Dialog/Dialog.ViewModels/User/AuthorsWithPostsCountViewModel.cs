using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models;
using Dialog.ViewModels.Base;
using System.Linq;

namespace Dialog.ViewModels.User
{
    public class AuthorsWithPostsCountViewModel : AuthorViewModel, IHaveCustomMappings
    {
        public int Posts { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, AuthorsWithPostsCountViewModel>()
                .ForMember(a => a.Posts, opt => opt.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)));
        }
    }
}