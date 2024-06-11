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
    public IActionResult OnGet()
    {
        ViewData["listFriend"] = _homePageService.GetAllFriendsOfUser();
        ViewData["upComingBirthdayFriend"] = _homePageService.GetUpComingBirthdayFriends();
        return Page();
    }
    public IList<Message> Messages { get; set; }
    public IList<MessageData> MessagesData { get; set; }

    public async Task<IActionResult> OnGetGetMessagesAsync(string senderId,string receiverId)
    {
        Messages = await _messageService.GetMessagesForReceiverAsync(senderId,receiverId);
        await _messageService.UpdateStatusOfMessage(senderId, receiverId);
        return new JsonResult(Messages);
    }
    
    public async Task<IActionResult> OnGetGetMessagesNotiAsync()
    {
        MessagesData = await _homePageService.GetUserChatWith();
        return new JsonResult(MessagesData);
    }
     
}
