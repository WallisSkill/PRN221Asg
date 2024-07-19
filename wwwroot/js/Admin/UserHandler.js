$(document).ready(function () {
    if (CURRENT_USER_ROLE == "Admin") {
        $('.toggle-lock').on('click', function () {
            var $this = $(this);
            var userId = $this.data('user-id');
            var userName = $this.data('user-name');

            $.post('/Admin/HomePage?handler=UpdateLock', { userId: userId });

            if ($this.find('i').hasClass('ri-lock-unlock-line')) {
                $this.find('i').removeClass('ri-lock-unlock-line').addClass('ri-lock-2-line');
                $this.text('Locked').prepend('<i class="ri-lock-2-line me-1"></i>');
            } else {
                $this.find('i').removeClass('ri-lock-2-line').addClass('ri-lock-unlock-line');
                $this.text('Unlocked').prepend('<i class="ri-lock-unlock-line me-1"></i>');
            }
        });
    }
});

