using Dialog.ViewModels.User;
using System.Collections.Generic;

namespace Dialog.ViewModels.Administration
{
    public class AdministrationUsersViewModel
    {
        public ICollection<UserSummaryViewModel> Users { get; set; }
    }
}