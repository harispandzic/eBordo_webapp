using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks; 
using Data.EntityModels;
using eBordo;
using eBordo.Data;
using eBordo.Helper;
using eBordo_seminarski_rad.ViewModels.Drzava;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;

namespace eBordo_seminarski_rad.Controllers
{
    public class DrzavaController : Controller
    {
        private ApplicationDbContext db;
        private IHubContext<MyHub> hubContext;

        public DrzavaController(ApplicationDbContext database, IHubContext<MyHub> _hubContext)
        {
            db = database;
            hubContext = _hubContext;
        }

        [Autorizacija(false, false, true)]
        public IActionResult Prikaz(string q,bool sort)
        {
            List<DrzavaPrikaziVM.Row> drzave;
            if (sort)
            {
                drzave = db.drzave
                         .Where(a => q == "" || q == null || a.nazivDrzave.StartsWith(q))
                         .Select(x => new DrzavaPrikaziVM.Row
                         {
                             drzavaID = x.drzavaID,
                             nazivDrzave = x.nazivDrzave,
                             zastavaCurrent = x.zastavaDrzave
                         }).OrderBy(a => a.nazivDrzave).ToList();
                        TempData["sort"] = true;
            }
            else
            {
                drzave = db.drzave
                            .Where(a => q == "" || q == null || a.nazivDrzave.StartsWith(q))
                            .Select(x => new DrzavaPrikaziVM.Row
                            {
                                drzavaID = x.drzavaID,
                                nazivDrzave = x.nazivDrzave,
                                zastavaCurrent = x.zastavaDrzave
                            }).ToList();
                TempData["sort"] = false;
            }
            DrzavaPrikaziVM model = new DrzavaPrikaziVM();
            model.drzave = drzave;
            model.q = q;

            if (model.drzave.Count() == 0 && q!=null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            return View("Prikaz",model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult UrediDodaj(int drzavaID)
        {
            DrzavaUrediDodajVM model = drzavaID == 0 ? new DrzavaUrediDodajVM()
            {

            } :
            db.drzave
            .Where(d => d.drzavaID == drzavaID)
            .Select(a => new DrzavaUrediDodajVM
            {
                drzavaID = a.drzavaID,
                nazivDrzave = a.nazivDrzave,
                zastavaCurrent = a.zastavaDrzave
            }).Single();

            return PartialView("UrediDodaj", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult SnimiUrediDodaj(DrzavaUrediDodajVM model)
        {
            List < Drzava > drzave = db.drzave.ToList();
            foreach (var d in drzave)
            {
                if (d.nazivDrzave == model.nazivDrzave)
                {
                    TempData["porukaInfo"] = "Zapis već postoji!";
                    return Redirect("Prikaz");
                }
            }
            Drzava drzava;
            if(model.drzavaID == 0)
            {
                drzava = new Drzava();
                db.Add(drzava);
                TempData["porukaInfo"] = "Uspješno ste dodali zapis!";
            }
            else
            {
                drzava = db.drzave.Find(model.drzavaID);
                TempData["porukaInfo"] = "Uspješno ste uredili zapis!";
            }

            if (model.zastavaNew != null)
            {
                string ekstenzija = Path.GetExtension(model.zastavaNew.FileName);
                string contentType = model.zastavaNew.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.zastavaNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                drzava.zastavaDrzave = filename;
            }

            drzava.nazivDrzave = model.nazivDrzave;
            db.SaveChanges();

            return Redirect("Prikaz");
        }
        [Autorizacija(false, false, true)]
        public IActionResult Obrisi(int drzavaID)
        {
            DrzavaObrisiVM model = new DrzavaObrisiVM();
            model.drzavaID = drzavaID;
            return PartialView("Obrisi", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult ObrisiSnimi(int drzavaID)
        {
            List<Drzava> drzave = db.drzave.ToList();

            List<Grad> gradoviZaBrisati = db.gradovi.Where(g => g.drzavaID == drzavaID).ToList();
            db.RemoveRange(gradoviZaBrisati);
            db.SaveChanges();

            if(drzavaID == 0)
            {
                foreach (var drzava in drzave)
                {
                    db.Remove(drzava);
                }
                db.SaveChanges();
                TempData["porukaInfo"] = "Uspješno ste obrisali sve zapise!";
                return Redirect("Prikaz");
            }
            Drzava d = db.drzave.Find(drzavaID);
            db.Remove(d);
            db.SaveChanges();
            TempData["porukaInfo"] = "Uspješno ste obrisali zapis!";
            return Redirect("Prikaz");
        }
    }
}