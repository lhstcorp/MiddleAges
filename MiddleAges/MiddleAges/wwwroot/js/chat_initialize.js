
document.addEventListener('DOMContentLoaded', (event) => {

    // Append everything to parent div
    let parentElement = document.getElementById("globalChatMessagesContainer");
    parentElement.scrollTop = parentElement.scrollHeight;

    // Update timestamps
    const timeElements = document.querySelectorAll('time.lhst_chat_message_timestamp');

    // Loop through each time element
    timeElements.forEach(timeElement => {
        const utcString = $(timeElement).text();
        const localTimeString = utcString;//convertUTCToLocal(utcString);
        $(timeElement).text(localTimeString);
    });

});


function convertUTCToLocal(utcString) {
    const utcDate = new Date(utcString + " UTC");

    if (isNaN(utcDate.getTime())) {
        console.error("Invalid date:", utcString);
        return utcString; 
    }

    const options = {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
        hour12: false
    };

    return utcDate.toLocaleString(undefined, options).replaceAll(',', '').replaceAll('/','.');
}