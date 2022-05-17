//inputs
const SMARTPHONE_NAME = document.getElementById("smartphone-name-input");
const SMARTPHONE_PRICE = document.getElementById("smartphone-price-input");
const SMARTPHONE_MEMORY = document.getElementById("smartphone-memory-input");
const SMARTPHONE_RAM = document.getElementById("smartphone-RAM-input");
const SMARTPHONE_DIAGONAL = document.getElementById("smartphone-diagonal-input");
const SMARTPHONE_CAMERA_RESOLUTION = document.getElementById("smartphone-camera-resolution-input");
const SMARTPHONE_STOCK = document.getElementById("smartphone-stock-input");
const SMARTPHONE_PHOTO = document.getElementById("smartphone-photo-input");
const SMARTPHONE_DEFAULT_PHOTO = document.getElementById("smartphone-checkbox");
//errors
const SMARTPHONE_NAME_ERROR = document.getElementById("smartphone-name-error");
const SMARTPHONE_PRICE_ERROR = document.getElementById("smartphone-price-error");
const SMARTPHONE_MEMORY_ERROR = document.getElementById("smartphone-memory-error");
const SMARTPHONE_RAM_ERROR = document.getElementById("smartphone-RAM-error");
const SMARTPHONE_DIAGONAL_ERROR = document.getElementById("smartphone-diagonal-error");
const SMARTPHONE_CAMERA_RESOLUTION_ERROR = document.getElementById("smartphone-camera-resolution-error");
const SMARTPHONE_STOCK_ERROR = document.getElementById("smartphone-stock-error");
const SMARTPHONE_PHOTO_ERROR = document.getElementById("smartphone-photo-error");
//form
var form = document.getElementById("smartphone-edit-form");

form.addEventListener('submit', (x) => {
    emptyFieldCheck(SMARTPHONE_NAME, SMARTPHONE_NAME_ERROR, x);
    emptyFieldCheck(SMARTPHONE_PRICE, SMARTPHONE_PRICE_ERROR, x);
    emptyFieldCheck(SMARTPHONE_MEMORY, SMARTPHONE_MEMORY_ERROR, x);
    emptyFieldCheck(SMARTPHONE_RAM, SMARTPHONE_RAM_ERROR, x);
    emptyFieldCheck(SMARTPHONE_DIAGONAL, SMARTPHONE_DIAGONAL_ERROR, x);
    emptyFieldCheck(SMARTPHONE_CAMERA_RESOLUTION, SMARTPHONE_CAMERA_RESOLUTION_ERROR, x);
    emptyFieldCheck(SMARTPHONE_STOCK, SMARTPHONE_STOCK_ERROR, x);
    if (SMARTPHONE_DEFAULT_PHOTO.checked) {
        SMARTPHONE_PHOTO_ERROR.innerHTML = null;
    }
    floatInputCheck(SMARTPHONE_PRICE, SMARTPHONE_PRICE_ERROR, x);
});