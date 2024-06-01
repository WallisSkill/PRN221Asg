using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class Post
    {
        public Post()
        {
            Bookmarks = new HashSet<Bookmark>();
            Comments = new HashSet<Comment>();
            PostLikes = new HashSet<PostLike>();
        }

        public int PostId { get; set; }
        public int? PhotoId { get; set; }
        public int UserId { get; set; }
        public string? Caption { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Photo? Photo { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
    }
}
