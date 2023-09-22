// addFlashcardButton.js
document.getElementById('addFlashcardButton').addEventListener('click', function () {
    // Toggle the visibility of the pop-up form when the button is clicked
    var popupForm = document.getElementById('addFlashcardForm');
    if (popupForm.style.display === 'none' || popupForm.style.display === '') {
        popupForm.style.display = 'block';
    } else {
        popupForm.style.display = 'none';
    }
});