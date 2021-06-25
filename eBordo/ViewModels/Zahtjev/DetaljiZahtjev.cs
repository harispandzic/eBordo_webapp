using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Zahtjev
{
    public class DetaljiZahtjev
    {
        public string datum { get; set; }
        public string svrha { get; set; }
        public string tip { get; set; }
        public string status { get; set; }
        public string prioritet { get; set; }
        public string odgovor { get; set; }
    }
}
