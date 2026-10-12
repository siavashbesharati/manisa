document.addEventListener('DOMContentLoaded', () => {
    const form = document.querySelector('.lead-form');
    if (!form) {
        return;
    }

    form.addEventListener('submit', () => {
        const submitButton = form.querySelector('button[type="submit"]');
        if (submitButton) {
            submitButton.disabled = true;
            submitButton.textContent = 'در حال ثبت...';
        }
    });
});
