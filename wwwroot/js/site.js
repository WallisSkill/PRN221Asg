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
    const dataItems = Array.from(document.querySelectorAll('.data-item-post'));
    const showAllBtn = document.getElementById('show-all-btn-post');
    const initialDisplayCount = 5;
    const searchTerm = '@searchTerm'.toLowerCase();

    const filteredItems = dataItems.filter(item => {
        const caption = item.querySelector('.post-caption').textContent.toLowerCase();
        return caption.includes(searchTerm);
    });

    if (filteredItems.length <= initialDisplayCount) {
        showAllBtn.style.display = 'none';
        filteredItems.forEach((item, index) => {
            if (index < initialDisplayCount) {
                item.classList.add('show-post');
            }
        });
    } else {
        filteredItems.forEach((item, index) => {
            if (index < initialDisplayCount) {
                item.classList.add('show-post');
            }
        });

        showAllBtn.addEventListener('click', function () {
            const isShowingAll = filteredItems.every(item => item.classList.contains('show-post'));

            if (isShowingAll) {
                filteredItems.forEach((item, index) => {
                    if (index >= initialDisplayCount) {
                        item.classList.remove('show-post');
                    }
                });
                showAllBtn.textContent = 'See all';
            } else {
                filteredItems.forEach(item => {
                    item.classList.add('show-post');
                });
                showAllBtn.textContent = 'Minimize';
            }
        });
    }
});

