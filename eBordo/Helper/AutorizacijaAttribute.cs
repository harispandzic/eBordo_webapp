using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace eBordo.Helper
{
    public class AutorizacijaAttribute : TypeFilterAttribute
    {
        public AutorizacijaAttribute(bool dozvoljenoTreneru, bool dozvoljenoIgracu,bool dozvoljenoAdminu)
            : base(typeof(MyAuthorizeImpl))
        {
            Arguments = new object[] { dozvoljenoTreneru, dozvoljenoIgracu, dozvoljenoAdminu };
        }
    }


    public class MyAuthorizeImpl : IActionFilter
    {
        public MyAuthorizeImpl(bool dozvoljenoTreneru, bool dozvoljenoIgracu, bool dozvoljenoAdminu)
        {
            _dozvoljenoTreneru = dozvoljenoTreneru;
            _dozvoljenoIgracu = dozvoljenoIgracu;
            _dozvoljenoAdminu = dozvoljenoAdminu;
        }
        private readonly bool _dozvoljenoTreneru;
        private readonly bool _dozvoljenoIgracu;
        private readonly bool _dozvoljenoAdminu;

        public void OnActionExecuted(ActionExecutedContext context)
        {


        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext httpContext = filterContext.HttpContext;

            Korisnik k = httpContext.LogiraniKorisnik();

            if (k == null)
            {
                if (filterContext.Controller is Controller controller)
                {
                    controller.TempData["PorukaError"] = "Niste logirani";
                }
                filterContext.Result = new RedirectResult("/Igrac/Prikaz");
                return;
            }

            //treneri mogu pristupiti 
            if (_dozvoljenoTreneru && k.trener != null)
            {
                return;
            }

            //igraci mogu pristupiti 
            if (_dozvoljenoIgracu && k.igrac != null)
            {
                return;//ok - ima pravo pristupa
            }

            if (_dozvoljenoAdminu && k.isAdmin)
            {
                return;//ok - ima pravo pristupa
            }

            if (filterContext.Controller is Controller c1)
            {
                c1.ViewData["PorukaError"] = "Nemate pravo pristupa";
            }
            filterContext.Result = new RedirectResult("/Account/AccessDenied");
        }
    }
}
