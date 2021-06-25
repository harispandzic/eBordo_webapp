using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class IgracSkills
    {
        public int igracSkillsID { get; set; }
        //public double prosjecnaOcjena { get; set; }

        public SkillsStavke skillsStavke { get; set; }
        public int? skillsStavkeID { get; set; }

        public Igrac igrac { get; set; }
        public int? igracID { get; set; }
    }
}
