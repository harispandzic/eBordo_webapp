using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.EntityModels
{
    public enum TrenerskaLicenca
    {
        [Display(Name = "UEFA A Licenca")]
        UEFA_A,
        [Display(Name = "UEFA B Licenca")]
        UEFA_B,
        [Display(Name = "UEFA Pro Licenca")]
        UEFA_Pro
    }
}
