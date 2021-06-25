using Data.Data;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels;
using eBordo.ViewModels.Notifikacija;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.Controllers
{
    public class NotifikacijaController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;
        


        public NotifikacijaController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }
        public IActionResult Prikaz(string korisnikID,bool poruka)
        {
            Korisnik korisnik = db.korisnici.Where(s => s.Id == korisnikID).Include(s => s.notifikacije).Single();

            List<NotifikacijeVM.Row> notifikacije = korisnik.notifikacije
                .Where(s => (s.statusNotifikacije == StatusNotifikacije.Nepročitana && s.poruka == poruka))
                .Select(s => new NotifikacijeVM.Row
                {
                    notifikacijaID = s.notifikacijaID,
                    tekstNotifikacije = s.tekstNotifikacije,
                    datumNotifikacije = s.datumNotifikacije,
                    tipNotifikacije = s.tipNotifikacije,
                    poslao = s.posiljaoc
                }).OrderByDescending(s => s.datumNotifikacije).ToList();

            NotifikacijeVM model = new NotifikacijeVM();
            model.notifikacije = notifikacije;

            ViewData["poruka"] = poruka;

            return PartialView("Prikaz",model);
        }
        public int getBrojNotifikacija(string korisnikID, bool tip)
        {
            Korisnik korisnik = db.korisnici.Where(s => s.Id == korisnikID).Include(s => s.notifikacije).Single();
            int broj = 0;
            foreach (var i in korisnik.notifikacije)
            {
                if(i.statusNotifikacije == StatusNotifikacije.Nepročitana && i.poruka == tip)
                {
                    broj++;
                }
            }
            return broj;

        }
        public string oznaciKaoProcitana(int notifikacijaID)
        {
            Notifikacija notifikacija = db.notifikacije.Find(notifikacijaID);
            notifikacija.statusNotifikacije = StatusNotifikacije.Pročitana;

            db.SaveChanges();
            return "OK";
        }
        public string oznaciSveKaoProcitano(string korisnikID, bool tip)
        {
            Korisnik korisnik = db.korisnici.Include(s => s.notifikacije).Where(s => s.Id == korisnikID).Single();

            foreach (var n in korisnik.notifikacije)
            {
                if (n.poruka == tip)
                {
                    n.statusNotifikacije = StatusNotifikacije.Pročitana;
                }
            }

            db.SaveChanges();
            return "OK";
        }
        public string posaljiPoruku(string tekstPoruke, string tipPoruke, bool tip, string posiljaoc)
        {
            List<Korisnik> korisnici = db.korisnici.ToList();
            for (int i = 0; i < korisnici.Count(); i++)
            {
                Notifikacija notifikacija = new Notifikacija();
                Korisnik k = korisnici[i];
                string poruka = tekstPoruke;
                hubContext.Clients.Group(k.UserName).SendAsync("prijemPoruke", tipPoruke, poruka);
                notifikacija.datumNotifikacije = DateTime.Now;
                notifikacija.statusNotifikacije = StatusNotifikacije.Nepročitana;
                notifikacija.tekstNotifikacije = poruka;
                if(tipPoruke == "success")
                {
                    notifikacija.tipNotifikacije = TipNotifikacije.Uspješno;
                }
                if (tipPoruke == "warning")
                {
                    notifikacija.tipNotifikacije = TipNotifikacije.Upozorenje;
                }
                if (tipPoruke == "error")
                {
                    notifikacija.tipNotifikacije = TipNotifikacije.Greška;
                }
                if (tipPoruke == "info")
                {
                    notifikacija.tipNotifikacije = TipNotifikacije.Obavještenje;
                }
                notifikacija.poruka = tip;
                notifikacija.posiljaoc = posiljaoc;
                db.Add(notifikacija);
                db.SaveChanges();
                //k.notifikacije = new List<Notifikacija>();
                k.notifikacije.Add(notifikacija);
                db.SaveChanges();
            }
            return "OK";
        }
        public string posaljiPorukuOdredjenim(string tekstPoruke, string tipPoruke, List<Korisnik> korisnici, bool tip, string posiljaoc)
        {
            for (int i = 0; i < korisnici.Count(); i++)
            {
                Notifikacija notifikacija = new Notifikacija();
                Korisnik k = korisnici[i];
                string poruka = tekstPoruke;
                hubContext.Clients.Group(k.UserName).SendAsync("prijemPoruke", tipPoruke, poruka);
                notifikacija.datumNotifikacije = DateTime.Now;
                notifikacija.statusNotifikacije = StatusNotifikacije.Nepročitana;
                notifikacija.tekstNotifikacije = poruka;
                if (tipPoruke == "success")
                {
                    notifikacija.tipNotifikacije = TipNotifikacije.Uspješno;
                }
                if (tipPoruke == "warning")
                {
                    notifikacija.tipNotifikacije = TipNotifikacije.Upozorenje;
                }
                if (tipPoruke == "error")
                {
                    notifikacija.tipNotifikacije = TipNotifikacije.Greška;
                }
                if (tipPoruke == "info")
                {
                    notifikacija.tipNotifikacije = TipNotifikacije.Obavještenje;
                }
                notifikacija.poruka = tip;
                notifikacija.posiljaoc = posiljaoc;
                db.Add(notifikacija);
                db.SaveChanges();
                k.notifikacije.Add(notifikacija);
                db.SaveChanges();
            }
            return "OK";
        }
        public string posaljiJednom(string tekstPoruke, string tipPoruke, Korisnik korisnik, bool tip, string posiljaoc)
        {
            Notifikacija notifikacija = new Notifikacija();
            hubContext.Clients.Group(korisnik.UserName).SendAsync("prijemPoruke", tipPoruke, tekstPoruke);
            notifikacija.datumNotifikacije = DateTime.Now;
            notifikacija.statusNotifikacije = StatusNotifikacije.Nepročitana;
            if (tipPoruke == "success")
            {
                notifikacija.tipNotifikacije = TipNotifikacije.Uspješno;
            }
            if (tipPoruke == "warning")
            {
                notifikacija.tipNotifikacije = TipNotifikacije.Upozorenje;
            }
            if (tipPoruke == "error")
            {
                notifikacija.tipNotifikacije = TipNotifikacije.Greška;
            }
            if (tipPoruke == "info")
            {
                notifikacija.tipNotifikacije = TipNotifikacije.Obavještenje;
            }
            notifikacija.tekstNotifikacije = tekstPoruke;
            notifikacija.poruka = tip;
            db.Add(notifikacija);
            notifikacija.posiljaoc = posiljaoc;
            db.SaveChanges();
            //k.notifikacije = new List<Notifikacija>();
            korisnik.notifikacije.Add(notifikacija);
            db.SaveChanges();
            return "OK";
        }
    }
}
