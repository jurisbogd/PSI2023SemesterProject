document.addEventListener("DOMContentLoaded", function () {
    var revealButton = document.getElementById("revealButton");
    var answer = document.getElementById("answer");

    answer.style.display = "none";

    revealButton.addEventListener("click", function (event) {
        event.preventDefault();
        answer.style.display = (answer.style.display === "none") ? "block" : "none";
        revealButton.style.display = "none"; // Hide the reveal button
    });
});
