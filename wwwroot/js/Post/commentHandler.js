function ReplyTo(replyee, nameReplyee, comment, postId,event =null) {
    var replyDiv = document.getElementById('replyDiv-'+postId);
    var displayReplyTo = document.getElementById('displayReplyTo-'+postId);
    var displayComment = document.getElementById('displayComment-' + postId)
    var replyTo = document.getElementById('replyTo-' + postId);
    if (document.querySelector(".replying-" + postId)) document.querySelector(".replying-" + postId).classList.remove("replying-" + postId);
    if (replyee !== '-1') {
        displayReplyTo.setAttribute('value', 'Replying to ' + nameReplyee);
        displayComment.setAttribute('value', comment);
        replyTo.setAttribute('value', replyee);
        replyDiv.style.display = 'flex';
    }
    else {
        replyDiv.style.display = 'none';
        replyTo.setAttribute('value', 0);

    }
    if(event != null){
        event.currentTarget.closest("li").classList.add("replying-" + postId);
    }
}

function formatDateTime(date) {
    var month = date.getMonth() + 1; // Lấy tháng (0-11) và cộng 1 để có tháng thực tế
    var day = date.getDate();        // Lấy ngày trong tháng
    var year = date.getFullYear();   // Lấy năm

    var hours = date.getHours();     // Lấy giờ (0-23)
    var minutes = date.getMinutes(); // Lấy phút
    var ampm = hours >= 12 ? 'PM' : 'AM'; // Xác định AM/PM
    hours = hours % 12;              // Chuyển đổi sang giờ 12 (0-11)
    hours = hours ? hours : 12;      // Giờ 0 phải chuyển thành 12
    minutes = minutes < 10 ? '0' + minutes : minutes; // Thêm số 0 vào trước nếu phút < 10

    var formattedDate = month + '/' + day + '/' + year;
    var formattedTime = hours + ':' + minutes + ' ' + ampm;

    return formattedDate + ' ' + formattedTime;
}


function newComment(event){

    var now = new Date();
    var formattedDateTime = formatDateTime(now);
    var commentsSection= event.currentTarget.closest(".col-sm-12").querySelector(".comments-section");
    var replyTo = event.currentTarget.closest("form").querySelector('input[id*="replyTo"]').value ?? 0;
    var commentText = event.currentTarget.value;
    var postId =  event.currentTarget.id;
    var currentReply = commentsSection.querySelector('[class^="replying-"]');
    var url = `/Index?handler=InsertComment&&commentText=${commentText}&&postId=${postId}&&parentId=${replyTo}`;
    fetch(url, {
        method: 'GET',
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Nếu thành công, thêm comment vào UI
            var commentId = data.commentId;
            if (currentReply) {
                const value = `
                 <ul>
                    <ul>
                        <li>
                            <div class="comment">
                                    <img src="${CURRENT_USER_IMAGE}" alt="img-user" class="avatar">
                                    <div class="comment-content ${body.classList.contains('bg-dark') ? 'comment-content-dark' : ''}">
                                            <span class="user-name ${body.classList.contains('bg-dark') ? 'text-white' : ''}">${CURRENT_USER_NAME}</span>
                                        <span class="comment-text ${body.classList.contains('bg-dark') ? 'text-white' : ''}">${commentText}</span>
                                    <div>
                                                     <span class= "comment-time ${body.classList.contains('bg-dark') ? 'text-white' : ''}" > ${formattedDateTime} </span>
                                            <span class="comment-actions ${body.classList.contains('bg-dark') ? 'text-white' : ''}">Like (0)</span>
                                                 <span class="comment-actions ${body.classList.contains('bg-dark') ? 'text-white' : ''}" onclick="ReplyTo('${commentId}','${CURRENT_USER_NAME}','${commentText.length > 50 ? commentText.substring(0, 50) + '...' : commentText}','${postId}', event)">Reply</span>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ul>
                </ul>
            `;
                const tempDiv = document.createElement('div');
                tempDiv.innerHTML = value;

                const replyingList = currentReply;
                if (replyingList) {
                    while (tempDiv.firstChild) {
                        replyingList.appendChild(tempDiv.firstChild);
                    }
                }
            }
            else{
                const value = `
                    <div class="comment">
                                        <img src="${CURRENT_USER_IMAGE}" alt="img-user" class="avatar">
                                            <div class="comment-content ${body.classList.contains('bg-dark') ? 'comment-content-dark' : ''}">
                                                <span class="user-name ${body.classList.contains('bg-dark') ? 'text-white' : ''}">${CURRENT_USER_NAME}</span>
                                            <span class="comment-text ${body.classList.contains('bg-dark') ? 'text-white' : ''}">${commentText}</span>
                                        <div>
                                                         <span class= "comment-time ${body.classList.contains('bg-dark') ? 'text-white' : ''}" > ${formattedDateTime} </span>
                                                <span class="comment-actions ${body.classList.contains('bg-dark') ? 'text-white' : ''}">Like (0)</span>
                                                     <span class="comment-actions ${body.classList.contains('bg-dark') ? 'text-white' : ''}" onclick="ReplyTo('${commentId}','${CURRENT_USER_NAME}','${commentText.length > 50 ? commentText.substring(0, 50) + '...' : commentText}','${postId}', event)">Reply</span>
                                        </div>
                                    </div>
                  </div>
            `;

                const tempDiv = document.createElement('div');
                tempDiv.innerHTML = value;
                if (!commentsSection.querySelector("li")) {
                    const li = document.createElement('li');
                    commentsSection.querySelector("ul").appendChild(li);
                }
                while (tempDiv.firstChild) {
                    commentsSection.querySelector("li").appendChild(tempDiv.firstChild);
                }

            }
        });
    event.currentTarget.value = '';
    ReplyTo('-1', '0', '0', event.currentTarget.id);
}