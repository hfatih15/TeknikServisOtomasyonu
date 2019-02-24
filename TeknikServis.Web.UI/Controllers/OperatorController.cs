using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        List<SelectListItem> ButunTeknisyenler = new List<SelectListItem>();
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

            var TeknisyenRolu = NewRoleManager().FindByName("Teknisyen").Users.Select(x => x.UserId).ToList();


            var T = new ArizaRepo().GetAll().ToList();

            for (int i = 0; i < TeknisyenRolu.Count; i++)
            {

                var User = NewUserManager().FindById(TeknisyenRolu[i]);

                foreach (var item in T)
                {
                    if (item.TeknisyenId != User.Id)
                    {
                        ButunTeknisyenler.Add(new SelectListItem()
                        {

                            Text = User.Ad + " " + User.Soyad,
                            Value = User.Id
                        });

                    }

                }

            }

            ViewBag.TeknisyenK = ButunTeknisyenler;

            var model = new ArizaViewModel()
            {

                UrunTipi = data.UrunTipi,
                MusteriId = data.MusteriId,
                UrunAdi = data.UrunAdi,
                MusteriYorumu = data.MusteriYorumu,
                UrunResmi = data.UrunResmi,
                FaturaResmi = data.FaturaResmi,
                SehirAdi = data.SehirAdi,
                Adres = data.Adres,
                GarantiDurumu = data.GarantiDurumu,
                ArizaId = data.Id

            };

            return View(model);

        }

        [HttpPost]
        public ActionResult OperatorAtama(ArizaViewModel model)
        {

            try
            {
                var id2 = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id2);
                var ariza = new ArizaRepo().GetById(model.ArizaId);
                if (user != null)
                {

                    ariza.OperatorId = user.Id;
                    new ArizaRepo().Update(ariza);
                    ariza.ArizaKabulEdildiMi = true;

                }

            }
            catch (Exception)
            {


                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Operator Atanması Sırasında Bir Hata Oluştu",
                    ActionName = "Index",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");

            }
            return RedirectToAction("Index");
        }

        public ActionResult TeknisyenAtama(ArizaViewModel model)

        {
            try
            {
                var teknisyen =  NewUserManager().FindById(model.TeknisyenId);
                var ariza = new ArizaRepo().GetById(model.ArizaId);
                if (teknisyen != null)
                {

                    ariza.TeknisyenId = teknisyen.Id;
                    ariza.ArizaTeknisyeneAtandiMi = true;

                    new ArizaRepo().Update(ariza);
                   
                }
                else
                    throw new Exception("Teknisyen atama işlemi sırasında bir hata oluştu !");

            }
            catch (Exception)
            {


                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Teknisyen Ataması Sırasında Bir Hata oluştu !",
                    ActionName = "Index",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");

            }

            return RedirectToAction("Index");

        }
    }
}