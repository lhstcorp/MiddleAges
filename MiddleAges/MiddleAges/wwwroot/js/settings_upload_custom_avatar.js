function validateCustomAvatarFile() {
    var input = document.getElementById('avatarFile');
    var file = input.files[0];
    var allowedExtensions = ['image/png', 'image/jpg', 'image/jpeg', 'image/webp'];
    var maxFileSize = 512 * 1024;
    const fileInfo = document.getElementById('fileInfo');
    
    if (file) {
        if (!allowedExtensions.includes(file.type)) {
            document.getElementById('fileError').innerText = 'Acceptable formats: png, jpg, jpeg, webp.';
            document.getElementById('fileError').style.display = 'block';
            input.value = ''; // Сбросить выбранный файл
            return;
        }
        
        if (file.size > maxFileSize) {
            document.getElementById('fileError').innerText = 'The file size must not exceed 512 KB.';
            document.getElementById('fileError').style.display = 'block';
            input.value = ''; // Сбросить выбранный файл
            return;
        }
        
        document.getElementById('fileError').style.display = 'none';
        const fileName = file.name;
        const fileSize = (file.size / 1024).toFixed(2);
        fileInfo.textContent = `${fileName} (${fileSize} KB)`;

        document.getElementById('uploadCustomAvatarBtn').classList.add('blinking');
    }
}

function validateCustomBannerFile() {
    var input = document.getElementById('bannerFile');
    var file = input.files[0];
    var allowedExtensions = ['image/png', 'image/jpg', 'image/jpeg', 'image/webp'];
    var maxFileSize = 512 * 1024;
    const fileInfo = document.getElementById('fileInfoBanner');

    if (file) {
        if (!allowedExtensions.includes(file.type)) {
            document.getElementById('fileErrorBanner').innerText = 'Acceptable formats: png, jpg, jpeg, webp.';
            document.getElementById('fileErrorBanner').style.display = 'block';
            input.value = ''; // Сбросить выбранный файл
            return;
        }

        if (file.size > maxFileSize) {
            document.getElementById('fileErrorBanner').innerText = 'The file size must not exceed 512 KB.';
            document.getElementById('fileErrorBanner').style.display = 'block';
            input.value = ''; // Сбросить выбранный файл
            return;
        }

        document.getElementById('fileErrorBanner').style.display = 'none';
        const fileName = file.name;
        const fileSize = (file.size / 1024).toFixed(2);
        fileInfo.textContent = `${fileName} (${fileSize} KB)`;

        document.getElementById('uploadCustomBannerBtn').classList.add('blinking');
    }
}