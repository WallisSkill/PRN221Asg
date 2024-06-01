using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class Emotion
    {
        public Emotion()
        {
            CommentLikes = new HashSet<CommentLike>();
            PostLikes = new HashSet<PostLike>();
        }

        public int EmotionId { get; set; }
        public string EmotionUrl { get; set; } = null!;

        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
    }
}
