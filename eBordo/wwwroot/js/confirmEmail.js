$(window).on('load', function () {
	$('#myModal').modal('show');
});
function redirectToLogin() {
	window.location.href = "/Identity/Account/Login";
}
function redirectToProfile() {
	window.location.href = "/Identity/Account/Manage";
}
function closePage() {
	window.location.href = "/Home";
}