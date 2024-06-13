using System;
using System.Collections.Generic;

namespace PRN221_Assignment.Models
{
    public partial class User
    {
        public User()
        {
            Bookmarks = new HashSet<Bookmark>();
            CommentLikes = new HashSet<CommentLike>();
            Comments = new HashSet<Comment>();
            FollowFollowees = new HashSet<Follow>();
            FollowFollowers = new HashSet<Follow>();
            FriendUser1s = new HashSet<Friend>();
            FriendUser2s = new HashSet<Friend>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            PostLikes = new HashSet<PostLike>();
            Posts = new HashSet<Post>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public string? profilePhotoUrl;
        public string? Bio { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Email { get; set; } = null!;
        public int? Gender { get; set; }
        public DateTime? Dob { get; set; }

        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follow> FollowFollowees { get; set; }
        public virtual ICollection<Follow> FollowFollowers { get; set; }
        public virtual ICollection<Friend> FriendUser1s { get; set; }
        public virtual ICollection<Friend> FriendUser2s { get; set; }
        public virtual ICollection<Message> MessageReceivers { get; set; }
        public virtual ICollection<Message> MessageSenders { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        
        public string? ProfilePhotoUrl
        {
            get => profilePhotoUrl ?? "./assets/images/user/null.png";
            set => profilePhotoUrl = value;
        }
    }
}
