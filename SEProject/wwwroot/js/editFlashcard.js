// Add event listeners to the Edit buttons
document.querySelectorAll(".edit-button").forEach(function(button) {
    button.addEventListener("click", function() {
        var flashcardId = button.getAttribute("data-flashcard-id");
        window.location.href = "/Flashcard/EditFlashcard?id=" + flashcardId;
    });
});