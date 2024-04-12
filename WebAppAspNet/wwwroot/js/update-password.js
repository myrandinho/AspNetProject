function validatePasswordFormAsdf(event) {
    /*event.preventDefault();*/

    var requiredFields = document.querySelectorAll('.required-field-pw');

    var allFieldsValid = true;

    requiredFields.forEach(function (field) {
        var input = field.value.trim();


        if (input === '') {
            field.style.borderWidth = "5px";
            field.style.borderColor = "lightcoral";
            allFieldsValid = false;
        } else {
            field.style.borderWidth = "";
            field.style.borderColor = "";
        }
    });

    return allFieldsValid;
}

