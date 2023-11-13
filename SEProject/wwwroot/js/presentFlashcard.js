document.addEventListener("DOMContentLoaded", function () {
    var revealButton = document.getElementById("revealButton");
    var answer = document.getElementById("answer");
    var arrowButton = document.getElementById("arrowButton");

    answer.style.display = "none";
    arrowButton.style.display = "none";

    revealButton.addEventListener("click", function (event) {
        event.preventDefault();
        answer.style.display = (answer.style.display === "none") ? "block" : "none";
        revealButton.style.display = (revealButton.style.display == "none") ? "block" : "none";
        arrowButton.style.display = (arrowButton.style.display == "none") ? "block" : "none";
    });
});
