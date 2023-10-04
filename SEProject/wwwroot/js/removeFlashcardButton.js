document.addEventListener("DOMContentLoaded", function () {
    var removeButtons = document.querySelectorAll(".remove-button");

    removeButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            // Get the flashcard ID from the data attribute
            var flashcardId = button.getAttribute("data-flashcard-id");

        });
    });
});
