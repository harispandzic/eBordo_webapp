using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class TrenerStatistika
    {
        public int trenerStatistikaID { get; set; }
        public int brojUtakmica { get; set; }
        public int brojPobjeda { get; set; }
        public int brojNerjesenih { get; set; }
        public int brojPoraza { get; set; }

        public Trener trener { get; set; }
        public int? trenerID { get; set; }
    }
}
