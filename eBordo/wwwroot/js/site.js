
function notifikacija(poruka, konfEmail) {
    $(document).ready(function () {
        var options = {
            autoClose: true,
            progressBar: true,
            enableSounds: true,
            sounds: {
                info: "https://res.cloudinary.com/dxfq3iotg/video/upload/v1557233294/info.mp3",
                success: "https://res.cloudinary.com/dxfq3iotg/video/upload/v1557233524/success.mp3",
                warning: "https://res.cloudinary.com/dxfq3iotg/video/upload/v1557233563/warning.mp3",
                error: "https://res.cloudinary.com/dxfq3iotg/video/upload/v1557233574/error.mp3",
            },
        };
        var toast = new Toasty(options);
        toast.configure(options);
        if (poruka == "Uspješno ste obrisali zapis!") {
            toast.warning(poruka);
        }
        if (poruka == "Uspješno ste arhivirali zapis!") {
            toast.warning(poruka);
        }
        if (poruka == "Uspješno ste obrisali sve zapise!") {
            toast.warning(poruka);
        }
        if (poruka == "Uspješno ste uredili zapis!") {
            toast.info(poruka);
        }
        if (poruka == "Nema rezultata pretrage!") {
            toast.info(poruka);
        }
        if (poruka == "Uspješno ste dodali zapis!") {
            toast.success(poruka);
        }
        if (poruka == "Zapis već postoji!") {
            toast.error(poruka);
        }
        if (poruka == "Nema zapisa u tabeli!") {
            toast.error(poruka);
        }
        if (poruka == "Greška na serveru!") {
            toast.error(poruka);
        }
        if (poruka == "Unesi naziv države!") {
            toast.error(poruka);
        }
        if (poruka == "Mail adresa ne postoji!") {
            toast.error(poruka);
        }
        if (poruka == "PDF dokument uspješno preuzet!") {
            toast.success(poruka);
        }
        if (poruka == "Preuzimanje počinje...") {
            toast.info(poruka);
        }
        if (poruka == "Greška prilikom download-a!") {
            toast.error(poruka);
        }
        if (konfEmail == "Poslan konfirmacijski e-mail!") {
            toast.success(konfEmail);
        }
        if (poruka == "Igrač već dodan!") {
            toast.error(poruka);
        }
        if (poruka == "Greška u komunikaciji sa serverom!") {
            toast.error(poruka);
        }
        if (poruka == "Statistika je već evidentirana!") {
            toast.error(poruka);
        }
        if (poruka == "Statistika uspješno evidentirana!") {
            toast.success(poruka);
        }
        if (poruka == "Izvještaj uspješno dodan!") {
            toast.success(poruka);
        }
        if (poruka == "Izvještaj uspješno uređen!") {
            toast.info(poruka);
        }
        if (poruka == "Izvještaj nije kreiran!") {
            toast.error(poruka);
        }
        if (poruka == "Statistika uspješno uređena!") {
            toast.info(poruka);
        }
        if (poruka == "Statistika uspješno obrisana!") {
            toast.warning(poruka);
        }
        if (poruka == "Ocjena ne smije biti veća od 10!") {
            toast.warning(poruka);
        }
        if (poruka == "Uspješno su ocjenjeni svi igrači!") {
            toast.info(poruka);
        }
        if (poruka == "Statistika nije spašena, igrač nije nastupio!") {
            toast.warning(poruka);
        }
        if (poruka == "Ocjena nije evidentirana!") {
            toast.warning(poruka);
        }
        if (poruka == "Uspješno ste uredili status igrača!") {
            toast.success(poruka);
        }
        if (poruka == "Uspješno ste uredili status trenera!") {
            toast.success(poruka);
        }
        if (poruka == "Izmjena uspješno evidentirana!") {
            toast.success(poruka);
        }
        if (poruka == "Sve izmjene su evidentirane!") {
            toast.warning(poruka);
        }
        if (poruka == "Izmjena uspješno obrisana!") {
            toast.warning(poruka);
        }
        if (poruka == "Izmjena uspješno uređena!") {
            toast.info(poruka);
        }
        if (poruka == "Nije evidentirana minuta!") {
            toast.warning(poruka);
        }
        if (poruka == "Profil je uspješno izmjenjen!") {
            toast.success(poruka);
        }
        if (poruka == "Lozinka je uspješno promjenjena!") {
            toast.success(poruka);
        }
        if (poruka == "Nije moguće poslati SMS poruku svim igračima!") {
            toast.warning(poruka);
        }
        if (poruka == "Notifikacija poslana svim igračima!") {
            toast.success(poruka);
        }
        if (poruka == "E-mail poruka poslana svim igračima!") {
            toast.success(poruka);
        }
        if (poruka == "SMS poruka uspješno poslana!") {
            toast.success(poruka);
        }
        if (poruka == "Notifikacija uspješno poslana!") {
            toast.success(poruka);
        }
        if (poruka == "E-mail uspješno poslan!") {
            toast.success(poruka);
        }
        if (poruka == ("Uspješno ste poslali zahtjev!")) {
            toast.success(poruka);
        }
        if (poruka == ("Uspješno ste zaključili zahtjev!")) {
            toast.success(poruka);
        }
        if (poruka == "Zahtjev uspješno poništen!") {
            toast.warning(poruka);
        }
    });
}
$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});
function generisiPreview(input) {
    var file = input.files[0];

    if (file) {
        var reader = new FileReader();

        reader.onload = function () {
            $("#previewImg").attr("src", reader.result);
        }

        reader.readAsDataURL(file);
    }
}
