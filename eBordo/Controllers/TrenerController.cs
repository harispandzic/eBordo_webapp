using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels.Trener;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
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

namespace eBordo.Controllers
{
    public class TrenerController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;

        public TrenerController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }
        [Autorizacija(true, false, false)]
        public IActionResult Index()
        {
            return View();
        }
        [Autorizacija(false, false, true)]
        public IActionResult Prikaz(string q, string statusTrenera)
        {
            List<TrenerPrikaziVM.Row> treneri;

            List<string> statusiString = new List<string>();
            var statusi = Enum.GetValues(typeof(Status)).Cast<Status>();
            foreach (Status l in statusi)
                statusiString.Add(l.ToString());

            List<SelectListItem> statusiList = statusiString.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();


            if (statusTrenera != null)
            {
                Status statusTrener = (Status)Enum.Parse(typeof(Status), statusTrenera);

                treneri = db.treneri
                .Where(a => (a.status == statusTrener))
                .Select(x => new TrenerPrikaziVM.Row
                {
                    trenerID = x.trenerID,
                    ime = x.korisnik.ime,
                    prezime = x.korisnik.prezime,
                    slikaTrenera = x.slika,
                    datumRodjenja = x.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
                    drzavljanstvo = x.korisnik.drzavljanstvo.nazivDrzave,
                    drzavaSlika = x.korisnik.drzavljanstvo.zastavaDrzave,
                    gradRodjenja = x.korisnik.gradRodjenja.nazivGrada,
                    gradRodjenjaZastava = x.korisnik.gradRodjenja.drzava.zastavaDrzave,
                    licenca = x.trenerskaLicenca,
                    statusTrenera = x.status
                }).ToList();
            }
            else
            {
                treneri = db.treneri
                   .Where(a => (q == "" || q == null || a.korisnik.ime.StartsWith(q) || a.korisnik.prezime.StartsWith(q)) && a.status == Status.Aktivan)
                   .Select(x => new TrenerPrikaziVM.Row
                   {
                       trenerID = x.trenerID,
                       ime = x.korisnik.ime,
                       prezime = x.korisnik.prezime,
                       slikaTrenera = x.slika,
                       datumRodjenja = x.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
                       drzavljanstvo = x.korisnik.drzavljanstvo.nazivDrzave,
                       drzavaSlika = x.korisnik.drzavljanstvo.zastavaDrzave,
                       gradRodjenja = x.korisnik.gradRodjenja.nazivGrada,
                       gradRodjenjaZastava = x.korisnik.gradRodjenja.drzava.zastavaDrzave,
                       licenca = x.trenerskaLicenca,
                       statusTrenera = x.status
                   }).ToList();
            }
            TrenerPrikaziVM model = new TrenerPrikaziVM();
            model.treneri = treneri;
            model.statusi = statusiList;
            model.q = q;
            model.statusTrenera = statusTrenera;

            if (model.treneri.Count() == 0 && q != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            if (model.treneri.Count() == 0 && statusTrenera != null)
                TempData["porukaInfo"] = "Nema rezultata pretrage!";

            return View("Prikaz", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult DodajTrenera()
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

            List<string> licenceStringArray = new List<string>();
            var licenca = Enum.GetValues(typeof(TrenerskaLicenca)).Cast<TrenerskaLicenca>();
            foreach (TrenerskaLicenca l in licenca)
                licenceStringArray.Add(l.ToString());

            List<SelectListItem> licence = licenceStringArray.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<string> formacijeStringArray = new List<string>();
            var formacija = Enum.GetValues(typeof(Formacija)).Cast<Formacija>();
            foreach (Formacija l in formacija)
                formacijeStringArray.Add(l.ToString());

            List<SelectListItem> formacije = formacijeStringArray.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            TrenerUrediDodaj model = new TrenerUrediDodaj();

            model.drzave = drzave;
            model.gradovi = gradovi;
            model.licence = licence;
            model.formacije = formacije;
            model.datumPristupaKlubu = DateTime.Now;
            model.datumRodjenja = DateTime.Now;
            model.prvaZvanicnaUtakmica = DateTime.Now;

            return PartialView("DodajTrenera", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult SnimiDodaj(TrenerUrediDodaj model)
        {
            List<Korisnik> korisnici = userManager.Users.ToList();
            foreach (var k in korisnici)
            {
                if (k.ime == model.ime && k.prezime == model.prezime)
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
                isTrener = true,
                isIgrac = false
                
            };

            AccountController ac = new AccountController(db,userManager);

            string password = ac.getRandomPassword();

            IdentityResult result = userManager.CreateAsync(korisnik, password).Result;

            string filePath = System.IO.Path.GetFullPath("wwwroot/ConfirmEmailTemplate.html");

            string confirmationToken = userManager.
                 GenerateEmailConfirmationTokenAsync(korisnik).Result;

            string confirmationLink = Url.Action("ConfirmEmail",
              "Trener", new
              {
                  userid = korisnik.Id,
                  token = confirmationToken,
                  pass = password
              },
               protocol: HttpContext.Request.Scheme);

            try
            {
                MailMessage mm = new MailMessage("eBordoApp@outlook.com", korisnik.emailAdresa);
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
            catch (SmtpException ex)
            {
                if (ex.StatusCode != SmtpStatusCode.MailboxUnavailable)
                {
                    TempData["porukaInfo"] = "Mail adresa ne postoji!";
                    return Redirect("Prikaz");
                }
            }

            TrenerStatistika statistika = new TrenerStatistika();
            db.trenerStatistika.Add(statistika);
            db.SaveChanges();

            Ugovor ugovor = new Ugovor();
            db.ugovori.Add(ugovor);
            db.SaveChanges();

            TrenerskaLicenca licenca = (TrenerskaLicenca)Enum.Parse(typeof(TrenerskaLicenca), model.nazivLicence);
            Formacija formacija = (Formacija)Enum.Parse(typeof(Formacija), model.preferiranaFormacija);

            Trener trener = new Trener
            {
                imePrezime = model.ime + " " + model.prezime,
                prvaZvanicnaUtakmica = model.prvaZvanicnaUtakmica,
                godineIskustva = DateTime.Now.Year - model.prvaZvanicnaUtakmica.Year,
                datumPristupaKlubu = model.datumPristupaKlubu,
                dosadasnjiKlubovi = model.dosadasnjiKlubovi,
                status = Status.Aktivan,
                preferiranaFormacija = formacija,
                trenerskaLicenca = licenca,
                trenerStatistika = statistika,
                tranfermarktAcc = model.transferMarktAcc,
                ugovor = ugovor,
                korisnik = korisnik
            };
            if (model.slikaNew != null)
            {
                string ekstenzija = Path.GetExtension(model.slikaNew.FileName);
                string contentType = model.slikaNew.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.slikaNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                trener.slika = filename;
            }

            db.treneri.Add(trener);
            db.SaveChanges();

            statistika.trenerID = trener.trenerID;
            db.SaveChanges();

            ugovor.trenerID = trener.trenerID;
            db.SaveChanges();

            korisnik.trenerID = trener.trenerID;
            db.SaveChanges();

            NotifikacijaController controller = new NotifikacijaController(db, userManager, hubContext);
            string poruka = korisnik.imePrezime + " dobrodošli na eBordo platformu!";
            string posiljaoc = "admin@fksarajevo.ba";
            controller.posaljiJednom(poruka, "success", korisnik, false, posiljaoc);


            TempData["porukaInfo"] = "Uspješno ste dodali zapis!";

            return Redirect("Prikaz");
        }
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
        [Autorizacija(false, false, true)]
        public IActionResult Obrisi(int trenerID)
        {
            TrenerObrisiVM model = new TrenerObrisiVM();
            model.trenerID = trenerID;
            return PartialView("TrenerObrisi", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult ObrisiSnimi(int trenerID)
        {
            Trener i = db.treneri.Find(trenerID);
            i.status = Status.Neaktivan;

            db.SaveChanges();

            TempData["porukaInfo"] = "Uspješno ste arhivirali zapis!";
            return Redirect("Prikaz?statusTrenera=Neaktivan");
        }
        [Autorizacija(false, false, true)]
        public IActionResult VratiTrenera(int trenerID)
        {
            Trener i = db.treneri.Find(trenerID);
            i.status = Status.Aktivan;

            db.SaveChanges();

            TempData["porukaInfo"] = "Uspješno ste uredili status trenera!";
            return Redirect("Prikaz?statusTrenera=Aktivan");
        }
        [Autorizacija(false, false, true)]
        public IActionResult UrediTrenera(int trenerID)
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

            List<string> licenceStringArray = new List<string>();
            var licenca = Enum.GetValues(typeof(TrenerskaLicenca)).Cast<TrenerskaLicenca>();
            foreach (TrenerskaLicenca l in licenca)
                licenceStringArray.Add(l.ToString());

            List<SelectListItem> licence = licenceStringArray.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            List<string> formacijeStringArray = new List<string>();
            var formacija = Enum.GetValues(typeof(Formacija)).Cast<Formacija>();
            foreach (Formacija l in formacija)
                formacijeStringArray.Add(l.ToString());

            List<SelectListItem> formacije = formacijeStringArray.Select(s => new SelectListItem
            {
                Value = s,
                Text = s
            }).ToList();

            TrenerUrediDodaj model = new TrenerUrediDodaj();
            model = db.treneri.Where(i => i.trenerID == trenerID)
                .Select(a => new TrenerUrediDodaj
                {
                    trenerID = a.trenerID,
                    ime = a.korisnik.ime,
                    prezime = a.korisnik.prezime,
                    datumRodjenja = a.korisnik.datumRodjenja,
                    adresaPrebivalista = a.korisnik.adresaPrebivalista,
                    telefon = a.korisnik.telefon,
                    emailAdresa = a.korisnik.emailAdresa,
                    drzavljanstvoID = a.korisnik.drzavljanstvoID,
                    gradRodjenjaID = a.korisnik.gradRodjenjaID,
                    prvaZvanicnaUtakmica = a.prvaZvanicnaUtakmica,
                    datumPristupaKlubu = a.datumPristupaKlubu,
                    dosadasnjiKlubovi = a.dosadasnjiKlubovi,
                    preferiranaFormacija = a.preferiranaFormacija.ToString(),
                    nazivLicence = a.trenerskaLicenca.ToString(),
                    trzisnaVrijednost = a.trzisnaVrijednost,
                    transferMarktAcc = a.tranfermarktAcc,
                    slikaCurrent = a.slika
                }).Single();

                model.drzave = drzave;
                model.gradovi = gradovi;
                model.licence = licence;
                model.formacije = formacije;
                model.datumPristupaKlubu = DateTime.Now;
                model.datumRodjenja = DateTime.Now;
                model.prvaZvanicnaUtakmica = DateTime.Now;

                return PartialView("UrediTrenera", model);
        }
        [Autorizacija(false, false, true)]
        public IActionResult SnimiUredi(TrenerUrediDodaj model)
        {
            TrenerskaLicenca licenca = (TrenerskaLicenca)Enum.Parse(typeof(TrenerskaLicenca), model.nazivLicence);
            Formacija formacija = (Formacija)Enum.Parse(typeof(Formacija), model.preferiranaFormacija);

            Trener trener = db.treneri.Find(model.trenerID);
            Korisnik user = userManager.FindByIdAsync(trener.korisnikID).Result;

            user.emailAdresa = model.emailAdresa;
            user.drzavljanstvoID = model.drzavljanstvoID;
            user.adresaPrebivalista = model.adresaPrebivalista;
            user.telefon = model.telefon;
            trener.trenerskaLicenca = licenca;
            trener.preferiranaFormacija = formacija;
            trener.dosadasnjiKlubovi = model.dosadasnjiKlubovi;
            trener.trzisnaVrijednost = model.trzisnaVrijednost;
            trener.tranfermarktAcc = model.transferMarktAcc;

            if (model.slikaNew != null)
            {
                string ekstenzija = Path.GetExtension(model.slikaNew.FileName);
                string contentType = model.slikaNew.ContentType;

                var filename = $"{Guid.NewGuid()}{ekstenzija}";
                model.slikaNew.CopyTo(new FileStream("wwwroot/uploads/" + filename, FileMode.Create));
                trener.slika = filename;
            }

            db.SaveChanges();
            TempData["porukaInfo"] = "Uspješno ste uredili zapis!";

            return Redirect("Prikaz");
        }
        public IActionResult prikaziDetalje(int trenerID)
        {
            int broj = db.utakmice.Where(s => s.statusUtakmice == StatusUtakmice.odigrana).Count();

            TrenerDetaljiVM model = db.treneri
                .Where(s => s.trenerID == trenerID)
                .Select(x => new TrenerDetaljiVM
                {
                    trenerID = x.trenerID,
                    ime = x.korisnik.ime,
                    prezime = x.korisnik.prezime,
                    datumRodjenja =x.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
                    godine = DateTime.Now.Year - x.korisnik.datumRodjenja.Year,
                    adresaPrebivalista = x.korisnik.adresaPrebivalista,
                    telefon = x.korisnik.telefon,
                    emailAdresa = x.korisnik.emailAdresa,
                    drzavljanstvo = x.korisnik.drzavljanstvo.nazivDrzave,
                    zastava = x.korisnik.drzavljanstvo.zastavaDrzave,
                    gradRodjenja = x.korisnik.gradRodjenja.nazivGrada,
                    zastavaZaGrad = x.korisnik.gradRodjenja.drzava.zastavaDrzave,
                    username = x.korisnik.UserName,
                    prvaZvanicnaUtakmica = x.prvaZvanicnaUtakmica.ToString("dd.MM.yyyy"),
                    godineIskustva = x.godineIskustva,
                    slika = x.slika,
                    dosadasnjiKlubovi = x.dosadasnjiKlubovi,
                    trzisnaVrijednost = x.trzisnaVrijednost,
                    statusIgraca = x.status,
                    brojOdigranihUtakmica = broj,
                    brojUtakmica = x.trenerStatistika.brojUtakmica,
                    brojPobjeda = x.trenerStatistika.brojPobjeda,
                    brojNerjesenih = x.trenerStatistika.brojNerjesenih,
                    brojPoraza = x.trenerStatistika.brojPoraza,
                    datumPocetka = x.ugovor.datumPocetka,  
                    datumZavrsetka = x.ugovor.datumZavrsetka,
                    iznosPlate = x.ugovor.iznosPlate,
                    napomene = x.ugovor.napomene,
                    preferiranaFormacija = x.preferiranaFormacija,
                    trenerskaLicenca = x.trenerskaLicenca,
                    transferMartkAcc = x.tranfermarktAcc
                }).Single();

            return PartialView("PrikaziDetalje", model);
        }
        public int getUgovor(int trenerID)
        {
            Ugovor ugovor = db.ugovori.Where(s => s.trenerID == trenerID).Single();
            return (ugovor.datumZavrsetka.Date - DateTime.Now.Date).Days;
        }
        public JsonResult getMiniStatistika(int trenerID)
        {
            List<int> model = new List<int>();
            TrenerStatistika statistika = db.trenerStatistika.Where(s => s.trenerID == trenerID).Single();
            model.Add(statistika.brojPobjeda);
            model.Add(statistika.brojPoraza);
            model.Add(statistika.brojNerjesenih);

            return Json(model);
        }
        public IActionResult statistika()
        {
            return View("Statistika");
        }
        public IActionResult trenerUtakmice(int trenerID)
        {
            List<TrenerUtakmiceVM.Row> utakmice = db.izvjestaji
                .Where(a => (a.trenerID == trenerID))
                .Select(s => new TrenerUtakmiceVM.Row
                {
                    domacin = s.utakmica.domacin.nazivKluba,
                    domacinGrb = s.utakmica.domacin.grbKluba,
                    gost = s.utakmica.gost.nazivKluba,
                    gostGrb = s.utakmica.gost.grbKluba,
                    goloviDomaci = s.goloviDomacih,
                    goloviGosti = s.goloviGostiju,
                    rezultat = s.rezultat,
                    datum = s.utakmica.datumOdigravanja,
                    vrstaTakmicenjaSlika = s.utakmica.vrstaTakmicenja.logoTakmicenja,
                    odigranaDani = (DateTime.Now.Date - s.utakmica.datumOdigravanja.Date).Days,
                }).ToList();

            TrenerUtakmiceVM model = new TrenerUtakmiceVM();
            model.utakmice = utakmice;

            return PartialView("TrenerUtakmice", model);
        }
        public IActionResult trenerStatistika(int trenerID)
        {
            int broj = db.utakmice.Where(s => (s.statusUtakmice == StatusUtakmice.odigrana && s.trenerID == trenerID)).Count();
            TrenerStatistikaVM model = db.trenerStatistika
                .Where(s => s.trenerID == trenerID)
                .Select(x => new TrenerStatistikaVM
                {
                    brojUtakmica = x.brojUtakmica,
                    brojOdigranihUtakmica = broj,
                    brojPobjeda = x.brojPobjeda,
                    brojNerjesenih = x.brojNerjesenih,
                    brojPoraza = x.brojPoraza,
                }).Single();

            return PartialView("TrenerStatistika", model);
        }
        public JsonResult NewChart(int trenerID)
        {
            TrenerStatistika stavke = db.trenerStatistika.Where(s => s.trenerID == trenerID).Single();

            List<object> iData = new List<object>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Opis", System.Type.GetType("System.String"));
            dt.Columns.Add("Broj", System.Type.GetType("System.Int32"));

            DataRow dr = dt.NewRow();
            dr["Opis"] = "Broj pobjeda";
            dr["Broj"] = stavke.brojPobjeda;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Opis"] = "Broj poraza";
            dr["Broj"] = stavke.brojPoraza;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Opis"] = "Broj nerješenih";
            dr["Broj"] = stavke.brojNerjesenih;
            dt.Rows.Add(dr);          

            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            return Json(iData);
        }
    }
}
