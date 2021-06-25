using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Grad
    {
        public int gradID { get; set; }
        public string nazivGrada { get; set; }

        public int drzavaID { get; set; }
        public Drzava drzava { get; set; }
    }
}
