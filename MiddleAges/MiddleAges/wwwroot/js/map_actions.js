function moveToLand() {
    let landId = $('#moveToBtn').data("selectedland").replace('_', ' ');

    $.ajax({
        url: 'Map/MoveToLand/' + landId,
        type: 'post',
        datatype: 'json',
        data: {
            landId: landId
        },
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                return 'Error';
            }
            else {
                return response;
            }
        },
        error: function (response) {
            return 'Error';
        }
    })
    .done(function (data) {
        let obj = JSON.parse(data);
        if (obj == 'Error') {
            alert("Unfortunately you haven't moved to the selected land");
        }
        else {
            changeIconPosition('currentLandIcon', landId);
            setLocationBtnEnableability(landId);
        }
    })
    .fail(function (data) {
        alert("Unfortunately you haven't moved to the selected land");
    });
}

function settleDown() {
    let landId = $('#settleBtn').data("selectedland").replace('_', ' ');

    $.ajax({
        url: 'Map/SettleDown/' + landId,
        type: 'post',
        datatype: 'json',
        data: {
            landId: landId
        },
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                return 'Error';
            }
            else {
                return response;
            }
        },
        error: function (response) {
            return 'Error';
        }
    })
    .done(function (data) {
        let obj = JSON.parse(data);
        if (obj == 'Error') {
            alert("Unfortunately you haven't settled down on this land");
        }
        else {
            changeIconPosition('residenceIcon', landId);
            setLocationBtnEnableability(landId);
        }
    })
    .fail(function (data) {
        alert("Unfortunately you haven't settled down on this land");
    });
}

function changeIconPosition(iconId, landId) {    
    const landPolygon = document.getElementById(landId.replace(' ', '_'));
    let landCenter = getPolygonCenter(landPolygon.animatedPoints);    

    var landCenterPoint = svg.createSVGPoint();

    landCenterPoint.x = landCenter[0] - 10;
    landCenterPoint.y = landCenter[1] - 15;

    const locationIcon = document.getElementById(iconId);
    locationIcon.setAttribute('x', landCenterPoint.x);
    locationIcon.setAttribute('y', landCenterPoint.y);
}

function startAnUprising() {
    let landId = $('#startAnUprisingBtn').data("selectedland").replace('_', ' ');

    $.ajax({
        url: 'Map/StartAnUprising/' + landId,
        type: 'post',
        datatype: 'json',
        data: {
            landId: landId
        },
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                return 'Error';
            }
            else {
                return response;
            }
        },
        error: function (response) {
            return 'Error';
        }
    })
        .done(function (data) {
            let obj = JSON.parse(data);
            if (obj == 'Error') {
                alert("Unfortunately you haven't started the rebellion on this land");
            }
            else {
                addRevoltIcon(landId);
                setLocationBtnEnableability(landId);
            }
        })
        .fail(function (data) {
            alert("Unfortunately you haven't settled down on this land");
        });
}

function addRevoltIcon(landId) {
    const landPolygon = document.getElementById(landId.replace(' ', '_'));
    let landCenter = getPolygonCenter(landPolygon.animatedPoints);

    var landCenterPoint = svg.createSVGPoint();

    landCenterPoint.x = landCenter[0] - 10;
    landCenterPoint.y = landCenter[1] - 15;

    var map = document.getElementById("global_region_map");
    const revoltSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    revoltSvg.setAttribute('x', landCenterPoint.x);
    revoltSvg.setAttribute('y', landCenterPoint.y);

    var revoltIcon = document.createElementNS('http://www.w3.org/2000/svg', 'image');
    revoltIcon.setAttribute('href', '../img/interface-icons/map-icons/revolt.svg');
    revoltSvg.appendChild(revoltIcon);

    map.appendChild(revoltSvg);
}

function toggleTextures() {
    const isChecked = document.getElementById('texture-toggle').checked;
    const map = document.getElementById("global_region_map");
    if (isChecked) {
        map.style.filter = 'url(#filter3)';
    }
    else {
        map.style.filter = 'none'; // Отключаем текстуры
    }

    document.getElementById('texture-toggle').addEventListener('change', toggleTextures);
}

function addHeraldicIcon(landId, heraldicImagePath) {
    // Получаем полигон региона по ID
    const landPolygon = document.getElementById(landId.replace(' ', '_'));

    // Получаем центр полигона (региона)
    let landCenter = getPolygonCenter(landPolygon.animatedPoints);

    // Создаём SVGPoint для центра региона
    var landCenterPoint = svg.createSVGPoint();
    landCenterPoint.x = landCenter[0] - 15; // Корректируем позицию по X для выравнивания
    landCenterPoint.y = landCenter[1] - 15; // Корректируем позицию по Y для выравнивания

    // Создаём новый SVG элемент для изображения герба
    var map = document.getElementById("global_region_map");
    const heraldicSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    heraldicSvg.setAttribute('x', landCenterPoint.x);
    heraldicSvg.setAttribute('y', landCenterPoint.y);
    heraldicSvg.setAttribute('width', '30');  // Указываем размеры изображения
    heraldicSvg.setAttribute('height', '30'); // Указываем размеры изображения

    // Создаём элемент image и указываем путь к изображению герба
    var heraldicIcon = document.createElementNS('http://www.w3.org/2000/svg', 'image');
    heraldicIcon.setAttribute('href', heraldicImagePath); // Путь к гербу
    heraldicIcon.setAttribute('width', '30');  // Ширина герба
    heraldicIcon.setAttribute('height', '30'); // Высота герба

    // Добавляем герб на карту
    heraldicSvg.appendChild(heraldicIcon);
    map.appendChild(heraldicSvg);
}

function toggleHeraldic() {
    const isChecked = document.getElementById('heraldic-toggle').checked;
    const map = document.getElementById("global_region_map");
    if (isChecked) {
        showHeraldicIcons();
    }
    else {
        hideHeraldicIcons();
    }

    document.getElementById('heraldic-toggle').addEventListener('change', toggleHeraldic);
}

function showHeraldicIcons() {
    // Логика для отображения гербов (например, добавление элементов SVG с гербами)
    addHeraldicIcon('Hrodna', '../img/map-regions-icons-middle-ages/Hrodna.png');
    addHeraldicIcon('Minsk', '../img/map-regions-icons-middle-ages/Minsk.png');
    addHeraldicIcon('Silale', '../img/map-regions-icons-middle-ages/Silale.png');
    // Добавляйте гербы для других регионов
}

function hideHeraldicIcons() {
    // Логика для скрытия гербов (удаление или скрытие SVG элементов с гербами)
    const heraldicIcons = document.querySelectorAll("#global_region_map svg image");
    heraldicIcons.forEach(icon => icon.remove());
}

function bringToFront(landId) {
    const landPolygon = document.getElementById(landId.replace(' ', '_'));
    landPolygon.parentNode.appendChild(landPolygon);  // Перемещаем элемент в конец
}

function sendToBack(landId) {
    const landPolygon = document.getElementById(landId.replace(' ', '_'));
    const parent = landPolygon.parentNode;
    parent.insertBefore(landPolygon, parent.firstChild);  // Возвращаем элемент в начало
}

document.querySelectorAll('polygon').forEach(polygon => {
    const landId = polygon.id;  // Получаем id региона

    polygon.addEventListener('mouseenter', function () {
        bringToFront(landId);
    });

    polygon.addEventListener('mouseleave', function () {
        sendToBack(landId);
    });
});