using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Izmjena
    {
        public int izmjenaID { get; set; }

        public Igrac igracOut { get; set; }
        public int igracOutID { get; set; }

        public Igrac igracIn { get; set; }
        public int igracInID { get; set; }

        public int minuta { get; set; }
        public RazlogIzmjene razlog { get; set; }
    }
}
