using Data.Data;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels.Igrac.Index;
using eBordo.ViewModels.Izvještaj;
using eBordo.ViewModels.Utakmica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.Controllers
{
    public class IzvještajController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;

        public IzvještajController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }
        public IActionResult Prikaz(string nazivKluba, string q, string nazivTakmicenja, string nazivVrste, string rezultat)
        {
            //Korisnik user = userManager.GetUserAsync(User).Result;

            //if (!user.isTrener)
            //{
            //    return Redirect("/Identity/Account/Login");
            //}

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

            List<string> rezultatiString = new List<string>();
            var rezultatString = Enum.GetValues(typeof(Rezultat)).Cast<Rezultat>();
            foreach (Rezultat r in rezultatString)
                rezultatiString.Add(r.ToString());

            List<SelectListItem> rezultati = rezultatiString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<IzvjestajPrikazVM.Row> odigraneUtakmice;

            if (nazivKluba != null)
            {
                odigraneUtakmice = db.izvjestaji
                    .Where(s => (s.utakmica.domacin.nazivKluba == nazivKluba || s.utakmica.gost.nazivKluba == nazivKluba) && s.utakmica.statusUtakmice == StatusUtakmice.odigrana)
                    .Select(s => new IzvjestajPrikazVM.Row
                    {
                        izvjestajID = s.izvještajID,
                        utakmicaID = s.utakmicaID,
                        domacin = s.utakmica.domacin.nazivKluba,
                        domacinGrb = s.utakmica.domacin.grbKluba,
                        gost = s.utakmica.gost.nazivKluba,
                        gostGrb = s.utakmica.gost.grbKluba,
                        goloviDomaci = s.goloviDomacih,
                        goloviGosti = s.goloviGostiju,
                        rezultat = s.rezultat,
                        igracUtakmice = s.igracUtakmica.korisnik.imePrezime,
                        igracUtakmiceSlika = s.igracUtakmica.slika,
                        stadion = s.utakmica.domacin.stadion.imeStadiona,
                        statusUtakmice = s.utakmica.statusUtakmice,
                        gradDomacina = s.utakmica.domacin.grad.nazivGrada,
                        datumOdigravanja = s.utakmica.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.utakmica.datumOdigravanja,
                        vrstaTakmicenjaSlika = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                        nastupiloIgraca = db.utakmicaIzmjena.Where(t => t.utakmicaID == s.utakmicaID).Count() + 11,
                        ocjenjenoIgraca = db.ocjene.Where(t => t.utakmicaID == s.utakmicaID).Count(),
                        odigranaDani = (DateTime.Now.Date - s.utakmica.datumOdigravanja.Date).Days,
                    }).ToList();
            }
            else if (q != null)
            {
                odigraneUtakmice = db.izvjestaji
                    .Where(s => (s.utakmica.domacin.stadion.imeStadiona.StartsWith(q)) && s.utakmica.statusUtakmice == StatusUtakmice.odigrana)
                    .Select(s => new IzvjestajPrikazVM.Row
                    {
                        izvjestajID = s.izvještajID,
                        utakmicaID = s.utakmicaID,
                        domacin = s.utakmica.domacin.nazivKluba,
                        domacinGrb = s.utakmica.domacin.grbKluba,
                        gost = s.utakmica.gost.nazivKluba,
                        gostGrb = s.utakmica.gost.grbKluba,
                        goloviDomaci = s.goloviDomacih,
                        goloviGosti = s.goloviGostiju,
                        rezultat = s.rezultat,
                        igracUtakmice = s.igracUtakmica.korisnik.imePrezime,
                        igracUtakmiceSlika = s.igracUtakmica.slika,
                        stadion = s.utakmica.domacin.stadion.imeStadiona,
                        statusUtakmice = s.utakmica.statusUtakmice,
                        gradDomacina = s.utakmica.domacin.grad.nazivGrada,
                        datumOdigravanja = s.utakmica.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.utakmica.datumOdigravanja,
                        vrstaTakmicenjaSlika = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                        nastupiloIgraca = db.utakmicaIzmjena.Where(t => t.utakmicaID == s.utakmicaID).Count() + 11,
                        ocjenjenoIgraca = db.ocjene.Where(t => t.utakmicaID == s.utakmicaID).Count(),
                        odigranaDani = (DateTime.Now.Date - s.utakmica.datumOdigravanja.Date).Days,
                    }).ToList();
            }
            else if (nazivTakmicenja != null)
            {
                odigraneUtakmice = db.izvjestaji
                    .Where(s => (s.utakmica.vrstaTakmicenja.nazivVrsteTakmicenja == nazivTakmicenja) && s.utakmica.statusUtakmice == StatusUtakmice.odigrana)
                    .Select(s => new IzvjestajPrikazVM.Row
                    {
                        izvjestajID = s.izvještajID,
                        utakmicaID = s.utakmicaID,
                        domacin = s.utakmica.domacin.nazivKluba,
                        domacinGrb = s.utakmica.domacin.grbKluba,
                        gost = s.utakmica.gost.nazivKluba,
                        gostGrb = s.utakmica.gost.grbKluba,
                        goloviDomaci = s.goloviDomacih,
                        goloviGosti = s.goloviGostiju,
                        rezultat = s.rezultat,
                        igracUtakmice = s.igracUtakmica.korisnik.imePrezime,
                        igracUtakmiceSlika = s.igracUtakmica.slika,
                        stadion = s.utakmica.domacin.stadion.imeStadiona,
                        statusUtakmice = s.utakmica.statusUtakmice,
                        gradDomacina = s.utakmica.domacin.grad.nazivGrada,
                        datumOdigravanja = s.utakmica.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.utakmica.datumOdigravanja,
                        vrstaTakmicenjaSlika = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                        nastupiloIgraca = db.utakmicaIzmjena.Where(t => t.utakmicaID == s.utakmicaID).Count() + 11,
                        ocjenjenoIgraca = db.ocjene.Where(t => t.utakmicaID == s.utakmicaID).Count(),
                        odigranaDani = (DateTime.Now.Date - s.utakmica.datumOdigravanja.Date).Days,
                    }).ToList();
            }
            else if (nazivVrste != null)
            {
                VrstaUtakmice vrsta = (VrstaUtakmice)Enum.Parse(typeof(VrstaUtakmice), nazivVrste);

                odigraneUtakmice = db.izvjestaji
                    .Where(s => (s.utakmica.vrstaUtakmice == vrsta) && s.utakmica.statusUtakmice == StatusUtakmice.odigrana)
                    .Select(s => new IzvjestajPrikazVM.Row
                    {
                        izvjestajID = s.izvještajID,
                        utakmicaID = s.utakmicaID,
                        domacin = s.utakmica.domacin.nazivKluba,
                        domacinGrb = s.utakmica.domacin.grbKluba,
                        gost = s.utakmica.gost.nazivKluba,
                        gostGrb = s.utakmica.gost.grbKluba,
                        goloviDomaci = s.goloviDomacih,
                        goloviGosti = s.goloviGostiju,
                        rezultat = s.rezultat,
                        igracUtakmice = s.igracUtakmica.korisnik.imePrezime,
                        igracUtakmiceSlika = s.igracUtakmica.slika,
                        stadion = s.utakmica.domacin.stadion.imeStadiona,
                        statusUtakmice = s.utakmica.statusUtakmice,
                        gradDomacina = s.utakmica.domacin.grad.nazivGrada,
                        datumOdigravanja = s.utakmica.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.utakmica.datumOdigravanja,
                        vrstaTakmicenjaSlika = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                        nastupiloIgraca = db.utakmicaIzmjena.Where(t => t.utakmicaID == s.utakmicaID).Count() + 11,
                        ocjenjenoIgraca = db.ocjene.Where(t => t.utakmicaID == s.utakmicaID).Count(),
                        odigranaDani = (DateTime.Now.Date - s.utakmica.datumOdigravanja.Date).Days,
                    }).ToList();
            }
            else if (rezultat != null)
            {
                Rezultat rezultatEnum = (Rezultat)Enum.Parse(typeof(Rezultat), rezultat);
                odigraneUtakmice = db.izvjestaji
                    .Where(s => (s.rezultat == rezultatEnum) && s.utakmica.statusUtakmice == StatusUtakmice.odigrana)
                    .Select(s => new IzvjestajPrikazVM.Row
                    {
                        izvjestajID = s.izvještajID,
                        utakmicaID = s.utakmicaID,
                        domacin = s.utakmica.domacin.nazivKluba,
                        domacinGrb = s.utakmica.domacin.grbKluba,
                        gost = s.utakmica.gost.nazivKluba,
                        gostGrb = s.utakmica.gost.grbKluba,
                        goloviDomaci = s.goloviDomacih,
                        goloviGosti = s.goloviGostiju,
                        rezultat = s.rezultat,
                        igracUtakmice = s.igracUtakmica.korisnik.imePrezime,
                        igracUtakmiceSlika = s.igracUtakmica.slika,
                        stadion = s.utakmica.domacin.stadion.imeStadiona,
                        statusUtakmice = s.utakmica.statusUtakmice,
                        gradDomacina = s.utakmica.domacin.grad.nazivGrada,
                        datumOdigravanja = s.utakmica.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.utakmica.datumOdigravanja,
                        vrstaTakmicenjaSlika = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                        nastupiloIgraca = db.utakmicaIzmjena.Where(t => t.utakmicaID == s.utakmicaID).Count() + 11,
                        ocjenjenoIgraca = db.ocjene.Where(t => t.utakmicaID == s.utakmicaID).Count(),
                        odigranaDani = (DateTime.Now.Date - s.utakmica.datumOdigravanja.Date).Days,
                    }).ToList();
            }
            else
            {
                odigraneUtakmice = db.izvjestaji
                    .Where(s => s.utakmica.statusUtakmice == StatusUtakmice.odigrana)
                    .Select(s => new IzvjestajPrikazVM.Row
                    {
                        izvjestajID = s.izvještajID,
                        utakmicaID = s.utakmicaID,
                        domacin = s.utakmica.domacin.nazivKluba,
                        domacinGrb = s.utakmica.domacin.grbKluba,
                        gost = s.utakmica.gost.nazivKluba,
                        gostGrb = s.utakmica.gost.grbKluba,
                        goloviDomaci = s.goloviDomacih,
                        goloviGosti = s.goloviGostiju,
                        rezultat = s.rezultat,
                        igracUtakmice = s.igracUtakmica.korisnik.imePrezime,
                        igracUtakmiceSlika = s.igracUtakmica.slika,
                        stadion = s.utakmica.domacin.stadion.imeStadiona,
                        statusUtakmice = s.utakmica.statusUtakmice,
                        gradDomacina = s.utakmica.domacin.grad.nazivGrada,
                        datumOdigravanja = s.utakmica.datumOdigravanja.ToString("dd.MM.yyyy"),
                        datum = s.utakmica.datumOdigravanja,
                        vrstaTakmicenjaSlika = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                        nastupiloIgraca = db.utakmicaIzmjena.Where(t => t.utakmicaID == s.utakmicaID).Count() + 11,
                        ocjenjenoIgraca = db.ocjene.Where(t => t.utakmicaID == s.utakmicaID).Count(),
                        odigranaDani = (DateTime.Now.Date - s.utakmica.datumOdigravanja.Date).Days,
                    }).ToList();
            }

            IzvjestajPrikazVM model = new IzvjestajPrikazVM();
            model.nazivKluba = nazivKluba;
            model.q = q;
            model.utakmice = odigraneUtakmice;
            model.klubovi = klubovi;
            model.nazivTakmicenja = nazivTakmicenja;
            model.takmicenja = takmicenja;
            model.vrste = vrste;
            model.rezultati = rezultati;

            if (model.utakmice.Count() == 0 && q != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.utakmice.Count() == 0 && nazivKluba != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.utakmice.Count() == 0 && nazivTakmicenja != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.utakmice.Count() == 0 && nazivVrste != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.utakmice.Count() == 0 && rezultat != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            return View(model);
        }
        [Autorizacija(true, false, false)]
        public string snimiDodaj(IzvjestajKreirajVM model)
        {
            List<Izvještaj> izvjestaji = db.izvjestaji.ToList();
            Izvještaj izvještaj;
            bool postoji = false;

            for (int i = 0; i < izvjestaji.Count(); i++)
            {
                if (izvjestaji[i].utakmicaID == model.utakmicaID)
                {
                    postoji = true;
                    break;
                }
            }

            if (postoji)
            {
                izvještaj = db.izvjestaji.Where(s => s.utakmicaID == model.utakmicaID).Single();
            }
            else
            {
                izvještaj = new Izvještaj();
            }
            Vrijeme vrijeme = (Vrijeme)Enum.Parse(typeof(Vrijeme), model.nazivVremena);


            Utakmica u = db.utakmice.Where(s => s.utakmicaID == model.utakmicaID)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.trener)
                .Include(s => s.trener.korisnik)
                .Single();
            PrvaPostava postava = db.prvaPostava.Where(s => s.utakmicaID == u.utakmicaID).Single();
            izvještaj.utakmica = u;
            izvještaj.trenerID = u.trenerID;
            izvještaj.igracUtakmicaID = model.igracUtakmiceID;
            izvještaj.goloviGostiju = model.goloviGostiju;
            izvještaj.goloviDomacih = model.goloviDomacih;
            izvještaj.vrijeme = vrijeme;
            izvještaj.delegatUtakmice = model.delegatUtakmice;
            izvještaj.brojGledalaca = model.brojGledalaca;
            izvještaj.datumKreiranja = DateTime.Now;
            izvještaj.zapisnik = model.zapisnik;
            izvještaj.youtubeVideoID = model.youTubeVideoID;

            if (model.slikaNew != null)
            {
                string ekstenzija = Path.GetExtension(model.slikaNew.FileName);
                string contentType = model.slikaNew.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.slikaNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                izvještaj.slikaSaUtakmice = filename;
            }

            if (!postoji)
            {
                db.Add(izvještaj);
            }

            db.SaveChanges();

            if (izvještaj.utakmica.domacinID == 1)
            {
                if (izvještaj.goloviDomacih > izvještaj.goloviGostiju)
                {
                    izvještaj.rezultat = Rezultat.Pobjeda;
                }
                else if (izvještaj.goloviDomacih < izvještaj.goloviGostiju)
                {
                    izvještaj.rezultat = Rezultat.Poraz;
                }
                else
                {
                    izvještaj.rezultat = Rezultat.Nerješeno;
                }
            }
            else
            {
                if (izvještaj.goloviDomacih < izvještaj.goloviGostiju)
                {
                    izvještaj.rezultat = Rezultat.Pobjeda;
                }
                else if (izvještaj.goloviDomacih > izvještaj.goloviGostiju)
                {
                    izvještaj.rezultat = Rezultat.Poraz;
                }
                else
                {
                    izvještaj.rezultat = Rezultat.Nerješeno;
                }
            }
            izvještaj.utakmica.statusUtakmice = StatusUtakmice.odigrana;
            izvještaj.nastupi = new List<UtakmicaNastupi>();
            izvještaj.ocjene = new List<UtakmicaOcjene>();
            db.SaveChanges();

            TrenerStatistika statistika = db.treneri.Where(s => s.trenerID == izvještaj.trenerID).Include(t => t.trenerStatistika)
                .Select(s => s.trenerStatistika)
                .Single();

            statistika.brojUtakmica++;
            if (izvještaj.rezultat == Rezultat.Pobjeda)
            {
                statistika.brojPobjeda++;
            }
            else if (izvještaj.rezultat == Rezultat.Nerješeno)
            {
                statistika.brojNerjesenih++;
            }
            else if (izvještaj.rezultat == Rezultat.Poraz)
            {
                statistika.brojPoraza++;
            }

            db.SaveChanges();

            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);


            Korisnik korisnik = db.igraci.Where(s => s.igracID == izvještaj.igracUtakmicaID).Select(s => s.korisnik).Single();

            SMSController sms = new SMSController(db, userManager);
            string tekstPoruke = korisnik.imePrezime + ", cestitamo na osvojenoj nagradi za igraca utakmice na mecu " + izvještaj.utakmica.domacin.nazivKluba + " - " + izvještaj.utakmica.gost.nazivKluba + " " + izvještaj.goloviDomacih + ":" + izvještaj.goloviGostiju + "!";
            sms.posaljiSMSAsync(korisnik.Id,tekstPoruke);

            string posiljaoc = "admin@fksarajevo.ba";

            controller.posaljiJednom(tekstPoruke, "success", korisnik, false,posiljaoc);

            posiljaoc = u.trener.korisnik.UserName;
            if (postoji)
            {
                controller.posaljiPoruku("Izvještaj za utakmicu " + u.domacin.nazivKluba + " - " + u.gost.nazivKluba + "(" + izvještaj.goloviDomacih + ":" + izvještaj.goloviGostiju + ") je ažuriran!", "warning",false,posiljaoc);
                return "Izvještaj uspješno uređen!";
            }
            else
            {
                controller.posaljiPoruku("Kreiran je izvještaj za utakmicu " + u.domacin.nazivKluba + " - " + u.gost.nazivKluba + "(" + izvještaj.goloviDomacih + ":" + izvještaj.goloviGostiju + ")", "info",false,posiljaoc);
                return "Izvještaj uspješno dodan!";
            }
        }
        public IActionResult IzvjestajDetalji(int utakmicaID)
        {
            List<IzvjestajDetaljiVM.Nastupi> nastupi = db.nastupi
                .Where(s => s.utakmicaID == utakmicaID)
                .Select(s => new IzvjestajDetaljiVM.Nastupi
                {
                    igracID = s.igracID,
                    imePrezime = s.igrac.korisnik.imePrezime,
                    iPrezime = s.igrac.korisnik.ime.Substring(0, 1) + "." + s.igrac.korisnik.prezime,
                    brojDresa = s.igrac.brojDresa,
                    slikaIgraca = s.igrac.slika,
                    minute = s.minute,
                    golovi = s.golovi,
                    asistencije = s.asistencije,
                    zutiKartoni = s.zutiKartoni,
                    crveniKartoni = s.crveniKartoni,
                    ocjena = s.ocjena,
                    prosjecnaOcjena = db.ocjene.Where(t => (t.utakmicaID == utakmicaID) && (t.igracID == s.igracID)).Select(s => s.prosjecnaOcjena).Single()
                }).OrderByDescending(s => s.ocjena).ToList();

            List<UtakmicaIzmjena> izmjene = db.utakmicaIzmjena.Where(s => s.utakmicaID == utakmicaID).ToList();

            List<IzvjestajDetaljiVM.Izmjena> izmjeneVM = izmjene
                    .Select(s => new IzvjestajDetaljiVM.Izmjena
                    {
                        izmjenaID = s.izmjenaID,
                        igracIN = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracIn).Include(k => k.igracIn.korisnik).Select(im => im.igracIn.korisnik.imePrezime).Single(),
                        brojDresaIN = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracIn).Select(im => im.igracIn.brojDresa).Single(),
                        slikaIN = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracIn).Select(im => im.igracIn.slika).Single(),
                        igracOUT = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracOut).Include(k => k.igracOut.korisnik).Select(im => im.igracOut.korisnik.imePrezime).Single(),
                        brojDresaOUT = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracOut).Select(im => im.igracOut.brojDresa).Single(),
                        slikaOUT = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracOut).Select(im => im.igracOut.slika).Single(),
                        minuta = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Select(im => im.minuta).Single(),
                        razlog = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Select(im => im.razlog.ToString()).Single(),
                    }).ToList();

            Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID).Single();

            IzvjestajDetaljiVM model = db.izvjestaji
                .Where(s => s.utakmicaID == utakmicaID)
                .Select(s => new IzvjestajDetaljiVM
                {
                    izvještajID = s.izvještajID,
                    utakmicaID = s.utakmicaID,
                    datumKreiranja = s.datumKreiranja.ToString("dd.MM.yyyy"),
                    brojGledalaca = s.brojGledalaca,
                    vrstaTakmicenja = s.utakmica.vrstaTakmicenja.nazivVrsteTakmicenja,
                    slikaVrstaTakmicenja = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                    domacin = s.utakmica.domacin.nazivKluba,
                    domacinGrb = s.utakmica.domacin.grbKluba,
                    gost = s.utakmica.gost.nazivKluba,
                    gostGrb = s.utakmica.gost.grbKluba,
                    goloviDomacih = s.goloviDomacih,
                    goloviGostiju = s.goloviGostiju,
                    rezultat = s.rezultat,
                    youtubeVideoID = s.youtubeVideoID,
                    delegat = s.delegatUtakmice,
                    nazivStadiona = s.utakmica.stadion.imeStadiona,
                    trenerSlika = s.trener.slika,
                    vrijeme = s.vrijeme,
                    trener = s.utakmica.trener.imePrezime,
                    igracUtakmice = s.igracUtakmica.korisnik.imePrezime,
                    slikaIgrac = s.igracUtakmica.slika,
                    nastupi = nastupi,
                    izmjene = izmjeneVM,
                    zapisnik = s.zapisnik,
                    slika = s.slikaSaUtakmice
                }).Single();


            //List<UtakmicaNastupi> nastupi = db.nastupi.Where(s => s.utakmicaID == utakmicaID)
            //    .Include(s => s.igrac)
            //    .Include(s => s.igrac.korisnik)
            //    .ToList();

            //Izvještaj i = db.izvjestaji.Where(s => s.izvještajID == utakmicaID).Single();
            //i.nastupi = nastupi;

            return PartialView("IzvjestajDetalji", model);
        }
        public IActionResult IzvjestajOcjene(int igracID, int utakmicaID)
        {
            //IzvjestajDetaljiVM model = new IzvjestajDetaljiVM();
            //UtakmicaOcjene ocjene = db.ocjene.Where(s => (s.igracID == igracID && s.utakmicaID == utakmicaID)).Single();

            Izvještaj i = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID).Single();

            IzvjestajOcjeneVM model = db.ocjene
                .Where(s => (s.igracID == igracID && s.utakmicaID == utakmicaID))
                .Select(s => new IzvjestajOcjeneVM
                {
                    igracID = s.igracID,
                    utakmicaID = s.utakmicaID,
                    slikaVrstaTakmicenja = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                    domacin = s.utakmica.domacin.nazivKluba,
                    domacinGrb = s.utakmica.domacin.grbKluba,
                    gost = s.utakmica.gost.nazivKluba,
                    gostGrb = s.utakmica.gost.grbKluba,
                    goloviDomacih = i.goloviDomacih,
                    goloviGostiju = i.goloviGostiju,
                    rezultat = i.rezultat,
                    imePrezime = s.igrac.korisnik.imePrezime,
                    slika = s.igrac.slika,
                    brojDresa = s.igrac.brojDresa,
                    komentar = db.nastupi.Where(s => (s.utakmicaID == utakmicaID && s.igracID == igracID)).Select(s => s.komentar).Single(),
                    pozicija = s.igrac.pozicija.ToString(),
                    kontrolaLopte = s.kontrolaLopte,
                    dribljanje = s.dribljanje,
                    dodavanje = s.dodavanje,
                    kretanje = s.kretanje,
                    brzina = s.brzina,
                    sut = s.sut,
                    snaga = s.snaga,
                    markiranje = s.markiranje,
                    klizeciStart = s.klizeciStart,
                    skok = s.klizeciStart,
                    odbrana = s.odbrana,
                    prosjecnaOcjena = s.prosjecnaOcjena
                }).Single();

            return PartialView("IzvjestajOcjene", model);
        }
        [Autorizacija(true, false, false)]
        public IActionResult dodajIzvjestaj()
        {
            List<SelectListItem> utakmice = db.utakmice
                .Where(s => (s.statusUtakmice == StatusUtakmice.poRasporedu && s.datumOdigravanja.Date < DateTime.Now.Date))
                .Select(s => new SelectListItem
                {
                    Value = s.utakmicaID.ToString(),
                    Text = s.domacin.nazivKluba + " - " + s.gost.nazivKluba + " - " + s.datumOdigravanja.ToString("dd.MM.yyyy")
                }).ToList();

            IzvjestajDodajIzborVM model = new IzvjestajDodajIzborVM();
            model.utakmice = utakmice;

            return PartialView("DodajIzvjestaj", model);
        }
        private List<Igrac> pozvaniIgraciUtakmica(int utakmicaID)
        {
            PrvaPostava postava = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID).Single();
            Klupa klupa = db.klupa.Where(s => s.utakmicaID == utakmicaID).Single();

            List<Igrac> pozvaniIgraci = new List<Igrac>();
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.golmanID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.lijeviBekID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.desniBekID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.prviStoperID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.drugiStoperID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.prviZadnjiVezniID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.drugiZadnjiVezniID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.prednjiVezniID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.lijevoKriloID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.desnoKriloID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.napadacID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.golmanKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.bekKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.odbranaKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.veznjakKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.kriloKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.napadacKlupaID).Include(s => s.korisnik).Single());

            return pozvaniIgraci;
        }
        [Autorizacija(true, false, false)]
        public IActionResult kreirajIzvjestaj(int utakmicaID, bool izmjena)
        {
            List<Igrac> pozvaniIgraci = pozvaniIgraciUtakmica(utakmicaID);


            List<SelectListItem> igraci = pozvaniIgraci.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.korisnik.imePrezime,
                    Value = a.igracID.ToString()
                };
            });

            Utakmica utakmica = db.utakmice
                .Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.stadion)
                .Include(s => s.vrstaTakmicenja)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.trener)
                .Single();

            List<SelectListItem> treneri = db.treneri
                .Select(a => new SelectListItem
                {
                    Text = a.imePrezime,
                    Value = a.trenerID.ToString()
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

            IzvjestajKreirajVM model = new IzvjestajKreirajVM();
            model.utakmicaID = utakmicaID;
            model.datumOdigravanja = utakmica.datumOdigravanja.ToString("dd.MM.yyyy");
            model.satnica = utakmica.satnica;
            model.nazivStadiona = utakmica.stadion.imeStadiona;
            model.slikaVrstaTakmicenja = utakmica.vrstaTakmicenja.logoTakmicenja;
            model.domacin = utakmica.domacin.nazivKluba;
            model.domacinGrb = utakmica.domacin.grbKluba;
            model.gost = utakmica.gost.nazivKluba;
            model.gostGrb = utakmica.gost.grbKluba;
            model.igraci = igraci;
            model.vrijeme = vrijemeSelect;
            model.trener = utakmica.trener.imePrezime;
            model.trenerSlika = utakmica.trener.slika;
            model.datumKreiranja = DateTime.Now;
            //model.igraci = igraci;


            if (izmjena)
            {
                Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID).Single();

                model.goloviDomacih = izvjestaj.goloviDomacih;
                model.goloviGostiju = izvjestaj.goloviGostiju;
                model.igracUtakmiceID = izvjestaj.igracUtakmicaID;
                model.delegatUtakmice = izvjestaj.delegatUtakmice;
                model.brojGledalaca = izvjestaj.brojGledalaca;
                model.nazivVremena = izvjestaj.vrijeme.ToString();
                model.zapisnik = izvjestaj.zapisnik;
                model.youTubeVideoID = izvjestaj.youtubeVideoID;
                model.slikaCurrent = izvjestaj.slikaSaUtakmice;
                ViewData["izmjena"] = true;
            }
            else
            {
                ViewData["izmjena"] = false;
            }

            return PartialView("KreirajIzvjestaj", model);
        }
        [Autorizacija(true, false, false)]
        public IActionResult UrediNastup(int utakmicaID, int igracID)
        {
            //List<Igrac> pozvaniIgraci = pozvaniIgraciUtakmica(utakmicaID);
            List<Igrac> ocjenjeniIgraci = db.ocjene.Where(s => s.utakmicaID == utakmicaID).Include(t => t.igrac.korisnik).Select(s => s.igrac).ToList();
            List<Igrac> pozvaniIgraci = pozvaniIgraciPrvaPostava(utakmicaID);
            List<Igrac> klupaNastupili = db.utakmicaIzmjena.Where(s => s.utakmicaID == utakmicaID).Include(t => t.izmjena).Include(g => g.izmjena.igracIn).Include(g => g.izmjena.igracIn.korisnik).Select(k => k.izmjena.igracIn).ToList();
            List<Igrac> neocjenjeniIgraci = new List<Igrac>();
            TempData["brojNastupa"] = klupaNastupili.Count() + 11;

            for (int j = 0; j < klupaNastupili.Count(); j++)
            {
                pozvaniIgraci.Add(klupaNastupili[j]);
            }

            if (ocjenjeniIgraci.Count == 0)
            {
                neocjenjeniIgraci = pozvaniIgraci;
            }
            else
            {
                for (int i = 0; i < pozvaniIgraci.Count; i++)
                {
                    if (!ocjenjeniIgraci.Contains(pozvaniIgraci[i]))
                    {
                        neocjenjeniIgraci.Add(pozvaniIgraci[i]);
                    }
                }
            }
            List<SelectListItem> igraci = ocjenjeniIgraci.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.korisnik.imePrezime,
                    Value = a.igracID.ToString()
                };
            });

            UtakmicaNastupi nastup = db.nastupi.Where(s => (s.utakmicaID == utakmicaID && s.igracID == igracID)).Single();
            UtakmicaOcjene ocjene = db.ocjene.Where(s => (s.utakmicaID == utakmicaID && s.igracID == igracID)).Single();

            StatistikaDodajVM model = new StatistikaDodajVM();
            model.igraci = igraci;
            model.igracID = igracID;
            model.utakmicaID = utakmicaID;
            model.minute = nastup.minute;
            model.golovi = nastup.golovi;
            model.asistencije = nastup.asistencije;
            model.zutiKartoni = nastup.zutiKartoni;
            model.crveniKartoni = nastup.crveniKartoni;
            model.ocjena = (int)nastup.ocjena;
            model.komentar = nastup.komentar;
            model.kontrolaLopte = ocjene.kontrolaLopte;
            model.dribljanje = ocjene.dribljanje;
            model.dodavanje = ocjene.dodavanje;
            model.kretanje = ocjene.kretanje;
            model.brzina = ocjene.brzina;
            model.sut = ocjene.sut;
            model.snaga = ocjene.snaga;
            model.markiranje = ocjene.markiranje;
            model.klizeciStart = ocjene.klizeciStart;
            model.skok = ocjene.skok;
            model.odbrana = ocjene.odbrana;
            model.prosjecnaOcjena = ocjene.prosjecnaOcjena;

            ViewData["izmjena"] = true;

            return PartialView("DodajStatistiku", model);
        }
        [Autorizacija(true, false, false)]
        public IActionResult DodajNastup(int utakmicaID)
        {

            List<Igrac> ocjenjeniIgraci = db.ocjene.Where(s => s.utakmicaID == utakmicaID).Select(s => s.igrac).ToList();
            List<Igrac> pozvaniIgraci = pozvaniIgraciPrvaPostava(utakmicaID);
            List<Igrac> klupaNastupili = db.utakmicaIzmjena.Where(s => s.utakmicaID == utakmicaID).Include(t => t.izmjena).Include(g => g.izmjena.igracIn).Include(g => g.izmjena.igracIn.korisnik).Select(k => k.izmjena.igracIn).ToList();
            //List<Igrac> klupaNastupili = pozvaniIgraciKlupa(utakmicaID);
            //List<Igrac> pozvaniIgraciK = pozvaniIgraciKlupa(utakmicaID);

            for (int j = 0; j < klupaNastupili.Count(); j++)
            {
                pozvaniIgraci.Add(klupaNastupili[j]);
            }


            List<Igrac> neocjenjeniIgraci = new List<Igrac>();

            if (ocjenjeniIgraci.Count == 0)
            {
                neocjenjeniIgraci = pozvaniIgraci;
            }
            else
            {
                for (int i = 0; i < pozvaniIgraci.Count; i++)
                {
                    if (!ocjenjeniIgraci.Contains(pozvaniIgraci[i]))
                    {
                        neocjenjeniIgraci.Add(pozvaniIgraci[i]);
                    }
                }
            }
            //List<Igrac> ocjenjeniIgraci = db.ocjene.Where(s => s.utakmicaID == utakmicaID).Select(s => s.igrac).ToList();
            //List<Igrac> pozvaniIgraci = pozvaniIgraciUtakmica(utakmicaID);

            //List<Igrac> validniIgraci = new List<Igrac>();

            //if (ocjenjeniIgraci.Count == 0)
            //{
            //    validniIgraci = pozvaniIgraci;
            //}
            //else
            //{
            //    for (int i = 0; i < pozvaniIgraci.Count; i++)
            //    {
            //        if (!ocjenjeniIgraci.Contains(pozvaniIgraci[i])){
            //            validniIgraci.Add(pozvaniIgraci[i]);
            //        }
            //    }
            //}
            TempData["brojNastupa"] = klupaNastupili.Count() + 11;
            List<SelectListItem> igraci = neocjenjeniIgraci.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.korisnik.imePrezime,
                    Value = a.igracID.ToString()
                };
            });
            StatistikaDodajVM model = new StatistikaDodajVM();
            model.igraci = igraci;
            model.utakmicaID = utakmicaID;

            ViewData["izmjena"] = false;

            return PartialView("DodajStatistiku", model);
        }
        public IActionResult getNastupi(int utakmicaID)
        {
            var broj = TempData["brojNastupa"];

            List<NastupiVM.Nastupi> nastupi = db.nastupi
                .Where(s => s.utakmicaID == utakmicaID)
                .Select(s => new NastupiVM.Nastupi
                {
                    igracID = s.igracID,
                    imePrezime = s.igrac.korisnik.imePrezime,
                    brojDresa = s.igrac.brojDresa,
                    slikaIgraca = s.igrac.slika,
                    minute = s.minute,
                    golovi = s.golovi,
                    asistencije = s.asistencije,
                    zutiKartoni = s.zutiKartoni,
                    crveniKartoni = s.crveniKartoni,
                    ocjena = s.ocjena,
                    prosjecnaOcjenaStavki = (float)db.ocjene.Where(t => (t.utakmicaID == utakmicaID) && (t.igracID == s.igracID)).Select(s => Math.Round(s.prosjecnaOcjena, 2)).Single()
                }).OrderByDescending(s => s.ocjena).ToList();

            NastupiVM model = new NastupiVM();
            model.nastupi = nastupi;
            model.utakmicaID = utakmicaID;

            return PartialView("getNastupi", model);
        }
        [Autorizacija(true, false, false)]
        public string spasiStatistiku(StatistikaDodajVM model)
        {
            Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == model.utakmicaID)
                .Include(s => s.nastupi)
                .Single();

            if (izvjestaj.izvještajID == 0)
            {
                return "Izvještaj nije kreiran!";
            }

            List<UtakmicaNastupi> nastupi = db.nastupi.Where(s => s.utakmicaID == model.utakmicaID).ToList();

            for (int j = 0; j < nastupi.Count(); j++)
            {
                if (nastupi[j].igracID == model.igracID)
                {
                    return "Statistika je već evidentirana!";
                }
            }

            Igrac i = db.igraci
                .Include(s => s.igracStatistika)
                .Include(s => s.igracSkills)
                .Include(s => s.igracSkills.skillsStavke)
                .Where(s => s.igracID == model.igracID).Single();

            UtakmicaNastupi nastup = new UtakmicaNastupi();
            nastup.igracID = model.igracID;
            nastup.utakmicaID = model.utakmicaID;
            nastup.minute = model.minute;
            nastup.golovi = model.golovi;
            nastup.asistencije = model.asistencije;
            //nastup.zutiKartoni = model.zutiKartoni;
            if (nastup.zutiKartoni >= 2)
            {
                nastup.crveniKartoni = 1;
                nastup.zutiKartoni = 2;
            }
            else
            {
                nastup.zutiKartoni = model.zutiKartoni;
                nastup.crveniKartoni = model.crveniKartoni;
            }

            if (nastup.crveniKartoni > 1)
            {
                nastup.crveniKartoni = 1;
            }
            nastup.ocjena = model.ocjena;
            nastup.komentar = model.komentar;
            db.Add(nastup);
            db.SaveChanges();

            izvjestaj.nastupi.Add(nastup);
            db.SaveChanges();

            if (nastup.minute > 0)
            {
                i.igracStatistika.brojNastupa++;
                i.igracStatistika.odigraneMinute += nastup.minute;
                i.igracStatistika.golovi += nastup.golovi;
                i.igracStatistika.asistencije += nastup.asistencije;
                i.igracStatistika.crveniKartoni += nastup.crveniKartoni;
                i.igracStatistika.zutiKartoni += nastup.zutiKartoni;
                i.igracStatistika.zbirOcjena += nastup.ocjena;
                //i.igracStatistika.prosjecnaOcjena = (float)Math.Round(i.igracStatistika.zbirOcjena / (float)i.igracStatistika.brojNastupa, 2);
                i.igracStatistika.prosjecnaOcjena = izracunajProsjek(i.igracStatistika.zbirOcjena, i.igracStatistika.brojNastupa);
            }


            UtakmicaOcjene ocjene = new UtakmicaOcjene();
            ocjene.igracID = model.igracID;
            ocjene.utakmicaID = model.utakmicaID;
            ocjene.kontrolaLopte = model.kontrolaLopte;
            ocjene.dribljanje = model.dribljanje;
            ocjene.dodavanje = model.dodavanje;
            ocjene.kretanje = model.kretanje;
            ocjene.brzina = model.brzina;
            ocjene.sut = model.sut;
            ocjene.snaga = model.snaga;
            ocjene.markiranje = model.markiranje;
            ocjene.klizeciStart = model.klizeciStart;
            ocjene.skok = model.skok;
            ocjene.odbrana = model.odbrana;
            ocjene.prosjecnaOcjena = model.prosjecnaOcjena * (float)1.0 / 100;

            db.Add(ocjene);
            db.SaveChanges();

            izvjestaj.ocjene = new List<UtakmicaOcjene>();
            izvjestaj.ocjene.Add(ocjene);
            db.SaveChanges();

            if (nastup.minute > 0)
            {
                int brojNastupa = i.igracStatistika.brojNastupa;

                i.igracSkills.skillsStavke.kontrolaLopteZbir += ocjene.kontrolaLopte;
                i.igracSkills.skillsStavke.kontrolaLopte = izracunajProsjek(i.igracSkills.skillsStavke.kontrolaLopteZbir, brojNastupa);

                i.igracSkills.skillsStavke.dribljanjeZbir += ocjene.dribljanje;
                i.igracSkills.skillsStavke.dribljanje = izracunajProsjek(i.igracSkills.skillsStavke.dribljanjeZbir, brojNastupa);

                i.igracSkills.skillsStavke.dodavanjeZbir += ocjene.dodavanje;
                i.igracSkills.skillsStavke.dodavanje = izracunajProsjek(i.igracSkills.skillsStavke.dodavanjeZbir, brojNastupa);

                i.igracSkills.skillsStavke.kretanjeZbir += ocjene.kretanje;
                i.igracSkills.skillsStavke.kretanje = izracunajProsjek(i.igracSkills.skillsStavke.kretanjeZbir, brojNastupa);

                i.igracSkills.skillsStavke.brzinaZbir += ocjene.brzina;
                i.igracSkills.skillsStavke.brzina = izracunajProsjek(i.igracSkills.skillsStavke.brzinaZbir, brojNastupa);

                i.igracSkills.skillsStavke.sutZbir += ocjene.sut;
                i.igracSkills.skillsStavke.sut = izracunajProsjek(i.igracSkills.skillsStavke.sutZbir, brojNastupa);

                i.igracSkills.skillsStavke.snagaZbir += ocjene.snaga;
                i.igracSkills.skillsStavke.snaga = izracunajProsjek(i.igracSkills.skillsStavke.snagaZbir, brojNastupa);

                i.igracSkills.skillsStavke.markiranjeZbir += ocjene.markiranje;
                i.igracSkills.skillsStavke.markiranje = izracunajProsjek(i.igracSkills.skillsStavke.markiranjeZbir, brojNastupa);

                i.igracSkills.skillsStavke.klizeciStartZbir += ocjene.klizeciStart;
                i.igracSkills.skillsStavke.klizeciStart = izracunajProsjek(i.igracSkills.skillsStavke.klizeciStartZbir, brojNastupa);

                i.igracSkills.skillsStavke.skokZbir += ocjene.skok;
                i.igracSkills.skillsStavke.skok = izracunajProsjek(i.igracSkills.skillsStavke.skokZbir, brojNastupa);

                i.igracSkills.skillsStavke.odbranaZbir += ocjene.odbrana;
                i.igracSkills.skillsStavke.odbrana = izracunajProsjek(i.igracSkills.skillsStavke.odbranaZbir, brojNastupa);

                i.igracSkills.skillsStavke.zbirOcjena += ocjene.prosjecnaOcjena;
                i.igracSkills.skillsStavke.prosjekOcjena = (float)Math.Round(i.igracSkills.skillsStavke.zbirOcjena / (float)brojNastupa, 2);
                db.SaveChanges();
            }
            Utakmica utakmica = db.utakmice.Where(s => s.utakmicaID == model.utakmicaID)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.trener)
                .Include(s => s.trener.korisnik)
                .Single();
            Korisnik k = db.korisnici.Find(i.korisnikID);
            NotifikacijaController nc = new NotifikacijaController(db, userManager, hubContext);
            string poruka = "Vaša ocjena na utakmici " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + " (" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ")" + " je " + model.ocjena + "!";
            string posiljaoc = utakmica.trener.korisnik.UserName;
            nc.posaljiJednom(poruka, "success", k, false,posiljaoc);

            return "OK";
        }
        [Autorizacija(true, false, false)]
        public string spasiIzmjene(StatistikaDodajVM model)
        {
            Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == model.utakmicaID)
                .Include(s => s.nastupi)
                .Include(s => s.ocjene)
                .Single();


            UtakmicaNastupi nastup = izvjestaj.nastupi.Where(s => (s.utakmicaID == model.utakmicaID && s.igracID == model.igracID)).Single();
            UtakmicaOcjene ocjena = izvjestaj.ocjene.Where(s => (s.utakmicaID == model.utakmicaID && s.igracID == model.igracID)).Single();

            Igrac i = db.igraci
                .Include(s => s.igracStatistika)
                .Include(s => s.igracSkills)
                .Include(s => s.igracSkills.skillsStavke)
                .Where(s => s.igracID == model.igracID).Single();

            i.igracStatistika.odigraneMinute -= nastup.minute;
            i.igracStatistika.golovi -= nastup.golovi;
            i.igracStatistika.asistencije -= nastup.asistencije;
            i.igracStatistika.zutiKartoni -= nastup.zutiKartoni;
            i.igracStatistika.crveniKartoni -= nastup.crveniKartoni;
            i.igracStatistika.zbirOcjena -= nastup.ocjena;

            nastup.minute = model.minute;
            nastup.golovi = model.golovi;
            nastup.asistencije = model.asistencije;
            if (model.zutiKartoni >= 2)
            {
                nastup.crveniKartoni = 1;
                nastup.zutiKartoni = 2;
            }
            else
            {
                nastup.zutiKartoni = model.zutiKartoni;
                nastup.crveniKartoni = model.crveniKartoni;
            }
            if (nastup.crveniKartoni > 1)
            {
                nastup.crveniKartoni = 1;
            }
            nastup.ocjena = model.ocjena;
            nastup.komentar = model.komentar;

            i.igracStatistika.odigraneMinute += nastup.minute;
            i.igracStatistika.golovi += nastup.golovi;
            i.igracStatistika.asistencije += nastup.asistencije;
            i.igracStatistika.crveniKartoni += nastup.crveniKartoni;
            i.igracStatistika.zutiKartoni += nastup.zutiKartoni;
            i.igracStatistika.zbirOcjena += nastup.ocjena;
            i.igracStatistika.prosjecnaOcjena = izracunajProsjek(i.igracStatistika.zbirOcjena, i.igracStatistika.brojNastupa);

            db.SaveChanges();

            i.igracSkills.skillsStavke.kontrolaLopteZbir -= ocjena.kontrolaLopte;
            i.igracSkills.skillsStavke.dribljanjeZbir -= ocjena.dribljanje;
            i.igracSkills.skillsStavke.dodavanjeZbir -= ocjena.dodavanje;
            i.igracSkills.skillsStavke.kretanjeZbir -= ocjena.kretanje;
            i.igracSkills.skillsStavke.brzinaZbir -= ocjena.brzina;
            i.igracSkills.skillsStavke.sutZbir -= ocjena.sut;
            i.igracSkills.skillsStavke.snagaZbir -= ocjena.snaga;
            i.igracSkills.skillsStavke.markiranjeZbir -= ocjena.markiranje;
            i.igracSkills.skillsStavke.klizeciStartZbir -= ocjena.klizeciStart;
            i.igracSkills.skillsStavke.skokZbir -= ocjena.skok;
            i.igracSkills.skillsStavke.odbranaZbir -= ocjena.odbrana;
            i.igracSkills.skillsStavke.zbirOcjena -= ocjena.prosjecnaOcjena;

            db.SaveChanges();

            ocjena.kontrolaLopte = model.kontrolaLopte;
            ocjena.dribljanje = model.dribljanje;
            ocjena.dodavanje = model.dodavanje;
            ocjena.kretanje = model.kretanje;
            ocjena.brzina = model.brzina;
            ocjena.sut = model.sut;
            ocjena.snaga = model.snaga;
            ocjena.markiranje = model.markiranje;
            ocjena.klizeciStart = model.klizeciStart;
            ocjena.skok = model.skok;
            ocjena.odbrana = model.odbrana;
            if (model.prosjecnaOcjena == ocjena.prosjecnaOcjena)
            {
                ocjena.prosjecnaOcjena = model.prosjecnaOcjena;
            }
            else
            {
                ocjena.prosjecnaOcjena = model.prosjecnaOcjena * (float)1.0 / 100;
            }

            db.SaveChanges();

            int brojNastupa = i.igracStatistika.brojNastupa;

            i.igracSkills.skillsStavke.kontrolaLopteZbir += ocjena.kontrolaLopte;
            i.igracSkills.skillsStavke.kontrolaLopte = izracunajProsjek(i.igracSkills.skillsStavke.kontrolaLopteZbir, brojNastupa);

            i.igracSkills.skillsStavke.dribljanjeZbir += ocjena.dribljanje;
            i.igracSkills.skillsStavke.dribljanje = izracunajProsjek(i.igracSkills.skillsStavke.dribljanjeZbir, brojNastupa);

            i.igracSkills.skillsStavke.dodavanjeZbir += ocjena.dodavanje;
            i.igracSkills.skillsStavke.dodavanje = izracunajProsjek(i.igracSkills.skillsStavke.dodavanjeZbir, brojNastupa);

            i.igracSkills.skillsStavke.kretanjeZbir += ocjena.kretanje;
            i.igracSkills.skillsStavke.kretanje = izracunajProsjek(i.igracSkills.skillsStavke.kretanjeZbir, brojNastupa);

            i.igracSkills.skillsStavke.brzinaZbir += ocjena.brzina;
            i.igracSkills.skillsStavke.brzina = izracunajProsjek(i.igracSkills.skillsStavke.brzinaZbir, brojNastupa);

            i.igracSkills.skillsStavke.sutZbir += ocjena.sut;
            i.igracSkills.skillsStavke.sut = izracunajProsjek(i.igracSkills.skillsStavke.sutZbir, brojNastupa);

            i.igracSkills.skillsStavke.snagaZbir += ocjena.snaga;
            i.igracSkills.skillsStavke.snaga = izracunajProsjek(i.igracSkills.skillsStavke.snagaZbir, brojNastupa);

            i.igracSkills.skillsStavke.markiranjeZbir += ocjena.markiranje;
            i.igracSkills.skillsStavke.markiranje = izracunajProsjek(i.igracSkills.skillsStavke.markiranjeZbir, brojNastupa);

            i.igracSkills.skillsStavke.klizeciStartZbir += ocjena.klizeciStart;
            i.igracSkills.skillsStavke.klizeciStart = izracunajProsjek(i.igracSkills.skillsStavke.klizeciStartZbir, brojNastupa);

            i.igracSkills.skillsStavke.skokZbir += ocjena.skok;
            i.igracSkills.skillsStavke.skok = izracunajProsjek(i.igracSkills.skillsStavke.skokZbir, brojNastupa);

            i.igracSkills.skillsStavke.odbranaZbir += ocjena.odbrana;
            i.igracSkills.skillsStavke.odbrana = izracunajProsjek(i.igracSkills.skillsStavke.odbranaZbir, brojNastupa);

            i.igracSkills.skillsStavke.zbirOcjena += ocjena.prosjecnaOcjena;
            i.igracSkills.skillsStavke.prosjekOcjena = (float)Math.Round(i.igracSkills.skillsStavke.zbirOcjena / (float)brojNastupa, 2);

            Utakmica utakmica = db.utakmice.Where(s => s.utakmicaID == model.utakmicaID)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.trener)
                .Include(s => s.trener.korisnik)
                .Single();

            Notifikacija notifikacija = new Notifikacija();
            Korisnik k = db.korisnici.Find(i.korisnikID);
            string poruka = "Vaša ocjena na utakmici " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + " (" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ")" + " je ažurirana. Vaša ocjena je " + model.ocjena + "!";

            NotifikacijaController nc = new NotifikacijaController(db, userManager, hubContext);
            string posiljaoc = utakmica.trener.korisnik.UserName;
            nc.posaljiJednom(poruka, "info", k, false,posiljaoc);

            return "OK";
        }
        public float izracunajProsjek(int zbir, int brojNastupa)
        {
            return (float)Math.Round(zbir / (float)brojNastupa, 2);
        }
        [Autorizacija(true, false, false)]
        public string obrisiOcjenu(int igracID, int utakmicaID)
        {
            Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.nastupi)
                .Include(s => s.ocjene)
                .Single();

            UtakmicaNastupi nastup = izvjestaj.nastupi.Where(s => (s.utakmicaID == utakmicaID && s.igracID == igracID)).Single();
            UtakmicaOcjene ocjena = izvjestaj.ocjene.Where(s => (s.utakmicaID == utakmicaID && s.igracID == igracID)).Single();

            Igrac i = db.igraci
                .Include(s => s.igracStatistika)
                .Include(s => s.igracSkills)
                .Include(s => s.igracSkills.skillsStavke)
                .Where(s => s.igracID == igracID).Single();

            IgracStatistika igracStatistika = db.igracStatistika.Where(s => s.igracID == igracID).Single();
            SkillsStavke stavke = db.skillsStavke.Where(s => s.igracID == i.igracID).Single();


            igracStatistika.zbirOcjena -= nastup.ocjena;

            if (igracStatistika.brojNastupa == 1)
            {
                igracStatistika.brojNastupa = 0;
                igracStatistika.prosjecnaOcjena = 0;
            }
            else
            {
                igracStatistika.brojNastupa--;
                igracStatistika.prosjecnaOcjena = izracunajProsjek(igracStatistika.zbirOcjena, igracStatistika.brojNastupa);
            }
            igracStatistika.odigraneMinute -= nastup.minute;
            igracStatistika.golovi -= nastup.golovi;
            igracStatistika.asistencije -= nastup.asistencije;
            igracStatistika.crveniKartoni -= nastup.crveniKartoni;
            igracStatistika.zutiKartoni -= nastup.zutiKartoni;

            db.SaveChanges();

            izvjestaj.nastupi.Remove(nastup);
            db.Remove(nastup);
            db.SaveChanges();

            int brojNastupa = igracStatistika.brojNastupa;

            stavke.kontrolaLopteZbir -= ocjena.kontrolaLopte;
            stavke.dribljanjeZbir -= ocjena.dribljanje;
            stavke.dodavanjeZbir -= ocjena.dodavanje;
            stavke.kretanjeZbir -= ocjena.kretanje;
            stavke.brzinaZbir -= ocjena.brzina;
            stavke.sutZbir -= ocjena.sut;
            stavke.snagaZbir -= ocjena.snaga;
            stavke.markiranjeZbir -= ocjena.markiranje;
            stavke.klizeciStartZbir -= ocjena.klizeciStart;
            stavke.skokZbir -= ocjena.skok;
            stavke.odbranaZbir -= ocjena.odbrana;
            stavke.zbirOcjena -= ocjena.prosjecnaOcjena;

            if (brojNastupa == 0)
            {
                stavke.kontrolaLopte = 0;
                stavke.dribljanje = 0;
                stavke.dodavanje = 0;
                stavke.kretanje = 0;
                stavke.brzina = 0;
                stavke.sut = 0;
                stavke.snaga = 0;
                stavke.markiranje = 0;
                stavke.klizeciStart = 0;
                stavke.skok = 0;
                stavke.odbrana = 0;
                stavke.prosjekOcjena = 0;
            }
            else
            {
                stavke.kontrolaLopte = izracunajProsjek(stavke.kontrolaLopteZbir, brojNastupa);
                stavke.dribljanje = izracunajProsjek(stavke.dribljanjeZbir, brojNastupa);
                stavke.dodavanje = izracunajProsjek(stavke.dodavanjeZbir, brojNastupa);
                stavke.kretanje = izracunajProsjek(stavke.kretanjeZbir, brojNastupa);
                stavke.brzina = izracunajProsjek(stavke.brzinaZbir, brojNastupa);
                stavke.sut = izracunajProsjek(stavke.sutZbir, brojNastupa);
                stavke.snaga = izracunajProsjek(stavke.snagaZbir, brojNastupa);
                stavke.markiranje = izracunajProsjek(stavke.markiranjeZbir, brojNastupa);
                stavke.klizeciStart = izracunajProsjek(stavke.klizeciStartZbir, brojNastupa);
                stavke.skok = izracunajProsjek(stavke.skokZbir, brojNastupa);
                stavke.odbrana = izracunajProsjek(stavke.odbranaZbir, brojNastupa);
                stavke.prosjekOcjena = (float)Math.Round(stavke.zbirOcjena / (float)brojNastupa, 2);
            }

            Utakmica utakmica = db.utakmice.Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.trener)
                .Include(s => s.trener.korisnik)
                .Single();
            Korisnik k = db.korisnici.Find(i.korisnikID);
            string poruka = "Vaša ocjena na utakmici " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + " (" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ")" + " je obrisana!";

            NotifikacijaController nc = new NotifikacijaController(db, userManager, hubContext);
            string posiljaoc = utakmica.trener.korisnik.UserName;
            nc.posaljiJednom(poruka, "error", k, false,posiljaoc);

            izvjestaj.ocjene.Remove(ocjena);
            db.Remove(ocjena);
            db.SaveChanges();

            return "OK";
        }
        [Autorizacija(false, false, true)]
        public IActionResult obrisiIzvjestaj(int utakmicaID)
        {
            IzvjestajObrisiVM model = new IzvjestajObrisiVM();
            model.utakmicaID = utakmicaID;
            return PartialView("ObrisiIzvjestaj", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult snimiObrisi(int utakmicaID)
        {
            Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.nastupi)
                .Include(s => s.izmjene)
                .Include(s => s.ocjene).Single();

            List<UtakmicaNastupi> nastupi = izvjestaj.nastupi.Where(s => s.utakmicaID == utakmicaID).ToList();
            for (int i = 0; i < nastupi.Count(); i++)
            {
                obrisiOcjenu(nastupi[i].igracID, utakmicaID);
            }
            List<UtakmicaIzmjena> izmjene = izvjestaj.izmjene.Where(s => s.utakmicaID == utakmicaID).ToList();
            for (int i = 0; i < izmjene.Count(); i++)
            {
                obrisiIzmjenu(izmjene[i].izmjenaID, utakmicaID);
            }
            //db.RemoveRange(nastupi);
            db.SaveChanges();

            TrenerStatistika statistika = db.treneri.Where(s => s.trenerID == izvjestaj.trenerID).Include(t => t.trenerStatistika)
                .Select(s => s.trenerStatistika)
                .Single();

            if (statistika.brojUtakmica == 1)
            {
                statistika.brojUtakmica = 0;
            }
            else
            {
                statistika.brojUtakmica--;
            }

            if (izvjestaj.rezultat == Rezultat.Pobjeda)
            {
                statistika.brojPobjeda--;
            }
            else if (izvjestaj.rezultat == Rezultat.Nerješeno)
            {
                statistika.brojNerjesenih--;
            }
            else if (izvjestaj.rezultat == Rezultat.Poraz)
            {
                statistika.brojPoraza--;
            }
            Utakmica utakmica = db.utakmice.Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.trener)
                .Include(s => s.trener.korisnik)
                .Single();
            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);

            string posiljaoc = utakmica.trener.korisnik.UserName;

            controller.posaljiPoruku("Izvještaj za utakmicu " + utakmica.domacin.nazivKluba + " - " + utakmica.gost.nazivKluba + "(" + utakmica.datumOdigravanja.ToString("dd.MM.yyyy") + ") je obrisan!", "error",false,posiljaoc);

            db.SaveChanges();



            //List<UtakmicaOcjene> ocjene = db.ocjene.Where(s => s.utakmicaID == utakmicaID).ToList();
            //db.RemoveRange(ocjene);
            //db.SaveChanges();

            db.Remove(izvjestaj);
            db.SaveChanges();

            utakmica.statusUtakmice = StatusUtakmice.poRasporedu;
            db.SaveChanges();

            TempData["porukaInfo"] = "Uspješno ste obrisali zapis!";
            return Redirect("Prikaz");
        }
        public List<Igrac> pozvaniIgraciPrvaPostava(int utakmicaID)
        {
            PrvaPostava postava = db.prvaPostava.Where(s => s.utakmicaID == utakmicaID).Single();
            List<Igrac> pozvaniIgraci = new List<Igrac>();
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.golmanID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.lijeviBekID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.desniBekID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.prviStoperID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.drugiStoperID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.prviZadnjiVezniID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.drugiZadnjiVezniID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.prednjiVezniID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.lijevoKriloID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.desnoKriloID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == postava.napadacID).Include(s => s.korisnik).Single());

            return pozvaniIgraci;
        }
        public List<Igrac> pozvaniIgraciKlupa(int utakmicaID)
        {
            Klupa klupa = db.klupa.Where(s => s.utakmicaID == utakmicaID).Single();

            List<Igrac> pozvaniIgraci = new List<Igrac>();
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.golmanKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.bekKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.odbranaKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.veznjakKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.kriloKlupaID).Include(s => s.korisnik).Single());
            pozvaniIgraci.Add(db.igraci.Where(s => s.igracID == klupa.napadacKlupaID).Include(s => s.korisnik).Single());

            return pozvaniIgraci;
        }
        [Autorizacija(true, false, false)]
        public IActionResult dodajIzmjenu(int utakmicaID)
        {
            DodajIzmjena model = new DodajIzmjena();
            List<Igrac> prvaPostava = pozvaniIgraciPrvaPostava(utakmicaID);
            List<Igrac> klupa = pozvaniIgraciKlupa(utakmicaID);

            List<Igrac> igraciOUT = db.utakmicaIzmjena.Where(s => s.utakmicaID == utakmicaID).Include(t => t.izmjena).Include(r => r.izmjena.igracOut).Select(i => i.izmjena.igracOut).ToList();
            List<Igrac> igraciIN = db.utakmicaIzmjena.Where(s => s.utakmicaID == utakmicaID).Include(t => t.izmjena).Include(r => r.izmjena.igracIn).Select(i => i.izmjena.igracIn).ToList();

            List<Igrac> validniIgraciPP = new List<Igrac>();
            List<Igrac> validniIgraciK = new List<Igrac>();


            for (int i = 0; i < prvaPostava.Count; i++)
            {
                if (!igraciOUT.Contains(prvaPostava[i]))
                {
                    validniIgraciPP.Add(prvaPostava[i]);
                }
            }

            for (int i = 0; i < klupa.Count; i++)
            {
                if (!igraciIN.Contains(klupa[i]))
                {
                    validniIgraciK.Add(klupa[i]);
                }
            }
            List<SelectListItem> igraciPrvaPostava = validniIgraciPP.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.brojDresa + " " + a.korisnik.imePrezime,
                    Value = a.igracID.ToString()
                };
            });

            List<SelectListItem> igraciKlupa = validniIgraciK.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.brojDresa + " " + a.korisnik.imePrezime,
                    Value = a.igracID.ToString()
                };
            });

            List<string> razloziIzmjene = new List<string>();
            var vrs = Enum.GetValues(typeof(RazlogIzmjene)).Cast<RazlogIzmjene>();
            foreach (RazlogIzmjene l in vrs)
                razloziIzmjene.Add(l.ToString());

            List<SelectListItem> razlozi = razloziIzmjene.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();


            model.igraciKlupa = igraciKlupa;
            model.igraciPrvaPostava = igraciPrvaPostava;
            model.utakmicaID = utakmicaID;
            model.razlozi = razlozi;

            //Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == model.utakmicaID).Single();
            //izvjestaj.izmjene = new List<UtakmicaIzmjena>();
            ViewData["izmjena"] = false;
            return PartialView("DodajIzmjenu", model);
        }
        [Autorizacija(true, false, false)]
        public string snimiIzmjenaDodaj(DodajIzmjena model)
        {

            Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == model.utakmicaID)
                .Include(a => a.izmjene)
                .Single();

            if (izvjestaj.izmjene.Count() == 3)
            {
                return "Sve izmjene su evidentirane!";
            }

            Izmjena izmjena = new Izmjena();
            izmjena.igracInID = model.igracIN;
            izmjena.igracOutID = model.igracOUT;
            izmjena.minuta = model.minuta;
            izmjena.razlog = model.razlog;

            db.Add(izmjena);
            db.SaveChanges();


            if (izvjestaj.izmjene == null)
            {
                izvjestaj.izmjene = new List<UtakmicaIzmjena>();
            }

            UtakmicaIzmjena izmjenaUtakmica = new UtakmicaIzmjena();
            izmjenaUtakmica.utakmicaID = model.utakmicaID;
            izmjenaUtakmica.izmjenaID = izmjena.izmjenaID;

            db.Add(izmjenaUtakmica);
            db.SaveChanges();

            izvjestaj.izmjene.Add(izmjenaUtakmica);

            db.SaveChanges();

            return "OK";
        }
        public IActionResult getIzmjene(int utakmicaID)
        {
            IzmjenaVM model = new IzmjenaVM();


            Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID).Single();

            List<UtakmicaIzmjena> izmjene = db.utakmicaIzmjena.Where(s => s.utakmicaID == utakmicaID).ToList();

            List<IzmjenaVM.Izmjena> izmjeneVM = izmjene
                    .Select(s => new IzmjenaVM.Izmjena
                    {
                        izmjenaID = s.izmjenaID,
                        igracIN = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracIn).Include(k => k.igracIn.korisnik).Select(im => im.igracIn.korisnik.imePrezime).Single(),
                        brojDresaIN = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracIn).Select(im => im.igracIn.brojDresa).Single(),
                        slikaIN = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracIn).Select(im => im.igracIn.slika).Single(),
                        igracOUT = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracOut).Include(k => k.igracOut.korisnik).Select(im => im.igracOut.korisnik.imePrezime).Single(),
                        brojDresaOUT = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracOut).Select(im => im.igracOut.brojDresa).Single(),
                        slikaOUT = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Include(i => i.igracOut).Select(im => im.igracOut.slika).Single(),
                        minuta = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Select(im => im.minuta).Single(),
                        razlog = db.izmjena.Where(t => t.izmjenaID == s.izmjenaID).Select(im => im.razlog.ToString()).Single(),
                    }).ToList();

            model.izmjene = izmjeneVM;
            model.utakmicaID = utakmicaID;

            return PartialView("getIzmjene", model);
        }
        [Autorizacija(true, false, false)]
        public string obrisiIzmjenu(int izmjenaID, int utakmicaID)
        {
            Izvještaj izvjestaj = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID)
                .Include(s => s.izmjene)
                .Single();

            Izmjena izmjena = db.izmjena.Find(izmjenaID);


            UtakmicaIzmjena utakmIzmjena = db.utakmicaIzmjena.Where(s => (s.izmjenaID == izmjenaID && s.utakmicaID == utakmicaID)).Single();
            izvjestaj.izmjene.Remove(utakmIzmjena);
            db.SaveChanges();

            db.Remove(utakmIzmjena);
            db.SaveChanges();

            db.Remove(izmjena);
            db.SaveChanges();


            return "OK";
        }
        [Autorizacija(true, false, false)]
        public IActionResult urediIzmjenu(int izmjenaID, int utakmicaID)
        {
            DodajIzmjena model = new DodajIzmjena();
            List<Igrac> prvaPostava = pozvaniIgraciPrvaPostava(utakmicaID);
            List<Igrac> klupa = pozvaniIgraciKlupa(utakmicaID);

            List<Igrac> igraciOUT = db.utakmicaIzmjena.Where(s => s.utakmicaID == utakmicaID).Include(t => t.izmjena).Include(r => r.izmjena.igracOut).Select(i => i.izmjena.igracOut).ToList();
            List<Igrac> igraciIN = db.utakmicaIzmjena.Where(s => s.utakmicaID == utakmicaID).Include(t => t.izmjena).Include(r => r.izmjena.igracIn).Select(i => i.izmjena.igracIn).ToList();


            List<SelectListItem> igraciPrvaPostava = prvaPostava.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.brojDresa + " " + a.korisnik.imePrezime,
                    Value = a.igracID.ToString()
                };
            });

            List<SelectListItem> igraciKlupa = klupa.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.brojDresa + " " + a.korisnik.imePrezime,
                    Value = a.igracID.ToString()
                };
            });
            List<string> razloziIzmjene = new List<string>();
            var vrs = Enum.GetValues(typeof(RazlogIzmjene)).Cast<RazlogIzmjene>();
            foreach (RazlogIzmjene l in vrs)
                razloziIzmjene.Add(l.ToString());

            List<SelectListItem> razlozi = razloziIzmjene.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();
            Izmjena izmjena = db.izmjena.Find(izmjenaID);

            model.igraciKlupa = igraciKlupa;
            model.igraciPrvaPostava = igraciPrvaPostava;
            model.utakmicaID = utakmicaID;
            model.igracIN = izmjena.igracInID;
            model.igracOUT = izmjena.igracOutID;
            model.minuta = izmjena.minuta;
            model.razlozi = razlozi;
            model.razlog = izmjena.razlog;
            model.izmjenaID = izmjena.izmjenaID;

            ViewData["izmjena"] = true;

            return PartialView("dodajIzmjenu", model);
        }
        [Autorizacija(true, false, false)]
        public string spasiIzmjeneIzmjene(DodajIzmjena model)
        {

            Izmjena izmjena = db.izmjena.Find(model.izmjenaID);

            izmjena.izmjenaID = model.izmjenaID;
            izmjena.igracInID = model.igracIN;
            izmjena.igracOutID = model.igracOUT;
            izmjena.minuta = model.minuta;
            izmjena.razlog = model.razlog;

            db.SaveChanges();

            return "OK";
        }
        public IActionResult getZadnjaStatistika(int igracID)
        {
            int utakmicaID = 0;
            int brojNastupa = db.nastupi
                .Where(s => s.igracID == igracID).Count();

            if (brojNastupa == 0)
            {
                return PartialView("nemaNastupa");
            }

            UtakmicaNastupi nastup = db.nastupi
                .Where(s => s.igracID == igracID)
                .Include(t => t.utakmica)
                .OrderByDescending(t => t.utakmica.datumOdigravanja)
                .First();

            utakmicaID = nastup.utakmicaID;

            Izvještaj i = db.izvjestaji.Where(s => s.utakmicaID == utakmicaID).Single();

            ZadnjaUtakmicaOcjene model = db.ocjene
                .Where(s => (s.igracID == igracID && s.utakmicaID == utakmicaID))
                .Select(s => new ZadnjaUtakmicaOcjene
                {
                    igracID = s.igracID,
                    utakmicaID = s.utakmicaID,
                    slikaVrstaTakmicenja = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                    domacin = s.utakmica.domacin.nazivKluba,
                    domacinGrb = s.utakmica.domacin.grbKluba,
                    stadion = s.utakmica.stadion.imeStadiona,
                    lokacija = s.utakmica.domacin.grad.nazivGrada,
                    gost = s.utakmica.gost.nazivKluba,
                    gostGrb = s.utakmica.gost.grbKluba,
                    goloviDomacih = i.goloviDomacih,
                    goloviGostiju = i.goloviGostiju,
                    rezultat = i.rezultat,
                    imePrezime = s.igrac.korisnik.imePrezime,
                    slika = s.igrac.slika,
                    brojDresa = s.igrac.brojDresa,
                    pozicija = s.igrac.pozicija.ToString(),
                    kontrolaLopte = s.kontrolaLopte,
                    komentar = nastup.komentar,
                    dribljanje = s.dribljanje,
                    dodavanje = s.dodavanje,
                    kretanje = s.kretanje,
                    brzina = s.brzina,
                    sut = s.sut,
                    snaga = s.snaga,
                    markiranje = s.markiranje,
                    klizeciStart = s.klizeciStart,
                    skok = s.klizeciStart,
                    odbrana = s.odbrana,
                    prosjecnaOcjena = s.prosjecnaOcjena,
                    minute = db.nastupi.Where(s => s.igracID == igracID).Include(t => t.utakmica).OrderByDescending(t => t.utakmica.datumOdigravanja).Select(s => s.minute).First(),
                    golovi = db.nastupi.Where(s => s.igracID == igracID).Include(t => t.utakmica).OrderByDescending(t => t.utakmica.datumOdigravanja).Select(s => s.golovi).First(),
                    asistencije = db.nastupi.Where(s => s.igracID == igracID).Include(t => t.utakmica).OrderByDescending(t => t.utakmica.datumOdigravanja).Select(s => s.asistencije).First(),
                    zutiKartoni = db.nastupi.Where(s => s.igracID == igracID).Include(t => t.utakmica).OrderByDescending(t => t.utakmica.datumOdigravanja).Select(s => s.zutiKartoni).First(),
                    crveniKartoni = db.nastupi.Where(s => s.igracID == igracID).Include(t => t.utakmica).OrderByDescending(t => t.utakmica.datumOdigravanja).Select(s => s.crveniKartoni).First(),
                    ocjena = db.nastupi.Where(s => s.igracID == igracID).Include(t => t.utakmica).OrderByDescending(t => t.utakmica.datumOdigravanja).Select(s => s.ocjena).First(),
                }).Single();

            return PartialView("getZadnjeOcjene", model);


        }
    }
}
