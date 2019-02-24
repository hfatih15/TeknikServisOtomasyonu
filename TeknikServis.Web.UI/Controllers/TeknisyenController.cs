using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;
using TeknikServis.Models.Idendity_Models;
using static TeknikServis.BLL.Identity.MemberShipTools;

namespace TeknikServis.Web.UI.Controllers
{
    public class TeknisyenController : Controller
    {
        // GET: Teknisyen

            
        public ActionResult ArizaRaporGiris()
        {
            return View();
        }
    }
}