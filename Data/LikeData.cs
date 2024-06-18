using PRN221_Assignment.Models;

namespace PRN221_Assignment.Data
{
    public class LikeData
    {
        public int ConnectId { get; set; }
        public User User { get; set; }
        public string EmotionURL { get; set; }
    }
}
