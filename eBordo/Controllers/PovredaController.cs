using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eBordo.Data;
using eBordo.ViewModels.Povreda;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cors;

namespace eBordo.Controllers
{
    public class PovredaController : Controller
    {
        private ApplicationDbContext db;
        private readonly IHostingEnvironment env;
        private UserManager<Korisnik> userManager;
        private readonly SignInManager<Korisnik> signInManager;

        public PovredaController(ApplicationDbContext database, IHostingEnvironment env, UserManager<Korisnik> _userManager, SignInManager<Korisnik> signInManager)
        {
            db = database;
            this.env = env;
            userManager = _userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Prikaz(string q, string pozicija, int currentPage = 1, int itemsPerPage = 5)
        {
            IQueryable<Povreda> povrede;

            if(pozicija != null)
            {
                Pozicija pozicijaE = (Pozicija)Enum.Parse(typeof(Pozicija), pozicija);
                povrede = db.povrede.Where(s => s.igrac.pozicija == pozicijaE);
            }
            else
            {
                povrede = db.povrede.Where(s => q == null || (s.igrac.korisnik.ime + " " + s.igrac.korisnik.prezime).StartsWith(q) ||
                            (s.igrac.korisnik.ime + " " + s.igrac.korisnik.prezime).StartsWith(q));
            }

            PovredaPrikazVM model = new PovredaPrikazVM
            {
                povrede = povrede
                .Skip((currentPage -1)*itemsPerPage)
                .Take(itemsPerPage)
                .Select(s => new PovredaPrikazVM.Row
                {
                    povredaId = s.povredaID,
                    igrac = s.igrac.korisnik.imePrezime,
                    datumPovrede = s.datumPovrede.ToString("dd.MM.yyyy"),
                    odsustvo = s.odsustvoDO.ToString("dd.MM.yyyy"),
                    brojDana = (s.odsustvoDO.Date - DateTime.Now.Date).Days,
                    tipPovrede = s.tipPovrede.ToString(),
                    kratkiOpis = s.kratkiOpis,
                    pozicija = s.igrac.pozicija.ToString(),
                    slika = s.igrac.slika,
                    oporavljen = s.odsustvoDO.Date < DateTime.Now.Date
                }).ToList(),
                total = povrede.Count()
            };
            return Ok(model);
        }
        public IActionResult Detalji(int ID)
        {
            Povreda povreda = db.povrede.Where(s => s.povredaID == ID)
                .Include(s => s.igrac)
                .Include(s => s.igrac.korisnik)
                .Single();

            PovredaDetaljiVM model = new PovredaDetaljiVM
            {
                povredaID = povreda.povredaID,
                igrac = povreda.igrac.korisnik.imePrezime,
                datumPovrede = povreda.datumPovrede.ToString("dd.MM.yyyy"),
                odsustvoDO = povreda.odsustvoDO.ToString("dd.MM.yyyy"),
                tipPovrede = povreda.tipPovrede.ToString(),
                kratkiOpis = povreda.kratkiOpis,
                misljenjeLjekara = povreda.misljenjeLjekara
            };

            return Ok(model);
        }
        public IActionResult Obrisi(int ID)
        {
            Povreda povreda = db.povrede.Find(ID);
            db.Remove(povreda);
            db.SaveChanges();

            return Ok();
        }

        public IActionResult Uredi(int ID)
        {
            Povreda povreda = db.povrede.Where(s => s.povredaID == ID)
                .Include(s => s.igrac)
                .Include(s => s.igrac.korisnik)
                .Single();

            PovredaUrediVM model = new PovredaUrediVM
            {
                povredaID = ID,
                datumPovrede = povreda.datumPovrede,
                odsustvo = povreda.odsustvoDO,
                tipPovrede = povreda.tipPovrede.ToString(),
                kratkiOpis = povreda.kratkiOpis,
                misljenjeLjekara = povreda.misljenjeLjekara,
                odabraniIgrac = povreda.igrac.korisnik.imePrezime
            };

            return Ok(model);
        }
        [HttpPost]
        public IActionResult SnimiUredi([FromBody]PovredaUrediVM model)
        {
            Povreda povreda = db.povrede.Where(s => s.povredaID == model.povredaID)
                .Include(s => s.igrac)
                .Include(s => s.igrac.korisnik)
                .Single();

            povreda.kratkiOpis = model.kratkiOpis;
            povreda.datumPovrede = model.datumPovrede;
            povreda.odsustvoDO = model.odsustvo;
            db.SaveChanges();

            return Ok();
        }
        public IActionResult Dodaj()
        {
            List<string> tipoviString = new List<string>();
            var tipovi = Enum.GetValues(typeof(Status)).Cast<Status>();
            foreach (TipPovrede l in tipovi)
                tipoviString.Add(l.ToString());

            List<SelectListItem> tipoviPovrede = tipoviString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();
            PovredaDodajVM model = new PovredaDodajVM
            {
                datumPovrede = DateTime.Now,
                odsustvo = DateTime.Now,
                tipovi = tipoviPovrede,
                tipPovrede = "Lakša",
                odabraniIgrac = "1",
                igraci = db.igraci.Select(s => new SelectListItem
                {
                    Value = s.igracID.ToString(),
                    Text = s.korisnik.imePrezime
                }).ToList()
            };
            return Ok(model);
        }
        public IActionResult SnimiDodaj([FromBody] PovredaDodajVM model)
        {
            TipPovrede povredaTip = (TipPovrede)Enum.Parse(typeof(TipPovrede), model.tipPovrede);

            int igracID = Int32.Parse(model.odabraniIgrac);


            Povreda povreda = new Povreda
            {
                igracID = igracID,
                datumPovrede = model.datumPovrede,
                odsustvoDO = model.odsustvo,
                kratkiOpis = model.kratkiOpis,
                misljenjeLjekara = model.misljenjeLjekara,
                tipPovrede = povredaTip
            };
            db.Add(povreda);
            db.SaveChanges();
            return Ok();
        }
    }
}
