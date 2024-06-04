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