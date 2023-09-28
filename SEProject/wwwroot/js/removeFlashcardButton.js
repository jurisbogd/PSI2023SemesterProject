// Add a click event listener to all "Remove" buttons
var removeButtons = document.querySelectorAll('.remove-button');
removeButtons.forEach(function (button) {
    button.addEventListener('click', function () {
        // Get the ID of the flashcard to remove from the data attribute
        var flashcardId = button.getAttribute('data-flashcard-id');
        
        // Find and remove the flashcard element with the matching ID
        var flashcardToRemove = document.querySelector('.flashcard[data-flashcard-id="' + flashcardId + '"]');
        if (flashcardToRemove) {
            flashcardToRemove.remove();
        }
    });
});