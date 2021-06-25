using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Povreda
    {
        public int povredaID { get; set; }
        public Igrac igrac { get; set; }
        public  int igracID { get; set; }
        public DateTime datumPovrede { get; set; }
        public DateTime odsustvoDO { get; set; }
        public TipPovrede tipPovrede { get; set; }
        public string kratkiOpis { get; set; }
        public string misljenjeLjekara { get; set; }
    }
}
