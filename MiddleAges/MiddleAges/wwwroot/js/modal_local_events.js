const localEventsImagesUrl = '../img/local-events/';

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
    getLocalEventById("1234-5678-9101-1213");

    $('#m_local_events_dialog').modal('show');
}

function getLocalEventById(id) {
    $.ajax({
        url: 'LocalEvent/GetLocalEventById/' + id,
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
                populateModalLocalEventDialog(obj);
            }
            else {
                alert("Failed to load local event data");
            }
        })
        .fail(function (data) {
            alert("Failed to load local event data");
        });
}

function populateModalLocalEventDialog(obj) {
    $('#m_local_events_title').text(obj.LocalEvent.Title);
    $('#m_local_events_img').attr('src', localEventsImagesUrl + obj.LocalEvent.EventId + '.jpg');
    $('#m_local_events_description').text(obj.LocalEvent.Description);

    $('#m_local_events_option1').html(obj.Option1Element);
    $('#m_local_events_option2').html(obj.Option2Element);
}

function hideModalLocalEventDialog() {
    $('#m_local_events_dialog').modal('hide');
}