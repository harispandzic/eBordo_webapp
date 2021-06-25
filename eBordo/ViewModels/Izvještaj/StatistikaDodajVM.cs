using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eBordo.ViewModels.Izvještaj
{
    public class StatistikaDodajVM
    {
        public int utakmicaID { get; set; }
        public int igracID { get; set; }
        public List<SelectListItem> igraci { get; set; }

        //statistika
        [Required]
        public int minute { get; set; }
        [Required]
        public int golovi { get; set; }
        [Required]
        public int asistencije { get; set; }
        [Required]
        public int zutiKartoni { get; set; }
        [Required]
        public int crveniKartoni { get; set; }
        [Required]
        public int ocjena { get; set; }
        [Required]
        public string komentar{ get; set; }

        //ocjene
        [Required]
        public int kontrolaLopte { get; set; }
        [Required]
        public int dribljanje { get; set; }
        [Required]
        public int dodavanje { get; set; }
        [Required]
        public int kretanje { get; set; }
        [Required]
        public int brzina { get; set; }
        [Required]
        public int sut { get; set; }
        [Required]
        public int snaga { get; set; }
        [Required]
        public int markiranje { get; set; }
        [Required]
        public int klizeciStart { get; set; }
        [Required]
        public int skok { get; set; }
        [Required]
        public int odbrana { get; set; }
        [Required]
        public float prosjecnaOcjena { get; set; }
    }
}
