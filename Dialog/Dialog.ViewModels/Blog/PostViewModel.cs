using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models.Blog;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Gallery;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [DataType(DataType.ImageUrl)]
        public ICollection<IFormFile> UploadImages { get; set; }

        public ImageViewModel Image { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForPath(e => e.Author.Id, opt => opt.MapFrom(src => src.Author.Id))
                .ForPath(e => e.Author.UserName, opt => opt.MapFrom(src => src.Author.UserName))
                .ReverseMap();
        }
    }
}