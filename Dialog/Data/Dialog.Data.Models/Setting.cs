using Dialog.Data.Common.Models;
using System;

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