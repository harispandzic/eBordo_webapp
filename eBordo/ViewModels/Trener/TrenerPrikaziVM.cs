using Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Trener
{
    public class TrenerPrikaziVM
    {
        public class Row
        {
            public Row() { }
            public int trenerID { get; set; }
            public string ime { get; set; }
            public string prezime { get; set; }
            public string slikaTrenera { get; set; }
            public string datumRodjenja { get; set; }
            public string drzavljanstvo { get; set; }
            public string drzavaSlika { get; set; }
            public string gradRodjenja { get; set; }
            public string gradRodjenjaZastava { get; set; }
            public TrenerskaLicenca licenca { get; set; }
            public Status statusTrenera { get; set; }
        }
        public List<Row> treneri { get; set; }
        public List<SelectListItem> statusi { get; set; }
        //Filteri
        public string statusTrenera { get; set; }
        public string q { get; set; }
    }
}
