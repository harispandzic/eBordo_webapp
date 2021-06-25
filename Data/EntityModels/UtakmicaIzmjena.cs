using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class UtakmicaIzmjena
    {
        public int utakmicaIzmjenaID { get; set; }

        public int utakmicaID { get; set; }
        public Utakmica utakmica { get; set; }
        
        public int izmjenaID { get; set; }
        public Izmjena izmjena { get; set; }
    }
}
