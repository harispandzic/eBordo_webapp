using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo_seminarski_rad.ViewModels.Grad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eBordo_seminarski_rad.Controllers
{
    public class GradController : Controller
    {
        private ApplicationDbContext db;

        public GradController(ApplicationDbContext database)
        {
            db = database;
        }
        [Autorizacija(false, false, true)]
        public IActionResult Prikaz(string q, bool sort, string nazivDrzave)
        {
            List<SelectListItem> drzave = db.drzave
                .OrderBy(a => a.nazivDrzave)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivDrzave,
                    Value = a.drzavaID.ToString()
                }).ToList();

            List<GradPrikaziVM.Row> gradovi;

            if (nazivDrzave != null)
            {
                gradovi = db.gradovi
                            .Where(a => a.drzava.nazivDrzave.StartsWith(nazivDrzave))
                            .Select(x => new GradPrikaziVM.Row
                            {
                                gradID = x.gradID,
                                nazivGrada = x.nazivGrada,
                                nazivDrzave = x.drzava.nazivDrzave,
                                zastavaCurrent = x.drzava.zastavaDrzave
                            }).ToList();
                TempData["sort"] = false;
            }
            else if (sort)
            {
                gradovi = db.gradovi
                .Where(a => q == "" || q == null ||  a.nazivGrada.StartsWith(q))
                .Select(x => new GradPrikaziVM.Row
                {
                    gradID = x.gradID,
                    nazivGrada = x.nazivGrada,
                    nazivDrzave = x.drzava.nazivDrzave,
                    zastavaCurrent = x.drzava.zastavaDrzave
                }).OrderBy(a => a.nazivDrzave).ToList();
                TempData["sort"] = true;
            }
            else
            {
                gradovi = db.gradovi
                            .Where(a => q == "" || q == null || a.nazivGrada.StartsWith(q))
                            .Select(x => new GradPrikaziVM.Row
                            {
                                gradID = x.gradID,
                                nazivGrada = x.nazivGrada,
                                nazivDrzave = x.drzava.nazivDrzave,
                                zastavaCurrent = x.drzava.zastavaDrzave
                            }).ToList();
                TempData["sort"] = false;
            }
            GradPrikaziVM model = new GradPrikaziVM();
            model.gradovi = gradovi;
            model.drzave = drzave;
            //Filter&&search
            model.nazivDrzave = nazivDrzave;
            model.q = q;

            if (model.gradovi.Count() == 0 && q != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.gradovi.Count() == 0 && nazivDrzave != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            return View("Prikaz", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult UrediDodaj(int gradID)
        {
            List<SelectListItem> drzave = db.drzave
                .OrderBy(a => a.nazivDrzave)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivDrzave,
                    Value = a.drzavaID.ToString()
                }).ToList();

            GradUrediDodajVM model = gradID == 0 ? new GradUrediDodajVM()
            {

            } :
            db.gradovi
            .Where(g => g.gradID == gradID)
            .Select(a => new GradUrediDodajVM
            {
                gradID = a.gradID,
                nazivGrada = a.nazivGrada,
                drzavaID = a.drzavaID
            }).Single();
            model.drzave = drzave;

            return PartialView("UrediDodaj", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult SnimiUrediDodaj(GradUrediDodajVM model)
        {
            List<Grad> gradovi = db.gradovi.ToList();
            foreach (var g in gradovi)
            {
                if (g.nazivGrada == model.nazivGrada)
                {
                    TempData["porukaInfo"] = "Zapis već postoji!";
                    return Redirect("Prikaz");
                }
            }
            Grad grad;
            if (model.gradID == 0)
            {
                grad = new Grad();
                db.Add(grad);
                grad.drzavaID = model.drzavaID;
                TempData["porukaInfo"] = "Uspješno ste dodali zapis!";
            }
            else
            {
                grad = db.gradovi.Find(model.gradID);
                TempData["porukaInfo"] = "Uspješno ste uredili zapis!";
            }
            grad.nazivGrada = model.nazivGrada;
            db.SaveChanges();
            return Redirect("Prikaz");
        }
        [Autorizacija(false, false, true)]
        public IActionResult Obrisi(int gradID)
        {
            GradObrisiVM model = new GradObrisiVM();
            model.gradID = gradID;
            return PartialView("Obrisi", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult ObrisiSnimi(int gradID)
        {
            List<Grad> gradovi = db.gradovi.ToList();
            if (gradID == 0)
            {
                foreach (var grad in gradovi)
                {
                    db.Remove(grad);
                }
                db.SaveChanges();
                TempData["porukaInfo"] = "Uspješno ste obrisali sve zapise!";
                return Redirect("Prikaz");
            }
            Grad g = db.gradovi.Find(gradID);
            db.Remove(g);
            db.SaveChanges();
            TempData["porukaInfo"] = "Uspješno ste obrisali zapis!";
            return Redirect("Prikaz");
        }
    }
}