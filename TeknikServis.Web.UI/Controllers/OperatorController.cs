using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;

namespace TeknikServis.Web.UI.Controllers
{
    public class OperatorController : Controller
    {
        // GET: Operator
        public ActionResult Index()
        {
            return View(new ArizaRepo().GetAll());
        }
    }
}