using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;

namespace Dialog.ViewModels.User
{
    public class AuthorsWithPostsCountViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int PostCount { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, AuthorsWithPostsCountViewModel>()
                .ForMember(a => a.PostCount, opt => opt.MapFrom(src => src.Posts.Count));
        }
    }
}