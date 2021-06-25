using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.Controllers
{
    public class DogađajController : Controller
    {
        private ApplicationDbContext db;

        public DogađajController(ApplicationDbContext database)
        {
            db = database;
        }
        public IActionResult Kalendar()
        {
            return View();
        }
        public JsonResult GetEvents()
        {
            List<Događaji> dogadjaji = new List<Događaji>();

            List<Utakmica> utakmice = db.utakmice.Where(s => s.statusUtakmice == StatusUtakmice.poRasporedu)
                .Include(s => s.domacin)
                .Include(s => s.gost)
                .Include(s => s.vrstaTakmicenja)
                .ToList();

            List<Trening> treninzi = db.treninzi.Where(s => s.statusTreninga == StatusUtakmice.poRasporedu).ToList();

            for (int i = 0; i < utakmice.Count(); i++)
            {
                Događaji događaj = new Događaji();
                događaj.Subject = utakmice[i].satnica + " h: " + utakmice[i].domacin.nazivKluba + " - " + utakmice[i].gost.nazivKluba + " (" + utakmice[i].vrstaTakmicenja.nazivVrsteTakmicenja + ")";
                događaj.Start = utakmice[i].datumOdigravanja;
                događaj.ThemeColor = "green";
                dogadjaji.Add(događaj);
            }
            for (int i = 0; i < treninzi.Count(); i++)
            {
                Događaji događaj = new Događaji();
                događaj.Subject = treninzi[i].satnica + " h: " + treninzi[i].lokacija.ToString();
                događaj.Start = treninzi[i].datumOdrzavanja;
                događaj.ThemeColor = "yellow";
                dogadjaji.Add(događaj);
            }

            return new JsonResult(dogadjaji);
        }
    }
}
