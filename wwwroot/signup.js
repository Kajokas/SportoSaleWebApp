document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector(".signup-form");

    form.addEventListener("submit", async function (event) {
        event.preventDefault();

        const name = document.getElementById("name").value.trim();
        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value.trim();
        const confirmPassword = document.getElementById("confirm-password").value.trim();

        if (password !== confirmPassword) {
            alert("Passwords do not match!");
            return;
        }

        const userDTO = {
            name: name,
            email: email,
            password: password
        };

        try {
            const response = await fetch("/users/signup", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(userDTO)
            });

            if (response.ok) {
                alert("Signup successful! You can now log in.");
                window.location.href = "login.html";
            } else {
                const errorText = await response.text();
                alert("Signup failed: " + errorText);
            }
        } catch (error) {
            console.error("Error during signup:", error);
            alert("An error occurred while signing up. Please try again.");
        }
    });
});
