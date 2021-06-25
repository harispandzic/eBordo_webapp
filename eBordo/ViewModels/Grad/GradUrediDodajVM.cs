using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo_seminarski_rad.ViewModels.Grad
{
    public class GradUrediDodajVM
    {
        public int gradID { get; set; }
        [Required]
        public string nazivGrada { get; set; }
        [Required]
        public int drzavaID { get; set; }
        public List<SelectListItem> drzave { get; set; }
    }
}
