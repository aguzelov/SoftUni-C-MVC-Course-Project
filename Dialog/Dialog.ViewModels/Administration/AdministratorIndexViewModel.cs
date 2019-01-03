using System.Collections.Generic;
using Dialog.ViewModels.Question;

namespace Dialog.ViewModels.Administration
{
    public class AdministratorIndexViewModel
    {
        public int PostsCount { get; set; }

        public int UsersCount { get; set; }

        public int ImagesCount { get; set; }

        public int NewsCount { get; set; }

        public ICollection<AdministrationQuestionViewModel> Questions { get; set; }
    }
}