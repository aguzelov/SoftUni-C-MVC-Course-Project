using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models;
using Dialog.ViewModels.Administration;

namespace Dialog.ViewModels.User
{
    public class UserSummaryViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [NotMapped]
        public string Role { get; set; }

        public DateTime CreatedOn { get; set; }

        public int PostsCount { get; set; }

        public int NewsCount { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserSummaryViewModel>()
                .ForMember(u => u.PostsCount, opt => opt.MapFrom(src => src.Posts.Count(p => !p.IsDeleted)))
                .ForMember(u => u.NewsCount, opt => opt.MapFrom(src => src.News.Count(n => !n.IsDeleted)));
        }
    }
}