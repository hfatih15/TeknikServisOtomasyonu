using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MemberShipTools;

namespace TeknikServis.Web.UI.Controllers
{
    public class OperatorController : Controller
    {
        // GET: Operator
        public ActionResult Index()
        {
            return View(new ArizaRepo().GetAll());
        }

        public ActionResult ArizaDetaySayfasi(int id = 0)
        {
            var data = new ArizaRepo().GetById(id);
            if (data == null)
                RedirectToAction("Index");

            var id2 = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var user = NewUserManager().FindById(id2);



         var model =   new ArizaViewModel()
            {

                UrunTipi = data.UrunTipi,
                 MusteriId=data.MusteriId,
                  UrunAdi=data.UrunAdi ,
                  MusteriYorumu=data.MusteriYorumu ,
                   UrunResmi=data.UrunResmi ,
                   FaturaResmi=data.FaturaResmi ,
                   SehirAdi=data.SehirAdi ,
                    Adres=data.Adres ,
                    GarantiDurumu=data.GarantiDurumu ,
                     ArizaId=data.Id

            };

            return View(model);
 
        }
        [HttpPost]
        public ActionResult OperatorAtama(ArizaViewModel model)
        {
            var id2 = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var user = NewUserManager().FindById(id2);


            var ariza = new ArizaRepo().GetById(model.ArizaId);

            
            
            ariza.OperatorId = user.Id;
            new ArizaRepo().Update(ariza);

     
            return RedirectToAction("Index");
        }
    }
}