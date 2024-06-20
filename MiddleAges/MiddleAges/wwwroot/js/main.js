$(document).ready(function () {
    $(document).on("click", ".attrBtn", upgradeAttribute);
});

function restartProduction() {
    $.ajax({
        url: 'Main/RestartProduction',
        type: 'post',
        datatype: 'json',
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
            if (obj == 'Error') {
                alert("Production hasn't been started");
            }
            else {
                const productionBtn = document.getElementById("productionBtn");
                productionBtn.value = "Restart production";

                const productionStatusStr = document.getElementById("productionStatusStr");
                productionStatusStr.innerHTML = "Production will end in 24 hours"
            }
        })
        .fail(function (data) {
            alert("Unfortunately production hasn't been started");
        });
}

function upgradeAttribute() {
    var attributename = $(this).data("attributename");
    $.ajax({
        url: 'Main/UpgradeAttribute/' + attributename,
        type: 'post',
        datatype: 'json',
        data: {
            attributename: attributename
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
            if (obj == 'Error') {
                alert("Attribute wasn't upgraded");
            }
            else {
                const attributeValue = document.getElementById(attributename);
                attributeValue.innerText = obj;

                const availAttrPoints = document.getElementById("availAttrPoints");
                availAttrPoints.innerText = parseInt(availAttrPoints.innerText) - 1;

                const availAttrPointsMainSection = document.getElementById("availAttrPointsMainSection");
                availAttrPointsMainSection.innerText = parseInt(availAttrPointsMainSection.innerText) - 1;
            }
        })
        .fail(function (data) {
            alert("Unfortunately attribute wasn't upgraded");
        });
}