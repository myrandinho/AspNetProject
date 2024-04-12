document.addEventListener("DOMContentLoaded", function () {
    document.querySelector('.delete-btn').addEventListener('click', function (event) {
        var checkbox = document.getElementById('deleting-user');
        var validationText = document.querySelector('.text-with-validation');

        if (!checkbox.checked) {
            validationText.classList.add('error-text'); // L�gg till klassen om checkboxen inte �r markerad
            event.preventDefault(); // F�rhindra formul�rins�ndning om checkboxen inte �r markerad
        } else {
            validationText.classList.remove('error-text'); // Ta bort klassen om checkboxen �r markerad
        }
    });
});