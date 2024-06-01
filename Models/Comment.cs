using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class Comment
    {
        public Comment()
        {
            CommentLikes = new HashSet<CommentLike>();
        }

        public int CommentId { get; set; }
        public string CommentText { get; set; } = null!;
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
    }
}
