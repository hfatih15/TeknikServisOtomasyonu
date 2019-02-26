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
                var tumArizalar = new ArizaRepo().GetAll();
                foreach (var item in tumArizalar)
                {
                    if (item.TeknisyenId == user.Id)
                    {
                        var model2 = new ArizaViewModel()
                        {
                            ArizaId = item.Id,
                            TeknisyenId = item.TeknisyenId

                        };


                        var ariza = new ArizaRepo().GetById(model2.ArizaId);


                        model.ArizaId = ariza.Id;
                        ariza.UrunDurumu = model.UrunDurumu;
                        new ArizaRepo().Update(ariza);
                        break;
                    }

                }


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
        public async System.Threading.Tasks.Task<ActionResult> ArizaBitir(ArizaViewModel model)
        {


            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id);
                var tumArizalar = new ArizaRepo().GetAll();
                foreach (var item in tumArizalar)
                {
                    if (item.TeknisyenId == user.Id)
                    {
                        var model2 = new ArizaViewModel()
                        {
                            ArizaId = item.Id,
                            TeknisyenId = item.TeknisyenId,
                            ArizaBitisTarihi = item.ArizaBitisTarihi,
                            TamirEdildiMi = item.TamirEdildiMi,
                            TeknisyenYorumu = item.TeknisyenYorumu,



                        };


                        var ariza = new ArizaRepo().GetById(model2.ArizaId);


                        ariza.Id = model2.ArizaId;
                        model.ArizaBitisTarihi = DateTime.Now;
                        ariza.TamirEdildiMi = model.TamirEdildiMi;
                        ariza.TeknisyenYorumu = model.TeknisyenYorumu;
                      //  model.TeknisyenId = null;


                        ariza.ArizaBitisTarihi = model.ArizaBitisTarihi;
                    //    ariza.TeknisyenId = model.TeknisyenId;
                    //    user.AtandiMi = false;


                        new ArizaRepo().Update(ariza);


                        //var userStore = NewUserStore();
                        //await userStore.UpdateAsync(user);
                        //userStore.Context.SaveChanges();
                        break;
                    }

                }


            }
            catch (Exception)
            {

                throw;
            }


            return RedirectToAction("ArizaRaporGiris", "Teknisyen");
        }
    }
}