//inputs
const pcName = document.getElementById("pc-name-input");
const pcPrice = document.getElementById("pc-price-input");
const pcProcessor = document.getElementById("pc-processor-input");
const pcGraphicCard = document.getElementById("pc-graphiccard-input");
const pcRAM = document.getElementById("pc-ram-input");
//errors
const pcNameError = document.getElementById("pc-name-error");
const pcPriceError = document.getElementById("pc-price-error");
const pcProcessorError = document.getElementById("pc-processor-error");
const pcGraphicCardError = document.getElementById("pc-graphiccard-error");
const pcRAMError = document.getElementById("pc-ram-error");
//form
const form = document.getElementById("pc-create-form");

form.addEventListener('submit', (x) => {
    emptyFieldCheck(pcName, pcNameError, x);
    emptyFieldCheck(pcPrice, pcPriceError, x);
    emptyFieldCheck(pcProcessor, pcProcessorError, x);
    emptyFieldCheck(pcGraphicCard, pcGraphicCardError, x);
    emptyFieldCheck(pcRAM, pcRAMError, x);

});