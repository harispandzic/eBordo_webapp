function urediDodajDrzavu(drzavaID) {
    $(document).ready(function () {
        var url = "/Drzava/UrediDodaj?drzavaID=" + drzavaID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function obrisiDrzavuModal(drzavaID,brojZapisa) {
    $(document).ready(function () {
        if (brojZapisa == 0) {
            notifikacija("Nema zapisa u tabeli!");
            return;
        }
        var url = "/Drzava/Obrisi?drzavaID=" + drzavaID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function sortDrzaveAsc() {
    $(document).ready(function () {
        window.location.href = "/Drzava/Prikaz?sort=" + true;
    });
}
function obrisiDrzavu(drzavaID) {
    $(document).ready(function () {
        window.location.href = "/Drzava/ObrisiSnimi?drzavaID=" + drzavaID;
    });
}
function refreshDrzave() {
    $(document).ready(function () {
        window.location.href = "/Drzava/Prikaz";
    });
}
