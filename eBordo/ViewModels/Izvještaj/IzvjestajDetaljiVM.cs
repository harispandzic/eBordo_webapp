using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class IzvjestajDetaljiVM
    {
        public class Nastupi
        {
            public Nastupi() { }
            public int igracID { get; set; }
            public string imePrezime { get; set; }
            public string iPrezime { get; set; }
            public string slikaIgraca { get; set; }
            public int brojDresa { get; set; }
            public int minute { get; set; }
            public int golovi { get; set; }
            public int asistencije { get; set; }
            public int zutiKartoni { get; set; }
            public int crveniKartoni { get; set; }
            public float ocjena { get; set; }
            public float prosjecnaOcjena { get; set; }
        }
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
        public int izvještajID { get; set; }
        public int utakmicaID { get; set; }
        public string datumKreiranja { get; set; }
        public int brojGledalaca { get; set; }
        public string vrstaTakmicenja { get; set; }
        public string slikaVrstaTakmicenja { get; set; }
        public string domacin { get; set; }
        public string domacinGrb { get; set; }
        public string gost { get; set; }
        public string gostGrb { get; set; }
        public int goloviGostiju { get; set; }
        public int goloviDomacih { get; set; }
        public Rezultat rezultat { get; set; }
        public string zapisnik { get; set; }
        public string youtubeVideoID { get; set; }
        public Vrijeme vrijeme { get; set; }
        public string trener { get; set; }
        public string igracUtakmice { get; set; }
        public string nazivStadiona { get; set; }
        public string trenerSlika { get; set; }
        public string slikaIgrac { get; set; }
        public string delegat { get; set; }
        public string slika { get; set; }
        public List<Nastupi> nastupi { get; set; }
        public List<Izmjena> izmjene { get; set; }
    }
}
