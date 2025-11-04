const goToLogin = () => window.location.href = "login.html";
const goToSignUp = () => window.location.href = "signup.html";

const getUsers = () => {
    fetch("/users")
	.then(res => res.text())
	.then(text => {
	    console.log("Server says:", text);
	    alert(text);
	})
	.catch(err => console.error("Error fetching /test:", err));
};
