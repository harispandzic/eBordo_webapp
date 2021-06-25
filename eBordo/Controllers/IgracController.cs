using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Data.EntityModels;
using eBordo.Areas.Identity.Pages.Account.Manage;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels.Igrac;
using eBordo.ViewModels.Igrac.Index;
using eBordo_seminarski_rad.Controllers;
using eBordo_seminarski_rad.ViewModels.Drzava;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using RazorPDF;

namespace eBordo.Controllers
{
    public class IgracController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;

        public IgracController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }
        public JsonResult pregledOcjena(int igracID)
        {
            SkillsStavke stavke = db.skillsStavke.Where(s => s.igracID == igracID).Single();
            List<PregledOcjena> model = new List<PregledOcjena>();
            model.Add(new PregledOcjena { opisOcjene = "Kontrola lopte", ocjena = stavke.kontrolaLopte });
            model.Add(new PregledOcjena { opisOcjene = "Driblanje", ocjena = stavke.dribljanje });
            model.Add(new PregledOcjena { opisOcjene = "Dodavanje", ocjena = stavke.dodavanje });
            model.Add(new PregledOcjena { opisOcjene = "Kretanje", ocjena = stavke.kretanje });
            model.Add(new PregledOcjena { opisOcjene = "Brzina", ocjena = stavke.brzina });
            model.Add(new PregledOcjena { opisOcjene = "Šut", ocjena = stavke.sut });
            model.Add(new PregledOcjena { opisOcjene = "Snaga", ocjena = stavke.snaga });
            model.Add(new PregledOcjena { opisOcjene = "Markiranje", ocjena = stavke.markiranje });
            model.Add(new PregledOcjena { opisOcjene = "Klizeći start", ocjena = stavke.klizeciStart });
            model.Add(new PregledOcjena { opisOcjene = "Skok", ocjena = stavke.skok });
            model.Add(new PregledOcjena { opisOcjene = "Odbrana", ocjena = stavke.odbrana });

            for (int i = 0; i < model.Count(); i++)
            {
                if(model[i].ocjena <= 2)
                {
                    model[i].status = "Popraviti";
                }
                else if(model[i].ocjena > 2 && model[i].ocjena < 8)
                {
                    model[i].status = "Prosjek";
                }
                else if (model[i].ocjena >= 8 && model[i].ocjena <= 10)
                {
                    model[i].status = "Zadovoljava";
                }
            }

            return Json(model);

        }

        public JsonResult NewChart(int igracID)
        {
            SkillsStavke stavke = db.skillsStavke.Where(s => s.igracID == igracID).Single();

            List<object> iData = new List<object>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Skill", System.Type.GetType("System.String"));
            dt.Columns.Add("Ocjena", System.Type.GetType("System.Single"));

            DataRow dr = dt.NewRow();
            dr["Skill"] = "Kontrola lopte";
            dr["Ocjena"] = stavke.kontrolaLopte;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Driblanje";
            dr["Ocjena"] = stavke.dribljanje;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Dodavanje";
            dr["Ocjena"] = stavke.dodavanje;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Kretanje";
            dr["Ocjena"] = stavke.kretanje;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Brzina";
            dr["Ocjena"] = stavke.brzina;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Šut";
            dr["Ocjena"] = stavke.sut;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Snaga";
            dr["Ocjena"] = stavke.snaga;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Markiranje";
            dr["Ocjena"] = stavke.markiranje;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Klizeći start";
            dr["Ocjena"] = stavke.klizeciStart;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Skok";
            dr["Ocjena"] = stavke.skok;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Odbrana";
            dr["Ocjena"] = stavke.odbrana;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Skill"] = "Prosjek";
            dr["Ocjena"] = stavke.prosjekOcjena;
            dt.Rows.Add(dr);


            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            return Json(iData);
        }
        [Autorizacija(false, true, false)]
        public IActionResult Index()
        {
            return View();
        }
        [Autorizacija(true, false, true)]
        public IActionResult Prikaz(string q, string nazivDrzave, string nazivPozicije, bool sortOcjena, bool sortVrijednost, string statusIgraca)
        {
            List<IgracPrikaziVM.Row> igraci;

            List<SelectListItem> drzave = db.drzave
                .OrderBy(a => a.nazivDrzave)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivDrzave,
                    Value = a.drzavaID.ToString()
                }).ToList();

            List<string> pozicijeString = new List<string>();
            var poz = Enum.GetValues(typeof(Pozicija)).Cast<Pozicija>();
            foreach (Pozicija l in poz)
                pozicijeString.Add(l.ToString());

            List<SelectListItem> pozicije = pozicijeString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<string> statusiString = new List<string>();
            var statusi = Enum.GetValues(typeof(Status)).Cast<Status>();
            foreach (Status l in statusi)
                statusiString.Add(l.ToString());

            List<SelectListItem> statusiList = statusiString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();


            if (nazivDrzave != null)
            {
                igraci = db.igraci
                .Where(a => (a.korisnik.drzavljanstvo.nazivDrzave.StartsWith(nazivDrzave)) && a.status == Status.Aktivan)
                .Select(x => new IgracPrikaziVM.Row
                {
                    igracID = x.igracID,
                    ime = x.korisnik.ime,
                    prezime = x.korisnik.prezime,
                    slikaIgraca = x.slika,
                    datumRodjenja = x.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
                    drzavljanstvo = x.korisnik.drzavljanstvo.nazivDrzave,
                    drzavaSlika = x.korisnik.drzavljanstvo.zastavaDrzave,
                    gradRodjenja = x.korisnik.gradRodjenja.nazivGrada,
                    gradRodjenjaZastava = x.korisnik.gradRodjenja.drzava.zastavaDrzave,
                    pozicija = x.pozicija.ToString(),
                    brojDresa = x.brojDresa,
                    trzisnaVrijednost = x.trzisnaVrijednost,
                    statusIgraca = x.status
                }).ToList();
            }
            else if (nazivPozicije != null)
            {
                Pozicija pozicija = (Pozicija)Enum.Parse(typeof(Pozicija), nazivPozicije);

                igraci = db.igraci
                .Where(a => (a.pozicija == pozicija) && a.status == Status.Aktivan)
                .Select(x => new IgracPrikaziVM.Row
                {
                    igracID = x.igracID,
                    ime = x.korisnik.ime,
                    prezime = x.korisnik.prezime,
                    slikaIgraca = x.slika,
                    datumRodjenja = x.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
                    drzavljanstvo = x.korisnik.drzavljanstvo.nazivDrzave,
                    drzavaSlika = x.korisnik.drzavljanstvo.zastavaDrzave,
                    gradRodjenja = x.korisnik.gradRodjenja.nazivGrada,
                    gradRodjenjaZastava = x.korisnik.gradRodjenja.drzava.zastavaDrzave,
                    pozicija = x.pozicija.ToString(),
                    brojDresa = x.brojDresa,
                    trzisnaVrijednost = x.trzisnaVrijednost,
                    statusIgraca = x.status
                }).ToList();
            }
            else if (statusIgraca != null)
            {
                Status statusIgrac = (Status)Enum.Parse(typeof(Status), statusIgraca);

                igraci = db.igraci
                .Where(a => (a.status == statusIgrac))
                .Select(x => new IgracPrikaziVM.Row
                {
                    igracID = x.igracID,
                    ime = x.korisnik.ime,
                    prezime = x.korisnik.prezime,
                    slikaIgraca = x.slika,
                    datumRodjenja = x.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
                    drzavljanstvo = x.korisnik.drzavljanstvo.nazivDrzave,
                    drzavaSlika = x.korisnik.drzavljanstvo.zastavaDrzave,
                    gradRodjenja = x.korisnik.gradRodjenja.nazivGrada,
                    gradRodjenjaZastava = x.korisnik.gradRodjenja.drzava.zastavaDrzave,
                    pozicija = x.pozicija.ToString(),
                    brojDresa = x.brojDresa,
                    trzisnaVrijednost = x.trzisnaVrijednost,
                    statusIgraca = x.status
                }).ToList();
            }
            else
            {
             igraci = db.igraci
                .Where(a => (q == "" || q == null || a.korisnik.ime.StartsWith(q) || a.korisnik.prezime.StartsWith(q)) && a.status == Status.Aktivan)
                .Select(x => new IgracPrikaziVM.Row
                {
                    igracID = x.igracID,
                    ime = x.korisnik.ime,
                    prezime = x.korisnik.prezime,
                    slikaIgraca = x.slika,
                    datumRodjenja = x.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
                    drzavljanstvo = x.korisnik.drzavljanstvo.nazivDrzave,
                    drzavaSlika = x.korisnik.drzavljanstvo.zastavaDrzave,
                    gradRodjenja = x.korisnik.gradRodjenja.nazivGrada,
                    gradRodjenjaZastava = x.korisnik.gradRodjenja.drzava.zastavaDrzave,
                    pozicija = x.pozicija.ToString(),
                    brojDresa = x.brojDresa,
                    trzisnaVrijednost = x.trzisnaVrijednost,
                    statusIgraca = x.status
                }).OrderBy(s=> s.brojDresa).ToList();
            }
            IgracPrikaziVM model = new IgracPrikaziVM();
            model.igraci = igraci;
            model.drzave = drzave;
            model.pozicije = pozicije;
            model.statusi = statusiList;
            model.q = q;
            model.statusIgraca = statusIgraca;

            if (model.igraci.Count() == 0 && q != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.igraci.Count() == 0 && nazivPozicije != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.igraci.Count() == 0 && nazivDrzave != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.igraci.Count() == 0 && statusIgraca != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            return View("Prikaz",model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult UrediIgraca(int igracID)
        {
             List<SelectListItem> drzave = db.drzave
            .OrderBy(a => a.nazivDrzave)
            .Select(a => new SelectListItem
            {
                Text = a.nazivDrzave,
                Value = a.drzavaID.ToString()
            }).ToList();

            List<SelectListItem> gradovi = db.gradovi
                .OrderBy(a => a.nazivGrada)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivGrada,
                    Value = a.gradID.ToString()
                }).ToList();

            List<string> pozicijeString = new List<string>();
            var poz = Enum.GetValues(typeof(Pozicija)).Cast<Pozicija>();
            foreach (Pozicija l in poz)
                pozicijeString.Add(l.ToString());

            List<SelectListItem> pozicije = pozicijeString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<string> boljaNogaString = new List<string>();
            var noge = Enum.GetValues(typeof(BoljaNoga)).Cast<BoljaNoga>();
            foreach (BoljaNoga l in noge)
                boljaNogaString.Add(l.ToString());

            List<SelectListItem> boljeNoge = boljaNogaString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            IgracUrediDodajVM model = new IgracUrediDodajVM();
            model = db.igraci.Where(i => i.igracID == igracID)
                .Select(a => new IgracUrediDodajVM
                {
                    igracID = a.igracID,
                    ime = a.korisnik.ime,
                    prezime = a.korisnik.prezime,
                    datumRodjenja = a.korisnik.datumRodjenja,
                    adresaPrebivalista = a.korisnik.adresaPrebivalista,
                    telefon = a.korisnik.telefon,
                    emailAdresa = a.korisnik.emailAdresa,
                    drzavljanstvoID = a.korisnik.drzavljanstvoID,
                    reprezentativac = a.reprezentativac,
                    gradRodjenjaID = a.korisnik.gradRodjenjaID,
                    visina = a.visina,
                    tezina = a.tezina,
                    brojDresa = a.brojDresa,
                    trzisnaVrijednost = a.trzisnaVrijednost,
                    datumPristupaKlubu = a.datumPristupaKlubu,
                    dosadasnjiKlubovi =a.dosadasnjiKlubovi,
                    nazivPozicije = a.pozicija.ToString(),
                    boljaNoga = a.boljaNoga.ToString(),
                    transferMarktAcc = a.trensferMarktAcc,
                    slikaCurrent =a.slika
                }).Single();

            model.drzave = drzave;
            model.datumPristupaKlubu = DateTime.Now;
            model.datumRodjenja = DateTime.Now;
            model.pozicije = pozicije;
            model.noga = boljeNoge;
            model.gradovi = gradovi;

            return PartialView("UrediIgraca", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult DodajIgraca()
        {
            List<SelectListItem> drzave = db.drzave
                .OrderBy(a => a.nazivDrzave)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivDrzave,
                    Value = a.drzavaID.ToString()
                }).ToList();

            List<SelectListItem> gradovi = db.gradovi
                .OrderBy(a => a.nazivGrada)
                .Select(a => new SelectListItem
                {
                    Text = a.nazivGrada,
                    Value = a.gradID.ToString()
                }).ToList();

            List<string> pozicijeString = new List<string>();
            var poz = Enum.GetValues(typeof(Pozicija)).Cast<Pozicija>();
            foreach (Pozicija l in poz)
                pozicijeString.Add(l.ToString());

            List<SelectListItem> pozicije = pozicijeString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<string> boljaNogaString = new List<string>();
            var noge = Enum.GetValues(typeof(BoljaNoga)).Cast<BoljaNoga>();
            foreach (BoljaNoga l in noge)
                boljaNogaString.Add(l.ToString());

            List<SelectListItem> boljeNoge = boljaNogaString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            IgracUrediDodajVM model = new IgracUrediDodajVM();

            model.drzave = drzave;
            model.datumPristupaKlubu = DateTime.Now;
            model.datumRodjenja = DateTime.Now;
            model.pozicije = pozicije;
            model.noga = boljeNoge;
            model.gradovi = gradovi;

            return PartialView("DodajIgraca", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult SnimiUredi(IgracUrediDodajVM model)
        {
            Pozicija pozicija = (Pozicija)Enum.Parse(typeof(Pozicija), model.nazivPozicije);

            Igrac igrac = db.igraci.Find(model.igracID);
            Korisnik user = userManager.FindByIdAsync(igrac.korisnikID).Result;

            user.emailAdresa = model.emailAdresa;
            user.drzavljanstvoID = model.drzavljanstvoID;
            user.adresaPrebivalista = model.adresaPrebivalista;
            user.telefon = model.telefon;
            igrac.pozicija = pozicija;
            igrac.dosadasnjiKlubovi = model.dosadasnjiKlubovi;
            igrac.visina = model.visina;
            igrac.tezina = model.tezina;
            igrac.brojDresa =model.brojDresa;
            igrac.trzisnaVrijednost = model.trzisnaVrijednost;
            igrac.reprezentativac = model.reprezentativac;
            igrac.trensferMarktAcc = model.transferMarktAcc;
            if (model.slikaNew != null)
            {
                string ekstenzija = Path.GetExtension(model.slikaNew.FileName);
                string contentType = model.slikaNew.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.slikaNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                igrac.slika = filename;
            }

            db.SaveChanges();
            TempData["porukaInfo"] = "Uspješno ste uredili zapis!";

            EmailController em = new EmailController(db, userManager, hubContext);

            int dan = user.datumRodjenja.Day;
            int mjesec = user.datumRodjenja.Month;

            RecurringJob.AddOrUpdate(() => posaljiRodjendan(user.ime,user.emailAdresa), Cron.Yearly(mjesec,dan,8,0));
            return Redirect("Prikaz");
        }
        public void posaljiRodjendan(string ime, string emailAdresa)
        {
            MailMessage mm = new MailMessage("eBordoApp@outlook.com", emailAdresa);
            mm.Subject = "Sretan rođendan";
            mm.Body = "Dragi naš " + ime    + ", kolektiv FK Sarajevo ti želi sretan rođendan!";
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.outlook.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("eBordoApp@outlook.com", "7wzS*S9coLUv^sS");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
        }
        [Autorizacija(false, false, true)]
        public IActionResult SnimiDodaj(IgracUrediDodajVM model)
        {
            List<Korisnik> korisnici = userManager.Users.ToList();
            foreach(var k in korisnici)
            {
                if(k.ime == model.ime && k.prezime == model.prezime)
                {
                    TempData["porukaInfo"] = "Zapis već postoji!";
                    return Redirect("Prikaz");
                }
            }

            string inputMail = model.ime.ToLower() + "." + model.prezime.ToLower() + "@fksarajevo.ba";

            string decomposed = inputMail.Normalize(NormalizationForm.FormD);
            char[] filtered = decomposed
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();

            string email = new String(filtered);

            email = email.Replace("đ", "dj");

            Korisnik korisnik = new Korisnik
            {
                Email = email,
                UserName = email,
                EmailConfirmed = false,
                ime = model.ime,
                prezime = model.prezime,
                emailAdresa = model.emailAdresa,
                datumRodjenja = model.datumRodjenja,
                adresaPrebivalista = model.adresaPrebivalista,
                telefon = model.telefon,
                drzavljanstvoID = model.drzavljanstvoID,
                gradRodjenjaID = model.gradRodjenjaID,
                notifikacije = new List<Notifikacija>(),
                isIgrac = true,
                isTrener = false,
            };

            AccountController ac = new AccountController(db,userManager);

            string password = ac.getRandomPassword();

            IdentityResult result = userManager.CreateAsync(korisnik, password).Result;

            string filePath = System.IO.Path.GetFullPath("wwwroot/ConfirmEmailTemplate.html");

            string confirmationToken = userManager.
                 GenerateEmailConfirmationTokenAsync(korisnik).Result;

            string confirmationLink = Url.Action("ConfirmEmail",
              "Igrac", new
              {
                  userid = korisnik.Id,
                  token = confirmationToken,
                  pass = password
              },
               protocol: HttpContext.Request.Scheme);

            try
            {
                MailMessage mm = new MailMessage("eBordoApp@outlook.com", korisnik.emailAdresa );
                mm.Subject = "Potvrda e-mail adrese";
                mm.Body = string.Empty;
                using (var reader = new System.IO.StreamReader(filePath))
                {
                    mm.Body = reader.ReadToEnd();
                }
                mm.Body = mm.Body.Replace("{imePrezime}", model.ime + " " + model.prezime);
                mm.Body = mm.Body.Replace("{link}", confirmationLink);
                mm.BodyEncoding = System.Text.Encoding.UTF8;
                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                NetworkCredential nc = new NetworkCredential("eBordoApp@outlook.com", "7wzS*S9coLUv^sS");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mm);
                TempData["mailInfo"] = "Poslan konfirmacijski e-mail!";
            }
            catch(SmtpException ex)
            {
                if (ex.StatusCode != SmtpStatusCode.MailboxUnavailable)
                {
                    TempData["porukaInfo"] = "Mail adresa ne postoji!";
                    return Redirect("Prikaz");
                }
            }

            IgracStatistika statistika = new IgracStatistika();
            db.igracStatistika.Add(statistika);
            db.SaveChanges();

            SkillsStavke skillsStavke = new SkillsStavke();
            db.skillsStavke.Add(skillsStavke);
            db.SaveChanges();

            IgracSkills skills = new IgracSkills();
            skills.skillsStavke = skillsStavke;
            db.igracSkills.Add(skills);
            db.SaveChanges();

            Ugovor ugovor = new Ugovor();
            db.ugovori.Add(ugovor);
            db.SaveChanges();

            Pozicija pozicija = (Pozicija)Enum.Parse(typeof(Pozicija), model.nazivPozicije);
            BoljaNoga boljaNoga = (BoljaNoga)Enum.Parse(typeof(BoljaNoga), model.boljaNoga);

            Igrac igrac = new Igrac
            {
                visina = model.visina,
                tezina = model.tezina,
                brojDresa = model.brojDresa,
                trzisnaVrijednost = model.trzisnaVrijednost,
                reprezentativac = model.reprezentativac,
                datumPristupaKlubu = model.datumPristupaKlubu,
                dosadasnjiKlubovi = model.dosadasnjiKlubovi,
                godine = DateTime.Now.Year - korisnik.datumRodjenja.Year,
                trensferMarktAcc = model.transferMarktAcc,
                pozicija = pozicija,
                boljaNoga = boljaNoga,
                status = Status.Aktivan,
                igracStatistika = statistika,
                igracSkills = skills,
                ugovorID = ugovor.ugovorID,
                korisnik = korisnik
            };
            if (model.slikaNew != null)
            {
                string ekstenzija = Path.GetExtension(model.slikaNew.FileName);
                string contentType = model.slikaNew.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.slikaNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                igrac.slika = filename;
            }

            db.igraci.Add(igrac);
            db.SaveChanges();

            statistika.igracID = igrac.igracID;
            db.SaveChanges();

            skillsStavke.igracID = igrac.igracID;
            db.SaveChanges();

            skills.igracID = igrac.igracID;
            db.SaveChanges();

            ugovor.igracID = igrac.igracID;
            db.SaveChanges();

            korisnik.igracID = igrac.igracID;
            db.SaveChanges();

            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);
            string poruka = korisnik.imePrezime + " dobrodošli na eBordo platformu!";
            string posiljaoc = "admin@fksarajevo.ba";
            controller.posaljiJednom(poruka, "success", korisnik, false, posiljaoc);

            TempData["porukaInfo"] = "Uspješno ste dodali zapis!";

            int dan = korisnik.datumRodjenja.Day;
            int mjesec = korisnik.datumRodjenja.Month;

            RecurringJob.AddOrUpdate(() => posaljiRodjendan(korisnik.ime, korisnik.emailAdresa), Cron.Yearly(mjesec, dan, 8, 0));

            return Redirect("Prikaz");
        }
        [Autorizacija(false, false, true)]
        public IActionResult Obrisi(int igracID)
        {
            IgracObrisiVM model = new IgracObrisiVM();
            model.igracID = igracID;
            return PartialView("IgracObrisi", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult ObrisiSnimi(int igracID)
        {
            //List<Igrac> igraci = db.igraci.ToList();
            //List<IgracStatistika> igraciStatistika = db.igracStatistika.ToList();
            //List<SkillsStavke> skillsStavke = db.skillsStavke.ToList();
            //List<IgracSkills> igraciSkills = db.igracSkills.ToList();
            //List<Ugovor> ugovori = db.ugovori.ToList();
            //List<Korisnik> korisnici = db.korisnici.ToList();

            //if (igracID == 0)
            //{
            //    foreach (var k in korisnici)
            //    {
            //        db.Remove(k);
            //    }
            //    db.SaveChanges();
            //    foreach (var statistika in igraciStatistika)
            //    {
            //        db.Remove(statistika);
            //    }
            //    db.SaveChanges();
            //    foreach (var skillsStavka in skillsStavke)
            //    {
            //        db.Remove(skillsStavka);
            //    }
            //    db.SaveChanges();
            //    foreach (var skills in igraciSkills)
            //    {
            //        db.Remove(skills);
            //    }
            //    db.SaveChanges();
            //    foreach (var ugovorIgrac in ugovori)
            //    {
            //        db.Remove(ugovorIgrac);
            //    }
            //    db.SaveChanges();
            //    TempData["porukaInfo"] = "Uspješno ste obrisali sve zapise!";
            //    return Redirect("Prikaz");
            //}
            //Igrac igrac = db.igraci.Find(igracID);

            //IgracStatistika igracStatistika = db.igracStatistika.Where(s => s.igracStatistikaID == igrac.igracStatistikaID).FirstOrDefault();
            //db.igracStatistika.Remove(igracStatistika);
            //db.SaveChanges();

            //SkillsStavke skillsStavkaIgrac = db.skillsStavke.Where(s => s.skillsStavkeID == igrac.igracSkills.skillsStavkeID).FirstOrDefault();
            //db.skillsStavke.Remove(skillsStavkaIgrac);
            //db.SaveChanges();

            //IgracSkills igracSkills = db.igracSkills.Where(s => s.igracSkillsID == igrac.igracSkillsID).FirstOrDefault();
            //db.igracSkills.Remove(igracSkills);
            //db.SaveChanges();

            //Ugovor ugovor = db.ugovori.Where(s => s.ugovorID == igrac.ugovorID).FirstOrDefault();
            //db.ugovori.Remove(ugovor);
            //db.SaveChanges();

            //Korisnik korisnik = db.korisnici.Find(igrac.korisnikID);
            //db.korisnici.Remove(korisnik);
            //db.SaveChanges();

            Igrac i = db.igraci.Find(igracID);
            i.status = Status.Neaktivan;

            db.SaveChanges();

            TempData["porukaInfo"] = "Uspješno ste arhivirali zapis!";
            return Redirect("Prikaz?statusIgraca=Neaktivan");
        }
        [Autorizacija(false, false, true)]
        public IActionResult VratiIgraca(int igracID)
        {
            Igrac i = db.igraci.Find(igracID);
            i.status = Status.Aktivan;

            db.SaveChanges();

            TempData["porukaInfo"] = "Uspješno ste uredili status igrača!";
            return Redirect("Prikaz?statusIgraca=Aktivan");
        }

        public IActionResult prikaziDetalje(int igracID)
        {
            int brojUtakmica = db.utakmice.Where(s => s.statusUtakmice == StatusUtakmice.odigrana).Count();
            int brojMinuta = 90 * brojUtakmica;
            IgracDetaljiVM model = db.igraci
                .Where(s => s.igracID == igracID)
                .Select(x => new IgracDetaljiVM{
                    igracID = x.igracID,
                    ime = x.korisnik.ime,
                    brojMinuta = brojMinuta,
                    brojUtakmica = brojUtakmica,
                    prezime = x.korisnik.prezime,
                    datumRodjenja = x.korisnik.datumRodjenja,
                    godine = DateTime.Now.Year - x.korisnik.datumRodjenja.Year,
                    adresaPrebivalista = x.korisnik.adresaPrebivalista,
                    telefon = x.korisnik.telefon,
                    emailAdresa = x.korisnik.emailAdresa,
                    drzavljanstvo = x.korisnik.drzavljanstvo.nazivDrzave,
                    zastava = x.korisnik.drzavljanstvo.zastavaDrzave,
                    gradRodjenja = x.korisnik.gradRodjenja.nazivGrada,
                    zastavaZaGrad = x.korisnik.gradRodjenja.drzava.zastavaDrzave,
                    username = x.korisnik.UserName,
                    visina = x.visina,
                    tezina = x.tezina,
                    brojDresa = x.brojDresa,
                    statusIgraca = x.status,
                    trzisnaVrijednost = x.trzisnaVrijednost,
                    reprezentativac = x.reprezentativac,
                    slika = x.slika,
                    datumPristupaKlubu = x.datumPristupaKlubu,
                    dosadasnjiKlubovi = x.dosadasnjiKlubovi,
                    pozicija = x.pozicija.ToString(),
                    boljaNoga = x.boljaNoga.ToString(),
                    brojNastupa = x.igracStatistika.brojNastupa,
                    odigraneMinute = x.igracStatistika.odigraneMinute,
                    golovi = x.igracStatistika.golovi,
                    asistencije = x.igracStatistika.asistencije,
                    zutiKartoni = x.igracStatistika.zutiKartoni,
                    crveniKartoni = x.igracStatistika.crveniKartoni,
                    prosjecnaOcjena = Math.Round(x.igracStatistika.prosjecnaOcjena, 1),
                    kontrolaLopte = Math.Round(x.igracSkills.skillsStavke.kontrolaLopte, 1),
                    dribljanje = Math.Round(x.igracSkills.skillsStavke.dribljanje, 1),
                    dodavanje = Math.Round(x.igracSkills.skillsStavke.dodavanje, 1),
                    kretanje = Math.Round(x.igracSkills.skillsStavke.kretanje, 1),
                    brzina = Math.Round(x.igracSkills.skillsStavke.brzina, 1),
                    sut = Math.Round(x.igracSkills.skillsStavke.sut, 1),
                    snaga = Math.Round(x.igracSkills.skillsStavke.snaga, 1),
                    markiranje = Math.Round(x.igracSkills.skillsStavke.markiranje, 1),
                    klizeciStart = Math.Round(x.igracSkills.skillsStavke.klizeciStart, 1),
                    prosjecnaOcjenaStavke = Math.Round(x.igracSkills.skillsStavke.prosjekOcjena, 1),
                    skok = Math.Round(x.igracSkills.skillsStavke.skok, 1),
                    odbrana = Math.Round(x.igracSkills.skillsStavke.odbrana, 1),
                    datumPocetka = x.ugovor.datumPocetka,
                    datumZavrsetka = x.ugovor.datumZavrsetka,
                    iznosPlate = x.ugovor.iznosPlate,
                    transferMarktAcc = x.trensferMarktAcc,
                    napomene = x.ugovor.napomene
                }).Single();


            return PartialView("PrikaziDetalje", model);
        }
        [Autorizacija(false, false, true)]
        public string getRandomPassword(int lenght)
        {
            const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            const string UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string NUMBERS = "123456789";
            const string SPECIALS = "!@£$%^&*()#€";

            char[] _password = new char[lenght];
            string charSet = ""; // Initialise to blank
            System.Random _random = new Random();
            int counter;

            charSet += LOWER_CASE;
            charSet += UPPER_CASE;
            charSet += NUMBERS;
            charSet += SPECIALS;

            for (counter = 0; counter < lenght; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
        public IActionResult ConfirmEmail(string userid, string token, string pass)
        {
            Korisnik user = userManager.FindByIdAsync(userid).Result;
            IdentityResult result = userManager.
                        ConfirmEmailAsync(user, token).Result;

            if (result.Succeeded)
            {
                string filePath = System.IO.Path.GetFullPath("wwwroot/eBordoLoginPodaci.html");

                try
                {
                    MailMessage mm = new MailMessage("eBordoApp@outlook.com", user.emailAdresa);
                    mm.Subject = "Login podaci";
                    mm.Body = string.Empty;
                    using (var reader = new System.IO.StreamReader(filePath))
                    {
                        mm.Body = reader.ReadToEnd();
                    }
                    mm.Body = mm.Body.Replace("{korisnik}", user.ime + " " + user.prezime);
                    mm.Body = mm.Body.Replace("{username}", user.UserName);
                    mm.Body = mm.Body.Replace("{password}", pass);
                    mm.BodyEncoding = System.Text.Encoding.UTF8;
                    mm.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    NetworkCredential nc = new NetworkCredential("eBordoApp@outlook.com", "7wzS*S9coLUv^sS");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Send(mm);
                    user.EmailConfirmed = true;

                    TempData["succes"] = true;
                }
                catch (SmtpException ex)
                {
                    if (ex.StatusCode != SmtpStatusCode.MailboxUnavailable)
                    {
                        TempData["porukaInfo"] = "Mail adresa ne postoji!";
                        return Redirect("Prikaz");
                    }
                }
                return PartialView();
            }
            else
            {
                user.EmailConfirmed = false;
                TempData["succes"] = false;

                return PartialView();
            }
        }

        public IActionResult getDetalji(int igracID)
        {
            int brojUtakmica = db.utakmice.Where(s => s.statusUtakmice == StatusUtakmice.odigrana).Count();
            int brojMinuta = 90 * brojUtakmica;
            DetaljiStatistikaVM model = db.igraci
                .Where(s => s.igracID == igracID)
                .Select(t => new DetaljiStatistikaVM
                {
                    igracID = t.igracID,
                    ime = t.korisnik.ime,
                    prezime = t.korisnik.prezime,
                    slika = t.slika,
                    pozicija = t.pozicija.ToString(),
                    brojDresa = t.brojDresa,
                    brojNastupa = t.igracStatistika.brojNastupa,
                    odigraneMinute = t.igracStatistika.odigraneMinute,
                    golovi = t.igracStatistika.golovi,
                    asistencije = t.igracStatistika.asistencije,
                    zutiKartoni = t.igracStatistika.zutiKartoni,
                    crveniKartoni = t.igracStatistika.crveniKartoni,
                    prosjecnaOcjena = (float)Math.Round((decimal)t.igracStatistika.prosjecnaOcjena, 2),
                    kontrolaLopte = t.igracSkills.skillsStavke.kontrolaLopte,
                    dribljanje = t.igracSkills.skillsStavke.dribljanje,
                    dodavanje = t.igracSkills.skillsStavke.dodavanje,
                    kretanje = t.igracSkills.skillsStavke.kretanje,
                    brzina = t.igracSkills.skillsStavke.brzina,
                    sut = t.igracSkills.skillsStavke.sut,
                    snaga = t.igracSkills.skillsStavke.snaga,
                    markiranje = t.igracSkills.skillsStavke.markiranje,
                    klizeciStart = t.igracSkills.skillsStavke.klizeciStart,
                    skok = t.igracSkills.skillsStavke.skok,
                    odbrana = t.igracSkills.skillsStavke.odbrana,
                    prosjecnaOcjenaStavke = (float)Math.Round((decimal)t.igracSkills.skillsStavke.prosjekOcjena,2),
                    brojMinuta = brojMinuta,
                    brojUtakmica = brojUtakmica,
                    drzavljanstvo = t.korisnik.drzavljanstvo.nazivDrzave,
                    zastava = t.korisnik.drzavljanstvo.zastavaDrzave,
                    gradRodjenja = t.korisnik.gradRodjenja.nazivGrada,
                    zastavaZaGrad = t.korisnik.gradRodjenja.drzava.zastavaDrzave
                }).Single();
            return PartialView("getDetalji", model);
        }

        public IActionResult statistikaIndex(int igracID)
        {
            int brojUtakmica = db.utakmice.Where(s => s.statusUtakmice == StatusUtakmice.odigrana).Count();
            int brojMinuta = 90 * brojUtakmica;
            StatistikaIndex model = db.igraci
                .Where(s => s.igracID == igracID)
                .Select(x => new StatistikaIndex
                {
                    igracID = x.igracID,
                    brojMinuta = brojMinuta,
                    brojUtakmica = brojUtakmica,
                    brojNastupa = x.igracStatistika.brojNastupa,
                    odigraneMinute = x.igracStatistika.odigraneMinute,
                    golovi = x.igracStatistika.golovi,
                    asistencije = x.igracStatistika.asistencije,
                    zutiKartoni = x.igracStatistika.zutiKartoni,
                    crveniKartoni = x.igracStatistika.crveniKartoni,
                    prosjecnaOcjena = x.igracStatistika.prosjecnaOcjena,
                    kontrolaLopte = x.igracSkills.skillsStavke.kontrolaLopte,
                    dribljanje = x.igracSkills.skillsStavke.dribljanje,
                    dodavanje = x.igracSkills.skillsStavke.dodavanje,
                    kretanje = x.igracSkills.skillsStavke.kretanje,
                    brzina = x.igracSkills.skillsStavke.brzina,
                    sut = x.igracSkills.skillsStavke.sut,
                    snaga = x.igracSkills.skillsStavke.snaga,
                    markiranje = x.igracSkills.skillsStavke.markiranje,
                    klizeciStart = x.igracSkills.skillsStavke.klizeciStart,
                    prosjecnaOcjenaStavke = x.igracSkills.skillsStavke.prosjekOcjena,
                    skok = x.igracSkills.skillsStavke.skok,
                    odbrana = x.igracSkills.skillsStavke.odbrana,
                }).Single();

            return PartialView("statistikaIndex",model);
        }

        public IActionResult statistika()
        {
            return View("Statistika");
        }

        public int getUgovor(int igracID)
        {
            Ugovor ugovor = db.ugovori.Where(s => s.igracID == igracID).Single();
            return (ugovor.datumZavrsetka.Date - DateTime.Now.Date).Days;
        }

        public JsonResult getMiniStatistika(int igracID)
        {
            MiniStatistika model = db.igracStatistika
                .Where(s => s.igracID == igracID)
                .Select(s => new MiniStatistika
                {
                    brojNastupa = s.brojNastupa,
                    ocjena = s.prosjecnaOcjena
                }).Single();

            return Json(model);
        }
    }
}