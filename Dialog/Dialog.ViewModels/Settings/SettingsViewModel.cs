using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dialog.ViewModels.Settings
{
    public class SettingsViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }
}