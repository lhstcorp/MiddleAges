function showModalLandTransferMoneyDialog() {
    landId = $('#m_land_transfer_money_show_btn').data("land").replace('_', ' ');

    if (landId) {
        m_land_transferMoney_loadDialog(landId);
        $('#m_land_transfer_money_dialog').modal('show');
    }
}

function hideModalLandTransferMoneyDialog() {
    $('#m_land_transfer_money_dialog').modal('hide');
}

function m_land_transferMoney_loadDialog(landId) {
    getLandForTransferingMoney(landId);
}

function getLandForTransferingMoney(id) {
    $.ajax({
        url: 'Map/GetLandForTransferingMoney/' + id,
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
                m_land_transferMoney_populateDialogFields(obj);
            }
            else {
                alert(unexpectedErrorMessage);
            }
        })
        .fail(function (data) {
            alert(unexpectedErrorMessage);
        });
}

function m_land_transferMoney_populateDialogFields(obj) {
    $('#m_land_transfer_money_landMoney').text(parseFloat(obj.Money).toFixed(2));
    $('#m_land_action_moneyTransferAmount').attr({
        'min': 1,
        'max': parseInt(obj.Money, 10),
        'value': parseInt(obj.Money, 10)
    });
}

function m_land_transferMoneyToCountryProcess() {
    let transferAmount = $('#m_land_action_moneyTransferAmount').val();

    if (transferAmount > 0) {
        m_land_transferMoneyToCountry(transferAmount);
    }
}

function m_land_transferMoneyToCountry(transferAmount) {
    $.ajax({
        url: 'Map/TransferMoneyToCountry',
        type: 'post',
        datatype: 'json',
        data: {
            landId: landId,
            transferAmountValue: transferAmount
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
            if (obj != 'Error') {
                hideModalLandTransferMoneyDialog();
                m_land_refreshLandData();
            }
            else {
                alert("Insufficient funds.");
            }
        })
        .fail(function (data) {
            alert("Insufficient funds.");
        });
}