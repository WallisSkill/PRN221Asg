using System.Collections.Concurrent;
using System.Security.Claims;
using Lombok.NET;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository.Interface;

namespace PRN221_Assignment.Hubs;

[RequiredArgsConstructor]
public partial class LiveHub : Hub
{
    private readonly IFriendRepository _friendRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IPostRepository _postRepository;
    public async Task SendMessage(string senderId, string receiverId, string message)
    {
        try
        {
            var newMessage = new Message
            {
                SenderId = Int32.Parse(senderId),
                ReceiverId = Int32.Parse(receiverId),
                Content = message,
                SendAt = DateTime.Now,
                IsRead = false
            };
          await _messageRepository.InsertNewMessage(newMessage);

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message, _profileRepository.GetUserInfo(Int32.Parse(senderId))?.Fullname);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            throw;
        }
    }
    
    public async Task SendFriendRequest(string userId, string friendUserId,string name)
    {
        _friendRepository.SendFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
        await Clients.User(friendUserId).SendAsync("ReceiveFriendRequest", userId, friendUserId,name);
    }
    public async Task AcceptFriendRequest(string userId, string friendUserId,string name)
    {
        _friendRepository.AcceptFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
        await Clients.User(friendUserId).SendAsync("AcceptFriendRequest", userId, friendUserId,name);
    }

    public void CancelFriendRequest(string userId, string friendUserId)
    {
        _friendRepository.CancelFriendRequest(Int32.Parse(userId),Int32.Parse(friendUserId));
    }
    public async Task NewPost(string userId, string name)
    {
        List<Friend> friends = _friendRepository.GetAllFriendRelationshipsOfUser(Int32.Parse(userId), true);
        var listFriendId = new List<string>();

        foreach (var item in friends)
        {
            if (item.User1Id == (Int32.Parse(userId)))
            {
                listFriendId.Add(item.User2Id.ToString());
            }
            else
            {
                listFriendId.Add(item.User1Id.ToString());
            }
        }
        
        await Clients.Users(listFriendId).SendAsync("NewNoti", userId, 1, name);
    }

    public async Task NewLike(string userId, string name, string postId)
    {
        Post post = _postRepository.GetPostById(int.Parse(postId));
        if (int.Parse(userId) != post.UserId)
        {
            if (_postRepository.GetPostLike(int.Parse(postId), int.Parse(userId)) != null)
            {
                await Clients.User(post.UserId.ToString()).SendAsync("NewNoti", userId, 2, name);
            }
        }
    }

    public async Task NewLikeCMT(string userId, string name, string commentId)
    {
        Comment comment = _postRepository.getCommentById(int.Parse(commentId));
        if (int.Parse(userId) != comment.UserId)
        {
            if (_postRepository.GetCommentLike(int.Parse(commentId), int.Parse(userId)) != null)
            {
                await Clients.User(comment.UserId.ToString()).SendAsync("NewNoti", userId, 5, name);
            }
        }
    }

    public async Task NewComment(string userId, string name, int commentId)
    {
        Comment? comment = _postRepository.getCommentById(commentId);

        if (comment != null)
        {
            Post post = _postRepository.GetPostById(comment.PostId);

            int parentId = (comment.ParentId != null ? (int)comment.ParentId : 0);

            if (parentId != 0)
            {
                Comment? commentReply = _postRepository.getCommentById(parentId);

                if (commentReply != null)
                {
                    if (!userId.Equals(commentReply.UserId.ToString()))
                    {
                        await Clients.User(commentReply.UserId.ToString()).SendAsync("NewNoti", userId, 4, name);
                    }
                }
            }
            else
            {
                if (!userId.Equals(post.UserId.ToString()))
                {
                    await Clients.User(post.UserId.ToString()).SendAsync("NewNoti", userId, 3, name);
                }
            }
        }

    }

    //status online

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private static ConcurrentDictionary<string, string> _users = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = Context.User.FindFirstValue(ClaimTypes.GivenName);

        if (userId != null)
        {
            _users.TryAdd(userId,Context.ConnectionId);
            await Clients.All.SendAsync("UserConnected", userId, username);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        _users.TryRemove(userId, out _);
        await Clients.All.SendAsync("UserDisconnected", userId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task GetOnlineUsers()
    {
        var onlineUsers = _users.Keys.ToList();
        await Clients.Caller.SendAsync("ReceiveOnlineUsers", onlineUsers);
    }
    
    public bool IsUserOnline(string userId)
    {
        return _users.ContainsKey(userId);
    }
}