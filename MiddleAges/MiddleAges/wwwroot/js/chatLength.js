const chatForm = document.getElementById("chatForm");
const messageValue = document.getElementById("messageValue");
const submit = document.getElementById("submitChat");
submit.addEventListener("click", validate);

function validate() {
    if (chatForm.messageValue.validity.valueMissing) {
        chatForm.messageValue.setCustomValidity("Minimum 1 symbol");
    }
    if (chatForm.messageValue.validity.tooLong) {
        chatForm.messageValue.setCustomValidity("Maximum 50 symblos");
    }
    if (chatForm.messageValue.validity.tooShort) {
        chatForm.messageValue.setCustomValidity("Minimum 1 symbol");
    }
}
