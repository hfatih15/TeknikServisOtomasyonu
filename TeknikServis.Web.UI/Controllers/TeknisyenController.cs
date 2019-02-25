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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teknisyen")]
        public ActionResult ArizaGuncelle(ArizaViewModel model)
        {


            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id);
                var ariza = new ArizaRepo().GetById(model.TeknisyenId);
                var model2 = new ArizaViewModel()
                {
                    ArizaId = ariza.Id,
                    TeknisyenId=ariza.TeknisyenId

                };
                
                model.ArizaId = ariza.Id;
              ariza.UrunDurumu = model.UrunDurumu;
                new ArizaRepo().Update(ariza);
            }
            catch (Exception)
            {

                throw;
            }


            return RedirectToAction("ArizaRaporGiris", "Teknisyen");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Teknisyen")]
        public ActionResult ArizaBitir()
        {
            return View();
        }
    }
}