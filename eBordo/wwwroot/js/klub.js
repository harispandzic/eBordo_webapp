function urediDodajKlub(klubID) {
    $(document).ready(function () {
        var url = "/Klub/UrediDodaj?klubID=" + klubID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function obrisiKlubModal(klubID, brojZapisa) {
    $(document).ready(function () {
        if (brojZapisa == 0) {
            notifikacija("Nema zapisa u tabeli!");
            return;
        }
        var url = "/Klub/Obrisi?klubID=" + klubID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function sortKluboveAsc() {
    $(document).ready(function () {
        window.location.href = "/Klub/Prikaz?sort=" + true;
    });
}
function obrisiKlub(klubID) {
    $(document).ready(function () {
        window.location.href = "/Klub/ObrisiSnimi?klubID=" + klubID;
    });
}
function refreshKlubove() {
    $(document).ready(function () {
        window.location.href = "/Klub/Prikaz";
    });
}
