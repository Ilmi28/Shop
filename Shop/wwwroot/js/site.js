// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
var header = document.getElementById("header");
var sticky = header.offsetTop;

window.onscroll = function () { HeaderFixedPos() }

/* When the user clicks on the button,
toggle between hiding and showing the dropdown content */
function toggleFunc() {
    document.getElementById("dropdown-user-content").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropdown-user-menu-btn') && !event.target.matches('#user-photo-navbar')) {
        var dropdowns = document.getElementsByClassName("dropdown-user-menu-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}
function emptyFieldCheck(field, errorField, x) {
    if (field.value === "" || field.value == null) {
        x.preventDefault();
        errorField.innerHTML = "Field cannot be empty";
        field.classList.remove("is-valid");
        field.classList.add("is-invalid");
    }
    if (field.value !== "" && field.value != null) {
        errorField.innerHTML = null;
        field.classList.remove("is-invalid");
        field.classList.add("is-valid");
    }
}
function emptyFileInputCheck(fileInput, errorField, x) {
    if (fileInput.files.length == 0) {
        x.preventDefault();
        errorField.innerHTML = "You must upload file";
    }
}
function HeaderFixedPos() {
    if (window.pageYOffset > sticky) {
        header.classList.add("sticky");
    }
    else {
        header.classList.remove("sticky");
    }
}
function setDefaultPhoto(inputId, checkboxId) {
    var checkbox = document.getElementById(checkboxId);
    var photoChange = document.getElementById(inputId);
    if (checkbox.checked) {
        photoChange.disabled = true;
    }
    else {
        photoChange.disabled = false;
    }
}

function validateImage(inputId, errorId) {
    var file = document.getElementById(inputId);
    var ext = file.value.split('.').pop().toLowerCase();
    if (ext != "jpg" && ext != "png") {
        document.getElementById(errorId).innerHTML = "Invalid file extension. Only png and jpg files are allowed"
        file.value = '';
        return false;
    }
    document.getElementById("photo-error").innerHTML = null;
    return true;
}

function minLengthCheck(field, errorField, minLength, x) {
    if (field.value.length < minLength) {
        x.preventDefault();
        errorField.innerHTML = "Minimum field's length is " + minLength;
        field.classList.remove("is-valid");
        field.classList.add("is-invalid");
    }
    else {
        errorField.innerHTML = null;
        field.classList.remove("is-invalid");
        field.classList.add("is-valid");
    }
}

function maxLengthCheck(field, errorField, maxLength, x) {
    if (field.value.length > maxLength) {
        x.preventDefault();
        errorField.innerHTML = "Maximum field's length is " + maxLength;
        field.classList.add("is-invalid");
    }
    else {
        errorField.innerHTML = null;
        field.classList.remove("is-invalid");
    }
}

function emailCheck(field, errorField, x) {
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(field.value)) {
        x.preventDefault();
        errorField.innerHTML = "Email adress is not valid";
        field.classList.remove("is-valid");
        field.classList.add("is-invalid");
    }
    else {
        errorField.innerHTML = null;
        field.classList.remove("is-invalid");
        field.classList.add("is-valid");
    }
}
function floatInputCheck(field, errorfield, x) {
    if (/[^0-9,]/.test(field.value)) {
        x.preventDefault();
        field.classList.add("is-invalid");
        field.classList.remove("is-valid");
        errorfield.innerHTML = "You must write a decimal number here(1,23 for example)"
    }
}
const USER_IMAGE_INPUT = document.getElementById("change-user-photo");
const CHANGE_PHOTO_PREVIEW = document.getElementById("change-photo-preview");
USER_IMAGE_INPUT.onchange = evt => {
    const [file] = USER_IMAGE_INPUT.files;
    if (file) {
        CHANGE_PHOTO_PREVIEW.src = URL.createObjectURL(file);
    }
    if (CHANGE_PHOTO_PREVIEW.src == "") {

    }
}

const CHANGE_USER_PHOTO = document.getElementById("change-user-photo");
const CHANGE_USER_PHOTO_ERROR = document.getElementById("change-user-photo-error");
var changeUserImageForm = document.getElementById("change-user-photo-form");
changeUserImageForm.addEventListener('submit', (x) => {
    emptyFileInputCheck(CHANGE_USER_PHOTO, CHANGE_USER_PHOTO_ERROR, x);
});