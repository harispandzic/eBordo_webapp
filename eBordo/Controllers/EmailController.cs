using Data.EntityModels;
using eBordo.Data;
using eBordo.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace eBordo.Controllers
{
    public class EmailController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<Korisnik> userManager;
        private IHubContext<MyHub> hubContext;

        public EmailController(ApplicationDbContext database, UserManager<Korisnik> _userManager, IHubContext<MyHub> _hubContext)
        {
            db = database;
            userManager = _userManager;
            hubContext = _hubContext;
        }
        public string posaljiEmail(List<Korisnik> korisnici,string poruka,string tipPoruka)
        {
            for (int i = 0; i < korisnici.Count(); i++)
            {
                string emailAdresa = korisnici[i].emailAdresa;
                try
                {
                    MailMessage mm = new MailMessage("eBordoApp@outlook.com", emailAdresa);
                    mm.Subject = tipPoruka;
                    mm.Body = poruka;
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
                catch (SmtpException ex)
                {

                }
            }
            return "OK";
        }
        public string posaljiEmailPojedinacno(Korisnik korisnik, string poruka, string tipPoruka)
        {
            string emailAdresa = korisnik.emailAdresa;
            try
            {
                MailMessage mm = new MailMessage("eBordoApp@outlook.com", emailAdresa);
                mm.Subject = tipPoruka;
                mm.Body = poruka;
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
            catch (SmtpException ex)
            {

            }

            return "OK";
        }
    }
}
