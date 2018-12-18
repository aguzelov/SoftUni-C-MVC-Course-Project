using System;
using System.Collections.Generic;
using System.Text;
using Dialog.Data.Common.Models;

namespace Dialog.Data.Models
{
    public class Setting : BaseDeletableModel<string>
    {
        public Setting()
        {
            base.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}