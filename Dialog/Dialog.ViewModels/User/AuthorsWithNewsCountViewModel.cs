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