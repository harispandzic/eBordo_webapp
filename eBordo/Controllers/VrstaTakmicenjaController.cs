using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo_seminarski_rad.ViewModels.VrstaTakmicenja;
using Microsoft.AspNetCore.Mvc;

namespace eBordo_seminarski_rad.Controllers
{
    public class VrstaTakmicenjaController : Controller
    {
        private ApplicationDbContext db;

        public VrstaTakmicenjaController(ApplicationDbContext database)
        {
            db = database;
        }

        [Autorizacija(false, false, true)]
        public IActionResult Prikaz(string q)
        {
            List<VrstaTakmicenjaPrikaziVM.Row> vrsteTakmicenja = db.vrsteTakmicenja
                .Where(a => q == "" || q == null || a.nazivVrsteTakmicenja.StartsWith(q))
                .Select(x => new VrstaTakmicenjaPrikaziVM.Row
                {
                    vrstaTakmicenjaID = x.vrstaTakmicenjaID,
                    nazivVrsteTakmicenja = x.nazivVrsteTakmicenja,
                    logoCurrent = x.logoTakmicenja
                }).ToList();

            VrstaTakmicenjaPrikaziVM model = new VrstaTakmicenjaPrikaziVM();
            model.vrsteTakmicenja = vrsteTakmicenja;
            model.q = q;

            if (model.vrsteTakmicenja.Count() == 0 && q != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            return View("Prikaz", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult UrediDodaj(int vrstaTakmicenjaID)
        {
            VrstaTakmicenjaUrediDodajVM model = vrstaTakmicenjaID == 0 ? new VrstaTakmicenjaUrediDodajVM()
            {

            } :
            db.vrsteTakmicenja
            .Where(k => k.vrstaTakmicenjaID == vrstaTakmicenjaID)
            .Select(a => new VrstaTakmicenjaUrediDodajVM
            {
                vrstaTakmicenjaID = a.vrstaTakmicenjaID,
                nazivVrsteTakmicenja = a.nazivVrsteTakmicenja,
                logoCurrent = a.logoTakmicenja
            }).Single();

            return PartialView("UrediDodaj", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult SnimiUrediDodaj(VrstaTakmicenjaUrediDodajVM model)
        {
            List<VrstaTakmicenja> vrsteTakmicenja = db.vrsteTakmicenja.ToList();
            foreach (var vt in vrsteTakmicenja)
            {
                if (vt.nazivVrsteTakmicenja == model.nazivVrsteTakmicenja)
                {
                    TempData["porukaInfo"] = "Zapis već postoji!";
                    return Redirect("Prikaz");
                }
            }
            VrstaTakmicenja vrstaTakmicenja;
            if (model.vrstaTakmicenjaID == 0)
            {
                vrstaTakmicenja = new VrstaTakmicenja();
                db.Add(vrstaTakmicenja);
                TempData["porukaInfo"] = "Uspješno ste dodali zapis!";
            }
            else
            {
                vrstaTakmicenja = db.vrsteTakmicenja.Find(model.vrstaTakmicenjaID);
                TempData["porukaInfo"] = "Uspješno ste uredili zapis!";
            }
            if (model.logoNew != null)
            {
                string ekstenzija = Path.GetExtension(model.logoNew.FileName);
                string contentType = model.logoNew.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.logoNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                vrstaTakmicenja.logoTakmicenja = filename;
            }
            vrstaTakmicenja.nazivVrsteTakmicenja = model.nazivVrsteTakmicenja;
            db.SaveChanges();
            return Redirect("Prikaz");
        }
        [Autorizacija(false, false, true)]
        public IActionResult Obrisi(int vrstaTakmicenjaID)
        {
            VrstaTakmicenjaObrisiVM model = new VrstaTakmicenjaObrisiVM();
            model.vrstaTakmicenjaID = vrstaTakmicenjaID;
            return PartialView("Obrisi", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult ObrisiSnimi(int vrstaTakmicenjaID)
        {
            List<VrstaTakmicenja> vrsteTakmicenja = db.vrsteTakmicenja.ToList();
            if (vrstaTakmicenjaID == 0)
            {
                foreach (var vrstaTakmicenja in vrsteTakmicenja)
                {
                    db.Remove(vrstaTakmicenja);
                }
                db.SaveChanges();
                TempData["porukaInfo"] = "Uspješno ste obrisali sve zapise!";
                return Redirect("Prikaz");
            }
            VrstaTakmicenja vt = db.vrsteTakmicenja.Find(vrstaTakmicenjaID);
            db.Remove(vt);
            db.SaveChanges();
            TempData["porukaInfo"] = "Uspješno ste obrisali zapis!";
            return Redirect("Prikaz");
        }
    }
}