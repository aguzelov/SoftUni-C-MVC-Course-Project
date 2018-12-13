using Dialog.ViewModels.Base;
using System.Collections.Generic;
using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models.Blog;

namespace Dialog.ViewModels.Blog
{
    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public AuthorViewModel Author { get; set; }

        public ICollection<TagViewModel> Tags { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }

        public CreateCommentViewModel CreateCommentViewModel { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForPath(e => e.Author.Id, opt => opt.MapFrom(src => src.Author.Id))
                .ForPath(e => e.Author.UserName, opt => opt.MapFrom(src => src.Author.UserName))
                .ReverseMap();
        }
    }
}