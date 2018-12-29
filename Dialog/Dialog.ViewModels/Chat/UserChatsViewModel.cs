using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Dialog.Common.Mapping;

namespace Dialog.ViewModels.Chat
{
    public class UserChatsViewModel : IMapFrom<Data.Models.Chat.Chat>, IHaveCustomMappings
    {
        public string ChatId { get; set; }

        public string ChatName { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Data.Models.Chat.Chat, UserChatsViewModel>()
                .ForMember(c => c.ChatId, opt => opt.MapFrom(src => src.Id))
                .ForMember(c => c.ChatName, opt => opt.MapFrom(src => src.Name));
        }
    }
}