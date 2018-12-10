using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.ViewModels.Base;

namespace Dialog.ViewModels.News
{
    public class NewsViewModel : IMapFrom<Data.Models.News.News>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public AuthorViewModel Author { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Data.Models.News.News, NewsViewModel>()
                .ForPath(e => e.Author.Id, opt => opt.MapFrom(src => src.Author.Id))
                .ForPath(e => e.Author.Name, opt => opt.MapFrom(src => src.Author.UserName))
                .ReverseMap();
        }
    }
}