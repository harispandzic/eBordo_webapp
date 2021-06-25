using Data.Data;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels.Igrac;
using eBordo.ViewModels.Igrac.Index;
using eBordo.ViewModels.Utakmica;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.Controllers
{
    public class UtakmicaController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;

        public UtakmicaController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }
        public IActionResult Prikaz(string nazivKluba, string q, string nazivTakmicenja, string nazivVrste)
        {
            List<UtakmicaPrikazVM.Row> utakmice;

            List<string> vrsteString = new List<string>();
            var vrs = Enum.GetValues(typeof(VrstaUtakmice)).Cast<VrstaUtakmice>();
            foreach (VrstaUtakmice l in vrs)
                vrsteString.Add(l.ToString());

            List<SelectListItem> vrste = vrsteString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<SelectListItem> klubovi = db.klubovi
                .Where(s => s.nazivKluba != "Sarajevo")
                .OrderBy(a => a.nazivKluba)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivKluba,
                    Value = a.klubID.ToString()
                }).ToList();

            List<SelectListItem> takmicenja = db.vrsteTakmicenja
                .OrderBy(a => a.nazivVrsteTakmicenja)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivVrsteTakmicenja,
                    Value = a.vrstaTakmicenjaID.ToString()
                }).ToList();

            if (nazivKluba != null)
            {
                utakmice = db.utakmice
                    .Where(s => (s.domacin.nazivKluba == nazivKluba || s.gost.nazivKluba == nazivKluba) && s.statusUtakmice == StatusUtakmice.poRasporedu)
                    .Select(s => new UtakmicaPrikazVM.Row
                    {
                        utakmicaID = s.utakmicaID,
                        domacin = s.domacin.nazivKluba,
                        domacinGrb = s.domacin.grbKluba,
                        gost = s.gost.nazivKluba,
                        gostGrb = s.gost.grbKluba,
                        stadion = s.domacin.stadion.imeStadiona,
                        gradDomacina = s.domacin.grad.nazivGrada,
                        datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.datumOdigravanja,
                        satnica = s.satnica,
                        vrstaTakmicenja = s.vrstaTakmicenja.nazivVrsteTakmicenja,
                        vrstaTakmicenjaSlika = s.vrstaTakmicenja.logoTakmicenja,
                        brojDana = s.datumOdigravanja.Day - DateTime.Now.Day,
                        kapiten = s.kapiten.korisnik.imePrezime,
                        kapitenSlika = s.kapiten.slika,
                        vrijeme =s.PredvidjenoVrijeme.ToString()
                    }).OrderBy(s => s.datum).ToList();
            }
            else if (q != null)
            {
                utakmice = db.utakmice
                    .Where(s => (s.domacin.stadion.imeStadiona.StartsWith(q)) && s.statusUtakmice == StatusUtakmice.poRasporedu)
                    .Select(s => new UtakmicaPrikazVM.Row
                    {
                        utakmicaID = s.utakmicaID,
                        domacin = s.domacin.nazivKluba,
                        domacinGrb = s.domacin.grbKluba,
                        gost = s.gost.nazivKluba,
                        gostGrb = s.gost.grbKluba,
                        stadion = s.domacin.stadion.imeStadiona,
                        gradDomacina = s.domacin.grad.nazivGrada,
                        datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.datumOdigravanja,
                        satnica = s.satnica,
                        vrstaTakmicenja = s.vrstaTakmicenja.nazivVrsteTakmicenja,
                        vrstaTakmicenjaSlika = s.vrstaTakmicenja.logoTakmicenja,
                        brojDana = (s.datumOdigravanja.Date - DateTime.Now.Date).Days,
                        kapiten = s.kapiten.korisnik.imePrezime,
                        kapitenSlika = s.kapiten.slika,
                        vrijeme = s.PredvidjenoVrijeme.ToString()
                    }).OrderBy(s => s.datum).ToList();
            }
            else if (nazivTakmicenja != null)
            {
                utakmice = db.utakmice
                    .Where(s => (s.vrstaTakmicenja.nazivVrsteTakmicenja == nazivTakmicenja) && s.statusUtakmice == StatusUtakmice.poRasporedu)
                    .Select(s => new UtakmicaPrikazVM.Row
                    {
                        utakmicaID = s.utakmicaID,
                        domacin = s.domacin.nazivKluba,
                        domacinGrb = s.domacin.grbKluba,
                        gost = s.gost.nazivKluba,
                        gostGrb = s.gost.grbKluba,
                        stadion = s.domacin.stadion.imeStadiona,
                        gradDomacina = s.domacin.grad.nazivGrada,
                        datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.datumOdigravanja,
                        satnica = s.satnica,
                        vrstaTakmicenja = s.vrstaTakmicenja.nazivVrsteTakmicenja,
                        vrstaTakmicenjaSlika = s.vrstaTakmicenja.logoTakmicenja,
                        brojDana = (s.datumOdigravanja.Date - DateTime.Now.Date).Days,
                        kapiten = s.kapiten.korisnik.imePrezime,
                        kapitenSlika = s.kapiten.slika,
                        vrijeme = s.PredvidjenoVrijeme.ToString()
                    }).OrderBy(s => s.datum).ToList();
            }
            else if (nazivVrste != null)
            {
                VrstaUtakmice vrsta = (VrstaUtakmice)Enum.Parse(typeof(VrstaUtakmice), nazivVrste);

                utakmice = db.utakmice
                    .Where(a => (a.vrstaUtakmice == vrsta) && a.statusUtakmice == StatusUtakmice.poRasporedu)
                    .Select(s => new UtakmicaPrikazVM.Row
                    {
                        utakmicaID = s.utakmicaID,
                        domacin = s.domacin.nazivKluba,
                        domacinGrb = s.domacin.grbKluba,
                        gost = s.gost.nazivKluba,
                        gostGrb = s.gost.grbKluba,
                        stadion = s.domacin.stadion.imeStadiona,
                        gradDomacina = s.domacin.grad.nazivGrada,
                        datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.datumOdigravanja,
                        satnica = s.satnica,
                        vrstaTakmicenja = s.vrstaTakmicenja.nazivVrsteTakmicenja,
                        vrstaTakmicenjaSlika = s.vrstaTakmicenja.logoTakmicenja,
                        brojDana = (s.datumOdigravanja.Date - DateTime.Now.Date).Days,
                        kapiten = s.kapiten.korisnik.imePrezime,
                        kapitenSlika = s.kapiten.slika,
                        vrijeme = s.PredvidjenoVrijeme.ToString()
                    }).OrderBy(s => s.datum).ToList();
            }
            else
            {
                utakmice = db.utakmice
                    .Where(s => s.statusUtakmice == StatusUtakmice.poRasporedu)
                    .Select(s => new UtakmicaPrikazVM.Row
                    {
                        utakmicaID = s.utakmicaID,
                        domacin = s.domacin.nazivKluba,
                        domacinGrb = s.domacin.grbKluba,
                        gost = s.gost.nazivKluba,
                        gostGrb = s.gost.grbKluba,
                        stadion = s.domacin.stadion.imeStadiona,
                        statusUtakmice = s.statusUtakmice,
                        gradDomacina = s.domacin.grad.nazivGrada,
                        datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.datumOdigravanja,
                        satnica = s.satnica,
                        vrstaTakmicenja = s.vrstaTakmicenja.nazivVrsteTakmicenja,
                        vrstaTakmicenjaSlika = s.vrstaTakmicenja.logoTakmicenja,
                        brojDana = (s.datumOdigravanja.Date - DateTime.Now.Date).Days,
                        kapiten = s.kapiten.korisnik.imePrezime,
                        kapitenSlika = s.kapiten.slika,
                        vrijeme = s.PredvidjenoVrijeme.ToString()
                    }).OrderBy(s => s.datum).ToList();
            }


            UtakmicaPrikazVM model = new UtakmicaPrikazVM();
            model.nazivKluba = nazivKluba;
            model.q = q;
            model.utakmice = utakmice;
            model.klubovi = klubovi;
            model.nazivTakmicenja = nazivTakmicenja;
            model.takmicenja = takmicenja;
            model.vrste = vrste;

            if (model.utakmice.Count() == 0 && q != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.utakmice.Count() == 0 && nazivKluba != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.utakmice.Count() == 0 && nazivTakmicenja != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.utakmice.Count() == 0 && nazivVrste != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            return View(model);
        }
        public IActionResult PrikaziDetalje(int utakmicaID)
        {
            //var korisnik = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
            //     .Select(s => s.golman).Single();

            var golman = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.golman).Single();
            var lijeviBek = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.lijeviBek).Single();
            var prviStoper = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.prviStoper).Single();
            var drugiStoper = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.drugiStoper).Single();
            var desniBek = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.desniBek).Single();
            var prviZadnjiVezni = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.prviZadnjiVezni).Single();
            var drugiZadnjiVezni = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.drugiZadnjiVezni).Single();
            var prednjiVezni = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.prednjiVezni).Single();
            var lijevoKrilo = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.lijevoKrilo).Single();
            var desnoKrilo = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.desnoKrilo).Single();
            var napadac = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.prvaPostava.napadac).Single();

            var golmanKorisnik = db.igraci.Where(s => s.igracID == golman.igracID).Select(s => s.korisnik).Single();
            var lijeviBekKorisnik = db.igraci.Where(s => s.igracID == lijeviBek.igracID).Select(s => s.korisnik).Single();
            var prviStoperKorisnik = db.igraci.Where(s => s.igracID == prviStoper.igracID).Select(s => s.korisnik).Single();
            var drugiStoperKorisnik = db.igraci.Where(s => s.igracID == drugiStoper.igracID).Select(s => s.korisnik).Single();
            var desniBekKorisnik = db.igraci.Where(s => s.igracID == desniBek.igracID).Select(s => s.korisnik).Single();
            var prviZadnjiVezniKorisnik = db.igraci.Where(s => s.igracID == prviZadnjiVezni.igracID).Select(s => s.korisnik).Single();
            var drugiZadnjiVezniKorisnik = db.igraci.Where(s => s.igracID == drugiZadnjiVezni.igracID).Select(s => s.korisnik).Single();
            var prednjiVezniKorisnik = db.igraci.Where(s => s.igracID == prednjiVezni.igracID).Select(s => s.korisnik).Single();
            var lijevoKriloKorisnik = db.igraci.Where(s => s.igracID == lijevoKrilo.igracID).Select(s => s.korisnik).Single();
            var desnoKriloKorisnik = db.igraci.Where(s => s.igracID == desnoKrilo.igracID).Select(s => s.korisnik).Single();
            var napadacKorisnik = db.igraci.Where(s => s.igracID == napadac.igracID).Select(s => s.korisnik).Single();

            var golmanKlupa = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.klupa.golmanKlupa).Single();
            var odbranaKlupa = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.klupa.odbranaKlupa).Single();
            var bekKlupa = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.klupa.bekKlupa).Single();
            var veznjakKlupa = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.klupa.veznjakKlupa).Single();
            var kriloKlupa = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.klupa.kriloKlupa).Single();
            var napadacKlupa = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Select(s => s.klupa.napadacKlupa).Single();

            var golmanKlupaKorisnik = db.igraci.Where(s => s.igracID == golmanKlupa.igracID).Select(s => s.korisnik).Single();
            var odbranaKlupaKorisnik = db.igraci.Where(s => s.igracID == odbranaKlupa.igracID).Select(s => s.korisnik).Single();
            var bekKlupaKorisnik = db.igraci.Where(s => s.igracID == bekKlupa.igracID).Select(s => s.korisnik).Single();
            var veznjakKlupaKorisnik = db.igraci.Where(s => s.igracID == veznjakKlupa.igracID).Select(s => s.korisnik).Single();
            var kriloKlupaKorisnik = db.igraci.Where(s => s.igracID == kriloKlupa.igracID).Select(s => s.korisnik).Single();
            var napadacKlupaKorisnik = db.igraci.Where(s => s.igracID == napadacKlupa.igracID).Select(s => s.korisnik).Single();

            //var test = db.utakmice.Where(s => s.utakmicaID == utakmicaID)
            //    .Include(s => s.prvaPostava)
            //    .Include(s => s.prvaPostava.golman)
            //    .Include(s => s.prvaPostava.golman.korisnik).Single();


            //return test.prvaPostava.golman.brojDresa.ToString() + test.prvaPostava.golman.korisnik.imePrezime;

            UtakmicaDetaljiVM.PozvaniIgraci golmanVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = golman.igracID,
                ime = golmanKorisnik.ime,
                prezime = golmanKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.golman.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.golman.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.golman.igracStatistika.prosjecnaOcjena).Single(),

            };
            UtakmicaDetaljiVM.PozvaniIgraci lijeviBekVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = lijeviBek.igracID,
                ime = lijeviBekKorisnik.ime,
                prezime = lijeviBekKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.lijeviBek.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.lijeviBek.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.lijeviBek.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci prviStoperVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = prviStoper.igracID,
                ime = prviStoperKorisnik.ime,
                prezime = prviStoperKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prviStoper.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prviStoper.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prviStoper.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci drugiStoperVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = drugiStoper.igracID,
                ime = drugiStoperKorisnik.ime,
                prezime = drugiStoperKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.drugiStoper.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.drugiStoper.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.drugiStoper.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci desniBekVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = desniBek.igracID,
                ime = desniBekKorisnik.ime,
                prezime = desniBekKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.desniBek.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.desniBek.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.desniBek.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci prviZadnjiVezniVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = prviZadnjiVezni.igracID,
                ime = prviZadnjiVezniKorisnik.ime,
                prezime = prviZadnjiVezniKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prviZadnjiVezni.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prviZadnjiVezni.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prviZadnjiVezni.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci drugiZadnjiVezniVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = drugiZadnjiVezni.igracID,
                ime = drugiZadnjiVezniKorisnik.ime,
                prezime = drugiZadnjiVezniKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.drugiZadnjiVezni.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.drugiZadnjiVezni.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.drugiZadnjiVezni.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci prednjiVezniVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = prednjiVezni.igracID,
                ime = prednjiVezniKorisnik.ime,
                prezime = prednjiVezniKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prednjiVezni.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prednjiVezni.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.prednjiVezni.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci lijevoKriloVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = lijevoKrilo.igracID,
                ime = lijevoKriloKorisnik.ime,
                prezime = lijevoKriloKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.lijevoKrilo.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.lijevoKrilo.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.lijevoKrilo.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci desnoKriloVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = desnoKrilo.igracID,
                ime = desnoKriloKorisnik.ime,
                prezime = desnoKriloKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.desnoKrilo.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.desnoKrilo.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.desnoKrilo.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.PozvaniIgraci napadacVM = new UtakmicaDetaljiVM.PozvaniIgraci
            {
                igracID = napadac.igracID,
                ime = napadacKorisnik.ime,
                prezime = napadacKorisnik.prezime,
                slika = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.napadac.slika).Single(),
                brojDresa = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.napadac.brojDresa).Single(),
                ocjena = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.napadac.igracStatistika.prosjecnaOcjena).Single(),
            };

            UtakmicaDetaljiVM.Klupa golmanKlupaVM = new UtakmicaDetaljiVM.Klupa
            {
                igracID = golmanKlupa.igracID,
                prezime = golmanKlupaKorisnik.prezime,
                brojDresa = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.golmanKlupa.brojDresa).Single(),
                ocjena = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.golmanKlupa.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.Klupa odbranaKlupaVM = new UtakmicaDetaljiVM.Klupa
            {
                igracID = odbranaKlupa.igracID,
                prezime = odbranaKlupaKorisnik.prezime,
                brojDresa = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.odbranaKlupa.brojDresa).Single(),
                ocjena = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.odbranaKlupa.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.Klupa bekKlupaVM = new UtakmicaDetaljiVM.Klupa
            {
                igracID = bekKlupa.igracID,
                prezime = bekKlupaKorisnik.prezime,
                brojDresa = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.bekKlupa.brojDresa).Single(),
                ocjena = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.bekKlupa.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.Klupa veznjakKlupaVM = new UtakmicaDetaljiVM.Klupa
            {
                igracID = veznjakKlupa.igracID,
                prezime = veznjakKlupaKorisnik.prezime,
                brojDresa = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.veznjakKlupa.brojDresa).Single(),
                ocjena = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.veznjakKlupa.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.Klupa kriloKlupaVM = new UtakmicaDetaljiVM.Klupa
            {
                igracID = kriloKlupa.igracID,
                prezime = kriloKlupaKorisnik.prezime,
                brojDresa = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.kriloKlupa.brojDresa).Single(),
                ocjena = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.kriloKlupa.igracStatistika.prosjecnaOcjena).Single(),
            };
            UtakmicaDetaljiVM.Klupa napadacKlupaVM = new UtakmicaDetaljiVM.Klupa
            {
                igracID = napadacKlupa.igracID,
                prezime = napadacKlupaKorisnik.prezime,
                brojDresa = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.napadacKlupa.brojDresa).Single(),
                ocjena = db.klupa.Where(s => s.utakmicaID == utakmicaID)
                 .Select(s => s.napadacKlupa.igracStatistika.prosjecnaOcjena).Single(),
            };
            //golmanVM.prezime = test.prvaPostava.golman.korisnik.prezime;
            //golmanVM.ime = test.prvaPostava.golman.korisnik.ime;
            //golmanVM.brojDresa = test.prvaPostava.golman.brojDresa;

            Utakmica utakmica = db.utakmice
                .Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.garnituraDresa)
                .Include(s => s.domacin)
                .Include(s => s.domacin.domaca)
                .Include(s => s.gost)
                .Include(s => s.gost.gostujuca).Single();

            string lokacijaDomacin ="";
            string lokacijaGost ="";
            if (utakmica.domacinID == 1)
            {
                lokacijaDomacin = utakmica.garnituraDresa.lokacijaSlika;
                lokacijaGost = utakmica.gost.gostujuca.lokacijaSlika;
            }
            else if(utakmica.gostID == 1)
            {
                lokacijaDomacin = utakmica.domacin.domaca.lokacijaSlika;
                lokacijaGost = utakmica.garnituraDresa.lokacijaSlika;
            }
            
            UtakmicaDetaljiVM model = new UtakmicaDetaljiVM();
            model = db.utakmice
                .Where(s => s.utakmicaID == utakmicaID)
                .Select(s => new UtakmicaDetaljiVM
                {
                    urakmicaID = s.utakmicaID,
                    datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                    satnica = s.satnica,
                    sudija = s.sudija,
                    status = s.statusUtakmice,
                    nazivStadiona = s.domacin.stadion.imeStadiona + ", " + s.domacin.grad.nazivGrada,
                    stadionSlika = s.domacin.stadion.slikaStadiona,
                    zastavicaDrzava = s.domacin.grad.drzava.zastavaDrzave,
                    napomene = s.napomene,
                    brojUlaznica = s.brojUlaznica,
                    vrstaTakmicenja = s.vrstaTakmicenja.nazivVrsteTakmicenja,
                    slikaVrstaTakmicenja = s.vrstaTakmicenja.logoTakmicenja,
                    kapiten = s.kapiten.korisnik.imePrezime,
                    kapitenID = s.kapitenID,
                    trener = s.trener.imePrezime,
                    domacin = s.domacin.nazivKluba,
                    vrstaUtakmice = (int)s.vrstaUtakmice,
                    vrijeme = s.PredvidjenoVrijeme,
                    domacinGrb = s.domacin.grbKluba,
                    garnituraDomacin = lokacijaDomacin,
                    gost = s.gost.nazivKluba,
                    gostGrb = s.gost.grbKluba,
                    garnituraGost = lokacijaGost,
                    cijenaUlanice = s.cijenaUlaznice,
                    kapitenSlika = s.kapiten.slika,
                    trenerSlika = s.trener.slika,
                    golman = golmanVM,
                    lijeviBek = lijeviBekVM,
                    prviStoper = prviStoperVM,
                    drugiStoper = drugiStoperVM,
                    desniBek = desniBekVM,
                    prviZadnjiVezni = prviZadnjiVezniVM,
                    drugiZadnjiVezni = drugiZadnjiVezniVM,
                    prednjiVezni = prednjiVezniVM,
                    lijevoKrilo = lijevoKriloVM,
                    desnoKrilo = desnoKriloVM,
                    napadac = napadacVM,
                    golmanKlupa = golmanKlupaVM,
                    odbranaKlupa = odbranaKlupaVM,
                    bekKlupa = bekKlupaVM,
                    veznjakKlupa = veznjakKlupaVM,
                    kriloKlupa = kriloKlupaVM,
                    napdacKlupa = napadacKlupaVM
                }).Single();

            return PartialView(model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult Obrisi(int utakmicaID)
        {
            UtakmicaObrisiVM model = new UtakmicaObrisiVM();
            model.utakmicaID = utakmicaID;
            return PartialView("UtakmicaObrisi", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult ObrisiSnimi(int utakmicaID)
        {
            Utakmica utakmica = db.utakmice.Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.trener)
                .Include(s => s.trener.korisnik)
                .Single();
            PrvaPostava postave = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID).Single();
            Klupa klupa = db.klupa.Where(s => s.utakmicaID == utakmicaID).Single();

            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);
            string posiljaoc = utakmica.trener.korisnik.UserName;

            controller.posaljiPoruku("Utakmica " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + "(" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ") je obrisana!", "error",false,posiljaoc);

            db.Remove(klupa);
            db.SaveChanges();

            db.Remove(postave);
            db.SaveChanges();

            TempData["porukaInfo"] = "Uspješno ste obrisali zapis!";


            return Redirect("Prikaz");
        }
        [Autorizacija(true, false, false)]
        public IActionResult DodajUtakmicu()
        {
            UtakmicaDodajVM model = new UtakmicaDodajVM();
            List<SelectListItem> vrsteTakmicenja = db.vrsteTakmicenja
                .OrderBy(a => a.nazivVrsteTakmicenja)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivVrsteTakmicenja,
                    Value = a.vrstaTakmicenjaID.ToString()
                }).ToList();

            List<SelectListItem> klubovi = db.klubovi
                .Where(s => s.klubID != 1)
                .OrderBy(a => a.nazivKluba)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivKluba,
                    Value = a.klubID.ToString()
                }).ToList();

            List<SelectListItem> igraci = db.igraci
                .Where(a => a.status == Status.Aktivan)
                .Include(a => a.korisnik).ToList()
                .Select(a => new SelectListItem
                {
                    Text = a.korisnik.ime.Substring(0,1) + ". " + a.korisnik.prezime,
                    Value = a.igracID.ToString()
                }).OrderBy(a => a.Value).ToList();

            //Trebace isptaviti
            List<SelectListItem> treneri = db.treneri
                .Select(a => new SelectListItem
                {
                    Text = a.imePrezime,
                    Value = a.trenerID.ToString()
                }).ToList();

            List<SelectListItem> stadioni = db.stadioni
                .OrderBy(a => a.imeStadiona)
                .Select(a => new SelectListItem
                {
                    Text = a.imeStadiona,
                    Value = a.stadionID.ToString()
                }).ToList();


            List<string> VrijemeString = new List<string>();
            var vrijeme = Enum.GetValues(typeof(Vrijeme)).Cast<Vrijeme>();
            foreach (Vrijeme v in vrijeme)
                VrijemeString.Add(v.ToString());

            List<SelectListItem> vrijemeSelect = VrijemeString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            model.takmicenja = vrsteTakmicenja;
            model.igraci = igraci;
            model.treneri = treneri;
            model.klubovi = klubovi;
            model.stadioni = stadioni;
            model.vrijeme = vrijemeSelect;
            model.datumOdigravanja = DateTime.Now;

            return PartialView("DodajUtakmicu", model);
        }
        [Autorizacija(true, false, false)]
        public IActionResult SnimiDodaj(UtakmicaDodajVM model)
        {
            var tipUtakmice = Request.Form["tipUtakmice"];
            var tipGarniture = Request.Form["garnituraDresova"];

            Klub domacin = null;
            Klub gost = null;

            if (tipUtakmice == "domaća")
            {
                domacin = db.klubovi.Where(s => s.klubID == 1).Single();
                gost = db.klubovi.Where(s => s.klubID == model.protivnikID).Single();
            }
            else if(tipUtakmice == "gostujuća")
            {
                gost = db.klubovi.Where(s => s.klubID == 1).Single();
                domacin = db.klubovi.Where(s => s.klubID == model.protivnikID).Single();
            }

            List<Utakmica> utakmice = db.utakmice.ToList();
            for(int i = 0; i < utakmice.Count(); i++)
            {
                if((utakmice[i].domacin == domacin && utakmice[i].gost == gost && utakmice[i].datumOdigravanja == model.datumOdigravanja) || (utakmice[i].gost == domacin && utakmice[i].domacin == gost && utakmice[i].datumOdigravanja == model.datumOdigravanja))
                {
                    TempData["porukaInfo"] = "Zapis već postoji!";
                    return Redirect("Prikaz");
                }
            }

            Utakmica utakmica = new Utakmica();
            PrvaPostava postava = new PrvaPostava();
            postava.golmanID = model.golmanID;
            postava.lijeviBekID = model.lijeviBekID;
            postava.prviStoperID = model.prviStoperID;
            postava.drugiStoperID = model.drugiStoperID;
            postava.desniBekID = model.desniBekID;
            postava.prviZadnjiVezniID = model.prviZadnjiVezniID;
            postava.drugiZadnjiVezniID = model.drugiZadnjiVezniID;
            postava.prednjiVezniID = model.prednjiVezniID;
            postava.lijevoKriloID = model.lijevoKriloID;
            postava.desnoKriloID = model.desnoKriloID;
            postava.napadacID = model.napadacID;

            db.Add(postava);
            db.SaveChanges();
          
            Klupa klupa = new Klupa();
            klupa.golmanKlupaID = model.golmanKlupaID;
            klupa.odbranaKlupaID = model.odbranaKlupaID;
            klupa.bekKlupaID = model.bekKlupaID;
            klupa.veznjakKlupaID = model.veznjakKlupaID;
            klupa.kriloKlupaID = model.kriloKlupaID;
            klupa.napadacKlupaID = model.napadacKlupaID;

            db.Add(klupa);
            db.SaveChanges();

            Vrijeme vrijeme = (Vrijeme)Enum.Parse(typeof(Vrijeme), model.nazivVremena);

            utakmica.datumOdigravanja = model.datumOdigravanja;
            utakmica.satnica = model.satnica;
            utakmica.sudija = model.sudija;
            utakmica.napomene = model.napomene;
            utakmica.brojUlaznica = model.brojUlaznica;
            utakmica.cijenaUlaznice = model.cijenaulaznice;
            utakmica.vrstaTakmicenjaID = model.takmicenjeID;
            utakmica.kapitenID = model.kapitenID;
            utakmica.trenerID = model.trenerID;
            utakmica.domacinID = domacin.klubID;
            utakmica.gostID = gost.klubID;
            utakmica.stadionID = model.stadionID;
            utakmica.prvaPostava = postava;
            utakmica.klupa = klupa;
            utakmica.PredvidjenoVrijeme = vrijeme;

            GarnituraDresova garnitura = null;

            if (domacin.klubID == 1)
            {
                utakmica.vrstaUtakmice = VrstaUtakmice.Domaća;
                if (tipGarniture == "gostujuća")
                {
                    garnitura = db.garnitureDresova.Where(s => s.garnituraDresovaID == domacin.gostujucaID).Single();
                }
                else
                {
                    garnitura = db.garnitureDresova.Where(s => s.garnituraDresovaID == domacin.domacaID).Single();
                }
            }
            else if (gost.klubID == 1)
            {
                utakmica.vrstaUtakmice = VrstaUtakmice.Gostujuća;
                if (tipGarniture == "domaća")
                {
                    garnitura = db.garnitureDresova.Where(s => s.garnituraDresovaID == gost.domacaID).Single();
                }
                else
                {
                    garnitura = db.garnitureDresova.Where(s => s.garnituraDresovaID == gost.gostujucaID).Single();
                }
            }
            utakmica.statusUtakmice = StatusUtakmice.poRasporedu;
            utakmica.garnituraDresaID = garnitura.garnituraDresovaID;

            db.Add(utakmica);
            db.SaveChanges();

            postava.utakmicaID = utakmica.utakmicaID;
            klupa.utakmicaID = utakmica.utakmicaID;

            db.SaveChanges();

            string posiljaoc = db.treneri.Where(s => s.trenerID == model.trenerID)
                .Include(s => s.korisnik)
                .Select(s => s.korisnik.UserName)
                .Single();

            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);
            controller.posaljiPoruku("Dodana je nova utakmica: " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + "(" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ")","info",false,posiljaoc);
            
            posaljiNotifikaciju(utakmica.utakmicaID);

            TempData["porukaInfo"] = "Uspješno ste dodali zapis!";
            return Redirect("Prikaz");
        }
        [Autorizacija(true, false, false)]
        public IActionResult UrediUtakmicu(int utakmicaID)
        {
            UtakmicaDodajVM model = new UtakmicaDodajVM();

            List<SelectListItem> vrsteTakmicenja = db.vrsteTakmicenja
                .OrderBy(a => a.nazivVrsteTakmicenja)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivVrsteTakmicenja,
                    Value = a.vrstaTakmicenjaID.ToString()
                }).ToList();

            List<SelectListItem> klubovi = db.klubovi
                .Where(s => s.klubID != 1)
                .OrderBy(a => a.nazivKluba)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivKluba,
                    Value = a.klubID.ToString()
                }).ToList();

            List<SelectListItem> igraci = db.igraci
                .Include(a => a.korisnik).ToList()
                .Select(a => new SelectListItem
                {
                    Text = a.korisnik.ime.Substring(0, 1) + ". " + a.korisnik.prezime,
                    Value = a.igracID.ToString()
                }).OrderBy(a => a.Value).ToList();

            //Trebace isptaviti
            List<SelectListItem> treneri = db.treneri
                .Select(a => new SelectListItem
                {
                    Text = a.imePrezime,
                    Value = a.trenerID.ToString()
                }).ToList();

            List<SelectListItem> stadioni = db.stadioni
                .OrderBy(a => a.imeStadiona)
                .Select(a => new SelectListItem
                {
                    Text = a.imeStadiona,
                    Value = a.stadionID.ToString()
                }).ToList();


            List<string> VrijemeString = new List<string>();
            var vrijeme = Enum.GetValues(typeof(Vrijeme)).Cast<Vrijeme>();
            foreach (Vrijeme v in vrijeme)
                VrijemeString.Add(v.ToString());

            List<SelectListItem> vrijemeSelect = VrijemeString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            //Klub domacin = null;
            //Klub gost = null;

            Utakmica utakmica = db.utakmice
                .Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.domacin)
                .Include(s => s.gost).Single();

            Utakmica u = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Single();

            int protivnik = 0;

            if(u.domacinID == 1)
            {
                protivnik = u.gost.klubID;
            }
            else if(u.gostID == 1)
            {
                protivnik = u.domacin.klubID;
            }

            model = db.utakmice.Where(s => s.utakmicaID == utakmicaID)
                .Select(a => new UtakmicaDodajVM
                {
                    utakmicaID = a.utakmicaID,
                    datumOdigravanja = a.datumOdigravanja,
                    satnica = a.satnica,
                    sudija = a.sudija,
                    napomene = a.napomene,
                    brojUlaznica = a.brojUlaznica,
                    cijenaulaznice = a.cijenaUlaznice,
                    takmicenjeID = a.vrstaTakmicenjaID,
                    kapitenID = a.kapitenID,
                    trenerID = a.trenerID,
                    protivnikID = protivnik,
                    nazivVremena = a.PredvidjenoVrijeme.ToString(),
                    vrstaUtakmice = a.vrstaUtakmice,
                    stadionID = a.stadionID,
                    garnituraDresovaID = a.garnituraDresaID,
                    golmanID = a.prvaPostava.golmanID,
                    lijeviBekID = a.prvaPostava.lijeviBekID,
                    prviStoperID = a.prvaPostava.prviStoperID,
                    drugiStoperID = a.prvaPostava.drugiStoperID,
                    desniBekID = a.prvaPostava.desniBekID,
                    prviZadnjiVezniID = a.prvaPostava.prviZadnjiVezniID,
                    drugiZadnjiVezniID = a.prvaPostava.drugiZadnjiVezniID,
                    prednjiVezniID = a.prvaPostava.prednjiVezniID,
                    lijevoKriloID = a.prvaPostava.lijevoKriloID,
                    desnoKriloID = a.prvaPostava.desnoKriloID,
                    napadacID = a.prvaPostava.napadacID,
                    golmanKlupaID = a.klupa.golmanKlupaID,
                    odbranaKlupaID = a.klupa.odbranaKlupaID,
                    bekKlupaID = a.klupa.bekKlupaID,
                    veznjakKlupaID = a.klupa.veznjakKlupaID,
                    kriloKlupaID = a.klupa.kriloKlupaID,
                    napadacKlupaID = a.klupa.napadacKlupaID
                }).Single();

            model.takmicenja = vrsteTakmicenja;
            model.igraci = igraci;
            model.treneri = treneri;
            model.klubovi = klubovi;
            model.stadioni = stadioni;
            model.vrijeme = vrijemeSelect;
            model.datumOdigravanja = DateTime.Now;



            return PartialView("UrediUtakmicu", model);
        }
        [Autorizacija(true, false, false)]
        public IActionResult SnimiUredi(UtakmicaDodajVM model)
        {
            var tipGarniture = Request.Form["garnituraDresova"];

            Utakmica utakmica = db.utakmice.Where(s => s.utakmicaID == model.utakmicaID)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.trener)
                .Include(s => s.trener.korisnik)
                .Single();

            PrvaPostava postava = db.prvaPostava.Where(s => s.utakmicaID == utakmica.utakmicaID).Single();

            postava.golmanID = model.golmanID;
            postava.lijeviBekID = model.lijeviBekID;
            postava.prviStoperID = model.prviStoperID;
            postava.drugiStoperID = model.drugiStoperID;
            postava.desniBekID = model.desniBekID;
            postava.prviZadnjiVezniID = model.prviZadnjiVezniID;
            postava.drugiZadnjiVezniID = model.drugiZadnjiVezniID;
            postava.prednjiVezniID = model.prednjiVezniID;
            postava.lijevoKriloID = model.lijevoKriloID;
            postava.desnoKriloID = model.desnoKriloID;
            postava.napadacID = model.napadacID;

            db.SaveChanges();

            Klupa klupa = db.klupa.Where(s => s.utakmicaID == utakmica.utakmicaID).Single();
            klupa.golmanKlupaID = model.golmanKlupaID;
            klupa.odbranaKlupaID = model.odbranaKlupaID;
            klupa.bekKlupaID = model.bekKlupaID;
            klupa.veznjakKlupaID = model.veznjakKlupaID;
            klupa.kriloKlupaID = model.kriloKlupaID;
            klupa.napadacKlupaID = model.napadacKlupaID;

            db.SaveChanges();

            Vrijeme vrijeme = (Vrijeme)Enum.Parse(typeof(Vrijeme), model.nazivVremena);

            utakmica.datumOdigravanja = model.datumOdigravanja;
            utakmica.satnica = model.satnica;
            utakmica.sudija = model.sudija;
            utakmica.napomene = model.napomene;
            utakmica.brojUlaznica = model.brojUlaznica;
            utakmica.cijenaUlaznice = model.cijenaulaznice;
            utakmica.kapitenID = model.kapitenID;
            utakmica.trenerID = model.trenerID;
            utakmica.stadionID = model.stadionID;
            utakmica.prvaPostava = postava;
            utakmica.klupa = klupa;
            utakmica.PredvidjenoVrijeme = vrijeme;

            GarnituraDresova garnitura = null;

            Klub sarajevo = db.klubovi.Find(1);

            if (tipGarniture == "gostujuća")
            {
                garnitura = db.garnitureDresova.Where(s => s.garnituraDresovaID == sarajevo.gostujucaID).Single();
            }
            else
            {
                garnitura = db.garnitureDresova.Where(s => s.garnituraDresovaID == sarajevo.domacaID).Single();
            }

            utakmica.garnituraDresaID = garnitura.garnituraDresovaID;

            db.SaveChanges();

            postava.utakmicaID = utakmica.utakmicaID;
            klupa.utakmicaID = utakmica.utakmicaID;

            db.SaveChanges();

            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);
            string posiljaoc = utakmica.trener.korisnik.UserName;
            controller.posaljiPoruku("Utakmica " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + "(" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ") je ažurirana!", "warning",false,posiljaoc);

            TempData["porukaInfo"] = "Uspješno ste uredili zapis!";
            return Redirect("Prikaz");
        }
        public void posaljiNotifikaciju(int utakmicaID)
        {
            IzvještajController izvjestajController = new IzvještajController(db,userManager,hubContext);
            List<Igrac> prvaPostava = izvjestajController.pozvaniIgraciPrvaPostava(utakmicaID);
            List<Igrac> klupa = izvjestajController.pozvaniIgraciKlupa(utakmicaID);
            Utakmica utakmica = db.utakmice.Where(s => s.utakmicaID == utakmicaID).Include(s => s.domacin).Include(s => s.gost).Include(s => s.trener).Include(s => s.trener.korisnik).Single();

            NotifikacijaController nc = new NotifikacijaController(db, userManager, hubContext);
            string posiljaoc = utakmica.trener.korisnik.UserName;
            for (int i = 0; i < prvaPostava.Count(); i++)
            {
                Korisnik k = db.korisnici.Where(s => s.Id == prvaPostava[i].korisnikID).Single();
                string poruka = "Trener " + utakmica.trener.imePrezime + " vas je uvrstio u prvu postavu za utakmicu " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + " (" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ")";
                nc.posaljiJednom(poruka, "success", k, false,posiljaoc);
            }

            for (int i = 0; i < klupa.Count(); i++)
            {
                Korisnik k = db.korisnici.Where(s => s.Id == klupa[i].korisnikID).Single();
                string poruka = "Trener " + utakmica.trener.imePrezime + " vas je uvrstio u rezervne igrače za utakmicu " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + " (" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ")";
                nc.posaljiJednom(poruka, "success", k, false,posiljaoc);
            }
        }
        public IActionResult getNadolazecaUtakmica()
        {
            int utakmicaID = db.utakmice
                .Where(s => (s.statusUtakmice == StatusUtakmice.poRasporedu && (s.datumOdigravanja.Date > DateTime.Now.Date)))
                .OrderBy(s => s.datumOdigravanja)
                .Select(s => s.utakmicaID)
                .First();

            Utakmica utakmica = db.utakmice.Find(utakmicaID);

            ZadnjaUtakmicaPrikaz model = db.utakmice
                .Where(s => s.utakmicaID == utakmicaID)
                .Select(s => new ZadnjaUtakmicaPrikaz
                {
                    urakmicaID = s.utakmicaID,
                    datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                    domacin = s.domacin.nazivKluba,
                    gost = s.gost.nazivKluba,
                    satnica = s.satnica,
                    nazivStadiona = s.stadion.imeStadiona,
                    vrstaTakmicenja = s.vrstaTakmicenja.nazivVrsteTakmicenja,
                }).Single();

            ViewData["utakmica"] = false;

            return PartialView("getZadnjaUtakmica", model);
        }
        public IActionResult getZadnjaUtakmica()
        {
            int utakmicaID = db.izvjestaji
                .Include(s => s.utakmica)
                .OrderByDescending(s => s.utakmica.datumOdigravanja)
                .Select(s => s.utakmicaID)
                .First();

            Izvještaj izvještaj = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID).Single();

            ZadnjaUtakmicaPrikaz model = db.utakmice
                .Where(s => s.utakmicaID == utakmicaID)
                .Select(s => new ZadnjaUtakmicaPrikaz
                {
                    urakmicaID = s.utakmicaID,
                    datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                    domacin = s.domacin.nazivKluba,
                    gost = s.gost.nazivKluba,
                    goloviDomacin = izvještaj.goloviDomacih,
                    goloviGost = izvještaj.goloviGostiju,
                    rezultat = izvještaj.rezultat,
                    satnica = s.satnica,
                    nazivStadiona = s.stadion.imeStadiona,
                    vrstaTakmicenja = s.vrstaTakmicenja.nazivVrsteTakmicenja,
                }).Single();

            ViewData["utakmica"] = true;

            return PartialView("getZadnjaUtakmica", model);
        }
        public string getZadnja()
        {
            int utakmicaID = db.izvjestaji
                .Include(s => s.utakmica)
                .OrderByDescending(s => s.utakmica.datumOdigravanja)
                .Select(s => s.utakmicaID)
                .First();

            Izvještaj izvještaj = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID).Single();

            ZadnjaUtakmicaPrikaz model = db.utakmice
                .Where(s => s.utakmicaID == utakmicaID)
                .Select(s => new ZadnjaUtakmicaPrikaz
                {
                    urakmicaID = s.utakmicaID,
                    datumOdigravanja = s.datumOdigravanja.ToString("dd.MM.yyyy"),
                    domacin = s.domacin.nazivKluba,
                    gost = s.gost.nazivKluba,
                    goloviDomacin = izvještaj.goloviDomacih,
                    goloviGost = izvještaj.goloviGostiju
                }).Single();

            return model.domacin + ":" + model.gost + " " + model.goloviDomacin + ":" + model.goloviGost;
        }
        public JsonResult getDays()
        {
            BrojacVM model = new BrojacVM();

            DateTime datum = db.utakmice
                .Where(s => (s.statusUtakmice == StatusUtakmice.poRasporedu && (s.datumOdigravanja.Date > DateTime.Now.Date)))
                .OrderBy(s => s.datumOdigravanja)
                .Select(s => s.datumOdigravanja)
                .First();

            int dani = (datum.Date - DateTime.Now.Date).Days;

            string satnica = db.utakmice
                .Where(s => s.statusUtakmice == StatusUtakmice.poRasporedu)
                .OrderBy(s => s.datumOdigravanja)
                .Select(s => s.satnica)
                .First();

            model.dani = dani;
            model.satnica = satnica;

            return Json(model);

        }
        public IActionResult getIgracUtakmice(int igracID)
        {
            List<IgracUtakmice.Row> utakmice = db.nastupi
                .Where(a => (a.igracID == igracID))
                .Select(s => new IgracUtakmice.Row
                {
                    domacin = s.utakmica.domacin.nazivKluba,
                    domacinGrb = s.utakmica.domacin.grbKluba,
                    gost = s.utakmica.gost.nazivKluba,
                    gostGrb = s.utakmica.gost.grbKluba,
                    goloviDomaci = db.izvjestaji.Where(t => t.utakmicaID == s.utakmicaID).Select(t => t.goloviDomacih).Single(),
                    goloviGosti = db.izvjestaji.Where(t => t.utakmicaID == s.utakmicaID).Select(t => t.goloviGostiju).Single(),
                    rezultat = db.izvjestaji.Where(t => t.utakmicaID == s.utakmicaID).Select(t => t.rezultat).Single(),
                    datum = s.utakmica.datumOdigravanja,
                    vrstaTakmicenjaSlika = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                    minute = s.minute,
                    golovi = s.golovi,
                    asistencije = s.asistencije,
                    zutiKartoni = s.zutiKartoni ,
                    crveniKartoni = s.crveniKartoni,
                    ocjena = s.ocjena,
                    odigranaDani = (DateTime.Now.Date - s.utakmica.datumOdigravanja.Date).Days,
                }).ToList();

            IgracUtakmice model = new IgracUtakmice();
            model.utakmice = utakmice;

            return View("IgracUtakmice", model);
        }
        public float getOcjena(int igracID)
        {
            return db.igracStatistika.Where(s => s.igracID == igracID).Select(s => s.prosjecnaOcjena).Single();
        }
    }
}
