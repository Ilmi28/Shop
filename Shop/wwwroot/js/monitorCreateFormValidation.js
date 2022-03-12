//inputs
const MONITOR_NAME = document.getElementById("monitor-name-input");
const MONITOR_PRICE = document.getElementById("monitor-price-input");
const MONITOR_REFRESHENING = document.getElementById("monitor-refreshening-input");
const MONITOR_RESOLUTION = document.getElementById("monitor-resolution-input");
const MONITOR_CHECKBOX = document.getElementById("monitor-checkbox");
const MONITOR_PHOTO = document.getElementById("upload-photo");
//errors
const MONITOR_NAME_ERROR = document.getElementById("monitor-name-error");
const MONITOR_PRICE_ERROR = document.getElementById("monitor-price-error");
const MONITOR_REFRESHENING_ERROR = document.getElementById("monitor-refreshening-error");
const MONITOR_RESOLUTION_ERROR = document.getElementById("monitor-resolution-error");
const MONITOR_PHOTO_ERROR = document.getElementById("photo-error");
//form
var form = document.getElementById("monitor-create-form");
form.addEventListener('submit', (x) => {
    emptyFieldCheck(MONITOR_NAME, MONITOR_NAME_ERROR, x);
    emptyFieldCheck(MONITOR_PRICE, MONITOR_PRICE_ERROR, x);
    emptyFieldCheck(MONITOR_REFRESHENING, MONITOR_REFRESHENING_ERROR, x);
    emptyFieldCheck(MONITOR_RESOLUTION, MONITOR_RESOLUTION_ERROR, x);
    if (!MONITOR_CHECKBOX.checked && document.getElementById("form-title").innerHTML != "Edit") {
        emptyFieldCheck(MONITOR_PHOTO, MONITOR_PHOTO_ERROR, x);
    }
    if (MONITOR_CHECKBOX.checked) {
        MONITOR_PHOTO_ERROR.innerHTML = null;
        MONITOR_PHOTO.classList.remove("is-invalid");
        MONITOR_PHOTO.classList.remove("is-valid");
        console.log(document.getElementById("form-title").innerHTML);
    }
    if (!/^[^-\s][a-zA-Z0-9_\s-]+$/.test(MONITOR_NAME.value)) {
        x.preventDefault();
        MONITOR_NAME_ERROR.innerHTML = "You can only use special characters like '-()'";
        MONITOR_NAME.classList.remove("is-valid");
        MONITOR_NAME.classList.add("is-invalid");
    }
    if (/[^0-9,]/.test(MONITOR_PRICE.value)) {
        x.preventDefault();
        MONITOR_PRICE_ERROR.innerHTML = 'You can use only numbers and comma';
        MONITOR_PRICE.classList.remove("is-valid");
        MONITOR_PRICE.classList.add("is-invalid");
    }
});