
document.addEventListener('DOMContentLoaded', (event) => {
    const handlePostClick = (postId, iconElement) => {
        if (iconElement.classList.contains('ri-save-line')) {
            interactSave(postId);
            iconElement.classList.remove('ri-save-line');
            iconElement.classList.add('ri-close-line');
            const dataElement = iconElement.closest('.option-post').querySelector('.data');
            if (dataElement) {
                dataElement.querySelector('h6').innerText = 'Remove Saved Post';
                dataElement.querySelector('p').innerText = 'Remove this from your saved items';
            }
        } else if (iconElement.classList.contains('ri-close-line')) {
            interactSave(postId, false);
            if (window.location.href.includes("Saved")) {
                iconElement.closest(".col-sm-12").remove();
            }
            iconElement.classList.remove('ri-close-line');
            iconElement.classList.add('ri-save-line');
            const dataElement = iconElement.closest('.option-post').querySelector('.data');
            if (dataElement) {
                dataElement.querySelector('h6').innerText = 'Save Post';
                dataElement.querySelector('p').innerText = 'Add this to your saved items';
            }
        }
    };

    document.addEventListener('click', (event) => {
        const optionPost = event.target.closest('.option-post');
        if (optionPost) {
            const iconElement = optionPost.querySelector('i[data-post-id]');
            if (iconElement.classList.contains("ri-save-line") || iconElement.classList.contains("ri-close-line")) {
                const postId = iconElement.getAttribute('data-post-id');
                handlePostClick(postId, iconElement);
            }
            if (iconElement.classList.contains("ri-delete-bin-line")) {
                const postId = iconElement.getAttribute('data-post-id');
                deletePost(postId, event);
            }
        }
    });
});


// Hàm save post
function interactSave(postId, type = true) {

    var formData = new FormData();
    formData.append("PostId", postId);
    formData.append("type", type);
    var url = `/Saved`;
    fetch(url, {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            showToast(type ? "Save post successfully" : "Remove post successfully");
        })
}


//Create New Post
$(document).ready(function () {
    const fileInput = document.getElementById('fileInput');
    let dataTransfer = new DataTransfer();

    $('#fileInput').on('change', function (event) {
        const files = event.target.files;
        const preview = $('#imagePreview');

        Array.from(files).forEach(file => {
            const reader = new FileReader();
            reader.onload = function (e) {
                const imgHTML = '<div class="post-image" data-file-name="' + file.name + '">' +
                    '<img src="' + e.target.result + '" class="img-fluid rounded w-100">' +
                    '<button type="button" class="delete-btn btn btn-danger btn-sm close-img"><i class="fa fa-close"></i></button>' +
                    '</div>';
                preview.slick('slickAdd', imgHTML);
            }
            reader.readAsDataURL(file);
            dataTransfer.items.add(file);
        });

        fileInput.files = dataTransfer.files;

        if (!preview.hasClass('slick-initialized')) {
            preview.slick({
                infinite: false
            });
        }
    });

    $('#imagePreview').on('click', '.delete-btn', function () {
        const preview = $('#imagePreview');
        const slideIndex = $(this).closest('.slick-slide').data('slick-index');
        const fileName = $(this).closest('.post-image').data('file-name');

        for (let i = 0; i < dataTransfer.items.length; i++) {
            if (dataTransfer.items[i].getAsFile().name === fileName) {
                dataTransfer.items.remove(i);
                break;
            }
        }

        fileInput.files = dataTransfer.files;

        if (document.getElementById('existingPhoto') != null) {
            deletePhoto(slideIndex);
        }
        if (preview.slick('getSlick').slideCount === 1) {
            preview.slick('unslick');
            preview.empty();
        } else {
            preview.slick('slickRemove', slideIndex);

            preview.find('.slick-slide').each(function (index) {
                $(this).attr('data-slick-index', index);
            });

            preview.slick('setPosition');
        }
    });

    let form = document.getElementById('postForm');
    if (form) {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();

            let formData = new FormData(e.target);

            try {
                let response = await fetch('/Index?handler=CreatePost', {
                    method: 'POST',
                    body: formData
                });

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                let responseData = await response.json();

                form.reset();
                document.getElementById('caption').value = '';
                $('#post_Caption').val('');

                if ($('#imagePreview').hasClass('slick-initialized')) {
                    $('#imagePreview').slick('unslick').empty();
                } else {
                    $('#imagePreview').empty();
                }

                addNewPostToDOM(responseData);

                dataTransfer = new DataTransfer();
                fileInput.files = dataTransfer.files;
                fileInput.value = '';
                connection.invoke("NewPost", CURRENT_USER_ID, CURRENT_USER_NAME)
                    .catch(err => console.error(err.toString()));

                $(".newpost").slick({
                    infinite: false
                });

            } catch (error) {
                console.error('Error posting data:', error);
            }
        });
    }
});

function addNewPostToDOM(data) {
    const postList = document.getElementById('postList');
    var newPostHtml = `
            <div class="col-sm-12">
                    <div class="card card-block card-stretch card-height ${body.classList.contains('bg-dark') ? 'bg-dark text-white' : ''}">
                        <div class="card-body ${body.classList.contains('bg-dark') ? 'bg-dark text-white' : ''}" id="card-body-${data.post.postId}" style="padding-bottom: 10px;">
                        <div class="user-post-data">
                            <div class="d-flex justify-content-between">
                                <div class="me-3">
                                    <img class="rounded-circle avatar-60 img-fluid" src="${CURRENT_USER_IMAGE}" alt="img">
                                </div>
                                <div class="w-100">
                                    <div class="d-flex justify-content-between">
                                        <div class="">
                                                <h5 class="mb-0 d-inline-block ${body.classList.contains("bg-dark") ? 'text-white' : ''}">${CURRENT_USER_NAME}</h5>
                                                <span class="mb-0 d-inline-block ${body.classList.contains('bg-dark') ? 'text-white' : ''}">Add New Post</span>
                                            <p class="mb-0 text-primary">just now</p>
                                        </div>
                                        <div class="card-post-toolbar ${body.classList.contains("bg-dark") ? 'text-white bg-dark' : ''}">
                                            <div class="dropdown">
                                                <span class="dropdown-toggle ${body.classList.contains("bg-dark") ? 'text-white' : ''}" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" role="button">
                                                    <i class="ri-more-fill"></i>
                                                </span>
                                                <div class="dropdown-menu m-0 p-0">
                                                    <a class="dropdown-item p-3 option-post clickable ${body.classList.contains("bg-dark") ? 'text-white bg-dark' : ''}">
                                                        <div class="d-flex align-items-top">
                                                            <div class="h4">
                                                                    <i data-post-id="${data.post.postId}" class="ri-save-line ${body.classList.contains("bg-dark") ? 'text-white' : ''}"></i>
                                                            </div>
                                                            <div class="data ms-2">
                                                                <h6 class="${body.classList.contains("bg-dark") ? 'text-white' : ''}">Save Post</h6>
                                                                <p class="mb-0">Add this to your saved items</p>
                                                            </div>
                                                        </div>
                                                    </a>
                                                  
                                                    <form method="get" asp-page="/EditPost" id="editPost-${data.post.postId}">
                                                                                                <input type="hidden" name="PostId" value="${data.post.postId}" />
                                                                                                    <a class="dropdown-item p-3 option-post clickable ${body.classList.contains("bg-dark") ? 'text-white bg-dark' : ''}">
                                                                                                        <div class="d-flex">
                                                                                                            <div class="h4">
                                                                                                                <i class="ri-pencil-line ${body.classList.contains("bg-dark") ? 'text-white' : ''}"></i>
                                                                                                            </div>
                                                                                                            <div class="data ms-2">
                                                                                                                <h6 class="${body.classList.contains("bg-dark") ? 'text-white' : ''}">Update Post</h6>
                                                                                                                <p class="mb-0">Update your post</p>
                                                                                                            </div>
                                                                                                        </div>
                                                                                                    </a>
                                                                                                </form>
                                                    
                                                <a class="dropdown-item p-3 option-post clickable ${body.classList.contains("bg-dark") ? 'text-white bg-dark' : ''}">
                                                    <div class="d-flex">
                                                        <div class="h4">
                                                            <i data-post-id="${data.post.postId}" class="ri-delete-bin-line ${body.classList.contains("bg-dark") ? 'text-white' : ''}"></i>
                                                        </div>
                                                        <div class="data ms-2">
                                                            <h6 class="${body.classList.contains("bg-dark") ? 'text-white' : ''}">Delete Post</h6>
                                                            <p class="mb-0">This post will be permanently deleted</p>
                                                        </div>
                                                    </div>
                                                </a>
                                                                                               

                                                                                                
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="mt-3">
                                <p>${data.post.caption ?? ""}</p>
                        </div>
                        <div class="user-post">
                            <div class="newt newpost">`;
    if (data.photos) {
        data.photos.forEach(function (photo) {
            newPostHtml += `
                <div class="post-image">
                    <img src="${photo}" alt="post-image" class="img-fluid rounded w-100">
                </div>`;
        });
    }
    newPostHtml += `
                                    </div>
                                </div>            
                            
                        
                        <div class="comment-area mt-3">
                                <div class="like-block position-relative d-flex align-items-center">
                                      <div class="d-flex align-items-center" id="like-post-${data.post.postId}">
                                  </div>
                               </div>
                            <hr style="margin-bottom: 0">
                            <div class="d-flex justify-content-between align-items-center flex-wrap" style="padding: 3px 0 3px 0">
                                        <div class="like-data row-block" onclick="HandleLike('${data.post.postId}', '1', 'true', event)" id="like-display-${data.post.postId}">
                                    <div class="dropdown d-flex" style="justify-content: center">
                                        <span class="dropdown-toggle ${body.classList.contains('bg-dark') ? 'text-white' : ''}" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" role="button">
                                            <i class="fa-regular fa-thumbs-up"></i>
                                            Like
                                        </span>
                                        <div class="dropdown-menu py-2" style="border-radius: 20px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); width: 243px">
                                                <a class="ms-2 icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${data.post.postId}', '1', 'false', event)" data-bs-original-title="" title=""><img src="/Image/Emoji/like.png" class="img-fluid" alt=""></a>
                                                <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${data.post.postId}', '2', 'false', event)" data-bs-original-title="" title=""><img src="/Image/Emoji/love.png" class="img-fluid" alt=""></a>
                                                <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${data.post.postId}', '3', 'false', event)" data-bs-original-title="" title=""><img src="/Image/Emoji/care.png" class="img-fluid" alt=""></a>
                                                <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${data.post.postId}', '4', 'false', event)" data-bs-original-title="" title=""><img src="/Image/Emoji/haha.png" class="img-fluid" alt=""></a>
                                                <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${data.post.postId}', '5', 'false', event)" data-bs-original-title="" title=""><img src="/Image/Emoji/wow.png" class="img-fluid" alt=""></a>
                                                <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${data.post.postId}', '6', 'false', event)" data-bs-original-title="" title=""><img src="/Image/Emoji/sad.png" class="img-fluid" alt=""></a>
                                                <a class="icon me-2" data-bs-toggle="tooltip" data-bs-placement="top" onclick="HandleLike('${data.post.postId}', '7', 'false', event)" data-bs-original-title="" title=""><img src="/Image/Emoji/angry.png" class="img-fluid" alt=""></a>
                                        </div>
                                    </div>
                                </div>
                                    <div class="share-block d-flex align-items-center feather-icon mt-2 mt-md-0 row-block clickable" onclick="toggleComment('${data.post.postId}')">
                                    <span class="${body.classList.contains('bg-dark') ? 'text-white' : ''}">
                                        <i class="fa-regular fa-comment"></i>
                                        Comment
                                    </span>
                                </div>
                            </div>
                            <div id="comment-container-${data.post.postId}" hidden>
                                <div class="comments-section">
                                    <ul></ul>
                                </div>
                                <form class="comment-text mt-3">
                                        <div id="replyDiv-${data.post.postId}" class="comment-text align-items-center mt-3 justify-content-between" style="display: none !important; border-top: 1px solid">
                                        <div>
                                                <input type="text" id="displayReplyTo-${data.post.postId}" style="border: none; width: 400px; font-weight: bold" readonly="" class="${body.classList.contains("bg-dark") ? 'bg-dark text-white' : ''}">
                                                <input type="text" id="displayComment-${data.post.postId}" style="border: none; width: 573px; color: gray" readonly="" class="${body.classList.contains("bg-dark") ? 'bg-dark text-white' : ''}">
                                        </div>
                                            <input hidden="" name="replyTo" value="0" type="text" id="replyTo-${data.post.postId}" class="${body.classList.contains("bg-dark") ? 'bg-dark text-white' : ''}">
                                            <input hidden="" name="postId" type="text" value="${data.post.postId}" class="${body.classList.contains("bg-dark") ? 'bg-dark text-white' : ''}">
                                            <i class="fa fa-close closer" onclick="ReplyTo('-1', '0', '0', '${data.post.postId}')"></i>
                                    </div>
                                <div class="comment-text d-flex align-items-center">
                                    <input id="${data.post.postId}" type="text" onkeydown="if (event.key === 'Enter') newComment(event)" class="form-control rounded ${body.classList.contains("bg-dark") ? 'bg-dark text-white' : ''}" placeholder="Enter Your Comment">
                                 </div>                               
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
                    </div>
            </div>`;

    postList.insertAdjacentHTML('afterbegin', newPostHtml);
    document.querySelector(".modal button.btn-secondary").click();


    document.getElementById(`editPost-${data.post.postId}`).addEventListener('click', function (event) {
        window.location.href = "/EditPost?PostId=" + data.post.postId ;
    });
}
function deletePost(postId, event) {
    const postElement = event.target.closest(".col-sm-12");
    event.preventDefault();
    if (postElement) {
        postElement.remove();
    }

    var formData = new FormData();
    formData.append("postId", postId);
    fetch('/Index?handler=DeletePost', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to delete post');
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                console.log('Post deleted successfully');
                showToast("Delete post successfully");
            } else {
                console.error('Failed to delete post:', data.error);
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

const form_update = document.getElementById('postForm-update');

if (form_update) {
    form_update.addEventListener('submit', async (e) => {
        e.preventDefault(); // Prevent default form submission

        const formData = new FormData(e.target);
        // formData.append('imageNames', document.getElementById('existingPhoto').value);

        try {
            const response = await fetch('/EditPost', {
                method: 'POST',
                body: formData
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const responseData = await response.json();

            console.log('Post successful:', responseData);

            document.getElementById('caption').value = '';

            $(".newpost").slick({
                infinite: false
            });

        } catch (error) {
            console.error('Error posting data:', error);
        }
    });
}

function deletePhoto(index) {

    var existingPhotoInput = document.getElementById('existingPhoto');
    var existingPhotoUrls = existingPhotoInput.value.split('||');
    if (index !== -1) {
        existingPhotoUrls.splice(index, 1);
        existingPhotoInput.value = existingPhotoUrls.join('||');
    }
}
