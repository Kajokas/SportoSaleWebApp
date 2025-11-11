// login.js

// Wait for the DOM to load before running
document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector(".login-box");

    form.addEventListener("submit", async function (event) {
        event.preventDefault(); // prevent page reload

        // Create user DTO from form inputs
        const userDTO = {
            email: document.getElementById("email").value.trim(),
            password: document.getElementById("password").value.trim()
        };

        try {
            // Send login request to backend
            const response = await fetch("http://localhost:5000/users/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(userDTO)
            });

            if (response.ok) {
                const data = await response.json();

                // You can store user info or token if backend returns it
                //localStorage.setItem("token", data.token || "");
                //localStorage.setItem("email", userDTO.email);

                // Example: redirect to home page or show success message
                alert("Login successful!");
                window.location.href = "index.html";
            } else {
                const errorText = await response.text();
                alert("Login failed: " + errorText);
            }
        } catch (error) {
            console.error("Error during login:", error);
            alert("An error occurred while logging in. Please try again.");
        }
    });
});
