document.addEventListener('DOMContentLoaded', function () {
    const dataItems = document.querySelectorAll('.data-item-search');
    const showAllBtn = document.getElementById('show-all-btn');
    const initialDisplayCount = 3;

    if (dataItems.length <= initialDisplayCount) {
        showAllBtn.style.display = 'none';
        dataItems.forEach((item, index) => {
            if (index < initialDisplayCount) {
                item.classList.add('show-search');
            }
        });
    } else {
        dataItems.forEach((item, index) => {
            if (index < initialDisplayCount) {
                item.classList.add('show-search');
            }
        });

        showAllBtn.addEventListener('click', function () {
            const isShowingAll = Array.from(dataItems).every(item => item.classList.contains('show-search'));

            if (isShowingAll) {
                dataItems.forEach((item, index) => {
                    if (index >= initialDisplayCount) {
                        item.classList.remove('show-search');
                    }
                });
                showAllBtn.textContent = 'See all';
            } else {
                dataItems.forEach(item => {
                    item.classList.add('show-search');
                });
                showAllBtn.textContent = 'Minimize';
            }
        });
    }
});

document.addEventListener('DOMContentLoaded', function () {
    const dataItemsPost = document.querySelectorAll('.data-item-post');
    const showAllBtnPost = document.getElementById('show-all-btn-post');
    const initialDisplayCountPost = 5;

    if (dataItemsPost.length <= initialDisplayCountPost) {
        showAllBtnPost.style.display = 'none';
        dataItemsPost.forEach((item, index) => {
            if (index < initialDisplayCountPost) {
                item.classList.add('show-post');
            }
        });
    } else {
        dataItemsPost.forEach((item, index) => {
            if (index < initialDisplayCountPost) {
                item.classList.add('show-post');
            }
        });

        showAllBtnPost.addEventListener('click', function () {
            const isShowingAll = Array.from(dataItemsPost).every(item => item.classList.contains('show-post'));

            if (isShowingAll) {
                dataItemsPost.forEach((item, index) => {
                    if (index >= initialDisplayCountPost) {
                        item.classList.remove('show-post');
                    }
                });
                showAllBtnPost.textContent = 'See all';
            } else {
                dataItemsPost.forEach(item => {
                    item.classList.add('show-post');
                });
                showAllBtnPost.textContent = 'Minimize';
            }
        });
    }
});

