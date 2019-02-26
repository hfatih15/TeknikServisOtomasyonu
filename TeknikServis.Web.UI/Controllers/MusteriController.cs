using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TeknikServis.BLL.Helpers;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MemberShipTools;

namespace TeknikServis.Web.UI.Controllers
{
    public class MusteriController : Controller
    {
        // GET: Musteri
        public ActionResult ArizaKayit()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles ="Admin,Musteri")]
        public ActionResult Ekle (ArizaViewModel model)
        {
            try
            {
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id);
                // arıza fotoğrafı
                if (model.UrunFotografi != null &&
                   model.UrunFotografi.ContentLength > 0)
                {
                    var file = model.UrunFotografi;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extName = Path.GetExtension(file.FileName);
                    fileName = StringHelpers.UrlFormatConverter(fileName);
                    fileName += StringHelpers.GetCode();
                    var klasoryolu = Server.MapPath("~/ArizaFoto/");
                    var dosyayolu = Server.MapPath("~/ArizaFoto/") + fileName + extName;

                    if (!Directory.Exists(klasoryolu))
                        Directory.CreateDirectory(klasoryolu);
                    file.SaveAs(dosyayolu);

                    WebImage img = new WebImage(dosyayolu);
                    img.Resize(250, 250, false);
                    img.AddTextWatermark("FK");
                    img.Save(dosyayolu);
                    var oldPath = model.UrunResmi;
                   model.UrunResmi = "/ArizaFoto/" + fileName + extName;

                    System.IO.File.Delete(Server.MapPath(oldPath));
                }

                // arıza fotoğrafı
                if (model.UrunFaturaFotografi != null &&
                   model.UrunFaturaFotografi.ContentLength > 0)
                {
                    var file = model.UrunFaturaFotografi;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extName = Path.GetExtension(file.FileName);
                    fileName = StringHelpers.UrlFormatConverter(fileName);
                    fileName += StringHelpers.GetCode();
                    var klasoryolu = Server.MapPath("~/FaturaFoto/");
                    var dosyayolu = Server.MapPath("~/FaturaFoto/") + fileName + extName;

                    if (!Directory.Exists(klasoryolu))
                        Directory.CreateDirectory(klasoryolu);
                    file.SaveAs(dosyayolu);

                    WebImage img = new WebImage(dosyayolu);
                    img.Resize(250, 250, false);
                    img.AddTextWatermark("FK");
                    img.Save(dosyayolu);
                    var oldPath = model.FaturaResmi;
                    model.FaturaResmi = "/FaturaFoto/" + fileName + extName;

                    System.IO.File.Delete(Server.MapPath(oldPath));
                }

                new ArizaRepo().Insert(new Ariza() {
                     UrunTipi=model.UrunTipi,
                     UrunAdi=model.UrunAdi,
                     MusteriYorumu=model.MusteriYorumu,
                      UrunResmi=model.UrunResmi,
                      FaturaResmi=model.FaturaResmi,
                      SehirAdi=model.SehirAdi,
                      Adres=model.Adres,
                      GarantiDurumu=model.GarantiDurumu,
                      MusteriId=id   
                });
                
  
            }
            catch (Exception)
            {


                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Arıza Eklenmesi Sırasında Bir Hata Oluştu",
                    ActionName = "ArizaKayit",
                    ControllerName = "Musteri",
                    ErrorCode = 404
                };
                return RedirectToAction("Error", "Home");

            }
            return RedirectToAction("ArizaKayit","Musteri");
        }

        public ActionResult ArizaDurumu()
        {
            var musteriid = HttpContext.User.Identity.GetUserId();
            var data = new ArizaRepo().GetAll(x => x.MusteriId == musteriid).ToList();
            return View(data);
        }

    }
}