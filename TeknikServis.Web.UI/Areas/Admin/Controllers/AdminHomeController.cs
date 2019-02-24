using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.Models.Idendity_Models;
using TeknikServis.BLL.Identity;
using System.Threading.Tasks;
using TeknikServis.BLL.Helpers;
using TeknikServis.BLL.Services.Senders;
using Microsoft.AspNet.Identity;
using TeknikServis.Models.ViewModels;
using System.IO;
using System.Web.Helpers;
using TeknikServis.Web.UI.Areas.Admin.ViewModels;
using TeknikServis.Web.UI.Areas.Admin.Models;
using static TeknikServis.BLL.Identity.MemberShipTools;

namespace TeknikServis.Web.UI.Areas.Admin.Controllers
{
    public class AdminHomeController : BaseController
    {

        // GET: Admin
        public ActionResult Index()
        {
            return View(NewUserStore().Users.ToList());
        }

        [HttpPost]
        public async Task<JsonResult> SendCode(string id)
        {
            try
            {
                var userStore = NewUserStore();
                var user = await userStore.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }
                if (user.EmailConfirmed)
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı zaten epostasını onaylamış",
                        success = false
                    });

                user.ActivationCode = StringHelpers.GetCode();
                await userStore.UpdateAsync(user);
                userStore.Context.SaveChanges();
                string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                var emailService = new EmailService();
                var body = $"Merhaba <b>{user.Ad} {user.Soyad}</b><br>Hesabınızı aktif etmek için aşadıdaki linke tıklayınız<br> <a href='{SiteUrl}/account/activation?code={user.ActivationCode}' >Aktivasyon Linki </a> ";
                await emailService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = "Sitemize Hoşgeldiniz"
                }, user.Email);
                return Json(new ResponseData()
                {
                    message = "Kullanıcıya yeni aktivasyon maili gönderildi",
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu: {ex.Message}",
                    success = false
                });
            }
        }
        [HttpPost]
        public async Task<JsonResult> SendPassword(string id)
        {
            try
            {
                var userStore = NewUserStore();
                var user = await userStore.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }

                var newPassword = StringHelpers.GetCode().Substring(0, 6);
                await userStore.SetPasswordHashAsync(user, NewUserManager().PasswordHasher.HashPassword(newPassword));
                await userStore.UpdateAsync(user);
                userStore.Context.SaveChanges();

                string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                var emailService = new EmailService();
                var body = $"Merhaba <b>{user.Ad} {user.Soyad}</b><br>Hesabınızın parolası sıfırlanmıştır<br> Yeni parolanız: <b>{newPassword}</b> <p>Yukarıdaki parolayı kullanarak sistemize giriş yapabilirsiniz.</p>";
                emailService.Send(new IdentityMessage() { Body = body, Subject = $"{user.UserName} Şifre Kurtarma" }, user.Email);

                return Json(new ResponseData()
                {
                    message = "Şifre sıfırlama maili gönderilmiştir",
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu: {ex.Message}",
                    success = false
                });
            }
        }
        [HttpGet]
        public ActionResult EditUser(string id)
        {
            try
            {
                var user = NewUserManager().FindById(id);
                if (user == null)
                    return RedirectToAction("Index");

                var roller = GetRoleList();
                foreach (var role in user.Roles)
                {
                    foreach (var selectListItem in roller)
                    {
                        if (selectListItem.Value == role.RoleId)
                            selectListItem.Selected = true;
                    }
                }

                ViewBag.RoleList = roller;


                var model = new UserProfileViewModel()
                {
                    AvatarPath = user.AvatarPath,
                    Ad = user.Ad,
                    Email = user.Email,
                    Soyad = user.Soyad,
                    Id = user.Id,
                    TelefonNO = user.PhoneNumber,
                    UserName = user.UserName,
                    Adres=user.Adres,
                    SehirAdi=user.SehirAdi
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Admin",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userManager = NewUserManager();
                var user = await userManager.FindByIdAsync(model.Id);

                user.Ad = model.Ad;
                user.Soyad = model.Soyad;
                user.PhoneNumber = model.TelefonNO;
                user.Email = model.Email;
                user.Adres = model.Adres;
                user.SehirAdi = model.SehirAdi;

                if (model.PostedFile != null &&
                    model.PostedFile.ContentLength > 0)
                {
                    var file = model.PostedFile;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extName = Path.GetExtension(file.FileName);
                    fileName = StringHelpers.UrlFormatConverter(fileName);
                    fileName += StringHelpers.GetCode();
                    var klasoryolu = Server.MapPath("~/Upload/");
                    var dosyayolu = Server.MapPath("~/Upload/") + fileName + extName;

                    if (!Directory.Exists(klasoryolu))
                        Directory.CreateDirectory(klasoryolu);
                    file.SaveAs(dosyayolu);

                    WebImage img = new WebImage(dosyayolu);
                    img.Resize(250, 250, false);
                    img.AddTextWatermark("Wissen");
                    img.Save(dosyayolu);
                    var oldPath = user.AvatarPath;
                    user.AvatarPath = "/Upload/" + fileName + extName;

                    System.IO.File.Delete(Server.MapPath(oldPath));
                }
                await userManager.UpdateAsync(user);
                TempData["Message"] = "Güncelleme işlemi başarılı";
                return RedirectToAction("EditUser", new { id = user.Id });
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Admin",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserRoles(UpdateUserRoleViewModel model)
        {
            //var userId = Request.Form[1].ToString();
            //var rolIdler = Request.Form[2].ToString().Split(',');
            var userId = model.Id;
            var rolIdler = model.Roles;
            var roleManager = NewRoleManager();
            var seciliRoller = new string[rolIdler.Count];
            for (var i = 0; i < rolIdler.Count; i++)
            {
                var rid = rolIdler[i];
                seciliRoller[i] = roleManager.FindById(rid).Name;
            }

            var userManager = NewUserManager();
            var user = userManager.FindById(userId);

            foreach (var identityUserRole in user.Roles.ToList())
            {
                userManager.RemoveFromRole(userId, roleManager.FindById(identityUserRole.RoleId).Name);
            }

            for (int i = 0; i < seciliRoller.Length; i++)
            {
                userManager.AddToRole(userId, seciliRoller[i]);
            }

            return RedirectToAction("EditUser", new { id = userId });
        }

       

    }
}