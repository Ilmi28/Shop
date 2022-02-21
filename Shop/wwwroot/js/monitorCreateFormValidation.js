//inputs
const monitorName = document.getElementById("monitor-name-input");
const monitorPrice = document.getElementById("monitor-price-input");
const monitorRefreshening = document.getElementById("monitor-refreshening-input");
const monitorResolution = document.getElementById("monitor-resolution-input");
const monitorCheckbox = document.getElementById("monitor-checkbox");
const monitorPhoto = document.getElementById("upload-photo");
//errors
const monitorNameError = document.getElementById("monitor-name-error");
const monitorPriceError = document.getElementById("monitor-price-error");
const monitorRefresheningError = document.getElementById("monitor-refreshening-error");
const monitorResolutionError = document.getElementById("monitor-resolution-error");
const monitorPhotoError = document.getElementById("photo-error");
//form
const form = document.getElementById("monitor-create-form");
form.addEventListener('submit', (x) => {
    emptyFieldCheck(monitorName, monitorNameError, x);
    emptyFieldCheck(monitorPrice, monitorPriceError, x);
    emptyFieldCheck(monitorRefreshening, monitorRefresheningError, x);
    emptyFieldCheck(monitorResolution, monitorResolutionError, x);
    if (!monitorCheckbox.checked && document.getElementById("form-title").innerHTML != "Edit") {
        emptyFieldCheck(monitorPhoto, monitorPhotoError, x);
    }
    if (monitorCheckbox.checked) {
        monitorPhotoError.innerHTML = null;
        monitorPhoto.classList.remove("is-invalid");
        monitorPhoto.classList.remove("is-valid");
        console.log(document.getElementById("form-title").innerHTML);
    }
    if (/[^a-zA-z0-9-()]/.test(monitorName.value)) {
        x.preventDefault();
        monitorNameError.innerHTML = "You can only use special characters like '-()'";
        monitorName.classList.remove("is-valid");
        monitorName.classList.add("is-invalid");
    }
    if (/[^0-9,]/.test(monitorPrice.value)) {
        x.preventDefault();
        monitorPriceError.innerHTML = 'You can use only numbers and comma';
        monitorPrice.classList.remove("is-valid");
        monitorPrice.classList.add("is-invalid");
    }
});