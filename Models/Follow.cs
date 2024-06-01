using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class Follow
    {
        public int FollowerId { get; set; }
        public int FolloweeId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User Followee { get; set; } = null!;
        public virtual User Follower { get; set; } = null!;
    }
}
