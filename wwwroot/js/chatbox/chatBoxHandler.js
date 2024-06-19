


let chatOffset = 250;
const openChatBoxes = [];

function openChat(userId, name, open = false) {
    let chatBox = document.getElementById(`chatBox-${CURRENT_USER_ID}-${userId}`);
    const chatContainer = document.getElementById('chatContainer');
    if (!chatBox) {
        if (openChatBoxes.length >= 3) {
            const oldestChatBoxId = openChatBoxes.shift();
            closeChat(oldestChatBoxId);
        }
        chatBox = createChatBox(userId, name);
        const chatBoxes = chatContainer.querySelectorAll('.chat-box');
        const totalWidth = Array.from(chatBoxes).reduce((acc, box) => acc + box.offsetWidth, 0);

        chatBox.style.right = `${totalWidth + chatOffset}px`;

        const index = chatBoxes.length + 1;
        const marginRight = index * 10;

        chatBox.style.marginRight = `${marginRight}px`;
        fetchMessages(userId, chatBox);
        chatContainer.appendChild(chatBox);
        openChatBoxes.push(userId);
    }
    if (open) {
        const inputField = chatBox.querySelector(`#chatinput-${userId}`);
        inputField.focus();
        fetchMessages(userId, chatBox, true);
    }
    fetchListMess();
}


async function fetchMessages(userId, chatBox, open = true) {
    try {
        const response = await fetch(`/Index?handler=GetMessages&senderId=${CURRENT_USER_ID}&receiverId=${userId}&open=${open}`);
        if (response.ok) {
            const messages = await response.json();
            displayMessages(messages, chatBox, open);
        } else {
            console.error('Failed to fetch messages');
        }
    } catch (error) {
        console.error('Error fetching messages:', error);
    }
}


function displayMessages(messages, chatBox, open = true) {
    const messagesList = chatBox.querySelector('.chat-messages');
    messagesList.innerHTML = ''; // Clear existing messages

    const startMessage = document.createElement('div');
    startMessage.classList.add('conversation-start');
    startMessage.textContent = '--- This is the start of this conversation ---';
    messagesList.appendChild(startMessage);

    messages.forEach(message => {
        const messageContainer = document.createElement('div');
        messageContainer.classList.add('message-container');

        const msg = document.createElement('div');
        msg.innerHTML = message.content.replaceAll(/\n/g, '<br>');
        msg.classList.add(message.senderId === parseInt(`${CURRENT_USER_ID}`) ? 'outgoing-message' : 'incoming-message');
        messageContainer.appendChild(msg);
        messagesList.appendChild(messageContainer);
    });
    if (open) {
        messagesList.scrollTop = messagesList.scrollHeight;
    }
}


async function fetchListMess() {
    try {
        const response = await fetch(`/Index?handler=GetMessagesNoti`);
        if (response.ok) {
            const listMess = await response.json();
            displayListMess(listMess);
            updateNotificationBadge(listMess);
        } else {
            console.error('Failed to fetch listMess');
        }

    } catch (error) {
        console.error('Error fetching listMess:', error);
    }
}

function displayListMess(listMess) {
    const chatContainer = document.querySelector('.chat-noti');
    chatContainer.innerHTML = '';

    if (listMess && listMess.length > 0) {
        listMess.forEach(messData => {
            const chatItem = document.createElement('a');
            chatItem.href = '#';
            chatItem.className = 'iq-sub-card';
            chatItem.onclick = () => {
                openChat(messData.receiverId, messData.receiverName, true)
                ; openChat(messData.receiverId, messData.receiverName, true);
            };

            const chatContent = `
                    <div class="d-flex align-items-center">
                        <div>
                            <img class="avatar-40 rounded" src="${messData.photoURL}" alt="">
                        </div>
                        <div class="w-100 ms-3">
                            <h6 class="mb-0 ${body.classList.contains('bg-dark') ? 'text-white' : ''}">${messData.receiverName}</h6>
                            <small class="float-left font-size-12">
                                ${messData.isSendedByUser
                    ? `<div>You: ${limitContent(messData.message)} ∙ ${getTimeFromMessage(messData.time)}</div>`
                    : `<div style="${!messData.readed ? 'font-weight: bold' : ''}">${limitContent(messData.message)} ∙ ${getTimeFromMessage(messData.time)}</div>`
                }
                            </small>
                        </div>
                    </div>
                `;

            chatItem.innerHTML = chatContent;
            chatContainer.appendChild(chatItem);
        });
    } else {
        chatContainer.innerHTML = '<div class="text-center">You don\'t have any messages!<br /> Let\'s chat with a friend.</div>';
    }
}

function createChatBox(userId, name) {
    const chatBox = document.createElement('div');
    chatBox.id = `chatBox-${CURRENT_USER_ID}-${userId}`;
    chatBox.className = 'chat-box';
    const currid = '@User.FindFirstValue(ClaimTypes.NameIdentifier)';
    // Add 'bg-dark' class if body has 'bg-dark' class
    if (body.classList.contains('bg-dark')) {
        chatBox.classList.add('bg-dark');
    }

    chatBox.innerHTML = `
                <div class="chat-header ${body.classList.contains('bg-dark') ? 'bg-dark' : ''}">
                    <span class="clickable" onclick="redirectToProfile(${userId})">${name}</span>
                    <button class="close-button" onclick="closeChat('${userId}')">×</button>
                </div>
                <div class="chat-messages ${body.classList.contains('bg-dark') ? 'bg-dark' : ''}">
                </div>
                <div class="chat-input ${body.classList.contains('bg-dark') ? 'bg-dark' : ''}">
                    <div class="input-container">
                <textarea oninput="autoResizeTextarea(this);" autocomplete="false" id="chatinput-${userId}" placeholder="Type a message..." onclick="openChat('${userId}','${name}',true);" onkeydown="if (event.key === 'Enter') handleKeyDown(event, '${userId}',this)"></textarea>
                        <button class="emoji-button ${body.classList.contains('bg-dark') ? '' : 'text-dark'}" onclick="toggleEmojiPicker('${userId}')">☺️</button>
                    </div>
                    <div id="emoji-picker-${userId}" class="emoji-picker-container ${body.classList.contains('bg-dark') ? 'bg-dark' : ''}">
                    </div>
                    <button onclick="sendMessage('${userId}');">Send</button>
                </div>
        `;
    // Apply dark mode styles to the input element if dark mode is active
    if (body.classList.contains('bg-dark')) {
        chatBox.classList.add('bg-dark');
        chatBox.querySelector('textarea').classList.add('bg-dark');
        chatBox.style.borderColor = '#555';
        // Set cursor color to white
        chatBox.querySelector('textarea').classList.add('text-white');
    } else {
        chatBox.style.borderColor = '#ccc';
    }
    return chatBox;
}

function autoResizeTextarea(textarea) {
    // Set the textarea height to auto to calculate the natural height
    textarea.style.height = '50px';

    // Set the maximum height of the textarea to accommodate 5 rows
    const maxHeight = 5 * parseFloat(getComputedStyle(textarea).lineHeight);

    // Set the height to either the scroll height or the maximum height, whichever is smaller
    textarea.style.height = Math.min(textarea.scrollHeight, maxHeight) + 'px';

    // Scroll to the bottom to show new content
    textarea.scrollTop = textarea.scrollHeight;
}
/////////Emoji
// Function to close all emoji pickers
function closeAllEmojiPickers() {
    const pickers = document.querySelectorAll('.emoji-picker-container');
    pickers.forEach(picker => {
        picker.style.display = 'none';
    });
    document.removeEventListener('click', handleClickOutsideEmojiPicker);
}

// Function to toggle the emoji picker for a specific chat box
function toggleEmojiPicker(userId) {
    const picker = document.getElementById(`emoji-picker-${userId}`);

    // Close all other emoji pickers first
    closeAllEmojiPickers();

    // Clear and create the search input
    picker.innerHTML = '';
    const emojiSearchInput = document.createElement('input');
    emojiSearchInput.id = `emojiSearch-${userId}`;
    emojiSearchInput.className = `search-emoji ${document.body.classList.contains('bg-dark') ? 'bg-dark text-white' : ''}`;
    emojiSearchInput.type = 'text';
    emojiSearchInput.placeholder = 'Search emojis...';
    emojiSearchInput.style.width = '100%';
    emojiSearchInput.addEventListener('keyup', () => fetchEmojis(userId, emojiSearchInput.value));
    picker.appendChild(emojiSearchInput);

    // Create the container for emojis
    const emojiContainer = document.createElement('div');
    emojiContainer.className = `picker-emoji-container-${userId}`;
    picker.appendChild(emojiContainer);

    // Toggle display and event listener for click outside
    if (picker.style.display === 'block') {
        picker.style.display = 'none';
        document.removeEventListener('click', handleClickOutsideEmojiPicker);
    } else {
        picker.style.display = 'block';
        fetchEmojis(userId, '');
        setTimeout(() => {
            document.addEventListener('click', handleClickOutsideEmojiPicker);
        }, 0);
    }
}

// Function to fetch emojis and display them
function fetchEmojis(userId, search) {
    const picker = document.querySelector(`.picker-emoji-container-${userId}`);
    picker.innerHTML = '';

    const loader = document.createElement('div');
    loader.className = 'loader-emoji';
    loader.textContent = 'Loading...';
    picker.appendChild(loader);

    fetch(`https://emoji-api.com/emojis?search=${search}&access_key=ed5b51ac5f79b6156b30ae79891eb9bb0af2326c`)
        .then(response => response.json())
        .then(data => {
            picker.removeChild(loader);
            data.forEach(emoji => {
                const emojiSpan = document.createElement('span');
                emojiSpan.className = 'emoji-item';
                emojiSpan.innerText = emoji.character;
                emojiSpan.setAttribute('emoji-name', emoji.slug);
                emojiSpan.onclick = () => addEmojiToInput(userId, emoji.character);
                picker.appendChild(emojiSpan);
            });
        })
        .catch(error => {
            console.error('Error fetching emojis:', error);
            picker.removeChild(loader);
            const errorMessage = document.createElement('div');
            errorMessage.className = 'error-message';
            errorMessage.textContent = 'Failed to load emojis.';
            picker.appendChild(errorMessage);
        });
}

// Function to add the selected emoji to the input field
function addEmojiToInput(userId, emoji) {
    const input = document.getElementById(`chatinput-${userId}`);
    input.value += emoji;
}

// Function to handle clicks outside the emoji picker
function handleClickOutsideEmojiPicker(event) {
    const pickers = document.querySelectorAll('.emoji-picker-container');
    pickers.forEach(picker => {
        if (!picker.contains(event.target) && !event.target.classList.contains('emoji-button')) {
            picker.style.display = 'none';
        }
    });

    // Remove event listener if all pickers are closed
    const anyPickerOpen = Array.from(pickers).some(picker => picker.style.display === 'block');
    if (!anyPickerOpen) {
        document.removeEventListener('click', handleClickOutsideEmojiPicker);
    }
}
///Emoji



function closeChat(userId) {
    const chatBox = document.getElementById(`chatBox-${CURRENT_USER_ID}-${userId}`);
    if (chatBox) {
        const offset = parseInt(chatBox.style.right, 10);
        const boxWidth = chatBox.offsetWidth;
        const deletedMargin = parseInt(chatBox.style.marginRight, 10);
        chatBox.remove();

        // After removing the chat box, adjust the position and margin of the remaining chat boxes
        const chatBoxes = document.querySelectorAll('.chat-box');
        chatBoxes.forEach(box => {
            const currentOffset = parseInt(box.style.right, 10);
            const currentMargin = parseInt(box.style.marginRight, 10);
            const boxWidthDiff = currentOffset > offset ? boxWidth : 0; // If the box is to the right of the removed box, subtract its width
            if (currentMargin > deletedMargin) {
                box.style.right = `${currentOffset - boxWidthDiff}px`;
                box.style.marginRight = `${currentMargin - 10}px`; // Decrease margin-right by 10
            }
        });
    }
}



function updateNotificationBadge(listMess) {
    const unreadCount = listMess.filter(mess => !mess.readed).length;
    const notificationBadge = document.querySelector('.message-noti');

    if (unreadCount > 0) {
        notificationBadge.textContent = unreadCount;
        notificationBadge.style.display = '';
    } else {
        notificationBadge.style.display = 'none';
    }
    const unrep = document.querySelector(".unrep");
    unrep.textContent = "Unread: " + unreadCount;
}


function sendMessage(receiverId, textarea) {
    const senderId = `${CURRENT_USER_ID}`;
    const messageInput = document.getElementById("chatinput-" + receiverId);
    const message = messageInput.value.trim();

    if (!message) return;
    event.preventDefault();
    displayOutgoingMessage(senderId, receiverId, messageInput.value);
    // Send the message
    connection.invoke("SendMessage", senderId, receiverId, messageInput.value)
        .catch(err => console.error(err.toString()));
    messageInput.value = '';
    autoResizeTextarea(textarea);
}

// Event handler for keydown in the textarea
function handleKeyDown(event, receiverId, textarea) {
    if (event.shiftKey) {
        return;
    }

    // Check if Enter key is pressed
    if (event.key === 'Enter') {
        sendMessage(receiverId, textarea); // Call sendMessage function
    }
}


function displayOutgoingMessage(senderId, receiverId, message) {
    const chatBox = document.getElementById(`chatBox-${senderId}-${receiverId}`);
    if (chatBox) {
        const messagesList = chatBox.querySelector('.chat-messages');
        const messageContainer = document.createElement('div'); // Create a container for the message
        messageContainer.classList.add('message-container'); // Add a class to the container
        const msg = document.createElement('div'); // Create the message element
        msg.innerHTML = message.replaceAll(/\n/g, '<br>'); // Set the message text
        msg.classList.add('outgoing-message'); // Add a class to style the message
        messageContainer.appendChild(msg); // Append the message to the container
        messagesList.appendChild(messageContainer); // Append the container to the messages list
        // Scroll to the bottom
        messagesList.scrollTop = messagesList.scrollHeight;
    }
}