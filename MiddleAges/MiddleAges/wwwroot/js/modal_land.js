var landId;

$(document).ready(function () {
    $(document).on("click", ".m_landBtn", showModalLandDialog);
});

function showModalLandDialog() {
    landId = $(this).data("landId");

    if (landId) {
        getLandById(landId);
    }

    $('#m_land_dialog').modal('show');
}

function hideModalLandDialog() {
    $('#m_land_dialog').modal('hide');
}

function m_land_tab_changed(evt, tabName) {
    // Объявить все переменные
    var i, tabs;
    // Получить все элементы с помощью class="warcontent" и спрятать их
    tabs = document.getElementsByClassName("m_land_tab");
    for (i = 0; i < tabs.length; i++) {
        tabs[i].classList.add("d-none");
        tabs[i].classList.remove("d-flex");
    }
    // Получить все элементы с помощью class="warlinks" и удалить class "active"
    //warlinks = document.getElementsByClassName("warlinks");
    //for (i = 0; i < warlinks.length; i++) {
    //    warlinks[i].className = warlinks[i].className.replace(" active", "");
    //}

    var tab = document.getElementById(tabName);
    tab.classList.add("d-flex");
    tab.classList.remove("d-none");
    //evt.target.className += " active";
}

function getLandById(id) {
    $.ajax({
        url: 'Main/GetLandById/' + id,
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
                m_land_populateOverviewData(obj);
            }
            else {
                alert(unexpectedErrorMessage);
            }
        })
        .fail(function (data) {
            alert(unexpectedErrorMessage);
        });
}

m_land_populateOverviewData() {

}