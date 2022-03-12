//inputs
const USER_NAME = document.getElementById("register-name-input");
const USER_EMAIL = document.getElementById("register-email-input");
const USER_PASSWORD = document.getElementById("register-password-input");
//errors
const USER_NAME_ERROR = document.getElementById("register-name-error");
const USER_EMAIL_ERROR = document.getElementById("register-email-error");
const USER_PASSWORD_ERROR = document.getElementById("register-password-error");
//form
var form = document.getElementById("register-form");

form.addEventListener('submit', (x) => {
    emptyFieldCheck(USER_NAME, USER_NAME_ERROR, x);
    emptyFieldCheck(USER_EMAIL, USER_EMAIL_ERROR, x);
    emptyFieldCheck(USER_PASSWORD, USER_PASSWORD_ERROR, x);
    if (USER_NAME.value != "") {
        minLengthCheck(USER_NAME, USER_NAME_ERROR, 3, x);
    }
    if (USER_PASSWORD.value != "") {
        minLengthCheck(USER_PASSWORD, USER_PASSWORD_ERROR, 4, x);
    }
});