using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models;

namespace Dialog.ViewModels.Base
{
    public class AuthorViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}