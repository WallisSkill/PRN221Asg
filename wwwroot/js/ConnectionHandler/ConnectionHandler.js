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
async function fetchFriendRequestOtherNoti(userId = CURRENT_USER_ID) {
    
    const response = await fetch(`/Index?handler=GetRequestsForNotification&&userid=${userId}`);
    const friendRequests = await response.json();
    const container = document.querySelector('.noti-list');
    container.innerHTML = ''; // Clear existing content
    const count = friendRequests.$values.length;
    document.querySelector(".noti-noti").style.display = '';
    friendRequests.$values.forEach(request => {
        const requestDiv = document.createElement('a');
        requestDiv.href = `/Profile?Id=${request.User.UserId}`;
        requestDiv.className = "iq-sub-card";
        requestDiv.innerHTML = `
                            <div class="d-flex align-items-center">
                                <div class="">
                                    <img class="avatar-40 rounded" src="${request.User.ProfilePhotoUrl}" alt="">
                                </div>
                                <div class="ms-3 w-100">
                                    <h6 class="mb-0 ${body.classList.contains('bg-dark') ? 'text-white' : ''}">${request.User.Fullname}</h6>
                                    <div class="d-flex justify-content-between align-items-center">
                                        <p class="errortext mb-0 ${body.classList.contains('bg-dark') ? 'text-white' : ''}">accepted your friend request</p>
                                        <small class="float-right font-size-12">${getTimeFromMessage(request.CreatedTime)}</small>
                                    </div>
                                </div>
                            </div>
                `;
        container.appendChild(requestDiv);
    })
    if (count === 0) {
        const noRequestsMessage = document.createElement('div');
        noRequestsMessage.textContent = 'No notifications to display';
        noRequestsMessage.classList.add("text-center");
        container.appendChild(noRequestsMessage);
    }
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
////Status friend
connection.on("UserConnected", function (userId, username) {
    const friendElement = document.getElementById(`friend-${userId}`);
    if (friendElement) {
        friendElement.querySelector(".iq-profile-avatar").classList.remove("status-offline");
        friendElement.querySelector(".iq-profile-avatar").classList.add("status-online");
    }
});

connection.on("UserDisconnected", function (userId) {
    const friendElement = document.getElementById(`friend-${userId}`);
    if (friendElement) {
        friendElement.querySelector(".iq-profile-avatar").classList.remove("status-online");
        friendElement.querySelector(".iq-profile-avatar").classList.add("status-offline");
    }
});

connection.on("ReceiveOnlineUsers", function (onlineUsers) {
    onlineUsers.forEach(function (userId) {
        const friendElement = document.getElementById(`friend-${userId}`);
        if (friendElement) {
            friendElement.querySelector(".iq-profile-avatar").classList.remove("status-offline");
            friendElement.querySelector(".iq-profile-avatar").classList.add("status-online");
        }
    });
});