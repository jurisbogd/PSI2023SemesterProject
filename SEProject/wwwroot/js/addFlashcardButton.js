    document.addEventListener("DOMContentLoaded", function () {

    var addFlashcardButton = document.getElementById("addFlashcardButton");

    addFlashcardButton.addEventListener("click", function () {
        var packID = addFlashcardButton.getAttribute("data-pack-id");
        window.location.href = "/FlashcardPack/AddFlashcard?packID=" + packID;
    });

});