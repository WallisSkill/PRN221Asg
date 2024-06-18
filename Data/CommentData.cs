using PRN221_Assignment.Models;

namespace PRN221_Assignment.Data
{
    public class CommentData
    {
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public User User { get; set; }
        public string Comment { get; set; }
        public DateTime Time { get; set; }
        public int CmtParent { get; set; }
        public List<CommentData> ListComment { get; set; }
        public List<LikeData> CmtLikes { get; set; }
        public int CountChildCmt { get; set; } = 0;
    }
}
