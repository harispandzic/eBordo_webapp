using Data.Data;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels.Poruka;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.Controllers
{
    public class PorukaController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;

        public PorukaController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }
        public string slanjePoruke(PorukaPosaljiVM model)
        {
            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);
            EmailController emailController = new EmailController(db, userManager, hubContext);
            SMSController smsController = new SMSController(db, userManager);
            Korisnik k = Autentifikacija.LogiraniKorisnik(HttpContext);
            string posiljaoc = k.UserName;

            string tipPoruke = "";
            if (model.vrstaID == TipNotifikacije.Greška)
            {
                tipPoruke = "error";
            }
            else if (model.vrstaID == TipNotifikacije.Uspješno)
            {
                tipPoruke = "success";
            }
            else if (model.vrstaID == TipNotifikacije.Upozorenje)
            {
                tipPoruke = "warning";
            }
            else if (model.vrstaID == TipNotifikacije.Obavještenje)
            {
                tipPoruke = "info";
            }

            if (model.korisnikID == "0")
            {
                List<Korisnik> korisnici = db.igraci.Include(s => s.korisnik).Include(s => s.korisnik.notifikacije).Select(s => s.korisnik).ToList();
                if (model.porukaID == TipPoruke.SMS)
                {
                    return "Nije moguće poslati SMS poruku svim igračima!";
                }
                else
                {
                    if (model.porukaID == TipPoruke.Notifikacija)
                    {
                        controller.posaljiPorukuOdredjenim(model.tekstPoruke, tipPoruke, korisnici, true, posiljaoc);
                        return "Notifikacija poslana svim igračima!";
                    }
                    else
                    {
                        emailController.posaljiEmail(korisnici, model.tekstPoruke, ((TipNotifikacije)model.vrstaID).ToString());
                        return "E-mail poruka poslana svim igračima!";
                    }
                }
            }
            else
            {
                Korisnik korisnik = db.korisnici.Where(s => s.Id == model.korisnikID).Single();
                if (model.porukaID == TipPoruke.SMS)
                {
                    smsController.posaljiSMSAsync(korisnik.Id, model.tekstPoruke);
                    return "SMS poruka uspješno poslana!";
                }
                else if (model.porukaID == TipPoruke.Notifikacija)
                {
                    controller.posaljiJednom(model.tekstPoruke, tipPoruke, korisnik, true, posiljaoc);
                    return "Notifikacija uspješno poslana!";
                }
                else if (model.porukaID == TipPoruke.Mail)
                {
                    emailController.posaljiEmailPojedinacno(korisnik, model.tekstPoruke, ((TipNotifikacije)model.vrstaID).ToString());
                    return "E-mail uspješno poslan!";
                }
                else
                {
                    return "OK";
                }
            }
        }
        public IActionResult posaljiPoruku()
        {
            Korisnik korisnik = Autentifikacija.LogiraniKorisnik(HttpContext);

            List<string> tipPorukeString = new List<string>();
            var tipPoruke = Enum.GetValues(typeof(TipPoruke)).Cast<TipPoruke>();
            


            List<string> vrstaPorukeString = new List<string>();
            var vrstaPoruke = Enum.GetValues(typeof(TipNotifikacije)).Cast<TipNotifikacije>();
            foreach (TipNotifikacije l in vrstaPoruke)
                vrstaPorukeString.Add(l.ToString());

            List<SelectListItem> vrstePoruke = vrstaPorukeString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<SelectListItem> korisnici;

            if (korisnik.isTrener)
            {
                korisnici = db.korisnici
                    .Where(s => (s.isIgrac || s.isAdmin))
                    .Select(a => new SelectListItem
                    {
                        Text = a.UserName,
                        Value = a.Id
                    }).OrderBy(a => a.Value).ToList();
                foreach (TipPoruke l in tipPoruke)
                {
                    tipPorukeString.Add(l.ToString());
                }
            }
            else if (korisnik.isAdmin)
            {
                korisnici = db.korisnici
                    .Where(s => (s.isIgrac || s.isTrener))
                    .Select(a => new SelectListItem
                    {
                        Text = a.UserName,
                        Value = a.Id
                    }).OrderBy(a => a.Value).ToList();
                foreach (TipPoruke l in tipPoruke)
                {
                    tipPorukeString.Add(l.ToString());
                }
            }
            else if (korisnik.isIgrac)
            {
                korisnici = db.korisnici
                    .Where(s => (s.isAdmin || s.isTrener))
                    .Select(a => new SelectListItem
                    {
                        Text = a.UserName,
                        Value = a.Id
                    }).OrderBy(a => a.Value).ToList();
                foreach (TipPoruke l in tipPoruke)
                {
                    if(l != TipPoruke.Mail)
                        tipPorukeString.Add(l.ToString());
                }
            }
            else
            {
                korisnici = db.korisnici
                    .Select(a => new SelectListItem
                    {
                        Text = a.UserName,
                        Value = a.Id
                    }).OrderBy(a => a.Value).ToList();
                    foreach (TipPoruke l in tipPoruke)
                    {
                        tipPorukeString.Add(l.ToString());
                    }
            }
            List<SelectListItem> tipovi = tipPorukeString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();
            PorukaPosaljiVM model = new PorukaPosaljiVM();
            model.korisnici = korisnici;
            model.tipoviPoruka = tipovi;
            model.vrstaPoruke = vrstePoruke;

            return PartialView(model);
        }
    }
}
