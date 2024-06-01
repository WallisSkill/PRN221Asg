using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class PostLike
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? EmotionId { get; set; }

        public virtual Emotion? Emotion { get; set; }
        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
