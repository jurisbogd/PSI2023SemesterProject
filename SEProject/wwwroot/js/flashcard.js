// flashcard.js
document.querySelectorAll('.reveal-button').forEach(button => {
    button.addEventListener('click', function () {
        const answerElement = this.previousElementSibling;

        if (answerElement.style.display === 'none') {
            answerElement.style.display = 'block';
            this.style.display = 'none';
        }
    });
});