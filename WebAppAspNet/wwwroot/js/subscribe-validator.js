function validateForm(event) {
    var emailInput = document.getElementById('emailInput');
    var email = emailInput.value;
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailRegex.test(email)) {
        emailInput.style.borderWidth = "5px";
        emailInput.style.borderColor = "lightcoral";
        return false; 
    } else {
        emailInput.style.borderWidth = ""; 
        emailInput.style.borderColor = ""; 
        return true; 
    }
}