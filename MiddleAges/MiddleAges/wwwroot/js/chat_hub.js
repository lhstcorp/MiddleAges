"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessageFromGlobalChat", function (userId, userName, profilePicUrl, messageContent, utcTimestamp) {

    // Get the formatted datetime string
    let formattedDate = formatDateTime(new Date(utcTimestamp));

    // Create the outer div
    let outerDiv = document.createElement("div");
    outerDiv.className = "align-items-start text-left d-inline-flex";

    // Create the image container div
    let imgContainerDiv = document.createElement("div");
    imgContainerDiv.className = "lhst_scale";

    // Create the image element
    let img = document.createElement("img");
    img.className = "lhst_country_history_img lhst_cursor_pointer m_playerBtn";
    img.height = 32;
    img.setAttribute("src", "/img/avatars/" + profilePicUrl + ".webp");
    img.alt = "profile picture";
    img.loading = "lazy";
    img.setAttribute("data-playerid", userId);

    // Append the image to the image container div
    imgContainerDiv.appendChild(img);

    // Create the text container div
    let textContainerDiv = document.createElement("div");
    textContainerDiv.className = "lhst_display_grid float-left";

    // Create the username paragraph
    let usernameP = document.createElement("p");
    usernameP.className = "animation-text lhst_country_history_text text-left lhst_username_text lhst_cursor_pointer m_playerBtn";
    usernameP.setAttribute("data-playerid", userId);
    usernameP.textContent = userName;

    // Create the message paragraph
    let messageP = document.createElement("p");
    messageP.className = "lhst_country_history_text text-left lhst_chat_text";
    messageP.textContent = messageContent;

    // Append the paragraphs to the text container div
    textContainerDiv.appendChild(usernameP);
    textContainerDiv.appendChild(messageP);

    // Create the time element
    let timeElement = document.createElement("time");
    timeElement.className = "mr-2";
    timeElement.setAttribute("datetime", "T09:54");
    timeElement.textContent = formattedDate;

    // Append inputs to the outer div
    outerDiv.appendChild(imgContainerDiv);
    outerDiv.appendChild(textContainerDiv);
    outerDiv.appendChild(timeElement);

    // Add separator div
    let separatorDiv = document.createElement("div");
    separatorDiv.className = "lhst_chat_line mb-1";

    // Append everything to parent div
    let parentElement = document.getElementById("globalChatMessagesContainer");
    parentElement.appendChild(outerDiv);
    parentElement.appendChild(separatorDiv);

    // Scroll chat to the bottom
    parentElement.scrollTop = parentElement.scrollHeight;
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendMessageToGlobalChatButton").addEventListener("click", function (event) {
    // Disable the send button
    triggerSendButtonCooldown();

    if (document.getElementById("globalChatMessageInput").value == "") {
        return;
    }

    // Pass message value to hub
    let message = document.getElementById("globalChatMessageInput").value;

    // Clear input
    document.getElementById("globalChatMessageInput").value = "";

    connection.invoke("SendMessageToGlobalChat", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// Put the datetime string in the correct format for display
function formatDateTime(dateTime) {
    return ("0" + dateTime.getDate()).slice(-2) + "."
        + ("0" + (dateTime.getMonth() + 1)).slice(-2) + "."
        + dateTime.getFullYear() + " "
        + ("0" + dateTime.getHours()).slice(-2) + ":"
        + ("0" + dateTime.getMinutes()).slice(-2) + ":"
        + ("0" + dateTime.getSeconds()).slice(-2); 
}

// Disable the send button for 1.5 seconds after sending message
function triggerSendButtonCooldown() {
    document.getElementById("sendMessageToGlobalChatButton").disabled = true;
    setTimeout(function () {
        document.getElementById("sendMessageToGlobalChatButton").disabled = false;
    }, 1500);
}