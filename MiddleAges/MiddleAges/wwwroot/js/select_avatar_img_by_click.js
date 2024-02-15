var selectedImgs = document.querySelectorAll('.lhst_small_avatar_image_selection');

for (var i = 0; i < selectedImgs.length; i++) {
    selectedImgs[i].onclick = function () {
        let url = '../img/avatars/';
        $('#avatar_selected_img').attr('src', url + this.id + '.jfif');
    }
}