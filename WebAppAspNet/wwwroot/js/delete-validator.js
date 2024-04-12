document.addEventListener("DOMContentLoaded", function () {
    document.querySelector('.delete-btn').addEventListener('click', function (event) {
        var checkbox = document.getElementById('deleting-user');
        var validationText = document.querySelector('.text-with-validation');

        if (!checkbox.checked) {
            validationText.classList.add('error-text'); // Lägg till klassen om checkboxen inte är markerad
            event.preventDefault(); // Förhindra formulärinsändning om checkboxen inte är markerad
        } else {
            validationText.classList.remove('error-text'); // Ta bort klassen om checkboxen är markerad
        }
    });
});