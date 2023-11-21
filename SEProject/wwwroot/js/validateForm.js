document.addEventListener("DOMContentLoaded", function () {

    var submitButton = document.getElementById("submit-button");

    submitButton.addEventListener("click", function (event) {
        var question = document.getElementById('Question').value;
        var answer = document.getElementById('Answer').value;

        if (question === '' || answer === '') {
            alert('Please fill in all fields before submitting.');
            event.preventDefault(); 
        }
    });
});