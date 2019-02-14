using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeknikServis.Web.UI.Controllers
{
    public class PartialController : Controller
    {
        public PartialViewResult NavbarPartial()
        {
            return PartialView("Partial/_NavbarPartial");
        }
        public PartialViewResult FooterPartial()
        {
            return PartialView("Partial/_FooterPartial");
        }
    }
}