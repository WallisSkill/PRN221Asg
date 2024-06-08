using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string? Content { get; set; }
        public int TypeId { get; set; }
        public DateTime? SendAt { get; set; }
        public bool? IsRead { get; set; }

        public virtual User Receiver { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }
}
