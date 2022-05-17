//inputs
const MONITOR_NAME = document.getElementById("monitor-name-input");
const MONITOR_PRICE = document.getElementById("monitor-price-input");
const MONITOR_REFRESHENING = document.getElementById("monitor-refreshening-input");
const MONITOR_RESOLUTION = document.getElementById("monitor-resolution-input");
const MONITOR_CHECKBOX = document.getElementById("monitor-checkbox");
const MONITOR_PHOTO = document.getElementById("monitor-photo-input");
const MONITOR_STOCK = document.getElementById("monitor-stock-input");
//errors
const MONITOR_NAME_ERROR = document.getElementById("monitor-name-error");
const MONITOR_PRICE_ERROR = document.getElementById("monitor-price-error");
const MONITOR_REFRESHENING_ERROR = document.getElementById("monitor-refreshening-error");
const MONITOR_RESOLUTION_ERROR = document.getElementById("monitor-resolution-error");
const MONITOR_PHOTO_ERROR = document.getElementById("monitor-photo-error");
const MONITOR_STOCK_ERROR = document.getElementById("monitor-stock-error");
//form
var form = document.getElementById("monitor-create-form");
form.addEventListener('submit', (x) => {
    emptyFieldCheck(MONITOR_NAME, MONITOR_NAME_ERROR, x);
    emptyFieldCheck(MONITOR_PRICE, MONITOR_PRICE_ERROR, x);
    emptyFieldCheck(MONITOR_REFRESHENING, MONITOR_REFRESHENING_ERROR, x);
    emptyFieldCheck(MONITOR_RESOLUTION, MONITOR_RESOLUTION_ERROR, x);
    emptyFieldCheck(MONITOR_STOCK, MONITOR_STOCK_ERROR, x);
    if (MONITOR_STOCK.value != "" && MONITOR_STOCK.value <= 0) {
        x.preventDefault();
        MONITOR_STOCK_ERROR.innerHTML = "Number must be positive";
        MONITOR_STOCK.classList.add("is-invalid");
    }
    if (!MONITOR_CHECKBOX.checked) {
        emptyFileInputCheck(MONITOR_PHOTO, MONITOR_PHOTO_ERROR, x);
    }
    if (MONITOR_CHECKBOX.checked) {
        MONITOR_PHOTO_ERROR.innerHTML = null;
        MONITOR_PHOTO.classList.remove("is-invalid");
    }
    floatInputCheck(MONITOR_PRICE, MONITOR_PRICE_ERROR, x);
});