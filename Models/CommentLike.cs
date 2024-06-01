using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class CommentLike
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? EmotionId { get; set; }

        public virtual Comment Comment { get; set; } = null!;
        public virtual Emotion? Emotion { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
