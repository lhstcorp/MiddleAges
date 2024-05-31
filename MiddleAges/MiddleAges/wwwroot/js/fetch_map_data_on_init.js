$(document).load(loadMapData());

function loadMapData() {
    colorizeMapLands();
    loadWars();
    loadPlayerLocations();
}

function colorizeMapLands() {
    $.ajax({
        url: 'Map/FetchLandColors',
        type: 'get',
        datatype: 'json',
        contentType: 'application/json;charset=utf-8',
        cache: false,
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                console.log("Error loading map colors");
            }
            else {
                return response;
            }
        },
        error: function (response) {
            console.log("Error loading map colors");
        }
    })
    .done(function (data) {
        if (data != null) {
            data.forEach(
                function (elem) {
                    $('#' + elem.landId.replace(' ', '_')).css("fill", elem.color);
                }
            );
        }
    })
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
    var warFlame = document.createElementNS('http://www.w3.org/2000/svg', 'svg');

    var flamePoint = svg.createSVGPoint();

    flamePoint.x = (landFromCenter[0] + landToCenter[0]) / 2 - 10;
    flamePoint.y = (landFromCenter[1] + landToCenter[1]) / 2 - 20;

    warFlame.setAttribute('x', flamePoint.x);
    warFlame.setAttribute('y', flamePoint.y);

    var flameImg = document.createElementNS('http://www.w3.org/2000/svg', 'image');
    flameImg.setAttribute('href', '../img/interface-icons/map-icons/flame.svg');
    warFlame.appendChild(flameImg);

    map.appendChild(warFlame);
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
        }
    })
}

function drawLocationsIcon(currentLand, residenceLand) {
    const currentLandPolygon = document.getElementById(currentLand);
    const residenceLandPolygon = document.getElementById(residenceLand);

    let currentLandCenter = getPolygonCenter(currentLandPolygon.animatedPoints);
    let residenceLandCenter = getPolygonCenter(residenceLandPolygon.animatedPoints);

    if (residenceLand == currentLand) {
        drawResidence(residenceLandCenter);
    }
    else {
        drawResidence(residenceLandCenter);
        drawCurrentLand(currentLandCenter);
    }
}

function drawResidence(residenceLandCenter) {
    var map = document.getElementById("global_region_map");
    var residenceLandSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');

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
