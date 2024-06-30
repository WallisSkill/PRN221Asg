document.addEventListener('DOMContentLoaded', (event) => {
    updateButton();
});

function updateButton() {
    const optionFollow = document.querySelector('.option-follow');
    const userId = optionFollow.getAttribute('data-user-id');
    const followButton = optionFollow.querySelector('#followButton');
    const unfollowAction = optionFollow.querySelector('#unfollowAction');

    if (followButton) {
        followButton.addEventListener('click', () => {
/*            updateFollowState(true);*/
            interactFollow(userId, true);

        });
    }

    if (unfollowAction) {
        unfollowAction.addEventListener('click', () => {
           /* updateFollowState(false);*/
             interactFollow(userId, false);

        });
    }
}

function interactFollow(userId, follow) {
    const url = `/Profile?handler=Follow`; 
    const method = follow ? 'POST' : 'DELETE'; 
    var formData = new FormData();
    formData.append("UserId", userId);
    fetch(url, {
        method: method,
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (follow) {
                showToast('Follow successfully');
                updateFollowState(follow);
            } else {
                showToast('Unfollowed successfully');
                updateFollowState();
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showToast('An error occurred');
        });
}


function updateFollowState(isFollowed = false) {
    const optionFollow = document.querySelector('.option-follow');
    const followButtonContainer = optionFollow.querySelector('.follow-button');

    // Xóa toàn bộ nội dung trong option-follow trước khi thêm mới
    optionFollow.innerHTML = '';

    if (isFollowed) {
        const dropdown = document.createElement('div');
        dropdown.classList.add('dropdown');

        const dropdownToggle = document.createElement('span');
        dropdownToggle.classList.add('btn', 'btn-primary', 'me-2', 'dropdown-toggle');
        dropdownToggle.setAttribute('role', 'button');
        dropdownToggle.setAttribute('data-bs-toggle', 'dropdown');
        dropdownToggle.setAttribute('aria-expanded', 'false');
        dropdownToggle.innerHTML = '<i class="ri-user-follow-line me-1 text-white"></i>Followed';

        const dropdownMenu = document.createElement('div');
        dropdownMenu.classList.add('dropdown-menu', 'dropdown-menu-right');
        dropdownMenu.setAttribute('aria-labelledby', 'followedButton');
        dropdownMenu.innerHTML = '<a class="dropdown-item" id="unfollowAction">Unfollow</a>';

        dropdown.appendChild(dropdownToggle);
        dropdown.appendChild(dropdownMenu);

        optionFollow.appendChild(dropdown);
    } else {
        const followButton = document.createElement('div');
        followButton.classList.add('btn', 'btn-primary', 'me-2');
        followButton.setAttribute('role', 'button');
        followButton.setAttribute('id', 'followButton');
        followButton.innerHTML = '<i class="ri-add-line me-1 text-white"></i>Follow';

        const followButtonContainer = document.createElement('div');
        followButtonContainer.classList.add('follow-button');
        followButtonContainer.appendChild(followButton);

        optionFollow.appendChild(followButtonContainer);
    }
    updateButton();
}