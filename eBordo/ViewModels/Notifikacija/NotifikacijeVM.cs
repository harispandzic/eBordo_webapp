using Data.Data;
using Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Notifikacija
{
    public class NotifikacijeVM
    {
        public class Row
        {
            public Row() { }
            public int notifikacijaID { get; set; }
            public string tekstNotifikacije { get; set; }
            public DateTime datumNotifikacije { get; set; }
            public TipNotifikacije tipNotifikacije { get; set; }
            public string poslao { get; set; }
        }
        public List<Row> notifikacije { get; set; }
    }
}
