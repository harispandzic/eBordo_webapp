using Data.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Notifikacija
    {
        public int notifikacijaID { get; set; }
        public string tekstNotifikacije { get; set; }
        public DateTime datumNotifikacije { get; set; }
        public StatusNotifikacije statusNotifikacije { get; set; }
        public TipNotifikacije tipNotifikacije { get; set; }
        public bool poruka { get; set; }
        public string posiljaoc { get; set; }
    }
}
