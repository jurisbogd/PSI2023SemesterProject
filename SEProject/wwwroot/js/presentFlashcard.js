document.addEventListener("DOMContentLoaded", function () {
    var revealButton = document.getElementById("revealButton");
    var answer = document.getElementById("answer");
    var filledStar = document.getElementById('filled');
    var nonFilledStar = document.getElementById('non-filled');
    var arrowButton = document.getElementById("arrowButton");

    var flashcardPropertyIsFavorite = document.getElementById("is-favorite");
    var flashcardPropertyPackID = document.getElementById("pack-id");
    var flashcardPropertyFlashcardID = document.getElementById("flashcard-id");

    var isFavorite = flashcardPropertyIsFavorite.getAttribute("data-isfavorite") === "true";
    var packID = flashcardPropertyPackID.getAttribute("data-pack-id");
    var flashcardID = flashcardPropertyFlashcardID.getAttribute("data-flashcard-id");

    console.log(isFavorite);

    answer.style.display = "none";
    arrowButton.style.display = "none";
    filledStar.style.display = isFavorite ? '' : 'none';
    nonFilledStar.style.display = isFavorite ? 'none' : '';


    revealButton.addEventListener("click", function (event) {
        event.preventDefault();
        answer.style.display = (answer.style.display === "none") ? "block" : "none";
        revealButton.style.display = (revealButton.style.display == "none") ? "block" : "none";
        arrowButton.style.display = (arrowButton.style.display == "none") ? "block" : "none";
    });

    document.getElementById('favorite-button').addEventListener('click', function () {
        isFavorite = !isFavorite;
        console.log(isFavorite);
        filledStar.style.display = isFavorite ? '' : 'none';
        nonFilledStar.style.display = isFavorite ? 'none' : '';
    });

    arrowButton.addEventListener('click', function () {
        updateFavoriteOnServer(isFavorite);
    });

    function updateFavoriteOnServer(isFavorite) {

        console.log("Script started");

        $.ajax({
            type: "POST",
            url: 'ToggleFavorite',
            data: { packID: packID, currentFlashcardID: flashcardID, isFavorite: isFavorite },
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            success: function (result) {
                console.log("Success")
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error);
            }
        });
    }
});
