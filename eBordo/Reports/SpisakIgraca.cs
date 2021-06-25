using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TmpReportDesigner
{
    public class SpisakIgraca
    {
        public string brojDresa { get; set; }
        public string imePrezime { get; set; }
        public string drzavljanstvo { get; set; }
        public string gradRodjenja { get; set; }
        public float trzisnaVrijednost { get; set; }
        public int godine { get; set; }
        public string pozicija { get; set; }
        public double ocjena { get; set; }
        public string istekUgovora { get; set; }

        public static List<SpisakIgraca> get()
        {
            return new List<SpisakIgraca> { };
        }
    }
}