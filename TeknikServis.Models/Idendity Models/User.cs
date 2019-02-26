using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Enums;

namespace TeknikServis.Models.Idendity_Models
{
   public class User : IdentityUser
    {
        [StringLength(50)]
        [Required]
        public string Ad { get; set; }
        [StringLength(50)]
        [Required]
        public string Soyad { get; set; }
        public string ActivationCode { get; set; }
        public string AvatarPath { get; set; }

        public SehirAdi? SehirAdi { get; set; }
        public string Adres { get; set; }
        public bool AtandiMi { get; set; } = false;

    }
}
