﻿using DependencyInjectionAutomatic.Service;
using PRN221_Assignment.Data;
using PRN221_Assignment.Models;
using PRN221_Assignment.Repository;
using PRN221_Assignment.Repository.Interface;
using PRN221_Assignment.Services.Interface;
using System.Runtime.InteropServices;




namespace PRN221_Assignment.Services
{
    [Service]
    public partial class HomePageService : IHomePageService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IPostRepository _postRepository;
        private readonly int _currentUserId;

        public HomePageService(IFriendRepository friendRepository, IUserResolverService userResolver,
            IMessageRepository messageRepository, IPostRepository postRepository)
        {
            _friendRepository = friendRepository;
            _currentUserId = userResolver.GetUser();
            _messageRepository = messageRepository;
            _postRepository = postRepository;
        }

        public List<User> GetAllFriendsOfUser()
        {
            var listRelationship = _friendRepository.GetAllFriendRelationshipsOfUser(_currentUserId,true);
            var listFriendId = new List<int>();
            listRelationship.ForEach(item =>
            {
                if (item.User1Id == _currentUserId)
                {
                    listFriendId.Add(item.User2Id);
                }
                else
                {
                    listFriendId.Add(item.User1Id);
                }
            });
            var listFriends = _friendRepository.GetFriendsOfUser(listFriendId);
            return listFriends;
        }

        public async Task<IList<UserFriend>> GetAllFriendRequestUser(int userid =-1)
        {
            var requestList = await _friendRepository.GetAllFriendRequestUser(_currentUserId);
            if(userid != -1) requestList = await _friendRepository.GetAllFriendRequestUser(userid);
            var listFriendId = new List<int>();

            foreach (var item in requestList)
            {
                if (item.User1Id == (userid == -1?_currentUserId :userid))
                {
                    listFriendId.Add(item.User2Id);
                }
                else
                {
                    listFriendId.Add(item.User1Id);
                }
            }

            var listFriends = await _friendRepository.GetFriendsOfUserAsync(listFriendId,userid == -1 ?_currentUserId : userid);
            return listFriends;
        }

        public async Task<IList<UserFriend>> GetAllFriendRequestUserOther(int userId =-1)
        {
            var requestList = await _friendRepository.GetAllFriendRequestUserOther(_currentUserId);
            if(userId != -1) requestList = await _friendRepository.GetAllFriendRequestUserOther(userId);
            var listFriendId = new List<int>();

            foreach (var item in requestList)
            {
                if (item.User1Id == (userId == -1?_currentUserId :userId))
                {
                    listFriendId.Add(item.User2Id);
                }
                else
                {
                    listFriendId.Add(item.User1Id);
                }
            }

            var listFriends = await _friendRepository.GetFriendsOfUserAsyncOther(listFriendId,userId == -1 ?_currentUserId : userId);
            return listFriends;
        }

        public async Task<IList<PostData>> GetAllPostOfFriendNoti(int userid)
        {
            var friends = _friendRepository.GetAllFriendRelationshipsOfUser(userid, true);
            var listFriendId = new List<int>();

            foreach (var item in friends)
            {
                if (item.User1Id == (userid == -1 ? _currentUserId : userid))
                {
                    listFriendId.Add(item.User2Id);
                }
                else
                {
                    listFriendId.Add(item.User1Id);
                }
            }
            var postList = _postRepository.GetAllPostAsync(listFriendId);
            return await postList;
        }

        public List<User> GetUpComingBirthdayFriends()
        {
            var listFriends = GetAllFriendsOfUser();
            listFriends = listFriends.Where(x => IsUpComingBirthday(x.Dob)).ToList();
            return listFriends;
        }

        private bool IsUpComingBirthday(DateTime? dob)
        {
            var today = DateTime.Now;
            var oneMonthFromToday = today.AddMonths(1);
            var birthday = (DateTime)dob;
            if (birthday.Month == today.Month && birthday.Day >= today.Day)
            {
                return true;
            }

            if (birthday.Month == oneMonthFromToday.Month && birthday.Day <= oneMonthFromToday.Day)
            {
                return true;
            }

            return false;
        }

        public async Task<IList<MessageData>> GetUserChatWith()
        {
            var listData = new List<MessageData>();

            // Fetch all messages for the current user
            var listMessage = await _messageRepository.GetAllMessage(_currentUserId); // Assuming async method

            // Group messages by SenderId and ReceiverId, then select the most recent message in each group
            var listMessageLater = listMessage
                .GroupBy(x => new { x.SenderId, x.ReceiverId })
                .Select(g => g.OrderByDescending(m => m.Time).FirstOrDefault())
                .Select(m => new MessageData
                {
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Message = m.Message,
                    Time = m.Time,
                    IsSendedByUser = m.IsSendedByUser,
                    Readed = m.Readed
                })
                .ToList();

            // Add the messages to the final listData and their reverse counterparts
            listData.AddRange(listMessageLater);
            listMessageLater.ForEach(item =>
            {
                var message = new MessageData
                {
                    SenderId = item.ReceiverId,
                    ReceiverId = item.SenderId,
                    Message = item.Message,
                    Time = item.Time,
                    IsSendedByUser = item.IsSendedByUser,
                    Readed = item.Readed
                };
                listData.Add(message);
            });

            // Filter messages where the current user is the sender
            listData = listData.Where(x => x.SenderId == _currentUserId).ToList();

            // Group again and select the most recent message in each group
            listData = listData
                .GroupBy(x => new { x.SenderId, x.ReceiverId })
                .Select(g => g.OrderByDescending(m => m.Time).FirstOrDefault())
                .OrderByDescending(x => x.Time)
                .ToList();

            // Add user details if there are messages
            if (listData.Count > 0)
            {
                var listUser = await _messageRepository.GetUsers(); // Assuming async method
                listData.ForEach(item =>
                {
                    var user = listUser.FirstOrDefault(x => x.UserId == item.ReceiverId);
                    if (user != null)
                    {
                        item.PhotoURL = user.ProfilePhotoUrl;
                        item.ReceiverName = user.Fullname;
                    }
                });
            }

            return await Task.FromResult(listData);
        }

    }
}

