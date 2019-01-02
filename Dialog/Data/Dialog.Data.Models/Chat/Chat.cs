using Dialog.Data.Common.Models;
using System;
using System.Collections.Generic;

namespace Dialog.Data.Models.Chat
{
    public class Chat : BaseDeletableModel<string>
    {
        public Chat()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public virtual ICollection<ChatLine> ChatLines { get; set; } = new HashSet<ChatLine>();

        public virtual ICollection<UserChat> UserChats { get; set; } = new HashSet<UserChat>();
    }
}