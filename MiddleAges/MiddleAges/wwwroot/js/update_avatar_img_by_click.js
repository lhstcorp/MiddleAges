$('#update_avatar_btn').click(function (event) {
    let url = $('#avatar_selected_img').attr('src');

    updateAvatar(url);
});

function updateAvatar(url) {
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: 'Settings/UpdateAvatar',
        data: { selectedImageId: url },
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
            $('#player_avatar').attr('src', url);
            $('#update_avatar_btn').removeClass('blinking');
        }
    })
    .fail(function (data) {
        // ToDo error log.
    });
}
