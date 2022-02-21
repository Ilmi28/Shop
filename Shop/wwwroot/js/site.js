﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
var header = document.getElementById("header");
var sticky = header.offsetTop;

window.onscroll = function () { HeaderFixedPos() }

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
function HeaderFixedPos() {
    if (window.pageYOffset > sticky) {
        header.classList.add("sticky");
    }
    else {
        header.classList.remove("sticky");
    }
}
function setDefaultPhoto() {
    var checkbox = document.getElementById("monitor-checkbox");
    var photoChange = document.getElementById("upload-photo");
    if (checkbox.checked) {
        photoChange.disabled = true;
    }
    else {
        photoChange.disabled = false;
    }
}

function validateImage() {
    var file = document.getElementById("upload-photo");
    var ext = file.value.split('.').pop().toLowerCase();
    if (ext != "jpg" && ext != "png") {
        document.getElementById("photo-error").innerHTML = "Invalid file extension. Only png and jpg files are allowed"
        file.value = '';
        return false;
    }
    document.getElementById("photo-error").innerHTML = null;
    return true;
}