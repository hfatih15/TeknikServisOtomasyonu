using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeknikServis.Web.UI.Areas.Admin.ViewModels
{
    public class UpdateUserRoleViewModel
    {
        public string Id { get; set; }
        public List<string> Roles { get; set; }
    }
}