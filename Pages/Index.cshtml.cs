using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services.Interface;


namespace PRN221_Assignment.Pages;

[Authorize(Roles = "User,Admin")]
[RequiredArgsConstructor]
public partial class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHomePageService _homePageService;
    private readonly IMessageService _messageService;
    private readonly IPostService _postService;
    private readonly IPhotoService _photoService;
    private readonly IFriendService _friendService;


    public IActionResult OnGet()
    {
        ViewData["listFriend"] = _homePageService.GetAllFriendsOfUser();
        ViewData["upComingBirthdayFriend"] = _homePageService.GetUpComingBirthdayFriends();
        ViewData["listPost"] = _postService.GetAllPostOfFriendAndFollower();
        ViewData["listSaved"] = _postService.GetAllPostIdsaved();
        ViewData["suggestFriend"] = _friendService.GetSuggestfriend();
        return Page();
    }
    public IList<Message> Messages { get; set; }
    public IList<MessageData> MessagesData { get; set; }
    public Post post { get; set; }

    //MessageChat
    public async Task<IActionResult> OnGetGetMessagesAsync(string senderId, string receiverId, bool open = false)
    {
        if (open)
        {
            await _messageService.UpdateStatusOfMessage(senderId, receiverId);
        }
        Messages = await _messageService.GetMessagesForReceiverAsync(senderId, receiverId);

        return new JsonResult(Messages);
    }

    public async Task<IActionResult> OnGetGetMessagesNotiAsync()
    {
        MessagesData = await _homePageService.GetUserChatWith();
        return new JsonResult(MessagesData);
    }


    //RequestFriend
    public async Task<IActionResult> OnGetGetRequestsAsync()
    {
        var friends = await _homePageService.GetAllFriendRequestUser();

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        return new JsonResult(friends, options);
    }

    public async Task<IActionResult> OnGetGetRequestsForNotificationAsync(int userid)
    {
        var friends = await _homePageService.GetAllFriendRequestUserOther(userid);
        var posts = await _homePageService.GetAllPostOfFriendNoti(userid);
        var postLikes = await _postService.GetAllPostLikeNoti();
        #region CommentLikes
        var commentLikes = await _postService.GetAllCommentLikesNoti();
        var commentsAll = await _postService.GetAllComments();
        var postsAll = _postService.GetAllPost();
        var commentWithUserIds = (from cl in commentLikes
                                 join c in commentsAll on cl.ConnectId equals c.CommentId
                                 join p in postsAll on c.PostId equals p.Id
                                 select new
                                 {
                                     CommentLike = cl,
                                     p.User.UserId
                                 }).ToList();
        #endregion
        var comment = await _postService.GetAllCommentNoti();
        #region CommentReplies
        var commentReplies = await _postService.GetAllCommentReplyNoti();
        var postIds = commentReplies.Select(cr => cr.PostId).Distinct().ToList();
        
        var postsDictionary = new Dictionary<int, Post>();
        foreach (var postId in postIds)
        {
            var post = await _postService.GetPostAsyncById(postId); 
            if (post != null)
            {
                postsDictionary[postId] = post;
            }
        }

        var commentRepliesWithPosts = commentReplies.Select(cr =>
        {
            var post = postsDictionary.ContainsKey(cr.PostId) ? postsDictionary[cr.PostId] : null;
            return new
            {
                CommentReply = cr,
                post.UserId
            };
        }).ToList();
        #endregion
        var result = new
        {
            Friends = friends,
            Posts = posts,
            PostLikes = postLikes,
            CommentLikes = commentWithUserIds,
            Comments = comment,
            CommentReplies = commentRepliesWithPosts
        };
        var options = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };
        var json = JsonConvert.SerializeObject(result, options);
        return Content(json, "application/json");
    }

    public IActionResult OnPostInsertComment(Comment comment)
    {
        return new JsonResult(_postService.InsertComment(comment));
    }

    //InsertComment
    public IActionResult OnPostHandleLike(int postId, int emotionId, bool deleteStatus)
    {

        var listPostLike = _postService.GetLikeDataOfPostAfterLike(postId, emotionId, deleteStatus);
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };
        return new JsonResult(listPostLike, options);
    }

    public IActionResult OnPostHandleLikeCmt(int cmtId, int emotionId)
    {

        var listCmtLike = _postService.GetLikeDataOfCmt(cmtId, emotionId);
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };
        return new JsonResult(listCmtLike, options);
    }

    public async Task<IActionResult> OnPostCreatePost(Post post, List<IFormFile> listfile)
    {
        List<string> listPhotoName = new List<string>();
        if (listfile != null)
        {
            foreach (var file in listfile)
            {
                if (file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", file.FileName);
                    int index = 1;

                    while (System.IO.File.Exists(filePath))
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", fileNameWithoutExtension + "(" + index + ")" + fileExtension);
                        index++;
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    listPhotoName.Add(Path.GetFileName(filePath));
                }
            }
        }

        post.CreatedAt = DateTime.Now;

        _postService.CreatePost(post);

        foreach (string photoName in listPhotoName)
        {
            Photo photo = new Photo();
            photo.PhotoUrl = "/Image/UploadPic/" + photoName;
            photo.CreatedAt = DateTime.Now;
            photo.PostId = post.PostId;
            _photoService.AddPhoto(photo);
        }
        var newPostData = new
        {
            post,
            Photos = listPhotoName.Select(photoName => "/Image/UploadPic/" + photoName).ToList()
        };

        return new JsonResult(newPostData);

    }

    public IActionResult OnPostDeletePost(int postId)
    {
        var listphoto = _photoService.GetPhotosById(postId);

        foreach (var photo in listphoto)
        {
            var photoName = photo.PhotoUrl.Split('/').Last();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Image/UploadPic", photoName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        _postService.DeletePost(postId);
        return new JsonResult(new { success = true });
    }

    public IActionResult OnPostRemoveComment(int cmtId)
    {
        if(cmtId != 0)
        {
            _postService.RemoveComment(cmtId);
        }
        return new JsonResult(new { success = true });
    }
}
