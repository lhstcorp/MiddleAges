function show_hide_password(target) {
    var input = document.getElementById('password-input');
	var input_confirm = document.getElementById('password-input-confirm');
	if (input.getAttribute('type') == 'password') {
		target.classList.add('view');
		input.setAttribute('type', 'text');
	} else {
		target.classList.remove('view');
		input.setAttribute('type', 'password');
	}
	return false;
}

function show_hide_password_confirm(target) {
	var input_confirm = document.getElementById('password-input-confirm');
	if (input_confirm.getAttribute('type') == 'password') {
		target.classList.add('view');
		input_confirm.setAttribute('type', 'text');
	} else {
		target.classList.remove('view');
		input_confirm.setAttribute('type', 'password');
	}
	return false;
}
function validateRecaptcha() {
	var response = grecaptcha.getResponse();  // Получаем ответ reCAPTCHA

	if (response.length === 0) {
		alert("Пожалуйста, подтвердите, что вы не робот.");
		return false;  // Останавливаем отправку формы
	}
	return true;  // Разрешаем отправку формы
}

//function checkPasswordStrength() {
//    const password = document.getElementByName("password-input-register").value;
//    const strengthBar = document.getElementById("strengthBar");
//    const strengthText = document.getElementById("strengthText");

//    let strength = 0;

//    // Оцениваем сложность пароля:
//    // 1. Длина пароля больше 8 символов
//    if (password.length > 8) strength += 1;

//    // 2. Наличие букв верхнего регистра
//    if (/[A-Z]/.test(password)) strength += 1;

//    // 3. Наличие букв нижнего регистра
//    if (/[a-z]/.test(password)) strength += 1;

//    // 4. Наличие цифр
//    if (/[0-9]/.test(password)) strength += 1;

//    // 5. Наличие специальных символов
//    if (/[\W_]/.test(password)) strength += 1;

//    // Обновляем прогресс-бар в зависимости от сложности
//    switch (strength) {
//        case 0:
//            strengthBar.style.width = "0%";
//            strengthBar.style.backgroundColor = "red";
//            strengthText.innerText = "";
//            break;
//        case 1:
//            strengthBar.style.width = "20%";
//            strengthBar.style.backgroundColor = "red";
//            strengthText.innerText = "Very Weak";
//            break;
//        case 2:
//            strengthBar.style.width = "40%";
//            strengthBar.style.backgroundColor = "orange";
//            strengthText.innerText = "Weak";
//            break;
//        case 3:
//            strengthBar.style.width = "60%";
//            strengthBar.style.backgroundColor = "yellow";
//            strengthText.innerText = "Medium";
//            break;
//        case 4:
//            strengthBar.style.width = "80%";
//            strengthBar.style.backgroundColor = "lightgreen";
//            strengthText.innerText = "Strong";
//            break;
//        case 5:
//            strengthBar.style.width = "100%";
//            strengthBar.style.backgroundColor = "green";
//            strengthText.innerText = "Very Strong";
//            break;
//    }
//}
