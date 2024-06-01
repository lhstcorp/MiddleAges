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
            $('#selected_country_name').text(obj.Land.Country.Name);
            $('#selected_land_name').text(obj.Land.LandId.replace('_', ' '));
            $('#population').text(obj.Population);
            $('#lordsCount').text(obj.LordsCount);

            let url = '../img/map-regions-icons-middle-ages/';
            $('#selected_land_coat_of_arms').attr('src', url + obj.Land.LandId + '.png');
            
            $('#moveToBtn').data("selectedland", obj.Land.LandId.replace(' ', '_'));
            $('#settleBtn').data("selectedland", obj.Land.LandId.replace(' ', '_'));

            setLocationBtnEnableability(obj.Land.LandId);
        }
        else {
            $('#selected_country_name').text('');
            $('#selected_land_name').text('');
            $('#population').text('0');
            $('#lordsCount').text('0');
        }
    })
    .fail(function (data) {
        $('#selected_land_name').text('');
    });
}

function setLocationBtnEnableability(landId) {
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
            const moveToBtn = document.getElementById("moveToBtn");
            const settleBtn = document.getElementById("settleBtn");

            moveToBtn.disabled = true;
            settleBtn.disabled = true;

            if (data.currentLand != landId) {
                moveToBtn.disabled = false;
            }
            else if (data.currentLand != data.residenceLand){
                settleBtn.disabled = false;
            }
        }
    })
}