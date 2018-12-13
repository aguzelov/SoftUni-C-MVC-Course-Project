using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Dialog.Common.Mapping;
using Dialog.Data.Models;
using Dialog.ViewModels.User;

namespace Dialog.ViewModels.Administration
{
    public class AdministrationUsersViewModel
    {
        public ICollection<UserSummaryViewModel> Users { get; set; }
    }
}