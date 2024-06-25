//$(document).load(rating('Money'));
window.addEventListener("load", rating('Total'));

function rating(attribute) {
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
    document.getElementById(attribute + "Btn").className += " active";
}