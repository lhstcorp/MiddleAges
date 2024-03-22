const popupOverlay = document.getElementById("okno");
button = document.getElementById("expandChatBtn");


function showPopup() {
    /*popupOverlay.style.display = "grid";*/
    popupOverlay.style.width = "";
}

function hidePopup() {
    /*popupOverlay.style.display = "none";*/
    popupOverlay.style.width = "0px";
}

//popupOverlay.addEventListener("click", hidePopup);
//popup.addEventListener("click", showPopup);
