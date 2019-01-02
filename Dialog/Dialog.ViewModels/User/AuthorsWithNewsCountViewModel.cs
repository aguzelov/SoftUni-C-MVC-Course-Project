using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models;
using Dialog.ViewModels.Base;
using System.Linq;

namespace Dialog.ViewModels.User
{
    public class AuthorsWithNewsCountViewModel : AuthorViewModel, IHaveCustomMappings
    {
        public int News { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, AuthorsWithNewsCountViewModel>()
                .ForMember(a => a.News, opt => opt.MapFrom(src => src.News.Count(n => !n.IsDeleted)));
        }
    }
}