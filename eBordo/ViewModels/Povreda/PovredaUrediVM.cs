using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Povreda
{
    public class PovredaUrediVM
    {
        public int povredaID { get; set; }
        public DateTime datumPovrede { get; set; }
        public DateTime odsustvo { get; set; }
        public string tipPovrede { get; set; }
        public string kratkiOpis { get; set; }
        public string misljenjeLjekara { get; set; }
        public string odabraniIgrac{ get; set; }
    }
}
