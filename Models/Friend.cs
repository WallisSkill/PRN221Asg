using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class Friend
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual User User1 { get; set; } = null!;
        public virtual User User2 { get; set; } = null!;
    }
}
