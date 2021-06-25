using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Trening
    {
        public int treningID { get; set; }

        public DateTime datumOdrzavanja { get; set; }
        public string satnica{ get; set; }
        public LokacijaTreninga lokacija{ get; set; }
        public string fokusTreninga{ get; set; }

        public StatusUtakmice statusTreninga{ get; set; }
        public Trener zabiljezio { get; set; }
        public int zabiljezioID { get; set; }
    }
}
