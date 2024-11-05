let popupOverlay = document.getElementById("chatWindow");
showButton = document.getElementById("showChatBtn");
hideButton = document.getElementById("hideChatBtn");
showButton.style.display = "none";

function showPopup() {
    popupOverlay.style.display = "grid";
    hideButton.style.display = "grid";
    showButton.style.display = "none";
}
function hidePopup() {
    popupOverlay.style.display = "none";
    hideButton.style.display = "none";
    showButton.style.display = "grid";
}

function toggleDropdown() {
    const menu = document.getElementById("menuContainer");
    menu.style.display = "block";
}

document.addEventListener('click', function (event) {
    const dropdown = document.getElementById("menuContainer");
    const toggleButton = document.querySelector(".dropdown-toggle");
    if (!dropdown.contains(event.target) && !toggleButton.contains(event.target)) {
        dropdown.style.display = "none";
    }
});

