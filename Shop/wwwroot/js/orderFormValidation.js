//inputs
const FIRST_NAME = document.getElementById("order-first-name");
const LAST_NAME = document.getElementById("order-last-name");
const CITY = document.getElementById("order-city");
const ADDRESS = document.getElementById("order-address");
const POST_CODE = document.getElementById("order-post-code");
const EMAIL = document.getElementById("order-email");
const PHONE_NUMBER = document.getElementById("order-phone");
const DELIVERY_METHOD = document.getElementById("delivery-method");
const PAYMENT_METHOD = document.getElementById("payment-method");
//errors
const FIRST_NAME_ERROR = document.getElementById("order-first-name-error");
const LAST_NAME_ERROR = document.getElementById("order-last-name-error");
const CITY_ERROR = document.getElementById("order-city-error");
const ADDRESS_ERROR = document.getElementById("order-address-error");
const POST_CODE_ERROR = document.getElementById("order-post-code-error");
const EMAIL_ERROR = document.getElementById("order-email-error");
const PHONE_NUMBER_ERROR = document.getElementById("order-phone-error");
const DELIVERY_METHOD_ERROR = document.getElementById("delivery-method-error");
const PAYMENT_METHOD_ERROR = document.getElementById("payment-method-error");
//form
var form = document.getElementById("order-form");

form.addEventListener('submit', (x) => {
    emptyFieldCheck(FIRST_NAME, FIRST_NAME_ERROR, x);
    emptyFieldCheck(LAST_NAME, LAST_NAME_ERROR, x);
    emptyFieldCheck(CITY, CITY_ERROR, x);
    emptyFieldCheck(ADDRESS, ADDRESS_ERROR, x);
    emptyFieldCheck(POST_CODE, POST_CODE_ERROR, x);
    emptyFieldCheck(EMAIL, EMAIL_ERROR, x);
    emptyFieldCheck(DELIVERY_METHOD, DELIVERY_METHOD_ERROR, x);
    emptyFieldCheck(PAYMENT_METHOD, PAYMENT_METHOD_ERROR, x);
    if (PHONE_NUMBER.value != "")
        phoneNumberCheck(PHONE_NUMBER, PHONE_NUMBER_ERROR, x);
    if (PHONE_NUMBER.value == "") {
        PHONE_NUMBER.classList.remove("is-valid");
        PHONE_NUMBER.classList.remove("is-invalid");
        PHONE_NUMBER_ERROR.innerHTML = null;
    }
    if (POST_CODE.value != "") {
        if (!/(^\d{2}-\d{3}$)/.test(POST_CODE.value)) {
            x.preventDefault();
            POST_CODE_ERROR.style.color = "red";
            POST_CODE_ERROR.innerHTML = "Invalid post code";
            POST_CODE.classList.add("is-invalid");
        }
    }

});