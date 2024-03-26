// Function to handle form submission
const handleFormData = (e) => {
    e.preventDefault();

    // Selecting form and input elements
    const form = document.querySelector("form");
    const passwordInput = document.getElementById("current-pw");
    const newPasswordInput = document.getElementById("new-pw");
    const confirmedNewPasswordInput = document.getElementById("confirmed-pw");
    

    // Remove input-validation-error class from all inputs
    document.querySelectorAll("input").forEach(input => input.classList.remove("input-validation-error"));

    // Function to display error messages
    const showError = (field, errorText) => {
        field.classList.add("error");
        const errorElement = document.createElement("small");
        errorElement.classList.add("error-text");
        errorElement.innerText = errorText;
        field.closest(".input-group").appendChild(errorElement);
    }

    // Getting trimmed values from input fields
    
    const password = passwordInput.value.trim();
    const newPassword = newPasswordInput.value.trim();
    const confirmedPassword = confirmedNewPasswordInput.value.trim();



    // Clearing previous error messages
    document.querySelectorAll(".input-group .error").forEach(field => field.classList.remove("error"));
    document.querySelectorAll(".error-text").forEach(errorText => errorText.remove());

    // Performing validation checks
    let hasErrors = false;

    
    if (password === "") {
        showError(passwordInput, "Enter your password");
        hasErrors = true;
    }
    if (newPassword === "") {
        showError(newPasswordInput, "Enter your new password");
        hasErrors = true;
    }
    if (confirmedPassword === "") {
        showError(confirmedNewPasswordInput, "Confirm your password");
        hasErrors = true;
    }

    // Submitting the form if no errors
    if (!hasErrors) {
        form.submit();
    }
}

// Handling form submission event
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");
    form.addEventListener("submit", handleFormData);
});
