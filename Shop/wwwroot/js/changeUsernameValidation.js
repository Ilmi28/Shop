const USERNAME = document.getElementById("change-username");
const USERNAME_ERROR = document.getElementById("change-username-error");
var form = document.getElementById("change-username-form");

form.addEventListener('submit', (x) => {
    emptyFieldCheck(USERNAME, USERNAME_ERROR, x);
    if (USERNAME.value != "") {
        minLengthCheck(USERNAME, USERNAME_ERROR, 3, x);
    }
    USERNAME.classList.remove("is-valid");
});
