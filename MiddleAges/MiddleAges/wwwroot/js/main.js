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
            alert("Unfortunately you haven't settled down on this land");
        });
}