function validate() {
    // get the checkbox element from the DOM using the getElementId function
    let checkbox = document.getElementById("deleting-user");
    // get the text element to display the status of checkbox
    let text = document.getElementById("confirm");
    // use the checked property to check if the checkbox is checked
    if (checkbox.checked) {
        // display result by assigning to innerHTML of the text element.
        text.innerHTML = "Thank you for accepting the agreement";
    }
    else {
        text.innerHTML = "Please accept the agreement to proceed";
    }


}