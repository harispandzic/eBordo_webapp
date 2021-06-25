using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.EntityModels
{
    public class Drzava
    {
        public int drzavaID { get; set; }
        public string nazivDrzave { get; set; }
        public string zastavaDrzave { get; set; }
    }
}
