using AspNetCore.Reporting;
using eBordo.Data;
using eBordo.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TmpReportDesigner;
using static System.Net.WebRequestMethods;

namespace eBordo.Controllers
{
    public class ReportController : Controller
    {
        private ApplicationDbContext _db;

        public ReportController(ApplicationDbContext applicationDbContext)
        {
            this._db = applicationDbContext;
        }
        public ActionResult getAll()
        {
            List<SpisakIgraca> podaci = getIgraci(_db);

            LocalReport _localReport = new LocalReport("Reports/Spisak_Igraca.rdlc");
            _localReport.AddDataSource("DataSet1", podaci);

            ReportResult result = _localReport.Execute(RenderType.Pdf);

            //return File(result.MainStream, "application/pdf");
            return File(result.MainStream, System.Net.Mime.MediaTypeNames.Application.Pdf, "Igrači_" + DateTime.Now.ToString() + ".pdf");
        }
        public static List<SpisakIgraca> getIgraci(ApplicationDbContext db)
        {
            List<SpisakIgraca> podaci = db.igraci.Select(s => new SpisakIgraca
            {
                brojDresa = "#" + s.brojDresa.ToString(),
                imePrezime = s.korisnik.imePrezime,
                drzavljanstvo = s.korisnik.drzavljanstvo.nazivDrzave,
                gradRodjenja = s.korisnik.gradRodjenja.nazivGrada,
                trzisnaVrijednost = s.trzisnaVrijednost,
                godine = s.godine,
                pozicija = s.pozicija.ToString(),
                ocjena= s.igracStatistika.prosjecnaOcjena,
                istekUgovora = s.ugovor.datumZavrsetka.ToString("dd.MM.yyyy")
            }).OrderBy(s=>s.brojDresa).ToList();

            return podaci;
        }
        public ActionResult getIgracByID(int igracID)
        {
            List<IgracDetalji> podaci = getIgracByID(_db, igracID);

            LocalReport _localReport = new LocalReport("Reports/Igrac_Detalji.rdlc");
            _localReport.AddDataSource("DataSet1", podaci);

            ReportResult result = _localReport.Execute(RenderType.Pdf);

            //return File(result.MainStream, "application/pdf");

            return File(result.MainStream, System.Net.Mime.MediaTypeNames.Application.Pdf, podaci[0].imePrezime + "_" + DateTime.Now.ToString() + ".pdf");
        }
        public static List<IgracDetalji> getIgracByID(ApplicationDbContext db, int igracID)
        {
            var igrac = db.igraci.Find(igracID);
            string reprezentativac = "";
            if (igrac.reprezentativac)
                reprezentativac = "DA";
            else
                reprezentativac = "NE";

            List<IgracDetalji> podaci = db.igraci
                .Where(s => s.igracID == igracID)
                .Select(s => new IgracDetalji
                {
                imePrezime = s.korisnik.imePrezime.ToUpper(),
                drzavljanstvo = s.korisnik.drzavljanstvo.nazivDrzave.ToUpper(),
                gradRodjenja = s.korisnik.gradRodjenja.nazivGrada.ToUpper(),
                datumRodjenja = s.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
                godine = s.godine,
                adresaPrebivalista = s.korisnik.adresaPrebivalista,
                telefon = s.korisnik.telefon,
                emailAdresa = s.korisnik.emailAdresa,
                username = s.korisnik.UserName,
                datumPristupaKlubu =s.datumPristupaKlubu.ToString("dd.MM.yyyy"),
                dosadasnjiKlubovi = s.dosadasnjiKlubovi,
                pozicija = s.pozicija.ToString(),
                boljaNoga = s.boljaNoga.ToString(),
                visina = s.visina,
                tezina = s.tezina,
                trzisnaVrijednost = s.trzisnaVrijednost,
                reprezentativac = reprezentativac,
                datumPocetka = s.ugovor.datumPocetka,
                datumZavrsetka = s.ugovor.datumZavrsetka,
                iznosPlate = s.ugovor.iznosPlate,
                napomene = s.ugovor.napomene,
                brojNastupa = s.igracStatistika.brojNastupa,
                odigraneMinute = s.igracStatistika.odigraneMinute,
                golovi = s.igracStatistika.golovi,
                asistencije = s.igracStatistika.asistencije,
                zutiKartoni = s.igracStatistika.zutiKartoni,
                crveniKartoni = s.igracStatistika.crveniKartoni,
                prosjecnaOcjena = s.igracStatistika.prosjecnaOcjena,
                kontrolaLopte = s.igracSkills.skillsStavke.kontrolaLopte,
                dribljanje = s.igracSkills.skillsStavke.dribljanje,
                dodavanje = s.igracSkills.skillsStavke.dodavanje,
                kretanje = s.igracSkills.skillsStavke.kretanje,
                brzina = s.igracSkills.skillsStavke.brzina,
                sut = s.igracSkills.skillsStavke.sut,
                snaga = s.igracSkills.skillsStavke.snaga,
                markiranje = s.igracSkills.skillsStavke.markiranje,
                klizeciStart = s.igracSkills.skillsStavke.klizeciStart,
                skok = s.igracSkills.skillsStavke.skok,
                odbrana = s.igracSkills.skillsStavke.odbrana,
                brojDresa = s.brojDresa
                }).ToList();

            return podaci;
        }
    }
}
