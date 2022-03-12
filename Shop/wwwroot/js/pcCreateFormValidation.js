//inputs
const PC_NAME = document.getElementById("pc-name-input");
const PC_PRICE = document.getElementById("pc-price-input");
const PC_PROCCESSOR = document.getElementById("pc-processor-input");
const PC_GRAPHIC_CARD = document.getElementById("pc-graphiccard-input");
const PC_RAM = document.getElementById("pc-ram-input");
//errors
const PC_NAME_ERROR = document.getElementById("pc-name-error");
const PC_PRICE_ERROR = document.getElementById("pc-price-error");
const PC_PROCCESSOR_ERROR = document.getElementById("pc-processor-error");
const PC_GRAPHIC_CARD_ERROR = document.getElementById("pc-graphiccard-error");
const PC_RAM_ERROR = document.getElementById("pc-ram-error");
//form
var form = document.getElementById("pc-create-form");

form.addEventListener('submit', (x) => {
    emptyFieldCheck(PC_NAME, PC_NAME_ERROR, x);
    emptyFieldCheck(PC_PRICE, PC_PRICE_ERROR, x);
    emptyFieldCheck(PC_PROCCESSOR, PC_PROCCESSOR_ERROR, x);
    emptyFieldCheck(PC_GRAPHIC_CARD, PC_GRAPHIC_CARD_ERROR, x);
    emptyFieldCheck(PC_RAM, PC_RAM_ERROR, x);

});