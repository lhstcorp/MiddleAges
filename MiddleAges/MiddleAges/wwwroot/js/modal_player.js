$(document).ready(function () {
    $(document).on("click", ".m_playerBtn", showModalPlayerDialog);
});

function showModalPlayerDialog() {
    var playerId = $(this).data("playerid");

    if (playerId.length > 0) {
        getPlayerById(playerId);
    }    

    $('#m_player_dialog').modal('show');
}
function hideModalPlayerDialog() {
    $('#m_player_dialog').modal('hide');
}

function getPlayerById(id) {
    $.ajax({
        url: 'Main/GetPlayerById/' + id,
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
                populateModalPlayerDialog(obj);
            }
            else {
                alert("Failed to load player data");
            }
        })
        .fail(function (data) {
            alert("Failed to load player data");
        });
}

function populateModalPlayerDialog(obj) {
    $('#m_player_playerName').text(obj.Player.UserName);
}