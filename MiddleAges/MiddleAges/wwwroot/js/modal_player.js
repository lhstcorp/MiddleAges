const coatOfArmsUrl = '../img/map-regions-icons-middle-ages/';
const playerAvatarsUrl = '../img/avatars/';

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
    $("#m_player_contact_information_div").empty();

    $('#m_player_playerName').text(obj.Player.UserName);
    $('#m_player_playerDescription').text(obj.PlayerDescription);
    $('#m_player_avatar').attr('src', playerAvatarsUrl + obj.Player.ImageURL + '.webp');
    $('#m_player_lvl').text("- - - - -[Lvl: " + obj.Player.Lvl + "] - - - - -");
    $('#m_player_lvl_progress_bar').val(parseInt(obj.LvlProgressBarValue));
    $('#m_player_lvl_progress_bar').attr('max', parseInt(obj.LvlProgressBarMaxValue));
    $('#m_player_lvl_progress_bar').attr('title', obj.Player.Exp + ' / ' + obj.NextLvlRequiredExp);
    $('#m_player_totalPlace').text("#" + obj.Rating.TotalPlace + " (");
    $('#m_player_expPlace').text(obj.Rating.ExpPlace + " /");
    $('#m_player_moneyPlace').text(obj.Rating.MoneyPlace + " /");
    $('#m_player_powerPlace').text(obj.Rating.WarPowerPlace + ")");
    $('#m_player_residence_land_coat_of_arms').attr('src', coatOfArmsUrl + obj.Player.ResidenceLand + '.png');    
    $('#m_player_residence_country_name').text(obj.ResidenceCountry.Name);
    $('#m_player_residence_land_name').text(obj.Player.ResidenceLand);
    $('#m_player_population').text(obj.Peasants.Count);

    if (obj.PlayerInformation.Description != null) {
        $('#m_player_notes').text(obj.PlayerInformation.Description);
    }
    else {
        $('#m_player_notes').text("No player notes");
    }
    const m_player_contact_information_div = document.getElementById("m_player_contact_information_div");

    if (obj.PlayerInformation.Vk == null
     && obj.PlayerInformation.Telegram == null
     && obj.PlayerInformation.Discord == null
     && obj.PlayerInformation.Facebook == null) {
        const noLinks = document.createElement("p");
        noLinks.innerHTML = "No contact data";
        noLinks.classList.add("font-weight-normal", "mb-0");
        m_player_contact_information_div.appendChild(noLinks);
    }
    else {
        if (obj.PlayerInformation.Vk != null) {
            const vkDiv = document.createElement("div");
            vkDiv.classList.add("lhst_scale", "d-flex");
            m_player_contact_information_div.appendChild(vkDiv);

            const vkImg = document.createElement("img");
            vkImg.src = 'img/interface-icons/vk.png';
            vkImg.loading = "lazy";
            vkImg.alt = "vk";
            vkImg.title = "vk";
            vkImg.classList.add("align-self-center", "mr-3", "lhst_share_img");
            vkDiv.appendChild(vkImg);

            const vkLink = document.createElement("p");
            vkLink.innerHTML = obj.PlayerInformation.Vk;
            vkLink.classList.add("font-weight-normal", "mb-0", "mt-1");
            vkDiv.appendChild(vkLink);
        }

        if (obj.PlayerInformation.Telegram != null) {

            const tgDiv = document.createElement("div");
            tgDiv.classList.add("lhst_scale", "d-flex");
            m_player_contact_information_div.appendChild(tgDiv);

            const tgImg = document.createElement("img");
            tgImg.src = 'img/interface-icons/telegram.png';
            tgImg.loading = "lazy";
            tgImg.alt = "telegram";
            tgImg.title = "telegram";
            tgImg.classList.add("align-self-center", "mr-3", "lhst_share_img");
            tgDiv.appendChild(tgImg);

            const tgLink = document.createElement("p");
            tgLink.innerHTML = obj.PlayerInformation.Telegram;
            tgLink.classList.add("font-weight-normal", "mb-0", "mt-1");
            tgDiv.appendChild(tgLink);
        }

        if (obj.PlayerInformation.Discord != null) {

            const dsDiv = document.createElement("div");
            dsDiv.classList.add("lhst_scale", "d-flex");
            m_player_contact_information_div.appendChild(dsDiv);

            const dsImg = document.createElement("img");
            dsImg.src = 'img/interface-icons/discord.png';
            dsImg.loading = "lazy";
            dsImg.alt = "discord";
            dsImg.title = "discord";
            dsImg.classList.add("align-self-center", "mr-3", "lhst_share_img");
            dsDiv.appendChild(dsImg);

            const dsLink = document.createElement("p");
            dsLink.innerHTML = obj.PlayerInformation.Discord;
            dsLink.classList.add("font-weight-normal", "mb-0", "mt-1");
            dsDiv.appendChild(dsLink);
        }

        if (obj.PlayerInformation.Facebook != null) {

            const fbDiv = document.createElement("div");
            fbDiv.classList.add("lhst_scale", "d-flex");
            m_player_contact_information_div.appendChild(fbDiv);

            const fbImg = document.createElement("img");
            fbImg.src = 'img/interface-icons/facebook.png';
            fbImg.loading = "lazy";
            fbImg.alt = "facebook";
            fbImg.title = "facebook";
            fbImg.classList.add("align-self-center", "mr-3", "lhst_share_img");
            fbDiv.appendChild(fbImg);

            const fbLink = document.createElement("p");
            fbLink.innerHTML = obj.PlayerInformation.Facebook;
            fbLink.classList.add("font-weight-normal", "mb-0", "mt-1");
            fbDiv.appendChild(fbLink);
        }
    }
}