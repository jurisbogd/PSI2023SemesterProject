document.addEventListener("DOMContentLoaded", function () {
    var removeButtons = document.querySelectorAll(".view-flashcard-pack-button");

    removeButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            // Get the flashcard ID from the data attribute
            var flashcardId = button.getAttribute("data-flashcardPack-id");

            // Construct the URL with the flashcard ID as a route parameter
            // Navigate to the URL
            // window.location.href = url;
        });
    });
});
