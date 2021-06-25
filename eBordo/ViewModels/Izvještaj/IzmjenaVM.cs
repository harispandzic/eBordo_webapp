using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class IzmjenaVM
    {
        public class Izmjena
        {
            public Izmjena() { }
            public int izmjenaID { get; set; }
            public string igracIN { get; set; }
            public string slikaIN { get; set; }
            public int brojDresaIN { get; set; }
            public string igracOUT { get; set; }
            public string slikaOUT { get; set; }
            public int brojDresaOUT { get; set; }
            public int minuta { get; set; }
            public string razlog { get; set; }
        }
        public List<Izmjena> izmjene { get; set; }
        public int utakmicaID { get; set; }
    }
}
