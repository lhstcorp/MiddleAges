var warId;

$(document).ready(function () {
    $(document).on("click", ".warDetailsBtn", openDetailsWar);    
});

function openDetailsWar() {
    warId = $(this).data("warid");

    getWarById(warId);
}

function getWarById(id) {
    $.ajax({
        url: 'War/GetWarById/' + id,
        type: 'get',
        datatype: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                return 'NotFound';
            }
            else {                
                return response;
            }
        },
        error: function (response) {
            return 'NotFound';
        }
    })
    .done(function (data) {
        if (data != 'NotFound') {
            let obj = JSON.parse(data);
            populateDialog(obj);
            showDetailsWar();
        }
        else {
            $('#selected_land_name').text('');
        }
    })
    .fail(function (data) {
        hideDetailsWar();
    });
}

function populateDialog(obj) {
    $('#countryFromName').text(obj.CountryFrom.Name);
    $('#landFromName').text(obj.LandFrom.LandId);
    $('#countryToName').text(obj.CountryTo.Name);
    $('#landToName').text(obj.LandTo.LandId);
    
    let url = '../img/map-regions-icons-middle-ages/';
    $('#imgFrom').attr('src', url + obj.LandFrom.LandId + '.png');
    $('#imgTo').attr('src', url + obj.LandTo.LandId + '.png');
}

function sendTroopsLeftSide() {
    let soldiersCountLeftValue = $('#soldiersCountLeftValue').val();

    sendTroops(soldiersCountLeftValue);
}

function sendTroopsRightSide() {
    let soldiersCountRightValue = $('#soldiersCountRightValue').val();

    sendTroops(soldiersCountRightValue);
}

function sendTroops(soldiersCount) {
    var params = new Object();
    params.warId = warId;
    params.soldiersCount = soldiersCount;

    $.ajax({
        url: 'War/SendTroops',
        type: 'post',
        datatype: 'json',
        data: {
            warId: warId,
            soldiersCount: soldiersCount},
        //data: JSON.stringify(params),
        success: function (response) {
            if (response == null || response == undefined || response.length == 0) {
                return 'NotFound';
            }
            else {
                return response;
            }
        },
        error: function (response) {
            return 'NotFound';
        }
    })
    .done(function (data) {
        if (data != 'NotFound') {
            let obj = JSON.parse(data);
            populateDialog(obj);
            showDetailsWar();
        }
        else {
            $('#selected_land_name').text('');
        }
    })
    .fail(function (data) {
        hideDetailsWar();
    });
}

function showDetailsWar() {
    $('#warsDetails').modal('show');
}

function hideDetailsWar() {
    $('#warsDetails').modal('hide');
}


