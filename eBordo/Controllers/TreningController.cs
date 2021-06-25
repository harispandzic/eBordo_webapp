using Data.Data;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels.Trening;
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
    public class TreningController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;
        public TreningController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }

        public IActionResult Prikaz()
        {
            List<TreningPrikazVM.Row> treninzi = db.treninzi
                .Select(s => new TreningPrikazVM.Row
                {
                    treningID = s.treningID,
                    datumOdrzavanja = s.datumOdrzavanja,
                    satnica = s.satnica,
                    lokacija = s.lokacija,
                    fokusTreninga = s.fokusTreninga,
                    trener = s.zabiljezio.imePrezime,
                    trenerSlika = s.zabiljezio.slika,
                    brojDana = (s.datumOdrzavanja.Date - DateTime.Now.Date).Days
                }).ToList();

            TreningPrikazVM model = new TreningPrikazVM();
            model.treninzi = treninzi;

            return PartialView("Prikaz", model);
        }
        [Autorizacija(false, false, true)]
        public string obrisiTrening(int treningID)
        {
            Trening trening = db.treninzi.Where(s => s.treningID == treningID).Include(s => s.zabiljezio).Include(s => s.zabiljezio.korisnik).Single();
            db.Remove(trening);
            db.SaveChanges();

            string poruka, tipPoruke, lokacija;

            if (trening.lokacija == LokacijaTreninga.Stadion)
            {
                lokacija = " na stadionu Koševo!";
            }
            else
            {
                lokacija = " u TC Butmir!";
            }
            poruka = "Trening zakazan za " + trening.datumOdrzavanja.ToString("dd.MM.yyyy") + " u " + trening.satnica + " sati " + lokacija + " je obrisan!";
            tipPoruke = "error";
            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);
            controller.posaljiPoruku(poruka, tipPoruke,false,trening.zabiljezio.korisnik.UserName);

            return "OK";
        }
        [Autorizacija(true, false, false)]
        public IActionResult dodajTrening(int treningID)
        {
            List<string> lokacijeStringArray = new List<string>();
            var lokacija = Enum.GetValues(typeof(LokacijaTreninga)).Cast<LokacijaTreninga>();
            foreach (LokacijaTreninga l in lokacija)
                lokacijeStringArray.Add(l.ToString());

            List<SelectListItem> lokacije = lokacijeStringArray.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            DodajTreningVM model = new DodajTreningVM();

            if (treningID != 0)
            {
                Trening trening = db.treninzi.Find(treningID);
                model.datumOdrzavanja = trening.datumOdrzavanja;
                model.satnica = trening.satnica;
                model.lokacije = lokacije;
                model.lokacijaID = trening.lokacija.ToString();
                model.fokusTreninga = trening.fokusTreninga;
                TempData["uredi"] = true;
            }
            else
            {
                model.datumOdrzavanja = DateTime.Now;
                model.lokacije = lokacije;
                TempData["uredi"] = false;
            }
            
            return PartialView("DodajTrening", model);

        }
        [Autorizacija(true, false, false)]
        public string spasiTrening(DodajTreningVM model)
        {
            LokacijaTreninga lokacijaTreninga = (LokacijaTreninga)Enum.Parse(typeof(LokacijaTreninga), model.lokacijaID);

            Korisnik k = Autentifikacija.LogiraniKorisnik(HttpContext);

            Trening trening;

            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);
            string povratna;

            if (model.treningID != 0)
            {
                trening = db.treninzi.Where(s => s.treningID == model.treningID).Include(s => s.zabiljezio).Include(s => s.zabiljezio.korisnik).Single();
                povratna = "Uredi";
            }
            else
            {
                trening = new Trening();
                db.Add(trening);
                povratna = "Dodaj";
            }

            trening.datumOdrzavanja = model.datumOdrzavanja;
            trening.satnica = model.satnica;
            trening.lokacija = lokacijaTreninga;
            trening.fokusTreninga = model.fokusTreninga;
            trening.zabiljezioID = k.trenerID;
            trening.statusTreninga = StatusUtakmice.poRasporedu;

            db.SaveChanges();

            string poruka, tipPoruke, lokacija;
            List<Korisnik> korisnici = db.korisnici.Where(s => s.isIgrac).ToList();

            if(model.lokacijaID == LokacijaTreninga.Stadion.ToString())
            {
                lokacija = " na stadionu Koševo!";
            }
            else
            {
                lokacija = " u TC Butmir!";
            }

            if (model.treningID != 0)
            {
                poruka = "Trening je ažuriran! Trening je " + trening.datumOdrzavanja.ToString("dd.MM.yyyy") + " u " + trening.satnica + " sati " + lokacija;
                tipPoruke = "warning";
            }
            else
            {
                poruka = "Trening je zakazan za " + trening.datumOdrzavanja.ToString("dd.MM.yyyy") + " u " + trening.satnica + " sati " + lokacija;
                tipPoruke = "success";
            }
            controller.posaljiPoruku(poruka, tipPoruke,false,trening.zabiljezio.korisnik.UserName);

            return povratna;
        }
        public IActionResult Index()
        {
            return View("Index");
        }
        public JsonResult getDays()
        {
            BrojacVM model = new BrojacVM();

            DateTime datum = db.treninzi
                .Where(s => (s.statusTreninga == StatusUtakmice.poRasporedu && (s.datumOdrzavanja.Date > DateTime.Now.Date)))
                .OrderBy(s => s.datumOdrzavanja)
                .Select(s => s.datumOdrzavanja)
                .First();

            int dani = (datum.Date - DateTime.Now.Date).Days;

            string satnica = db.treninzi
                 .Where(s => s.statusTreninga == StatusUtakmice.poRasporedu)
                 .OrderBy(s => s.datumOdrzavanja)
                 .Select(s => s.satnica)
                 .First();

            model.dani = dani;
            model.satnica = satnica;

            return Json(model);

        }
    }
}
