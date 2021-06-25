using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

namespace eBordo.Controllers
{
    public class SMSController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<Korisnik> _userManager;

        public SMSController(ApplicationDbContext db, UserManager<Korisnik> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public string posaljiSMSAsync(string korisnikID, string poruka)
        {
            Korisnik korisnik = _db.korisnici.Find(korisnikID);

            var VONAGE_API_KEY = "2ff991e0";
            var VONAGE_API_SECRET = "UN9YY0OCnlhxJxh8";
            var credentials = Credentials.FromApiKeyAndSecret(
            VONAGE_API_KEY,
            VONAGE_API_SECRET
            );

            var vonageClient = new VonageClient(credentials);

            var response = vonageClient.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest()
            {
                To = korisnik.telefon,
                From = "FK Sarajevo",
                Text = poruka
            });

            return "OK";
        }
    }
}
