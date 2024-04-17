function validateForm(event) {
    var isValid = true;

    // Hämta referenser till de olika fälten
    var fullNameField = document.getElementById("fullname");
    var emailField = document.getElementById("email");
    var messageField = document.getElementById("message");

    // Återställ tidigare felmeddelanden och border-stilar
    fullNameField.style.borderColor = "";
    emailField.style.borderColor = "";
    messageField.style.borderColor = "";
    fullNameField.nextElementSibling.textContent = "";
    emailField.nextElementSibling.textContent = "";
    messageField.nextElementSibling.textContent = "";

    // Validera att alla fält är ifyllda
    if (fullNameField.value.trim() === "") {
        fullNameField.style.borderColor = "lightcoral";
        fullNameField.style.borderWidth = "5px";
        fullNameField.nextElementSibling.textContent = "Full Name is required";
        isValid = false;
    }
    if (emailField.value.trim() === "") {
        emailField.style.borderColor = "lightcoral";
        emailField.style.borderWidth = "5px";
        emailField.nextElementSibling.textContent = "Email is required";
        isValid = false;
    }
    if (messageField.value.trim() === "") {
        messageField.style.borderColor = "lightcoral";
        messageField.style.borderWidth = "5px";
        messageField.nextElementSibling.textContent = "Message is required";
        isValid = false;
    }

    // Validera e-postadress med RegEx
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(emailField.value.trim())) {
        emailField.style.borderColor = "lightcoral";
        emailField.style.borderWidth = "5px";
        emailField.nextElementSibling.textContent = "Invalid email format";
        isValid = false;
    }

    // Om valideringen inte lyckas, förhindra formuläret från att skickas
    if (!isValid) {
        event.preventDefault();
    }

    return isValid;
}