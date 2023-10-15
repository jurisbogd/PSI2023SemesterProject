// JavaScript code to handle the "Edit" button click
document.querySelectorAll(".edit-button").forEach(function(button) {
    button.addEventListener("click", function() {
        var flashcardId = button.getAttribute("data-flashcard-id");
        window.location.href = "/FlashcardPack/EditFlashcard?flashcardID=" + flashcardId; // Use the correct query parameter name
    });
});
