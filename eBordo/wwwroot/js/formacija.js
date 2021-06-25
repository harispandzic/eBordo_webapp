function urediDodajFormaciju(formacijaID) {
    $(document).ready(function () {
        var url = "/Formacija/UrediDodaj?formacijaID=" + formacijaID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function obrisiFormacijuModal(formacijaID, brojZapisa) {
    $(document).ready(function () {
        if (brojZapisa == 0) {
            notifikacija("Nema zapisa u tabeli!");
            return;
        }
        var url = "/Formacija/Obrisi?formacijaID=" + formacijaID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function obrisiFormaciju(formacijaID) {
    $(document).ready(function () {
        window.location.href = "/Formacija/ObrisiSnimi?formacijaID=" + formacijaID;
    });
}
function refreshFormacije() {
    $(document).ready(function () {
        window.location.href = "/Formacija/Prikaz";
    });
}
