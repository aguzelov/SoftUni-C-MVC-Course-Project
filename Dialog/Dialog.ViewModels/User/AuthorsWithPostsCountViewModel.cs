using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;
using Dialog.ViewModels.Base;

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