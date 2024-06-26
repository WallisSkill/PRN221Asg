
document.addEventListener('DOMContentLoaded', (event) => {
    const optionPosts = document.querySelectorAll('.option-post');

    optionPosts.forEach(optionPost => {
        const iconElement = optionPost.querySelector('i[data-post-id]');
        if (iconElement) {
            const postId = iconElement.getAttribute('data-post-id');
            optionPost.addEventListener('click', () => {
                if (iconElement.classList.contains('ri-save-line')) {
                    interactSave(postId);
                    iconElement.classList.remove('ri-save-line');
                    iconElement.classList.add('ri-close-line');
                    const dataElement = optionPost.querySelector('.data');
                    if (dataElement) {
                        dataElement.querySelector('h6').innerText = 'Remove Saved Post';
                        dataElement.querySelector('p').innerText = 'Remove this from your saved items';
                    }
                } else if (iconElement.classList.contains('ri-close-line')) {
                    interactSave(postId, false);
                    if (window.location.href.includes("Saved")) {
                        optionPost.closest(".col-sm-12").remove();
                    }
                    iconElement.classList.remove('ri-close-line');
                    iconElement.classList.add('ri-save-line');
                    const dataElement = optionPost.querySelector('.data');
                    if (dataElement) {
                        dataElement.querySelector('h6').innerText = 'Save Post';
                        dataElement.querySelector('p').innerText = 'Add this to your saved items';
                    }
                }
            });
        }
    });
});

// Hàm save post
function interactSave(postId,type = true) {

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

