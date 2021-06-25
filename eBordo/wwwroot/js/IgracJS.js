function obrisiIgracaModal(igracID, brojZapisa) {
    $(document).ready(function () {
        if (brojZapisa == 0) {
            notifikacija("Nema zapisa u tabeli!");
            return;
        }
        var url = "/Igrac/Obrisi?igracID=" + igracID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}

function obrisiIgraca(igracID) {
    $(document).ready(function () {
        window.location.href = "/Igrac/ObrisiSnimi?igracID=" + igracID;
    });
}

function prikaziDetaljeIgraca(igracID) {
    $(document).ready(function () {
        var url = "/Igrac/prikaziDetalje?igracID=" + igracID;
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}

function dodajIgraca() {
    $(document).ready(function () {
        var url = "/Igrac/DodajIgraca";
        $.get(url, function (d) {
            $("#ajaxDiv").html(d);
            $("#ajaxDiv").find('.modal').modal('show');
        });
    });
}
function filterDrzava() {
    $(document).ready(function () {
        var e = document.getElementById("filterDrzava");
        var result = e.options[e.selectedIndex].text;
        window.location.href = "/Igrac/Prikaz?nazivDrzave=" + result;
    });
}
function filterPozicija() {
    $(document).ready(function () {
        var e = document.getElementById("filterPozicija");
        var result = e.options[e.selectedIndex].text;
        window.location.href = "/Igrac/Prikaz?nazivPozicije=" + result;
    });
}
function refreshIgrace() {
    $(document).ready(function () {
        window.location.href = "/Igrac/Prikaz";
    });
}


