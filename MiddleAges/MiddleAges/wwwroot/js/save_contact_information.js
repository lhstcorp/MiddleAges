$('#save_contact_information_btn').click(function (event) {

    var vk = document.getElementById('vk').value;
    var tg = document.getElementById('tg').value;
    var ds = document.getElementById('ds').value;
    var fb = document.getElementById('fb').value;
    var description = document.getElementById('player_description').value;

    updateContactInformation(vk, tg, ds, fb, description);
});

function updateContactInformation(vk, tg, ds, fb, description) {
    var contactInfoStatus = document.getElementById('contactInfoStatus');

    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: 'Settings/UpdateContactInformation',
        data: {
            vk: vk,
            tg: tg,
            ds: ds,
            fb: fb,
            description: description
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
            if (data == 'OK') {
                contactInfoStatus.innerHTML = "Saved";
            }
        })
        .fail(function (data) {
            contactInfoStatus.innerHTML = "Not saved";
        });
}