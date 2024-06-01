using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class Photo
    {
        public Photo()
        {
            Posts = new HashSet<Post>();
        }

        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; } = null!;
        public int PostId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
