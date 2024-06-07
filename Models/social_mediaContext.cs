using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN221_Assignment.Models
{
    public partial class social_mediaContext : DbContext
    {
        public social_mediaContext()
        {
        }

        public social_mediaContext(DbContextOptions<social_mediaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bookmark> Bookmarks { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<CommentLike> CommentLikes { get; set; } = null!;
        public virtual DbSet<Emotion> Emotions { get; set; } = null!;
        public virtual DbSet<Follow> Follows { get; set; } = null!;
        public virtual DbSet<Friend> Friends { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostLike> PostLikes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            if (!optionsBuilder.IsConfigured) { optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection")); }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bookmark>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PostId })
                    .HasName("PK__bookmark__CA534F799ABB135B");

                entity.ToTable("bookmarks");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Bookmarks)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bookmarks__post___5CD6CB2B");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookmarks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__bookmarks__user___5DCAEF64");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comments");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CommentText)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("comment_text");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comments__post_i__44FF419A");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comments__user_i__45F365D3");
            });

            modelBuilder.Entity<CommentLike>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CommentId })
                    .HasName("PK__comment___D7C76067B2A367CB");

                entity.ToTable("comment_likes");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmotionId).HasColumnName("emotion_id");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.CommentLikes)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comment_l__comme__5441852A");

                entity.HasOne(d => d.Emotion)
                    .WithMany(p => p.CommentLikes)
                    .HasForeignKey(d => d.EmotionId)
                    .HasConstraintName("FK__comment_l__emoti__52593CB8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CommentLikes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comment_l__user___534D60F1");
            });

            modelBuilder.Entity<Emotion>(entity =>
            {
                entity.ToTable("emotion");

                entity.HasIndex(e => e.EmotionUrl, "UQ__emotion__765A2B5C16F90E3A")
                    .IsUnique();

                entity.Property(e => e.EmotionId)
                    .ValueGeneratedNever()
                    .HasColumnName("emotion_id");

                entity.Property(e => e.EmotionUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("emotion_url");
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(e => new { e.FollowerId, e.FolloweeId })
                    .HasName("PK__follows__710D19E6C8DB29E8");

                entity.ToTable("follows");

                entity.Property(e => e.FollowerId).HasColumnName("follower_id");

                entity.Property(e => e.FolloweeId).HasColumnName("followee_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Followee)
                    .WithMany(p => p.FollowFollowees)
                    .HasForeignKey(d => d.FolloweeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__follows__followe__59063A47");

                entity.HasOne(d => d.Follower)
                    .WithMany(p => p.FollowFollowers)
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__follows__followe__5812160E");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(e => new { e.User1Id, e.User2Id })
                    .HasName("PK__friend__AAA434A6E9177E16");

                entity.ToTable("friend");

                entity.Property(e => e.User1Id).HasColumnName("user1_id");

                entity.Property(e => e.User2Id).HasColumnName("user2_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.HasOne(d => d.User1)
                    .WithMany(p => p.FriendUser1s)
                    .HasForeignKey(d => d.User1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__friend__user1_id__6FE99F9F");

                entity.HasOne(d => d.User2)
                    .WithMany(p => p.FriendUser2s)
                    .HasForeignKey(d => d.User2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__friend__user2_id__70DDC3D8");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");

                entity.Property(e => e.MessageId).HasColumnName("message_id");

                entity.Property(e => e.IsRead)
                    .HasColumnName("is_read")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReceiverId).HasColumnName("receiver_id");

                entity.Property(e => e.SendAt)
                    .HasColumnType("datetime")
                    .HasColumnName("send_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SenderId).HasColumnName("sender_id");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__messages__receiv__6383C8BA");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__messages__sender__628FA481");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("photos");

                entity.HasIndex(e => e.PhotoUrl, "UQ__photos__1464808BDE553307")
                    .IsUnique();

                entity.Property(e => e.PhotoId).HasColumnName("photo_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhotoUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("photo_url");

                entity.Property(e => e.PostId).HasColumnName("post_id");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Caption)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("caption");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PhotoId).HasColumnName("photo_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK__post__photo_id__412EB0B6");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__post__user_id__403A8C7D");
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PostId })
                    .HasName("PK__post_lik__CA534F792A393A03");

                entity.ToTable("post_likes");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmotionId).HasColumnName("emotion_id");

                entity.HasOne(d => d.Emotion)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.EmotionId)
                    .HasConstraintName("FK__post_like__emoti__4CA06362");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__post_like__post___4E88ABD4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__post_like__user___4D94879B");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Username, "UQ__users__F3DBC572A8D92706")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Bio)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("bio");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("fullname");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.ProfilePhotoUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("profile_photo_url");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
