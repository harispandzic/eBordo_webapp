using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityModels
{
    public class SkillsStavke
    {
        public int skillsStavkeID { get; set; }
        public float kontrolaLopte { get; set; }
        public int kontrolaLopteZbir { get; set; }

        public float dribljanje { get; set; }
        public int dribljanjeZbir { get; set; }

        public float dodavanje { get; set; }
        public int dodavanjeZbir { get; set; }

        public float kretanje { get; set; }
        public int kretanjeZbir { get; set; }

        public float brzina { get; set; }
        public int brzinaZbir { get; set; }

        public float sut { get; set; }
        public int sutZbir { get; set; }

        public float snaga { get; set; }
        public int snagaZbir { get; set; }

        public float markiranje { get; set; }
        public int markiranjeZbir { get; set; }

        public float klizeciStart { get; set; }
        public int klizeciStartZbir { get; set; }

        public float skok { get; set; }
        public int skokZbir { get; set; }

        public float odbrana { get; set; }
        public int odbranaZbir { get; set; }

        public float zbirOcjena { get; set; }
        public float prosjekOcjena { get; set; }

        public Igrac igrac { get; set; }
        public int? igracID { get; set; }
    }
}
