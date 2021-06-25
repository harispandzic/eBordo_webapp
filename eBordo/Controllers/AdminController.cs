using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;

        public AdminController(ApplicationDbContext database, UserManager<Korisnik> _userManager)
        {
            db = database;
            userManager = _userManager;
        }
        [Autorizacija(false, false, true)]
        public IActionResult Index()
        {
            return View();
        }
        public int brojIgraci()
        {
            return db.igraci.Where(s => s.status == Status.Aktivan).Count();
        }
        public int brojTreneri()
        {
            return db.treneri.Where(s => s.status == Status.Aktivan).Count();
        }
        public int brojPobjeda()
        {
            return db.izvjestaji.Where(s => s.rezultat == Rezultat.Pobjeda).Count();
        }
        public int brojNerjsenih()
        {
            return db.izvjestaji.Where(s => s.rezultat == Rezultat.Nerješeno).Count();
        }
        public int brojPoraza()
        {
            return db.izvjestaji.Where(s => s.rezultat == Rezultat.Poraz).Count();
        }
    }
}
