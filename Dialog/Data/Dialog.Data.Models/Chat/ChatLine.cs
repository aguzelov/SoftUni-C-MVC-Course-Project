using Dialog.Data.Common.Models;
using System;

namespace Dialog.Data.Models.Chat
{
    public class ChatLine : IAuditInfo
    {
        public ChatLine()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string Text { get; set; }

        public string ChatId { get; set; }
        public virtual Chat Chat { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}