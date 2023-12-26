$('#global_region_map').click(function (event) {
    var id = $(event.target).attr('id');

    if ($(event.target).hasClass('map_land')) {

        fillLandSideBar(id);
    }
    
});

function fillLandSideBar(id) {
    $.ajax({
        url: 'Map/GetLandDataById/' + id,
        type: 'get',
        datatype: 'json',
        contentType: 'application/json;charset=utf-8',
        cache: false,
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
            var obj = JSON.parse(data);
            $('#selected_land_name').text(obj.LandId);
        }
        else {
            $('#selected_land_name').text('');
        }
    })
    .fail(function (data) {
        $('#selected_land_name').text('');
    });
}