using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class Događaji
    {
        public int događajiID { get; set; }
        public string Subject { get; set; }
        public DateTime Start { get; set; }
        public string ThemeColor { get; set; }
    }
}
