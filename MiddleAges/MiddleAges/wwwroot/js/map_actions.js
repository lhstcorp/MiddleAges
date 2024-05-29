function moveToLand() {
    let landId = $('#moveToBtn').data("selectedland");

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
    })
    .fail(function (data) {
        alert("Unfortunately you haven't moved to the selected land");
    });
}

function settleDown() {
    let landId = $('#settleBtn').data("selectedland");

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
    })
    .fail(function (data) {
        alert("Unfortunately you haven't settled down on this land");
    });
}