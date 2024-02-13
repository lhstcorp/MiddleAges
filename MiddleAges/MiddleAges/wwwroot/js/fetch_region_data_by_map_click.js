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
            let obj = JSON.parse(data);
            $('#selected_land_name').text(obj.LandId);

            let url = '../img/map-regions-icons/';
            $('#selected_land_coat_of_arms').attr('src', url + obj.LandId + '.png');
        }
        else {
            $('#selected_land_name').text('');
        }
    })
    .fail(function (data) {
        $('#selected_land_name').text('');
    });
}