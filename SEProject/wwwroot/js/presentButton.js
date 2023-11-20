document.addEventListener("DOMContentLoaded", function () {

    var presentButton = document.getElementById("presentFlashcardButton");

    var flashcardCount = document
        .getElementById("flashcard-count")
        .getAttribute("data-flashcard-count");

    if (flashcardCount > 0) {
        presentButton.removeAttribute("disabled");
    }
});