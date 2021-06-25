using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Povreda
{
    public class PovredaPrikazVM
    {
        public class Row
        {
            public int povredaId { get; set; }
            public string igrac { get; set; }
            public string datumPovrede { get; set; }
            public string odsustvo { get; set; }
            public int brojDana { get; set; }
            public string tipPovrede { get; set; }
            public string kratkiOpis { get; set; }
            public string pozicija { get; set; }
            public string slika { get; set; }
            public bool oporavljen { get; set; }
        }
        public List<Row> povrede { get; set; }
        public int total { get; set; }
    }
}
