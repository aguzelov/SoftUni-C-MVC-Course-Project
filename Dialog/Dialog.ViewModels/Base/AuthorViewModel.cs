using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models;

namespace Dialog.ViewModels.Base
{
    public class AuthorViewModel : IMapFrom<ApplicationUser> , IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, AuthorViewModel>()
                .ForMember(a => a.Name, opt => opt.MapFrom(u => u.UserName));

        }
    }
}