using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
