document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector(".login-box");

    form.addEventListener("submit", async function (event) {
        event.preventDefault();

        const userDTO = {
            email: document.getElementById("email").value.trim(),
            password: document.getElementById("password").value.trim()
        };

        try {
            const response = await fetch("/users/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(userDTO)
            });

            if (response.ok) {
                const data = await response.json();

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
