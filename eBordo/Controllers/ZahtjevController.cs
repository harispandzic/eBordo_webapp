using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels.Zahtjev;
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
    public class ZahtjevController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;

        public ZahtjevController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }
        public string PonistiZahtjev(int zahtjevID)
        {
            Zahtjev zahtjev = db.zahtjevi.Where(s => s.zahtjevID == zahtjevID).Include(s => s.korisnik).Single();
            db.Remove(zahtjev);
            db.SaveChanges();
            NotifikacijaController nc = new NotifikacijaController(db, userManager, hubContext);
            string poruka = "Vaš zahtjev za " + zahtjev.svrha.ToString() + " je poništen!";
            nc.posaljiJednom(poruka, "error", zahtjev.korisnik, false, "admin@fksarajevo.ba");

            return "OK";
        }
        public IActionResult PrikaziDetalje(int zahtjevID)
        {
            DetaljiZahtjev model = db.zahtjevi
                .Where(s => s.zahtjevID == zahtjevID)
                .Select(s => new DetaljiZahtjev
                {
                    datum = s.datum.ToString("dd.MM.yyyy"),
                    svrha = s.svrha.ToString(),
                    tip = s.tipZahtjeva.ToString(),
                    status = s.statusZahtjeva.ToString(),
                    prioritet = s.prioritet.ToString(),
                    odgovor = s.odgovor
                }).Single();

            return PartialView("Detalji",model);
        }
        public int getBrojZahtjeva(string korisnikID)
        {
            if(korisnikID == null)
            {
                return db.zahtjevi.Where(s => s.statusZahtjeva == StatusZahtjeva.Obrada).Count();
            }
            else
            {
                return db.zahtjevi.Where(s => (s.korisnikID == korisnikID && s.statusZahtjeva == StatusZahtjeva.Obrada)).Count();
            }
        }
        public IActionResult Prikaz(string korisnikID)
        {
            List<ZahtjevPrikazVM.Row> zahtjevi;

            if (korisnikID != null)
            {
                zahtjevi = db.zahtjevi
                         .Where(a => a.korisnikID == korisnikID)
                         .Select(x => new ZahtjevPrikazVM.Row
                         {
                             zahtjevID = x.zahtjevID,
                             datum = x.datum.ToString("dd.MM.yyyy"),
                             svrha = x.svrha.ToString(),
                             tip = x.tipZahtjeva.ToString(),
                             status = x.statusZahtjeva.ToString(),
                             prioritet = x.prioritet.ToString(),
                             korisnik = x.korisnik.imePrezime,
                             prijeDana = (x.datum.Date - DateTime.Now.Date).Days,
                         }).ToList();
            }
            else
            {
                zahtjevi = db.zahtjevi
                         .Where(a => a.statusZahtjeva == StatusZahtjeva.Obrada)
                         .Select(x => new ZahtjevPrikazVM.Row
                         {
                             zahtjevID = x.zahtjevID,
                             datum = x.datum.ToString("dd.MM.yyyy"),
                             svrha = x.svrha.ToString(),
                             tip = x.tipZahtjeva.ToString(),
                             status = x.statusZahtjeva.ToString(),
                             prioritet = x.prioritet.ToString(),
                             korisnik = x.korisnik.imePrezime,
                             prijeDana = (x.datum.Date - DateTime.Now.Date).Days,
                         }).ToList();
            }
            ZahtjevPrikazVM model = new ZahtjevPrikazVM();
            model.zahtjevi = zahtjevi;

            return PartialView(model);
        }
        public string spasiZahtjev(DodajZahtjevVM model)
        {
            Korisnik logiraniKorisnik = Autentifikacija.LogiraniKorisnik(HttpContext);
            Zahtjev zahtjev;

            if(model.zahtjevID == 0)
            {
                zahtjev = new Zahtjev();
                zahtjev.korisnikID = logiraniKorisnik.Id;
                zahtjev.svrha = (SvrhaZahtjeva)Enum.Parse(typeof(SvrhaZahtjeva), model.svrhaID);
                zahtjev.tipZahtjeva = (TipZahtjeva)Enum.Parse(typeof(TipZahtjeva), model.tipID);
                zahtjev.statusZahtjeva = StatusZahtjeva.Obrada;
                zahtjev.prioritet = (PrioritetZahtjeva)Enum.Parse(typeof(PrioritetZahtjeva), model.prioritetID);
                zahtjev.datum = model.datum;
                db.Add(zahtjev);
                db.SaveChanges();
                NotifikacijaController nc = new NotifikacijaController(db, userManager, hubContext);
                string poruka = "Vaš zahtjev za " + zahtjev.svrha.ToString() + " je zaprimljen!";
                nc.posaljiJednom(poruka, "success", zahtjev.korisnik, false, "admin@fksarajevo.ba");
                return "Dodan";
            }
            else
            {
                zahtjev = db.zahtjevi.Where(s => s.zahtjevID == model.zahtjevID).Include(s => s.korisnik).Single();
                zahtjev.odgovor = model.odgovor;
                zahtjev.statusZahtjeva = (StatusZahtjeva)Enum.Parse(typeof(StatusZahtjeva), model.statusID);
                db.SaveChanges();
                NotifikacijaController nc = new NotifikacijaController(db, userManager, hubContext);
                string poruka = "Vaš zahtjev za " + zahtjev.svrha.ToString() + " je ";
                string tipPoruke;
                if (zahtjev.statusZahtjeva == StatusZahtjeva.Odbijen)
                {
                    poruka += "odbijen!";
                    tipPoruke = "error";
                }
                else
                {
                    poruka += "završen!";
                    tipPoruke = "success";
                }
                nc.posaljiJednom(poruka, tipPoruke, zahtjev.korisnik, false, "admin@fksarajevo.ba");
                return "Uredjen";
            }

        }
        public IActionResult DodajZahtjev(int zahtjevID)
        {
            List<string> svrheZahtjevaString = new List<string>();
            var svrha = Enum.GetValues(typeof(SvrhaZahtjeva)).Cast<SvrhaZahtjeva>();
            foreach (SvrhaZahtjeva l in svrha)
                svrheZahtjevaString.Add(l.ToString());

            List<SelectListItem> svrhe = svrheZahtjevaString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<string> tipoviZahtjevaString = new List<string>();
            var tip = Enum.GetValues(typeof(TipZahtjeva)).Cast<TipZahtjeva>();
            foreach (TipZahtjeva l in tip)
                tipoviZahtjevaString.Add(l.ToString());

            List<SelectListItem> tipovi = tipoviZahtjevaString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();


            List<string> prioritetiZahtjevaString = new List<string>();
            var prioritet = Enum.GetValues(typeof(PrioritetZahtjeva)).Cast<PrioritetZahtjeva>();
            foreach (PrioritetZahtjeva l in prioritet)
                prioritetiZahtjevaString.Add(l.ToString());

            List<SelectListItem> prioriteti = prioritetiZahtjevaString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();


            List<string> statustiZahtjevaString = new List<string>();
            var status = Enum.GetValues(typeof(StatusZahtjeva)).Cast<StatusZahtjeva>();
            foreach (StatusZahtjeva l in status)
            {
                if(l != StatusZahtjeva.Obrada)
                    statustiZahtjevaString.Add(l.ToString());
            }

            List<SelectListItem> statusi = statustiZahtjevaString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            DodajZahtjevVM model = new DodajZahtjevVM();

            if(zahtjevID == 0)
            {
                model.zahtjevID = 0;
                model.datum = DateTime.Now;
                model.svrhe = svrhe;
                model.tipoviZahtjeva = tipovi;
                model.prioriteti = prioriteti;
            }
            else
            {
                Zahtjev zahtjev = db.zahtjevi.Find(zahtjevID);
                model.zahtjevID = zahtjev.zahtjevID;
                model.datum = zahtjev.datum;
                model.svrhe = svrhe;
                model.svrhaID = zahtjev.svrha.ToString();
                model.tipoviZahtjeva = tipovi;
                model.tipID = zahtjev.tipZahtjeva.ToString();
                model.prioriteti = prioriteti;
                model.prioritetID = zahtjev.prioritet.ToString();
                model.statusiZahtjeva = statusi;
                model.statusID = zahtjev.statusZahtjeva.ToString();
            }

            return PartialView(model);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
