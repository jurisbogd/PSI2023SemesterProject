    // // JavaScript to show and hide the modal when the "Edit" button is clicked
    // document.querySelectorAll(".edit-flashcard-pack-button").forEach(function(button) {
    //     button.addEventListener("click", function() {
    //         var modalId = "editModal-" + button.getAttribute("data-flashcardPack-id");
    //         var modal = document.getElementById(modalId);
    //         modal.style.display = "block";
    //     });
    // });

    // JavaScript to show and hide the modal when the "Edit" button is clicked
document.querySelectorAll(".edit-flashcard-pack-button").forEach(function(button) {
    button.addEventListener("click", function() {
        var modalId = "editModal-" + button.getAttribute("data-flashcardPack-id");
        var modal = document.getElementById(modalId);
        modal.style.display = "block";
    });
});
