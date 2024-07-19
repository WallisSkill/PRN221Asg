toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": false,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "2000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

function showToast(message, icon = 'success', userid) {
    toastr.options.onclick = function () { userid != null ? redirectToProfile(userid) : e.preventDefault() };
    toastr[icon](message);
}


///Other
function getUserFromData(dataArray, userObj) {
    if (userObj && userObj.UserId) {
        return userObj;
    } else if (userObj && userObj.$ref) {
        return dataArray.find(item => item.User && item.User.$id === userObj.$ref)?.User;
    }
    return null;
}

function getUserFromDataReply(dataArray, userObj) {
    if (userObj && userObj.UserId) {
        return userObj;
    } else if (userObj && userObj.$ref) {
        return dataArray.find(item => item.CommentReply.User && item.CommentReply.User.$id === userObj.$ref)?.User;
    }
    return null;
}

//fetch Notifications
async function fetchFriendRequestOtherNoti(userId = CURRENT_USER_ID) {
    const response = await fetch(`/Index?handler=GetRequestsForNotification&&userid=${userId}`);
    const data = await response.json();
    //data
    const friendRequests = data.Friends;
    const posts = data.Posts;
    const postLikes = data.PostLikes;
    const commentLikes = data.CommentLikes;
    const comments = data.Comments;
    const commentReplies = data.CommentReplies;
    //
    const container = document.querySelector('.noti-list');
    container.innerHTML = ''; 

    const notifications = [];
/////////////////////
    friendRequests.forEach(request => {
        notifications.push({
            type: 'friendRequest',
            time: new Date(request.CreatedTime),
            data: request
        });
    });

    posts.forEach(post => {
        notifications.push({
            type: 'post',
            time: new Date(post.Time),
            data: post
        });
    });

    postLikes.forEach(like => {
        notifications.push({
            type: 'postLike',
            time: new Date(like.createDate),
            data: like
        });
    });

    commentLikes.forEach(like => {
        notifications.push({
            type: 'commentLike',
            time: new Date(like.CommentLike.createDate),
            data: like
        });
    });

    comments.forEach(comment => {
        notifications.push({
            type: 'comment',
            time: new Date(comment.Time),
            data: comment
        });
    });

    commentReplies.forEach(reply => {
        notifications.push({
            type: 'commentReply',
            time: new Date(reply.CommentReply.Time),
            data: reply
        });
    });
////////////////////
    notifications.sort((a, b) => b.time - a.time);

    notifications.forEach(notification => {
        if (notification.type === 'friendRequest') {
            const request = notification.data;
            const requestDiv = document.createElement('a');
            requestDiv.href = `/Profile?Id=${request.User.UserId}`;
            requestDiv.className = "iq-sub-card";
            requestDiv.innerHTML = `
                <div class="d-flex align-items-center">
                    <div class="">
                        <img class="avatar-40 rounded" src="${request.User.ProfilePhotoUrl}" alt="">
                    </div>
                    <div class="ms-3 w-100">
                        <h6 class="mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}">${request.User.Fullname}</h6>
                        <div class="d-flex justify-content-between align-items-center">
                            <p class="errortext mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}">accepted your friend request</p>
                            <small class="float-right font-size-12">${getTimeFromMessage(request.CreatedTime)}</small>
                        </div>
                    </div>
                </div>
            `;
            container.appendChild(requestDiv);
        }
        else if (notification.type === 'post') {
            const post = notification.data;
            const postDiv = document.createElement('a');
            postDiv.href = `/Profile?Id=${post.User.UserId}&&postId=${post.Id}`;
            postDiv.className = "iq-sub-card";
            postDiv.innerHTML = `
                <div class="d-flex align-items-center">
                    <div class="">
                        <img class="avatar-40 rounded" src="${post.User.ProfilePhotoUrl}" alt="">
                    </div>
                    <div class="ms-3 w-100">
                        <h6 class="mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}">${post.User.Fullname}</h6>
                        <div class="d-flex justify-content-between align-items-center">
                            <p class="errortext mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}">${post.Caption != null ? 'Add new Post' : 'Add new Picture'}</p>
                            <small class="float-right font-size-12">${getTimeFromMessage(post.Time)}</small>
                        </div>
                    </div>
                </div>
            `;
            container.appendChild(postDiv);
        }
        else if (notification.type === 'postLike') {
            const like = notification.data;

            let user;

            if (like.User && like.User.UserId) {
                user = like.User;
            }
            else if (like.User && like.User.$ref) {
                user = postLikes.find(pl => pl.User && pl.User.$id === like.User.$ref)?.User;
            }

            const likeDiv = document.createElement('a');
            likeDiv.href = `/Profile?Id=${CURRENT_USER_ID}&&postId=${like.ConnectId}`;
            likeDiv.className = "iq-sub-card";
            likeDiv.innerHTML = `
                <div class="d-flex align-items-center">
                    <div class="">
                        <img class="avatar-40 rounded" src="${user.ProfilePhotoUrl}" alt="">
                    </div>
                    <div class="ms-3 w-100">
                        <h6 class="mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}">${user.Fullname}</h6>
                        <div class="d-flex justify-content-between align-items-center">
                            <p class="errortext mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}"> react your post</p>
                            <small class="float-right font-size-12">${getTimeFromMessage(like.createDate)}</small>
                        </div>
                    </div>
                </div>
            `;
            container.appendChild(likeDiv);
        }
        else if (notification.type === 'comment') {
            const comment = notification.data;
            let user = getUserFromData(comments, comment.User) ?? getUserFromDataReply(commentReplies, comment.User);

            if (user) {
                const commentDiv = document.createElement('a');
                commentDiv.href = `/Profile?Id=${CURRENT_USER_ID}&&commentId=${comment.CommentId}`;
                commentDiv.className = "iq-sub-card";
                commentDiv.innerHTML = `
                    <div class="d-flex align-items-center">
                        <div class="">
                            <img class="avatar-40 rounded" src="${user.ProfilePhotoUrl}" alt="">
                        </div>
                        <div class="ms-3 w-100">
                            <h6 class="mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}">${user.Fullname}</h6>
                            <div class="d-flex justify-content-between align-items-center">
                                <p class="errortext mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}"> commented on your post</p>
                                <small class="float-right font-size-12">${getTimeFromMessage(comment.Time)}</small>
                            </div>
                        </div>
                    </div>
                `;
                container.appendChild(commentDiv);
            }
        }
        else if (notification.type === 'commentReply') {
            const reply = notification.data;
            let user = getUserFromDataReply(commentReplies, reply.CommentReply.User) ?? getUserFromData(comments, reply.CommentReply.User) ;

            if (user) {
                const replyDiv = document.createElement('a');
                replyDiv.href = `/Profile?Id=${reply.UserId}&&commentId=${reply.CommentReply.CommentId}`;
                replyDiv.className = "iq-sub-card";
                replyDiv.innerHTML = `
                    <div class="d-flex align-items-center">
                        <div class="">
                            <img class="avatar-40 rounded" src="${user.ProfilePhotoUrl}" alt="">
                        </div>
                        <div class="ms-3 w-100">
                            <h6 class="mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}">${user.Fullname}</h6>
                            <div class="d-flex justify-content-between align-items-center">
                                <p class="errortext mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}"> replied to your comment</p>
                                <small class="float-right font-size-12">${getTimeFromMessage(reply.CommentReply.Time)}</small>
                            </div>
                        </div>
                    </div>
                `;
                container.appendChild(replyDiv);
            }
        }
        else if (notification.type === 'commentLike') {
            const like = notification.data;
            const likeDiv = document.createElement('a');
            likeDiv.href = `/Profile?Id=${like.UserId}&&commentId=${like.CommentLike.ConnectId}`;
            likeDiv.className = "iq-sub-card";
            likeDiv.innerHTML = `
                <div class="d-flex align-items-center">
                    <div class="">
                        <img class="avatar-40 rounded" src="${like.CommentLike.User.ProfilePhotoUrl}" alt="">
                    </div>
                    <div class="ms-3 w-100">
                        <h6 class="mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}">${like.CommentLike.User.Fullname}</h6>
                        <div class="d-flex justify-content-between align-items-center">
                            <p class="errortext mb-0 ${document.body.classList.contains('bg-dark') ? 'text-white' : ''}"> reacted your comment on a post</p>
                            <small class="float-right font-size-12">${getTimeFromMessage(like.CommentLike.createDate)}</small>
                        </div>
                    </div>
                </div>
            `;
            container.appendChild(likeDiv);
        }
    });

    if (notifications.length === 0) {
        const noRequestsMessage = document.createElement('div');
        noRequestsMessage.textContent = 'No notifications to display';
        noRequestsMessage.classList.add("text-center");
        container.appendChild(noRequestsMessage);
    }
    updateNotificationBadgeG();
}
///Current
async function fetchFriendRequests() {
    const response = await fetch('/Index?handler=GetRequests');
    const friendRequests = await response.json();
    const container = document.querySelector('.request-list');
    container.innerHTML = ''; // Clear existing content
    const count = friendRequests.$values.length;
    const truecount = friendRequests.$values.filter(request => !request.Status).length;
    document.querySelector(".req-count").innerHTML = truecount;
    document.querySelector(".request-noti").style.display = '';
    friendRequests.$values.forEach(request => {
        const requestDiv = document.createElement('div');
        requestDiv.className = 'iq-friend-request';
        requestDiv.innerHTML = `
                    <div class="iq-sub-card iq-sub-card-big d-flex align-items-center justify-content-between">
                        <div class="d-flex align-items-center">
                            <img class="avatar-40 rounded" src="${request.User.ProfilePhotoUrl}" alt="">
                            <div class="ms-3">
                                <h6 onclick="redirectToProfile(${request.User.UserId})" class="clickable mb-0 ${body.classList.contains('bg-dark') ? 'text-white' : ''}">${request.User.Fullname}</h6>
                            </div>
                        </div>
                        <div class="d-flex align-items-center">
                           <div>You accepted</div>
                        </div>
                    </div>
                `;
        if (!request.Status) {
            requestDiv.innerHTML = `
                    <div class="iq-sub-card iq-sub-card-big d-flex align-items-center justify-content-between">
                        <div class="d-flex align-items-center">
                            <img class="avatar-40 rounded" src="${request.User.ProfilePhotoUrl}" alt="">
                            <div class="ms-3">
                                <h6 onclick="redirectToProfile(${request.User.UserId})" class="clickable mb-0 ${body.classList.contains('bg-dark') ? 'text-white' : ''}">${request.User.Fullname}</h6>
                            </div>
                        </div>
                        <div class="d-flex align-items-center">
                            <a data-user-id="${request.User.UserId}" class="me-3 btn btn-primary rounded accept-friend">Confirm</a>
                            <a data-user-id="${request.User.UserId}" data-user-name="${request.User.Fullname}" class="me-3 btn btn-secondary rounded cancel-friend">Cancel Request</a>
                        </div>
                    </div>
                `;
        }
        container.appendChild(requestDiv);
    });
    if (truecount == 0) document.querySelector(".request-noti").style.display = 'none';
    if (count === 0) {
        const noRequestsMessage = document.createElement('div');
        noRequestsMessage.textContent = 'No friend requests to display';
        noRequestsMessage.classList.add("text-center");
        container.appendChild(noRequestsMessage);
    }
    updateNotificationBadgeM();
}

// Call the fetchFriendRequests function when the page loads
document.addEventListener('DOMContentLoaded', fetchFriendRequests);

connection.on("ReceiveMessage", (user, message, name) => {
    fetchListMess();
    const chatBox = document.getElementById(`chatBox-${CURRENT_USER_ID}-${user}`);
    if (chatBox) {
        const messagesList = chatBox.querySelector('.chat-messages');
        const messageContainer = document.createElement('div'); // Create a container for the message
        messageContainer.classList.add('message-container'); // Add a class to the container
        const msg = document.createElement('div'); // Create the message element
        msg.innerHTML = message.replaceAll(/\n/g, '<br>'); // Set the message text
        msg.classList.add('incoming-message'); // Add a class to style the message
        messageContainer.appendChild(msg); // Append the message to the container
        messagesList.appendChild(messageContainer); // Append the container to the messages list

        // Scroll to the bottom
        messagesList.scrollTop = messagesList.scrollHeight;
    }
    else {
        if (CURRENT_USER_ID != user) {
            openChat(user, name, false);
        }
    }
});

connection.on("ReceiveFriendRequest", function (userId, friendUserId, name) {
    showToast(`${name} just send you friend request.`, 'info', userId);
    fetchFriendRequests().then(() => updateNotificationBadgeM());
});

connection.on("AcceptFriendRequest", function (userId, friendUserId, name) {
    showToast(`${name} has accepted your friend request.`, `success`, userId);
    fetchFriendRequests().then(() => updateNotificationBadgeM());
    fetchFriendRequestOtherNoti().then(() => updateNotificationBadgeG());
});

connection.on("NewNoti", function (userId,type, name) {
    if (type == '1') showToast(`${name} has added new post.`, `info`, userId);
    if (type == '2') showToast(`${name} has reacted to your post.`, `info`, userId);
    if (type == '3') showToast(`${name} has comment to your post.`, `info`, userId);
    if (type == '4') showToast(`${name} has reply to your comment.`, `info`, userId);
    if (type == '5') showToast(`${name} has liked your comment.`, `info`, userId);
    fetchFriendRequestOtherNoti().then(() => updateNotificationBadgeG());
});

function handleAddFriend(button) {
    const userId = `${CURRENT_USER_ID}`;
    const friendUserId = button.getAttribute('data-user-id');
    const fullname = button.getAttribute('data-user-name');
    console.log(userId + "-" + friendUserId);

    connection.invoke("SendFriendRequest", userId, friendUserId, fullname)
        .catch(function (err) {
            return console.error(err.toString());
        });

    // Change the button to "Pending"
    button.outerHTML = `
        <li class="text-center" style="list-style: none;">
            <div class="dropdown">
                <span class="dropdown-toggle btn btn-warning me-2 ${body.classList.contains('bg-dark') ? 'text-white' : ''}" id="dropdownMenuButton02" data-bs-toggle="dropdown" aria-expanded="true" role="button">
                    <i class="clickable ri-time-line me-1 ${body.classList.contains('bg-dark') ? 'text-white' : ''}"></i> Pending
                </span>
                <div class="dropdown-menu dropdown-menu-right ${body.classList.contains('bg-dark') ? 'text-white' : ''}" aria-labelledby="dropdownMenuButton02">
                    <a data-user-id="${friendUserId}" data-user-name="${fullname}" class="dropdown-item cancel-friend clickable" class="dropdown-item">Cancel Pending</a>
                </div>
            </div>
        </li>
    `;
}

function handleCancelFriend(button) {
    const userId = `${CURRENT_USER_ID}`;
    const friendUserId = button.getAttribute('data-user-id');
    const fullname = button.getAttribute('data-user-name');
    connection.invoke("CancelFriendRequest", userId, friendUserId)
        .catch(function (err) {
            return console.error(err.toString());
        });
    console.log("meow");
    // Change the button to "Pending"
    if (button.closest(".text-center")) {
        button.closest(".text-center").outerHTML = `
                    <span data-user-id="${friendUserId}" data-user-name="${fullname}"  class="btn btn-primary me-2 add-friend" role="button">
                         <i class="ri-add-line me-1 text-white"></i>Add&nbspFriend
                    </span>
                `;
    }

    if (button.closest("#pendinger")) {
        button.closest("#pendinger").outerHTML = `
                                 <span data-user-id="${friendUserId}" data-user-name="${fullname}"  class="btn btn-primary me-2 add-friend" role="button">
                                     <i class="ri-add-line me-1 text-white"></i>Add&nbspFriend
                                </span>
                                        `;
    }
    fetchFriendRequests();
    fetchFriendRequests();
}

function handleAcceptFriend(button) {
    const userId = `${CURRENT_USER_ID}`;
    const friendUserId = button.getAttribute('data-user-id');
    const fullname = `${CURRENT_USER_NAME}`;

    connection.invoke("AcceptFriendRequest", userId, friendUserId, fullname)
        .catch(function (err) {
            return console.error(err.toString());
        });

    // Change the button to "Pending"
    if (button.closest(".text-center")) {
        button.closest(".text-center").innerHTML = `
                    <div class="dropdown">
                        <span class="dropdown-toggle btn btn-secondary me-2" id="dropdownMenuButton03" data-bs-toggle="dropdown" aria-expanded="true" role="button">
                            <i class="ri-check-line me-1 text-white"></i> Friend
                        </span>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton03">
                            <a data-user-id="${userId}" data-user-name="${fullname}" class="dropdown-item clickable cancel-friend">Unfriend</a>
                        </div>
                    </div>
                `;
    }
    if (button.closest("#pendinger")) {
        button.closest("#pendinger").innerHTML = `
                                <div class="dropdown">
                                    <span class="dropdown-toggle btn btn-secondary me-2" id="dropdownMenuButton03" data-bs-toggle="dropdown" aria-expanded="true" role="button">
                                        <i class="ri-check-line me-1 text-white"></i> Friend
                                    </span>
                                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton03">
                                        <a data-user-id="${userId}" data-user-name="${fullname}" class="dropdown-item clickable cancel-friend">Unfriend</a>
                                    </div>
                                </div>
                            `;
    }

    fetchFriendRequests().then(() => updateNotificationBadgeM());
    fetchFriendRequests();
}

function handleAcceptFriend(button) {
    const userId = `${CURRENT_USER_ID}`;
    const friendUserId = button.getAttribute('data-user-id');
    const fullname = `${CURRENT_USER_NAME}`;

    connection.invoke("AcceptFriendRequest", userId, friendUserId, fullname)
        .catch(function (err) {
            return console.error(err.toString());
        });

    // Change the button to "Pending"
    if (button.closest(".text-center")) {
        button.closest(".text-center").innerHTML = `
                    <div class="dropdown">
                        <span class="dropdown-toggle btn btn-secondary me-2" id="dropdownMenuButton03" data-bs-toggle="dropdown" aria-expanded="true" role="button">
                            <i class="ri-check-line me-1 text-white"></i> Friend
                        </span>
                        <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton03">
                            <a data-user-id="${userId}" data-user-name="${fullname}" class="dropdown-item clickable cancel-friend">Unfriend</a>
                        </div>
                    </div>
                `;
    }
    if (button.closest("#pendinger")) {
        button.closest("#pendinger").innerHTML = `
                                <div class="dropdown">
                                    <span class="dropdown-toggle btn btn-secondary me-2" id="dropdownMenuButton03" data-bs-toggle="dropdown" aria-expanded="true" role="button">
                                        <i class="ri-check-line me-1 text-white"></i> Friend
                                    </span>
                                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenuButton03">
                                        <a data-user-id="${userId}" data-user-name="${fullname}" class="dropdown-item clickable cancel-friend">Unfriend</a>
                                    </div>
                                </div>
                            `;
    }

    fetchFriendRequests().then(() => updateNotificationBadgeM());
    fetchFriendRequests();
}

function handleAcceptFriendSearch(button) {
    const userId = `${CURRENT_USER_ID}`;
    const friendUserId = button.getAttribute('data-user-id');
    const fullname = `${CURRENT_USER_NAME}`;

    connection.invoke("AcceptFriendRequest", userId, friendUserId, fullname)
        .catch(function (err) {
            return console.error(err.toString());
        });

    // Change the button to "Pending"
    if (button.closest(".text-center")) {
        button.closest(".text-center").innerHTML = `
                    <div class="">
                        <span onclick="openChat('@u.UserId','@u.Fullname')" class="btn btn-primary me-2" role="button">
                             <i class="ri-message-line me-1 text-white"></i>Message
                        </span>
                    </div>
                `;
    }
    if (button.closest("#pendinger")) {
        button.closest("#pendinger").innerHTML = `
                    <div class="">
                        <span onclick="openChat('@u.UserId','@u.Fullname')" class="btn btn-primary me-2" role="button">
                              <i class="ri-message-line me-1 text-white"></i>Message
                        </span>
                    </div>
                 `;
    }

    fetchFriendRequests().then(() => updateNotificationBadgeM());
    fetchFriendRequests();
}
////Status friend
function moveUserToTop(userId) {
    const friendElement = document.getElementById(`friend-${userId}`);
    if (friendElement) {
        const parentElement = friendElement.parentElement;
        parentElement.removeChild(friendElement);
        parentElement.insertBefore(friendElement, parentElement.firstChild);
    }
}


connection.on("UserConnected", function (userId, username) {
    const friendElement = document.getElementById(`friend-${userId}`);
    if (friendElement) {
        friendElement.querySelector(".iq-profile-avatar").classList.remove("status-offline");
        friendElement.querySelector(".iq-profile-avatar").classList.add("status-online");
        moveUserToTop(userId); // Di chuyển người dùng lên đầu danh sách
    }
});

connection.on("UserDisconnected", function (userId) {
    const friendElement = document.getElementById(`friend-${userId}`);
    if (friendElement) {
        friendElement.querySelector(".iq-profile-avatar").classList.remove("status-online");
        friendElement.querySelector(".iq-profile-avatar").classList.add("status-offline");
        moveUserToBottom(userId);
    }
});

connection.on("ReceiveOnlineUsers", function (onlineUsers) {
    onlineUsers.forEach(function (userId) {
        const friendElement = document.getElementById(`friend-${userId}`);
        if (friendElement) {
            friendElement.querySelector(".iq-profile-avatar").classList.remove("status-offline");
            friendElement.querySelector(".iq-profile-avatar").classList.add("status-online");
            moveUserToTop(userId); // Di chuyển người dùng lên đầu danh sách
        }
    });
});

function moveUserToBottom(userId) {
    const friendElement = document.getElementById(`friend-${userId}`);
    if (friendElement) {
        const parentElement = friendElement.parentElement;
        parentElement.removeChild(friendElement);
        parentElement.appendChild(friendElement);
    }
}