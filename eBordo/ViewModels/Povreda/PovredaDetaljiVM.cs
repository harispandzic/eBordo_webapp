using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Povreda
{
    public class PovredaDetaljiVM
    {
        public int povredaID { get; set; }
        public string igrac { get; set; }
        public string datumPovrede { get; set; }
        public string odsustvoDO { get; set; }
        public string tipPovrede { get; set; }
        public string kratkiOpis { get; set; }
        public string misljenjeLjekara { get; set; }
    }
}
