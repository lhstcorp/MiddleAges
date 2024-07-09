$(document).ready(function () {
    $(document).on("click", ".lhst_event_btn", showModalLocalEventDialog);
});

function showModalLocalEventDialog() {
    /*
    var playerId = $(this).data("playerid");

    if (playerId.length > 0) {
        getPlayerById(playerId);
    }
    */
    $('#m_local_events_dialog').modal('show');
}

function hideModalLocalEventDialog() {
    $('#m_local_events_dialog').modal('hide');
}