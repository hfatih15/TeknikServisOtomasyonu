using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Idendity_Models;

namespace TeknikServis.DAL
{
    class MyContext : IdentityDbContext<User>
    {
        public MyContext () : base("name= MyCon")
        {
        }

        public DateTime InstanceDate { get; set; }
    }
}
