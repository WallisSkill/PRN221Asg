using System.Text.Json;
using System.Text.Json.Serialization;
using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Services.Interface;

namespace PRN221_Assignment.Pages;

[Authorize]
[RequiredArgsConstructor]
public partial class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHomePageService _homePageService;
    private readonly IMessageService _messageService;
    private readonly IPostService _postService;
    public IActionResult OnGet()
    {
        ViewData["listFriend"] = _homePageService.GetAllFriendsOfUser();
        ViewData["upComingBirthdayFriend"] = _homePageService.GetUpComingBirthdayFriends();
        ViewData["listPost"] = _postService.GetAllPostOfFriendAndFollower();
        return Page();
    }
    public IList<Message> Messages { get; set; }
    public IList<MessageData> MessagesData { get; set; }

    
    //MessageChat
    public async Task<IActionResult> OnGetGetMessagesAsync(string senderId,string receiverId,bool open = false)
    {
        if (open)
        {
            await _messageService.UpdateStatusOfMessage(senderId, receiverId);
        }
        Messages = await _messageService.GetMessagesForReceiverAsync(senderId,receiverId);

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

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        return new JsonResult(friends, options);
    }
    
    public IActionResult OnGetInsertComment(string commentText,int parentId, int postId)
    {
        Comment comment = new Comment()
        {
            CommentText = commentText,
            PostId = postId,
            ParentId = parentId
        };
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
}
