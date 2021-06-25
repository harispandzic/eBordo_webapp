using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Ugovor
    {
        public int ugovorID { get; set; }
        public DateTime datumPocetka { get; set; }
        public DateTime datumZavrsetka { get; set; }
        public float iznosPlate { get; set; }
        public string napomene { get; set; }
        public int duzinaUgovora { get; set; }

        public Igrac igrac { get; set; }
        public int? igracID { get; set; }

        public Trener trener { get; set; }
        public int? trenerID { get; set; }
        //public string lokacijaDokumenta { get; set; }
    }
}
