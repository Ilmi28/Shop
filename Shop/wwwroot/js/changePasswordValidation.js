//inputs
const CURRENT_PASSWORD = document.getElementById("current-password");
const NEW_PASSWORD = document.getElementById("new-password");
const CONFIRM_PASSWORD = document.getElementById("confirm-password");
//errors
const CURRENT_PASSWORD_ERROR = document.getElementById("current-password-error");
const NEW_PASSWORD_ERROR = document.getElementById("new-password-error");
const CONFIRM_PASSWORD_ERROR = document.getElementById("confirm-password-error");
//form
var form = document.getElementById("change-password-form");

form.addEventListener('submit', (x) => {
    emptyFieldCheck(CURRENT_PASSWORD, CURRENT_PASSWORD_ERROR, x);
    emptyFieldCheck(NEW_PASSWORD, NEW_PASSWORD_ERROR, x);
    emptyFieldCheck(CONFIRM_PASSWORD, CONFIRM_PASSWORD_ERROR, x);
    if (NEW_PASSWORD.value != "") {
        minLengthCheck(NEW_PASSWORD, NEW_PASSWORD_ERROR, 4, x);
    }
    CURRENT_PASSWORD.classList.remove("is-valid");
    NEW_PASSWORD.classList.remove("is-valid");
    CONFIRM_PASSWORD.classList.remove("is-valid");
    if (NEW_PASSWORD.value != CONFIRM_PASSWORD.value && CONFIRM_PASSWORD.value != "") {
        x.preventDefault();
        CONFIRM_PASSWORD_ERROR.innerHTML = "Password is not match";
        CONFIRM_PASSWORD.classList.add("is-invalid");
    }
    
});