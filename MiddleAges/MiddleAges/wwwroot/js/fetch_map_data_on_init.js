$(document).load(loadMapData());

function loadMapData() {
    colorizeMapLands();
    loadWars();
    loadPlayerLocations();
}

//function colorizeMapLands() {
//    $.ajax({
//        url: 'Map/FetchLandColors',
//        type: 'get',
//        datatype: 'json',
//        contentType: 'application/json;charset=utf-8',
//        cache: false,
//        success: function (response) {
//            if (response == null || response == undefined || response.length == 0) {
//                console.log("Error loading map colors");
//            }
//            else {
//                return response;
//            }
//        },
//        error: function (response) {
//            console.log("Error loading map colors");
//        }
//    })
//    .done(function (data) {
//        if (data != null) {
//            data.forEach(
//                function (elem) {
//                    $('#' + elem.landId.replace(' ', '_')).css("fill", elem.color);
//                }
//            );
//        }
//    })
//}
function colorizeMapLands() {
    $.ajax({
        url: 'Map/FetchLandColors',
        type: 'get',
        datatype: 'json',
        contentType: 'application/json;charset=utf-8',
        cache: false,
        success: function (response) {
            if (!response || response.length === 0) {
                console.log("Error loading map colors");
            } else {
                return response;
            }
        },
        error: function (response) {
            console.log("Error loading map colors");
        }
    })
        .done(function (data) {
            if (data != null) {
                data.forEach(function (elem) {
                    const landId = elem.landId.replace(' ', '_');
                    const color = elem.color;

                    // Создаем градиент с плавным минимальным затемнением (на 5%)
                    const darkerColor = shadeColor(color, -5); // Снижаем интенсивность изменения цвета

                    // Создаем градиент для каждого элемента
                    const gradientId = `grad_${landId}`;
                    const svgDefs = document.querySelector("svg defs") || createSVGDefs();

                    const gradient = document.createElementNS("http://www.w3.org/2000/svg", "linearGradient");
                    gradient.setAttribute("id", gradientId);
                    gradient.setAttribute("x1", "0%");
                    gradient.setAttribute("y1", "0%");
                    gradient.setAttribute("x2", "100%");
                    gradient.setAttribute("y2", "100%");

                    const stop1 = document.createElementNS("http://www.w3.org/2000/svg", "stop");
                    stop1.setAttribute("offset", "0%");
                    stop1.setAttribute("stop-color", color);

                    const stop2 = document.createElementNS("http://www.w3.org/2000/svg", "stop");
                    stop2.setAttribute("offset", "100%");
                    stop2.setAttribute("stop-color", darkerColor);

                    gradient.appendChild(stop1);
                    gradient.appendChild(stop2);

                    svgDefs.appendChild(gradient);

                    // Применяем градиент к полигону
                    $('#' + landId).css("fill", `url(#${gradientId})`);
                });
            }
        });
}

// Функция для создания более плавного темного оттенка
function shadeColor(color, percent) {
    const f = parseInt(color.slice(1), 16);
    const t = percent < 0 ? 0 : 255;
    const p = percent / 1; // Уменьшение интенсивности
    const R = f >> 16;
    const G = f >> 8 & 0x00FF;
    const B = f & 0x0000FF;

    return `#${(0x1000000 +
        (Math.round((t - R) * p + R)) * 0x10000 +
        (Math.round((t - G) * p + G)) * 0x100 +
        (Math.round((t - B) * p + B))).toString(16).slice(1)}`;
}

// Функция для создания <defs>, если его еще нет в SVG
function createSVGDefs() {
    const svg = document.querySelector("svg");
    const defs = document.createElementNS("http://www.w3.org/2000/svg", "defs");
    svg.insertBefore(defs, svg.firstChild);
    return defs;
}


function loadWars() {
    $.ajax({
        url: 'Map/FetchWars',
        type: 'get',
        datatype: 'json',
        contentType: 'application/json;charset=utf-8',
        cache: false,
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                console.log("Error loading wars");
            }
            else {
                return response;
            }
        },
        error: function (response) {
            console.log("Error loading wars");
        }
    })
    .done(function (data) {
        if (data != null) {
            data.forEach(
                function (elem) {
                    drawWarIcon(elem.war, elem.landFrom, elem.landTo);
                }
            );
        }
    })
}

function drawWarIcon(war, landFrom, landTo) {
    const landFromPolygon = document.getElementById(landFrom.landId);
    const landToPolygon = document.getElementById(landTo.landId);

    let landFromCenter = getPolygonCenter(landFromPolygon.animatedPoints);
    let landToCenter = getPolygonCenter(landToPolygon.animatedPoints);

    var map = document.getElementById("global_region_map");
    var warSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');

    var warPoint = svg.createSVGPoint();

    if (war.isRevolt == true) {
        warPoint.x = landFromCenter[0] - 10;
        warPoint.y = landFromCenter[1] - 15;
    }
    else {
        warPoint.x = (landFromCenter[0] + landToCenter[0]) / 2 - 10;
        warPoint.y = (landFromCenter[1] + landToCenter[1]) / 2 - 20;
    }
       
    warSvg.setAttribute('x', warPoint.x);
    warSvg.setAttribute('y', warPoint.y);

    var warImg = document.createElementNS('http://www.w3.org/2000/svg', 'image');

    if (war.isRevolt == true) {
        warImg.setAttribute('href', '../img/interface-icons/map-icons/revolt.svg');
    }
    else {
        warImg.setAttribute('href', '../img/interface-icons/map-icons/flame.svg');
    }

    warImg.dataset.warid = war.warId;
    warImg.classList.add("lhst_cursor_pointer", "warDetailsBtn");

    warSvg.appendChild(warImg);

    map.appendChild(warSvg);
}

function getPolygonCenter(points) {
    var minX, maxX, minY, maxY;
    for (var i = 0; i < points.length; i++) {
        minX = (points[i].x< minX || minX == null) ? points[i].x : minX;
        maxX = (points[i].x > maxX || maxX == null) ? points[i].x : maxX;
        minY = (points[i].y < minY || minY == null) ? points[i].y : minY;
        maxY = (points[i].y > maxY || maxY == null) ? points[i].y : maxY;
    }
    return [(minX + maxX) / 2, (minY + maxY) / 2];
}

function loadPlayerLocations() {
    $.ajax({
        url: 'Map/FetchPlayer',
        type: 'get',
        datatype: 'json',
        contentType: 'application/json;charset=utf-8',
        cache: false,
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                console.log("Error loading player locations");
            }
            else {
                return response;
            }
        },
        error: function (response) {
            console.log("Error loading player locations");
        }
    })
    .done(function (data) {
        if (data != null) {
            drawLocationsIcon(data.currentLand, data.residenceLand);
            initLocationBtnEnableability(data.currentLand, data.residenceLand);
        }
    })
}

function drawLocationsIcon(currentLand, residenceLand) {
    const currentLandPolygon = document.getElementById(currentLand.replace(' ', '_'));
    const residenceLandPolygon = document.getElementById(residenceLand.replace(' ', '_'));

    let currentLandCenter = getPolygonCenter(currentLandPolygon.animatedPoints);
    let residenceLandCenter = getPolygonCenter(residenceLandPolygon.animatedPoints);

    drawCurrentLand(currentLandCenter);
    drawResidence(residenceLandCenter);  

    /*
    if (residenceLand == currentLand) {
        drawCurrentLand(currentLandCenter);
        drawResidence(residenceLandCenter);        
    }
    else {
        drawResidence(residenceLandCenter);
        drawCurrentLand(currentLandCenter);
    }
    */
}

function drawResidence(residenceLandCenter) {
    var map = document.getElementById("global_region_map");
    var residenceLandSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    residenceLandSvg.id = "residenceIcon";

    var residenceLandPoint = svg.createSVGPoint();

    residenceLandPoint.x = residenceLandCenter[0] - 10;
    residenceLandPoint.y = residenceLandCenter[1] - 15;

    residenceLandSvg.setAttribute('x', residenceLandPoint.x);
    residenceLandSvg.setAttribute('y', residenceLandPoint.y);

    var residenceLandImg = document.createElementNS('http://www.w3.org/2000/svg', 'image');    
    residenceLandImg.setAttribute('href', '../img/interface-icons/map-icons/residence.svg');
    residenceLandSvg.appendChild(residenceLandImg);

    map.appendChild(residenceLandSvg);
}

function drawCurrentLand(currentLandCenter) {
    var map = document.getElementById("global_region_map");
    var currentLandSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    currentLandSvg.id = "currentLandIcon";

    var currentLandPoint = svg.createSVGPoint();

    currentLandPoint.x = currentLandCenter[0] - 10;
    currentLandPoint.y = currentLandCenter[1] - 15;

    currentLandSvg.setAttribute('x', currentLandPoint.x);
    currentLandSvg.setAttribute('y', currentLandPoint.y);

    var currentLandImg = document.createElementNS('http://www.w3.org/2000/svg', 'image');    
    currentLandImg.setAttribute('href', '../img/interface-icons/map-icons/currentland.svg');
    currentLandSvg.appendChild(currentLandImg);

    map.appendChild(currentLandSvg);
}

function initLocationBtnEnableability(currentLand, residenceLand) {
    const moveToBtn = document.getElementById("moveToBtn");
    const settleBtn = document.getElementById("settleBtn");

    moveToBtn.disabled = true;

    if (currentLand == residenceLand) {
        settleBtn.disabled = true;
    }
}
