$(document).ready(function () {
    $(document).on("click", ".m_landBtn", showModalLandDialog);
});

function showModalLandDialog() {
    /*
    var playerId = $(this).data("playerid");

    if (playerId.length > 0) {
        getPlayerById(playerId);
    }
    */
    $('#m_land_dialog').modal('show');
}

function hideModalLandDialog() {
    $('#m_land_dialog').modal('hide');
}