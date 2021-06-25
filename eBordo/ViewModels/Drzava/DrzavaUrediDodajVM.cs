using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo_seminarski_rad.ViewModels.Drzava
{
    public class DrzavaUrediDodajVM
    {
        public int drzavaID { get; set; }
        [Required]
        public string nazivDrzave { get; set; }
        [Required]
        public IFormFile zastavaNew { set; get; }
        public string zastavaCurrent { get; set; }
    }
}
