const localEventsImagesUrl = '../img/local-events/';
var localEventId;
var localEventButtonControl;

$(document).ready(function () {
    $(document).on("click", ".lhst_event_btn", showModalLocalEventDialog);
});

$(document).ready(function () {
    $(document).on("click", ".lhst_event_option_btn", localEventOptionClicked);
});

function showModalLocalEventDialog() {    
    localEventButtonControl = $(this);
    localEventId = $(this).data("localeventid");

    getLocalEventById(localEventId);

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
/*    $('#m_local_events_asssignedDateTime').text('Event assigned ' + obj.AssignedDateTime);*/
    const dateTime = new Date(obj.AssignedDateTime); // Преобразуем строку в объект Date
    const dateOnly = dateTime.toLocaleDateString(); // Извлекаем только дату
    const timeOnly = dateTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }); // Извлекаем только часы и минуты
    $('#m_local_events_asssignedDateTime').text('Event assigned on ' + dateOnly + ' at ' + timeOnly);
    $('#m_local_events_option1').html(obj.Option1Element);
    $('#m_local_events_option2').html(obj.Option2Element);
}

function localEventOptionClicked() {
    var optionNum = $(this).data("option");

    selectLocalEventOption(optionNum);
}

function selectLocalEventOption(optionNum) {
    $.ajax({
        url: 'LocalEvent/SelectLocalEventOption',
        type: 'post',
        datatype: 'json',
        data: {
            localEventId: localEventId,
            optionNum: optionNum
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
        if (obj == 'Ok') {
            getPlayerData();
            hideLocalEventButton();
        }
        else if (obj == 'ValidationFailed') {
            alert("Not enough property to fulfill the option.");
        }
        else if (obj == 'Error') {
            alert(unexpectedErrorMessage);
        }
        hideModalLocalEventDialog();
    })
    .fail(function (data) {
        alert(unexpectedErrorMessage);
    });
}

function getPlayerData() {
    $.ajax({
        url: 'LocalEvent/GetPlayerData',
        type: 'get',
        datatype: 'json',
        contentType: 'application/json;charset=utf-8',
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
    }).done(function (data) {
        if (data != 'Error') {
            let obj = JSON.parse(data);
            refreshPlayerData(obj);
        }
        else {
            alert("Failed to load player data");
        }
    }).fail(function (data) {
        alert("Failed to load player data");
    });
}

function refreshPlayerData(obj) {
    $('#player-money').text(parseFloat(obj.Player.Money).toFixed(2));
    $('#player-recruits').text(obj.Player.RecruitAmount);
    $('#player-exp-progressbar').val(obj.ProgressbarExpNow);
    let progressbarTitle = $('#player-exp-progressbar').prop('title');
    let progressbarTitleParts = progressbarTitle.split(" / ");
    $('#player-exp-progressbar').prop('title', parseInt(obj.Player.Exp) + " / " + progressbarTitleParts[1]);
    $('#unit-0-count').text(obj.Peasants.Count);
    $('#unit-1-count').text(obj.Soldiers.Count);
    $('#recruitunitinput0').prop('max', parseInt(obj.PeasantsMaxAvailable));
    $('#recruitunitinput1').prop('max', parseInt(obj.SoldiersMaxAvailable));
    
}

function hideLocalEventButton() {
    localEventButtonControl.hide();
}

function hideModalLocalEventDialog() {
    $('#m_local_events_dialog').modal('hide');
}
