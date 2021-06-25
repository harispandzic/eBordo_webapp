using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Zahtjev
    {
        public int zahtjevID { get; set; }
        public SvrhaZahtjeva svrha { get; set; }
        public TipZahtjeva tipZahtjeva { get; set; }
        public StatusZahtjeva statusZahtjeva{ get; set; }
        public PrioritetZahtjeva prioritet{ get; set; }
        public DateTime datum{ get; set; }
        public string odgovor{ get; set; }
        public Korisnik korisnik { get; set; }
        public string korisnikID { get; set; }
    }
}
