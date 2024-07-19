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
