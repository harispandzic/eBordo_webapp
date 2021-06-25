function urediDodajGrad(gradID) {
    $(document).ready(function () {
        var url = "/Grad/UrediDodaj?gradID=" + gradID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function obrisiGradModal(gradID,brojZapisa) {
    $(document).ready(function () {
        if (brojZapisa == 0) {
            notifikacija("Nema zapisa u tabeli!");
            return;
        }
        var url = "/Grad/Obrisi?gradID=" + gradID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function sortGradoveAsc() {
    $(document).ready(function () {
        window.location.href = "/Grad/Prikaz?sort=" + true;
    });
}
function obrisiGrad(gradID) {
    $(document).ready(function () {
        window.location.href = "/Grad/ObrisiSnimi?gradID=" + gradID;
    });
}
function refreshGradove() {
    $(document).ready(function () {
        window.location.href = "/Grad/Prikaz";
    });
}
function filterDrzava() {
    $(document).ready(function () {
        var e = document.getElementById("filterDrzava");
        var result = e.options[e.selectedIndex].text;
        window.location.href = "/Grad/Prikaz?nazivDrzave=" + result;
    });
}

