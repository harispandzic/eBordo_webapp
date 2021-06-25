using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using eBordo.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eBordo.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;

        public AccountController(ApplicationDbContext database, UserManager<Korisnik> _userManager)
        {
            db = database;
            userManager = _userManager;
        }
        [Autorizacija(false, false, true)]
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        [Autorizacija(false, false, true)]
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        [Autorizacija(false, false, true)]
        private string RandomSpecial(int size = 10)
        {
            const string SPECIALS = "!@£$%^&*()#€";

            char[] _password = new char[size];
            string charSet = "";
            System.Random _random = new Random();
            int counter;

            charSet += SPECIALS;

            for (counter = 0; counter < size; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
        [Autorizacija(false, false, true)]
        public string getRandomPassword()
        {
            StringBuilder builder = new StringBuilder();

            Random random = new Random();
            int broj;
            bool special = false, lowerString = false, upperString = false, numbers = false;
            do
            {
                broj = random.Next(1, 5);
                if (broj == 1 && !special)
                {
                    builder.Append(RandomString(2, true));
                    builder.Append(RandomSpecial(2));
                    special = true;
                }
                if (broj == 2 && !lowerString)
                {
                    builder.Append(RandomString(2, false));
                    builder.Append(RandomNumber(1000, 9999));
                    lowerString = true;
                }
                if (broj == 3 && !upperString)
                {
                    builder.Append(RandomString(2, false));
                    builder.Append(RandomSpecial(2));
                    upperString = true;
                }
                if (broj == 4 && !numbers)
                {
                    builder.Append(RandomSpecial(2));
                    builder.Append(RandomString(2, true));
                    numbers = true;
                }
            } while (!special || !lowerString || !upperString || !numbers);

            return builder.ToString();
        }

        public IActionResult ConfirmEmail(string userid, string token, string pass)
        {
            Korisnik user = userManager.FindByIdAsync(userid).Result;
            IdentityResult result = userManager.
                        ConfirmEmailAsync(user, token).Result;

            if (result.Succeeded)
            { 
                try
                {
                    MailMessage mm = new MailMessage("eBordoApp@outlook.com", user.emailAdresa);
                    mm.Subject = "Login podaci";
                
                    mm.Body = mm.Body.Replace("{password}", pass);
                    mm.IsBodyHtml = false;

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
                        TempData["succes"] = false;
                        return PartialView();
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
        public IActionResult getTrenerPodaci(int trenerID)
        {
            TrenerPodaciVM model = db.treneri
                .Where(s => s.trenerID == trenerID)
                .Select(x => new TrenerPodaciVM
                {
                    trenerID = x.trenerID,
                    ime = x.korisnik.ime,
                    prezime = x.korisnik.prezime,
                    datumRodjenja = x.korisnik.datumRodjenja.ToString("dd.MM.yyyy"),
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
                    datumPocetka = x.ugovor.datumPocetka,
                    datumZavrsetka = x.ugovor.datumZavrsetka,
                    iznosPlate = x.ugovor.iznosPlate,
                    napomene = x.ugovor.napomene,
                    preferiranaFormacija = x.preferiranaFormacija,
                    trenerskaLicenca = x.trenerskaLicenca,
                    transferMartkAcc = x.tranfermarktAcc
                }).Single();
            return PartialView(model);
        }
        public IActionResult getPodatke(int igracID)
        {
            LicniPodaciVM model = db.igraci
                .Where(s => s.igracID == igracID)
                .Select(x => new LicniPodaciVM
                {
                    igracID = x.igracID,
                    ime = x.korisnik.ime,
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
                    datumPocetka = x.ugovor.datumPocetka,
                    datumZavrsetka = x.ugovor.datumZavrsetka,
                    iznosPlate = x.ugovor.iznosPlate,
                    transferMarktAcc = x.trensferMarktAcc,
                    napomene = x.ugovor.napomene
                }).Single();

            return PartialView(model);
        }
        public IActionResult AccessDenied()
        {
            return PartialView("AccessDenied");
        }
    }
}