document.addEventListener('DOMContentLoaded', function () {
    console.log('Login script loaded.');
});
function validateLoginForm() {
    const username = document.querySelector('input[name="username"]').value;
    const password = document.querySelector('input[name="password"]').value;
    if (!username || !password) {
        alert('Please enter both username and password.');
        return false;
    }
    return true;
}