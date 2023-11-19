document.querySelectorAll(".edit-button").forEach(function (button) {

    button.addEventListener("click", function () {
        var flashcardId = button.getAttribute("data-flashcard-id");
        window.location.href = "/FlashcardPack/EditFlashcard?flashcardId=" + flashcardId; 
    });

});
