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
    const dataItems = document.querySelectorAll('.data-item-post');
    const showAllBtn = document.getElementById('show-all-btn-post');
    const initialDisplayCount = 5;

    if (dataItems.length <= initialDisplayCount) {
        showAllBtn.style.display = 'none';
        dataItems.forEach((item, index) => {
            if (index < initialDisplayCount) {
                item.classList.add('show-post');
            }
        });
    } else {
        dataItems.forEach((item, index) => {
            if (index < initialDisplayCount) {
                item.classList.add('show-post');
            }
        });

        showAllBtn.addEventListener('click', function () {
            const isShowingAll = Array.from(dataItems).every(item => item.classList.contains('show-post'));

            if (isShowingAll) {
                dataItems.forEach((item, index) => {
                    if (index >= initialDisplayCount) {
                        item.classList.remove('show-post');
                    }
                });
                showAllBtn.textContent = 'See all';
            } else {
                dataItems.forEach(item => {
                    item.classList.add('show-post');
                });
                showAllBtn.textContent = 'Minimize';
            }
        });
    }
});

