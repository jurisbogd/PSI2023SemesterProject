document.addEventListener("DOMContentLoaded", function () {
    var editButtons = document.querySelectorAll(".edit-flashcard-pack-button");

    editButtons.forEach(function (editButton) {
        var flashcardPackId = editButton.getAttribute('data-flashcardPack-id');
        var saveButton = document.querySelector(".save-button[data-flashcardPack-id='" + flashcardPackId + "']");
        var cancelButton = document.querySelector(".cancel-button[data-flashcardPack-id='" + flashcardPackId + "']");
        var textarea = document.getElementById('pack-' + flashcardPackId);

        editButton.addEventListener("click", function () {

            textarea.readOnly = false;

            textarea.focus();
            textarea.classList.add("editable");

            editButton.style.display = "none";
            saveButton.style.display = "inline-block";
            cancelButton.style.display = "inline-block";
        });

        saveButton.addEventListener("click", function () {

            textarea.classList.remove("editable");

            var updatedValue = textarea.value;

            updatePackOnServer(updatedValue);

            textarea.readOnly = true;
            editButton.style.display = "inline-block";
            saveButton.style.display = "none";
            cancelButton.style.display = "none";
        });

        function updatePackOnServer(value) {
            $.ajax({
                type: "POST",
                url: 'EditFlashcardPackName',
                data: { id: flashcardPackId, newName: value},
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                success: function (result) {
                    console.log("Success")
                }
            });
        }
    });
});
