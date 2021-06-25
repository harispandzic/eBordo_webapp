using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.EntityModels
{
    public class Korisnik:IdentityUser
    {
        public Korisnik()
        {
            notifikacije = new List<Notifikacija>();
        }
        public string ime { get; set; }
        public string prezime { get; set; }
        public DateTime datumRodjenja { get; set; }
        public string adresaPrebivalista { get; set; }
        public string telefon { get; set; }
        public string emailAdresa { get; set; }

        public Drzava drzavljanstvo { get; set; }
        public int drzavljanstvoID { get; set; }

        public Grad gradRodjenja { get; set; }
        public int gradRodjenjaID { get; set; }

        //public bool isIgrac => this is Igrac;

        public Igrac igrac { get; set; }
        public int igracID { get; set; }

        public Trener trener { get; set; }
        public int trenerID { get; set; }

        public bool isIgrac { get; set; }
        public bool isTrener { get; set; }
        public bool isAdmin { get; set; }

        public string imePrezime => ime + " " + prezime;
        public ICollection<Notifikacija> notifikacije { get; set; }
    }
}
