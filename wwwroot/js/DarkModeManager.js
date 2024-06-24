function toggleDarkMode() {
    const body = document.body;
    const isDarkMode = body.classList.toggle('bg-dark');

    // Save the current mode to localStorage
    localStorage.setItem('darkMode', isDarkMode ? 'enabled' : 'disabled');

    // Helper function to toggle classes on a set of elements
    function toggleClasses(elements, ...classes) {
        elements.forEach(element => {
            classes.forEach(cls => element.classList.toggle(cls));
        });
    }

    // Toggle classes for different elements
    toggleClasses(document.querySelectorAll('[class*="iq"]'), 'bg-dark', 'text-white');
    toggleClasses(document.querySelectorAll('.ri-time-line'), 'text-white');
    toggleClasses(document.querySelectorAll('.swal2-popup'), 'bg-dark');
    toggleClasses(document.querySelectorAll('.nav-link'), 'text-white');
    toggleClasses(document.querySelectorAll('span'), 'text-white');
    toggleClasses(document.querySelectorAll('.dropdown-item'), 'text-white', 'bg-dark');
    toggleClasses(document.querySelectorAll('h2'), 'text-white');
    toggleClasses(document.querySelectorAll('[class*="las"]'), 'text-white');
    toggleClasses(document.querySelectorAll('[class*="card"]'), 'bg-dark', 'text-white');
    toggleClasses(document.querySelectorAll('[class*="modal"]'), 'bg-dark');
    toggleClasses(document.querySelectorAll('.modal-title'), 'text-white');
    toggleClasses(document.querySelectorAll('.card-body h5'), 'text-white');
    toggleClasses(document.querySelectorAll('.profile-detail h3'), 'text-white');
    toggleClasses(document.querySelectorAll('h6'), 'text-white');
    toggleClasses(document.querySelectorAll('.right-sidebar-panel'), 'bg-dark');
    toggleClasses(document.querySelectorAll('input[type="text"]'), 'bg-dark','text-white');
    toggleClasses(document.querySelectorAll('input[type="date"]'), 'date-dark', 'bg-dark', 'text-white');
    toggleClasses(document.querySelectorAll('[class*="chat"]'), 'bg-dark');
    toggleClasses(document.querySelectorAll('.chat-input textarea'), 'bg-dark', 'text-white');
    toggleClasses(document.querySelectorAll('.errortext'), 'text-white');
    toggleClasses(document.querySelectorAll('.emoji-picker-container'), 'bg-dark');
    toggleClasses(document.querySelectorAll('.emoji-button'), 'text-dark');
    toggleClasses(document.querySelectorAll('.search-emoji'), 'bg-dark', 'text-white');
    toggleClasses(document.querySelectorAll('.comment-content'), 'comment-content-dark');
    toggleClasses(document.querySelectorAll('#about h4'), 'text-white');
    ///////////////Longngu
    toggleClasses(document.querySelectorAll('.editprofile-address'), 'bg-dark', 'text-white');

    //////////////HoangAnh

    /////////////Hung
    ///postArrrow
    var arrowColor = isDarkMode ? "white" : "black";
    document.querySelectorAll('.slick-prev, .slick-next').forEach(el => {
        el.style.color = arrowColor;
    });
    // Handle special cases separately
    document.querySelector('.navbar-collapse').classList.toggle('bg-dark');

    document.querySelectorAll('.nav-link').forEach(navlinkElement => {
        navlinkElement.style.color = isDarkMode ? '#50b5ff' : '';
    });

    document.querySelectorAll('h4 h7').forEach(element => {
        element.classList.toggle('text-friend');
    });

    // Toggle the icon class for the button
    const button = document.getElementById("changeView");
    button.classList.toggle('ri-moon-line', isDarkMode);
    button.classList.toggle('ri-sun-line', !isDarkMode);
}

// Call toggleDarkMode once at the beginning to apply the correct mode based on saved preference
document.addEventListener('DOMContentLoaded', () => {
    if (localStorage.getItem('darkMode') === 'enabled') {
        toggleDarkMode();
    }
});