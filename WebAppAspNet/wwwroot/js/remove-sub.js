function unsubscribe(email) {
    if (confirm("Are you sure you want to unsubscribe?")) {
        fetch(`/Home/Unsubscribe/${email}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    // Handle success
                    alert("Unsubscribed successfully!");
                    // Optionally, you can reload the page or update UI as needed
                } else {
                    // Handle errors
                    alert("Failed to unsubscribe. Please try again later.");
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert("An unexpected error occurred. Please try again later.");
            });
    }
}