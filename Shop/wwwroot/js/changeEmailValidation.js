const EMAIL = document.getElementById("change-email");
const EMAIL_ERROR = document.getElementById("change-email-error");
var form = document.getElementById("change-email-form");

form.addEventListener('submit', (x) => {
    emptyFieldCheck(EMAIL, EMAIL_ERROR, x);
    if (EMAIL.value != "") {
        emailCheck(EMAIL, EMAIL_ERROR, x);
    }
    EMAIL.classList.remove("is-valid")
});