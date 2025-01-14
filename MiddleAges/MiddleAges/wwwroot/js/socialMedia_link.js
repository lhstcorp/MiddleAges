// Функция для перехода на страницу VK с введенным ID
function redirectToVK() {
    var vkId = document.getElementById('vk').value.trim();  // Получаем значение поля input
    if (vkId) {
        // Открываем новую вкладку с ссылкой на VK, подставляя ID пользователя
        window.open('https://vk.com/' + vkId, '_blank');
    } else {
        alert("Please enter a valid VK ID.");  // Предупреждение, если поле пустое
    }
}

// Добавляем обработчик события клика для картинки
document.getElementById('vkLink').addEventListener('click', redirectToVK);


// Функция для перехода на страницу tg с введенным ID
function redirectToTG() {
    var tgUsername = document.getElementById('tg').value.trim();  // Получаем значение поля input
    if (tgUsername) {
        // Открываем новую вкладку с ссылкой на VK, подставляя ID пользователя
        window.open('https://t.me/' + tgUsername, '_blank');
    } else {
        alert("Please enter a valid Telegram ID.");  // Предупреждение, если поле пустое
    }
}

// Добавляем обработчик события клика для картинки
document.getElementById('tgLink').addEventListener('click', redirectToTG);


// Функция для перехода на страницу Discord с введенным ID пользователя
function redirectToDiscord() {
    var discordID = document.getElementById('ds').value.trim();  // Получаем значение поля input
    if (discordID) {
        // Открываем новую вкладку с ссылкой на Discord, подставляя user ID пользователя
        window.open('https://discord.com/users/' + discordID, '_blank');
    } else {
        alert("Please enter a valid Discord ID.");  // Предупреждение, если поле пустое
    }
}

// Добавляем обработчик события клика для картинки
document.getElementById('dsLink').addEventListener('click', redirectToDiscord);

// Функция для перехода на страницу Facebook с введенным Username или ID
function redirectToFacebook() {
    var facebookID = document.getElementById('fb').value.trim();  // Получаем значение поля input
    if (facebookID) {
        // Открываем новую вкладку с ссылкой на Facebook, подставляя username или ID
        window.open('https://www.facebook.com/' + facebookID, '_blank');
    } else {
        alert("Please enter a valid Facebook username or ID.");  // Предупреждение, если поле пустое
    }
}

// Добавляем обработчик события клика для картинки
document.getElementById('fbLink').addEventListener('click', redirectToFacebook);

function redirectToGrandDuchyTG() {
    var tgGrandDuchy = '+SmUblfwPtJE0OWNi';
    window.open('https://t.me/' + tgGrandDuchy, '_blank');
}

function redirectToGrandDuchyDiscord() {
    var discordGrandDuchyID = '+SmUblfwPtJE0OWNi';
    window.open('https://discord.gg/DS4dcrAg');
}

