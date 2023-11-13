document.addEventListener("DOMContentLoaded", function () {
    var revealButton = document.getElementById("revealButton");
    var question = document.getElementById("question");
    var answer = document.getElementById("answer");

    answer.style.display = "none";

    revealButton.addEventListener("click", function (event) {
        event.preventDefault();
        question.style.display = (question.style.display === "none") ? "block" : "none";
        answer.style.display = (answer.style.display === "none") ? "block" : "none";
        revealButton.style.display = "none";
    });

}); 