using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using Dialog.Common.Mapping;
using Dialog.Data.Models;
using Dialog.ViewModels.Settings;

namespace Dialog.ViewModels.Administration
{
    public class AdministrationSettingsViewModel : SettingsViewModel, IMapFrom<Setting>
    {
        public string MeaningfulName => ToMeaningfulName(this.Name);

        public string ToMeaningfulName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return Regex.Replace(value, "(?!^)([A-Z])", " $1");
        }
    }
}