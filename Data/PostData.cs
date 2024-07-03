using PRN221_Assignment.Models;

namespace PRN221_Assignment.Data
{
    public class PostData
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<string>? PhotoURL { get; set; }
        public string? Caption { get; set; }
        public DateTime Time { get; set; }
        public int countComment { get; set; } 
        public List<CommentData> ListComments { get; set; }
        public List<LikeData> ListLike { get; set; }
        public List<CommentData> ListCommentsTotal { get; set; }

    }
}
