
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Data.EntityModels;
using Microsoft.AspNetCore.Identity;
using eBordo.Data;
using eBordo.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.SignalR;
using Hangfire;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace eBordo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private UserManager<Korisnik> _userManager;
        public IConfiguration Configuration { get; set; }
        private IHubContext<MyHub> hubContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<Korisnik> userManager, IConfiguration config, IHubContext<MyHub> _hubContext)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            Configuration = config;
            hubContext = _hubContext;
        }
        public void posaljiPoruku(string poruka)
        {
            List<Korisnik> korisnici = _db.igraci.Include(s => s.korisnik).Include(s => s.korisnik.notifikacije).Select(s => s.korisnik).ToList();
            NotifikacijaController controller = new NotifikacijaController(_db, _userManager, hubContext);
            controller.posaljiPorukuOdredjenim(poruka, "success", korisnici, true, "eBordo");
        }
        public void prijavaMail(string imePrezime, string emailAdresa)
        {
            MailMessage mm = new MailMessage("eBordoApp@outlook.com", emailAdresa);
            mm.Subject = "Prijava na sistem";
            mm.Body = "Poštovani, " + imePrezime + " uspješno ste se prijavili na sistem. Vrijeme prijave: " + DateTime.Now.ToString();
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

        public IActionResult Index()
        {
            Korisnik user = Autentifikacija.LogiraniKorisnik(HttpContext);
            RecurringJob.AddOrUpdate(() => posaljiPoruku("Sretna Nova godina!"), Cron.Yearly(1,1,0,0));

            if (user == null)
            {
                return Redirect("Identity/Account/Login");
            }
            else
            {
                if (user.isIgrac)
                {
                    BackgroundJob.Schedule(() => prijavaMail(user.imePrezime, user.emailAdresa), DateTime.Now.AddSeconds(5));
                    return Redirect("/Igrac/Index");
                }
                if (user.isTrener)
                {
                    BackgroundJob.Schedule(() => prijavaMail(user.imePrezime, user.emailAdresa), DateTime.Now.AddSeconds(5));
                    return Redirect("/Trener/Index");
                }
                if (user.isAdmin)
                {
                    BackgroundJob.Schedule(() => prijavaMail("Administrator", user.emailAdresa), DateTime.Now.AddSeconds(5));
                    return Redirect("/Admin/Index");
                }
                return Redirect("Identity/Account/Login");
            }
        }
    }
}
