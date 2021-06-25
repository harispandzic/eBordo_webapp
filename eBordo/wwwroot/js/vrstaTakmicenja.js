function urediDodajVrstuTakmicenja(vrstaTakmicenjaID) {
    $(document).ready(function () {
        var url = "/VrstaTakmicenja/UrediDodaj?vrstaTakmicenjaID=" + vrstaTakmicenjaID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function obrisiVrstuTakmicenjaModal(vrstaTakmicenjaID,brojZapisa) {
    $(document).ready(function () {
        if (brojZapisa == 0) {
            notifikacija("Nema zapisa u tabeli!");
            return;
        }
        var url = "/VrstaTakmicenja/Obrisi?vrstaTakmicenjaID=" + vrstaTakmicenjaID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function obrisiVrstuTakmicenja(vrstaTakmicenjaID) {
    $(document).ready(function () {
        window.location.href = "/VrstaTakmicenja/ObrisiSnimi?vrstaTakmicenjaID=" + vrstaTakmicenjaID;
    });
}
function refreshVrsteTakmicenja() {
    $(document).ready(function () {
        window.location.href = "/VrstaTakmicenja/Prikaz";
    });
}
