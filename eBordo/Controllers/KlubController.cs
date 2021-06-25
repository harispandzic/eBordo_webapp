using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo_seminarski_rad.ViewModels.Klub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eBordo_seminarski_rad.Controllers
{
    public class KlubController : Controller
    {
        private ApplicationDbContext db;

        public KlubController(ApplicationDbContext database)
        {
            db = database;
        }
        [Autorizacija(false, false, true)]
        public IActionResult Prikaz(string q, bool sort)
        {
            List<KlubPrikaziVM.Row> klubovi;
            if (sort)
            {
                klubovi = db.klubovi
                         .Where(a => q == "" || q == null || a.nazivKluba.StartsWith(q))
                         .Select(x => new KlubPrikaziVM.Row
                         {
                             klubID = x.klubID,
                             nazivKluba = x.nazivKluba,
                             grbCurrent = x.grbKluba
                         }).OrderBy(a => a.nazivKluba).ToList();
                TempData["sort"] = true;
            }
            else
            {
                klubovi = db.klubovi
                            .Where(a => q == "" || q == null || a.nazivKluba.StartsWith(q))
                            .Select(x => new KlubPrikaziVM.Row
                            {
                                klubID = x.klubID,
                                nazivKluba = x.nazivKluba,
                                grbCurrent = x.grbKluba
                            }).ToList();
                TempData["sort"] = false;
            }
            KlubPrikaziVM model = new KlubPrikaziVM();
            model.klubovi = klubovi;
            model.q = q;

            if (model.klubovi.Count() == 0 && q != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            return View("Prikaz", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult UrediDodaj(int klubID)
        {
            List<SelectListItem> gradovi = db.gradovi
                .OrderBy(a => a.nazivGrada)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivGrada,
                    Value = a.gradID.ToString()
                }).ToList();

            KlubUrediDodajVM.GarnitureDresova domaca = null;
            KlubUrediDodajVM.GarnitureDresova gostujuca = null;

            if (klubID != 0)
            {
                domaca = db.klubovi
                    .Where(s => s.klubID == klubID)
                    .Select(a => new KlubUrediDodajVM.GarnitureDresova
                    {
                        garnituraID = a.domacaID,
                        slikaTrenutna = a.domaca.lokacijaSlika
                    }).Single();

                gostujuca = db.klubovi
                    .Where(s => s.klubID == klubID)
                    .Select(a => new KlubUrediDodajVM.GarnitureDresova
                    {
                        garnituraID = a.gostujucaID,
                        slikaTrenutna = a.gostujuca.lokacijaSlika
                    }).Single();
            }


            KlubUrediDodajVM model = klubID == 0 ? new KlubUrediDodajVM()
            {
                domaci = new KlubUrediDodajVM.GarnitureDresova(),
                gostujuci = new KlubUrediDodajVM.GarnitureDresova()
            } :
            db.klubovi
            .Where(k => k.klubID == klubID)
            .Select(a => new KlubUrediDodajVM
            {
                klubID = a.klubID,
                nazivKluba = a.nazivKluba,
                grbCurrent = a.grbKluba,
                imeStadiona = a.stadion.imeStadiona,
                slikaStadionaCurrent = a.stadion.slikaStadiona,
                domaci = domaca,
                gostujuci = gostujuca
        }).Single();

            model.gradovi = gradovi;
            return PartialView("UrediDodaj", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult SnimiUrediDodaj(KlubUrediDodajVM model)
        {
            List<Klub> klubovi = db.klubovi.ToList();
            foreach (var k in klubovi)
            {
                if (k.nazivKluba == model.nazivKluba)
                {
                    TempData["porukaInfo"] = "Zapis već postoji!";
                    return Redirect("Prikaz");
                }
            }
            Klub klub;
            Stadion stadion;
            GarnituraDresova domaca;
            GarnituraDresova gostujuca;
            if (model.klubID == 0)
            {
                stadion = new Stadion();
                stadion.imeStadiona = model.imeStadiona;
                if (model.slikaStadionaNew != null)
                {
                    string ekstenzija = Path.GetExtension(model.slikaStadionaNew.FileName);
                    string contentType = model.slikaStadionaNew.ContentType;

                    var filename = $"{Guid.NewGuid()}{ekstenzija}";
                    model.slikaStadionaNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                    stadion.slikaStadiona = filename;
                }
                db.Add(stadion);
                db.SaveChanges();

                domaca = new GarnituraDresova();
                domaca.opis = GarnituraOpis.DOMAĆI;

                gostujuca = new GarnituraDresova();
                gostujuca.opis = GarnituraOpis.GOSTUJUĆI;

                db.Add(domaca);
                db.Add(gostujuca);

                db.SaveChanges();

                klub = new Klub();
                klub.nazivKluba = model.nazivKluba;
                klub.gradID = model.klubGradID;
                klub.stadionID = stadion.stadionID;
                klub.domacaID = domaca.garnituraDresovaID;
                klub.gostujucaID = gostujuca.garnituraDresovaID;

                if (model.grbNew != null)
                {
                    string ekstenzija = Path.GetExtension(model.grbNew.FileName);
                    string contentType = model.grbNew.ContentType;

                    var filename = $"{Guid.NewGuid()}{ekstenzija}";
                    model.grbNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                    klub.grbKluba = filename;
                }

                db.Add(klub);
                TempData["porukaInfo"] = "Uspješno ste dodali zapis!";
            }
            else
            {
                klub = db.klubovi.Find(model.klubID);
                domaca = db.garnitureDresova.Find(model.domaci.garnituraID);
                gostujuca = db.garnitureDresova.Find(model.gostujuci.garnituraID);
                TempData["porukaInfo"] = "Uspješno ste uredili zapis!";
            }
            if (model.domaci.garnituraSlika != null)
            {
                string ekstenzija = Path.GetExtension(model.domaci.garnituraSlika.FileName);
                string contentType = model.domaci.garnituraSlika.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.domaci.garnituraSlika.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                domaca.lokacijaSlika = filename;
            }
            if (model.gostujuci.garnituraSlika != null)
            {
                string ekstenzija = Path.GetExtension(model.gostujuci.garnituraSlika.FileName);
                string contentType = model.gostujuci.garnituraSlika.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.gostujuci.garnituraSlika.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                gostujuca.lokacijaSlika = filename;
            }
            db.SaveChanges();
            return Redirect("Prikaz");
        }
        [Autorizacija(false, false, true)]
        public IActionResult Obrisi(int klubID)
        {
            KlubObrisiVM model = new KlubObrisiVM();
            model.klubID = klubID;
            return PartialView("Obrisi", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult ObrisiSnimi(int klubID)
        {
            List<Klub> klubovi = db.klubovi.ToList();
            if (klubID == 0)
            {
                foreach (var klub in klubovi)
                {
                    db.Remove(klub);
                }
                db.SaveChanges();
                TempData["porukaInfo"] = "Uspješno ste obrisali sve zapise!";
                return Redirect("Prikaz");
            }
            Klub k = db.klubovi.Find(klubID);
            Stadion s = db.stadioni.Where(s => s.stadionID == k.stadionID).Single();
            GarnituraDresova d = db.garnitureDresova.Where(s => s.garnituraDresovaID == k.domacaID).Single();
            GarnituraDresova g = db.garnitureDresova.Where(s => s.garnituraDresovaID == k.gostujucaID).Single();


            db.Remove(k);
            db.SaveChanges();

            db.Remove(s);
            db.SaveChanges();

            db.Remove(d);
            db.SaveChanges();

            db.Remove(g);
            db.SaveChanges();


            //Brisanje utakmica

            TempData["porukaInfo"] = "Uspješno ste obrisali zapis!";
            return Redirect("Prikaz");
        }
    }
}