const popupOverlay = document.getElementById("okno");
showButton = document.getElementById("showChatBtn");
hideButton = document.getElementById("hideChatBtn");
showButton.style.display = "none";
zhopa = document.getElementById("zhopa");

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

//if (zhopa.style.width < "1000px") {
//    popupOverlay.style.display = "none";
//    hideButton.style.display = "none";
//    showButton.style.display = "grid";
//    showButton.style.zindex = 1;
//}
//popupOverlay.addEventListener("click", hidePopup);
//popup.addEventListener("click", showPopup);
