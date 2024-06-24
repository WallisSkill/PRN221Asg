function ReplyTo(replyee, nameReplyee, comment, postId, event = null) {
    var replyDiv = document.getElementById('replyDiv-' + postId);
    var displayReplyTo = document.getElementById('displayReplyTo-' + postId);
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
    if (event != null) {
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


function newComment(event) {

    var now = new Date();
    var formattedDateTime = formatDateTime(now);
    var commentsSection = event.currentTarget.closest(".col-sm-12").querySelector(".comments-section");
    var replyTo = event.currentTarget.closest("form").querySelector('input[id*="replyTo"]').value ?? 0;
    var commentText = event.currentTarget.value;
    var postId = event.currentTarget.id;
    var currentReply = commentsSection.querySelector('[class^="replying-"]');
    var formData = new FormData();
    formData.append("CommentText",commentText);
    formData.append("PostId",postId);
    formData.append("ParentId",replyTo);
    formData.forEach((value, key) => {
        console.log(key + ': ' + value);
    });
    var url = `/Index?handler=InsertComment`;
    fetch(url, {
        method: 'POST',
        body: formData
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
            else {
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

function HandleLike(postId, emotionId, deleteStatus, event) {
    $.ajax({
        type: "post",
        url: `/Index?handler=HandleLike`,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            postId: postId,
            emotionId: emotionId,
            deleteStatus: deleteStatus
        },
        dataType: "json",
        success: function (data) {
            UpdateLikeData(data, postId);
        },
        error: function (error) {
            console.log(error.responseText);
        }
    });
    event.stopPropagation();
}
function getColorOfEmotion(emotionURL) {
    const emotion = emotionURL.split('/').pop().replace(".png", "");
    switch (emotion) {
        case "Like":
            return "#0861f2";
        case "Love":
            return "#e73b54";
        case "Angry":
            return "#dd6b0e";
        default:
            return "#eaa823";
    }
}
function UpdateLikeData(data, postId) {
    var likeOfPost = document.getElementById("like-post-" + postId);
    var likePostData = ``;
    if (data != null) {
        var dataGrouped = _.groupBy(data.$values, likeData => likeData.EmotionURL);
        var dataOrder = _.orderBy(dataGrouped, 'length', ['desc']);
        var dataTake3 = _.take(dataOrder, 3);
        _.forEach(dataTake3, function (value) {
            var dataValue = _.first(value);
            likePostData += ` <div class="total-like-block">
                                                                <div class="dropdown">
                                                                    <span class="dropdown-toggle" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" role="button">
                                                                        <img src='${dataValue.EmotionURL}' ${dataValue == _.first(value) ? 'class="nofirst-icon"' : ''}/>
                                                                    </span>
                                                                    <div class="dropdown-menu" style="background-color: rgba(60, 60, 60, 0.7)">`
            _.forEach(value, function (like) {
                likePostData += `<div style="color: white; padding-left: 10px;">${like.User.Fullname}</div>`;
            })
            likePostData += `</div ></div ></div > `;
        });
        likePostData += `<div class="total-like-block ms-2 me-3">
    <div class="dropdown">
        <span class="dropdown-toggle" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" role="button">
           ${data.$values.length}
        </span>
        <div class="dropdown-menu">`
        _.forEach(data.$values, function (like) {
            likePostData += `<div class="dropdown-item">${like.User.Fullname}</div>`
        });
        likePostData += `</div ></div ></div > `;
    }
    likeOfPost.innerHTML = likePostData;

    var likeDisplay = document.getElementById("like-display-" + postId);
    var userLike = _.find(data.$values, function (obj) {
        if (obj.User.UserId == CURRENT_USER_ID) {
            return true;
        }
    });
    var likeDisplayElement = ` <div class="dropdown d-flex" style="justify-content: center">`
    if (typeof (userLike) === 'undefined') {
        likeDisplayElement += `                                                        <span class="dropdown-toggle" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" role="button">
                                                            <i class="fa-regular fa-thumbs-up"></i>
                                                            Like
                                                        </span>`
    } else {
        likeDisplayElement += ` <span class="dropdown-toggle" style="color: ${getColorOfEmotion(userLike.EmotionURL)}" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" role="button">
                                                            <img src="${userLike.EmotionURL}" style="margin-top: -5px"></img>
                                                            ${userLike.EmotionURL.split('/').pop().replace(".png", "")}
                                                        </span>`
    }

    likeDisplayElement += `<div class="dropdown-menu py-2" style="border-radius: 20px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); width: 243px">
                                                        <a class="ms-2 icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${postId}', '1', 'false', event)"><img src="Image/Emoji/like.png" class="img-fluid" alt=""></a>
                                                        <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${postId}', '2', 'false', event)"><img src="Image/Emoji/love.png" class="img-fluid" alt=""></a>
                                                        <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${postId}', '3', 'false', event)"><img src="Image/Emoji/care.png" class="img-fluid" alt=""></a>
                                                        <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${postId}', '4', 'false', event)"><img src="Image/Emoji/haha.png" class="img-fluid" alt=""></a>
                                                        <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${postId}', '5', 'false', event)"><img src="Image/Emoji/wow.png" class="img-fluid" alt=""></a>
                                                        <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${postId}', '6', 'false', event)"><img src="Image/Emoji/sad.png" class="img-fluid" alt=""></a>
                                                        <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${postId}', '7', 'false', event)"><img src="Image/Emoji/angry.png" class="img-fluid" alt=""></a>
                                                    </div>
                                                </div>`;
    likeDisplay.innerHTML = likeDisplayElement;
}