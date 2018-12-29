using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models.Chat;

namespace Dialog.ViewModels.Chat
{
    public class RecentMessagesViewModel : IMapFrom<ChatLine>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Date => this.CreatedOn.ToString(this.CreatedOn.Day != DateTime.UtcNow.Day ? "dd.MM.yyyy hh:mm" : "hh:mm:ss");

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ChatLine, RecentMessagesViewModel>()
                .ForMember(u => u.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName));
        }
    }
}