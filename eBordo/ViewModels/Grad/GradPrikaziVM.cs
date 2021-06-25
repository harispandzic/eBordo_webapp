using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo_seminarski_rad.ViewModels.Grad
{
    public class GradPrikaziVM
    {
        public class Row
        {
            public Row() { }
            public int gradID { get; set; }
            public string nazivGrada { get; set; }
            public string nazivDrzave { get; set; }
            public string zastavaCurrent { get; set; }
        }
        public List<Row> gradovi { get; set; }
        public List<SelectListItem> drzave { get; set; }
        //Filter&&Search
        public string nazivDrzave { get; set; }
        public string q;
    }
}
