using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Izvještaj
    {
        public int izvještajID { get; set; }
        public int goloviGostiju { get; set; }
        public int goloviDomacih { get; set; }
        public Rezultat rezultat{ get; set; } //za klub Sarajevo

        public Igrac igracUtakmica { get; set; }
        public int igracUtakmicaID { get; set; }
        public string delegatUtakmice { get; set; }

        public Utakmica utakmica { get; set; }
        public int utakmicaID { get; set; }

        public Trener trener { get; set; }
        public int trenerID { get; set; }
        public int brojGledalaca { get; set; }
        public Vrijeme vrijeme{ get; set; }
        public DateTime datumKreiranja { get; set; }
        
        public string zapisnik{ get; set; }
        public string slikaSaUtakmice { get; set; }
        public string youtubeVideoID { get; set; }

        public ICollection<UtakmicaNastupi> nastupi { get; set; }
        public ICollection<UtakmicaOcjene> ocjene { get; set; }
        public ICollection<UtakmicaIzmjena> izmjene { get; set; }
    }
}
