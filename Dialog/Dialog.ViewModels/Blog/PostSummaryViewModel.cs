using System;
using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models.Blog;

namespace Dialog.ViewModels.Blog
{
    public class PostSummaryViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int CommmentsCount { get; set; }

        public string AuthorName { get; set; }

        public string ShortAuthorName
        {
            get
            {
                if (this.AuthorName.Contains("@"))
                {
                    var indexOfAt = this.AuthorName.IndexOf("@");

                    var result = this.AuthorName.Substring(0, indexOfAt);

                    return result;
                }
                else
                {
                    return this.AuthorName;
                }
            }
        }

        public string ShortContent =>
            this.Content.Length > 50 ?
            this.Content.Substring(0, 50) :
            this.Content;

        public string ModifiedDateText => this.ModifiedOn.Equals(default(DateTime))
            ? "Not Modified"
            : this.ModifiedOn.ToString("dd.MM.yyyy");

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Post, PostSummaryViewModel>()
                .ForMember(e => e.CommmentsCount, opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(e => e.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
                .ReverseMap();
        }
    }
}