using Dialog.Data.Common.Models;
using System;

namespace Dialog.Data.Models.Chat
{
    public class UserChat : BaseDeletableModel<string>
    {
        public UserChat()
        {
            base.Id = Guid.NewGuid().ToString();
        }

        public string ChatId { get; set; }

        public virtual Chat Chat { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}