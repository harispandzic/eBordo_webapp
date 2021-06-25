using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Data.EntityModels;
using eBordo.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace eBordo.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly SignInManager<Korisnik> _signInManager;
        private ApplicationDbContext db;
        
        public IndexModel(
            UserManager<Korisnik> userManager,
            SignInManager<Korisnik> signInManager,
            ApplicationDbContext database)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            db = database;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string ime { get; set; }
            public string prezime { get; set; }
            public DateTime datumRodjenja { get; set; }
            public string adresaPrebivalista { get; set; }
            public string telefon { get; set; }
            public string emailAdresa { get; set; }
            public string drzavljanstvo { get; set; }
            public string gradRodjenja { get; set; }
        }

        private async Task LoadAsync(Korisnik user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            Username = userName;
            

            Input = new InputModel
            {
                ime = user.ime,
                prezime = user.prezime,
                datumRodjenja = user.datumRodjenja,
                adresaPrebivalista = user.adresaPrebivalista,
                emailAdresa = user.emailAdresa,
                drzavljanstvo = db.korisnici.Where(s => s.Id == user.Id).Include(s => s.drzavljanstvo).Select(s => s.drzavljanstvo.nazivDrzave).Single(),
                gradRodjenja = db.korisnici.Where(s => s.Id == user.Id).Include(s => s.gradRodjenja).Select(s => s.gradRodjenja.nazivGrada).Single(),
                telefon = db.korisnici.Where(s => s.Id == user.Id).Select(s => s.telefon).Single()
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.emailAdresa = Input.emailAdresa;
            string filePath = System.IO.Path.GetFullPath("wwwroot/ConfirmEmailTemplate.html");

            string confirmationToken = _userManager.
                 GenerateEmailConfirmationTokenAsync(user).Result;

            string confirmationLink = Url.Action("ConfirmEmail",
              "Account", new
              {
                  userid = user.Id,
                  token = confirmationToken
              },
               protocol: HttpContext.Request.Scheme);

            

            try
            {
                MailMessage mm = new MailMessage("eBordoApp@outlook.com", user.emailAdresa);
                mm.Subject = "Potvrda e-mail adrese";
                mm.Body = string.Empty;
                using (var reader = new System.IO.StreamReader(filePath))
                {
                    mm.Body = reader.ReadToEnd();
                }
                mm.Body = mm.Body.Replace("{imePrezime}", Input.ime + " " + Input.prezime);
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
                    return RedirectToPage("");
                }
            }
            user.telefon = "387" + Input.telefon;
            user.EmailConfirmed = false;
            db.SaveChanges();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Profil je uspješno izmjenjen!";
            TempData["porukaInfo"] = "Profil je uspješno izmjenjen!";

            return RedirectToPage("/Account/Login");
        }
    }
}
