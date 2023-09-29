document.addEventListener("DOMContentLoaded", function () {
    var removeButtons = document.querySelectorAll(".remove-button");

    removeButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            // Get the flashcard ID from the data attribute
            var flashcardId = button.getAttribute("data-flashcard-id");

            // Construct the URL with the flashcard ID as a route parameter
            var url = "/Flashcard/RemoveSampleFlashcard/" + flashcardId;
            // Navigate to the URL
            // window.location.href = url;
        });
    });
});
