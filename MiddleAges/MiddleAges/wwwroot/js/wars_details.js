var warId;
const coatOfArmsUrl = '../img/map-regions-icons-middle-ages/';
const playerAvatarsUrl = '../img/avatars/';
const interfaceIconsUrl = '../img/interface-icons/';
const unexpectedErrorMessage = "Something went wrong";

$(document).ready(function () {
    $(document).on("click", ".warDetailsBtn", openDetailsWar);    
});

function openDetailsWar() {
    warId = $(this).data("warid");

    refreshWarData();
}

function getWarById(id) {
    $.ajax({
        url: 'War/GetWarById/' + id,
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
            populateWarData(obj);
        }
        else {
            alert(unexpectedErrorMessage);
        }
    })
    .fail(function (data) {
        alert(unexpectedErrorMessage);
    });
}

function getArmiesByWarId(id) {
    $.ajax({
        url: 'War/GetArmiesByWarId/' + id,
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
            let obj = JSON.parse(data);
            if (obj != 'NotFound') { 
                populateArmyData(obj);
                showDetailsWar();
            }
            else {
                alert(unexpectedErrorMessage);
            }
        })
        .fail(function (data) {
            alert(unexpectedErrorMessage);
            hideDetailsWar();
        });
}

function populateWarData(obj) {
    $('#countryFromName').text(obj.CountryFrom.Name);
    $('#landFromName').text(obj.LandFrom.LandId);
    $('#countryToName').text(obj.CountryTo.Name);
    $('#landToName').text(obj.LandTo.LandId);    
    
    $('#imgFrom').attr('src', coatOfArmsUrl + obj.LandFrom.LandId + '.png');
    $('#imgTo').attr('src', coatOfArmsUrl + obj.LandTo.LandId + '.png');
}

function populateArmyData(obj) {
    $('#armiesCountLeft').text(obj.AttackersArmies.length);
    $('#armiesCountRight').text(obj.DefendersArmies.length);
    $('#soldiersCountLeft').text(obj.AttackersSoldiersCount);
    $('#soldiersCountRight').text(obj.DefendersSoldiersCount);

    populateArmySideDiv("attackersDiv", obj.AttackersArmies, obj.Player); //populates attackers armies
    populateArmySideDiv("defendersDiv", obj.DefendersArmies, obj.Player); //populates defenders armies
}

function populateArmySideDiv(divName, armyList, currentPlayer) {
    const armySideDiv = document.getElementById(divName);

    for (let i = 0; i < armyList.length; i++) {
        const armyNode = document.createElement("div");

        const playerImg = document.createElement("img");
        playerImg.src = playerAvatarsUrl + armyList[i].Player.ImageURL + '.webp'
        playerImg.height = 32;
        playerImg.loading = "lazy";
        playerImg.classList.add("lhst_country_history_img");
        armyNode.appendChild(playerImg);

        const soldiersInArmy = document.createElement("p");
        soldiersInArmy.innerHTML = armyList[i].SoldiersCount;
        soldiersInArmy.classList.add("lhst_country_info_region_value");
        soldiersInArmy.classList.add("pl-1");
        armyNode.appendChild(soldiersInArmy);

        if (armyList[i].Player.Id === currentPlayer.Id) {
            const disbandArmyImg = document.createElement("img");
            disbandArmyImg.src = interfaceIconsUrl + 'red-diagonal-cross.png'
            disbandArmyImg.height = 20;
            disbandArmyImg.loading = "lazy";
            disbandArmyImg.classList.add("lhst_country_history_img");
            disbandArmyImg.classList.add("pl-1");
            disbandArmyImg.dataset.armyid = armyList[i].ArmyId;
            disbandArmyImg.classList.add("lhst_cursor_pointer");
            disbandArmyImg.onclick = disbandPlayerArmy;
            armyNode.appendChild(disbandArmyImg);
        }

        armySideDiv.appendChild(armyNode);
    }
}

function refreshWarData() {
    $('#attackersDiv').empty();
    $('#defendersDiv').empty();
    getWarById(warId);
    getArmiesByWarId(warId);
}

function sendTroopsLeftSide() {
    let soldiersCountLeftValue = $('#soldiersCountLeftValue').val();

    sendTroops(soldiersCountLeftValue);
}

function sendTroopsRightSide() {
    let soldiersCountRightValue = $('#soldiersCountRightValue').val();

    sendTroops(soldiersCountRightValue);
}

function sendTroops(soldiersCount) {
    $.ajax({
        url: 'War/SendTroops',
        type: 'post',
        datatype: 'json',
        data: {
            warId: warId,
            soldiersCount: soldiersCount},
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
            refreshWarData();
        }
        else {
            alert("No troops were sent to the battle.");
        }
    })
    .fail(function (data) {
        hideDetailsWar();
    });
}

function disbandPlayerArmy() {
    var armyId = $(this).data("armyid");

    $.ajax({
        url: 'War/DisbandPlayerArmy/' + armyId,
        type: 'post',
        datatype: 'json',
        data: {
            armyId: armyId
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
                refreshWarData();
            }
            else {
                alert("The army wasn't disbanded.");
            }
        })
        .fail(function (data) {
            hideDetailsWar();
        });
}

function showDetailsWar() {
    $('#warsDetails').modal('show');
}

function hideDetailsWar() {
    $('#warsDetails').modal('hide');
}


