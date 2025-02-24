const chatForm = document.getElementById("chatForm");
const globalChatMessageInput = document.getElementById("globalChatMessageInput");
const submit = document.getElementById("sendMessageToGlobalChatButton");
submit.addEventListener("click", validate);

function validate() {
    if (chatForm.globalChatMessageInput.validity.valueMissing) {
        chatForm.globalChatMessageInput.setCustomValidity("Minimum 1 symbol");
    }
    if (chatForm.globalChatMessageInput.validity.tooLong) {
        chatForm.globalChatMessageInput.setCustomValidity("Maximum 200 symblos");
    }
    if (chatForm.globalChatMessageInput.validity.tooShort) {
        chatForm.globalChatMessageInput.setCustomValidity("Minimum 1 symbol");
    }
}
