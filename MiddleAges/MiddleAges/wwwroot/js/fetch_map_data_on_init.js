$(document).load(colorizeMapLands());

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
