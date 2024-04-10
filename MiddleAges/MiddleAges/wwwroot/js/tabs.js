function rating(evt, attribute) {
    // Объявить все переменные
    var i, tabcontent, tablinks;
    // Получить все элементы с помощью class="tabcontent" и спрятать их
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }
    // Получить все элементы с помощью class="tablinks" и удалить class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    // Показать текущую вкладку и добавить "active" класс для кнопки, которая открыла вкладку
    document.getElementById(attribute).style.display = "block";
    evt.currentTarget.className += " active";
}

function wars(evt, attribute) {
    // Объявить все переменные
    var i, warcontent, warlinks;
    // Получить все элементы с помощью class="warcontent" и спрятать их
    warcontent = document.getElementsByClassName("warcontent");
    for (i = 0; i < warcontent.length; i++) {
        warcontent[i].style.display = "none";
    }
    // Получить все элементы с помощью class="warlinks" и удалить class "active"
    warlinks = document.getElementsByClassName("warlinks");
    for (i = 0; i < warlinks.length; i++) {
        warlinks[i].className = warlinks[i].className.replace(" active", "");
    }
    // Показать текущую вкладку и добавить "active" класс для кнопки, которая открыла вкладку
    document.getElementById(attribute).style.display = "block";
    evt.currentTarget.className += " active";
}