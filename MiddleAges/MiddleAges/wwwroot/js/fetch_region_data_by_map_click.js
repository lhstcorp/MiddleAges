$('#global_region_map').click(function (event) {
    let id = $(event.target).attr('id');

    if ($(event.target).hasClass('map_land')) {
        fillLandSideBar(id);
    }
});

function fillLandSideBar(id) {
    $.ajax({
        url: 'Map/GetLandDataById/' + id.replace('_', ' '),
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
            $('#selected_land_name').text(obj.LandId);

            let url = '../img/map-regions-icons-middle-ages/';
            $('#selected_land_coat_of_arms').attr('src', url + obj.LandId + '.png');
            $('#moveToBtn').data("selectedland", obj.LandId);
        }
        else {
            $('#selected_land_name').text('');
        }
    })
    .fail(function (data) {
        $('#selected_land_name').text('');
    });
}

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