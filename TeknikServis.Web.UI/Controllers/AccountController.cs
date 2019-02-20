using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static TeknikServis.BLL.Identity.MemberShipTools;
using TeknikServis.Models.ViewModels;
using System.Threading.Tasks;
using TeknikServis.Models.Idendity_Models;
using Microsoft.AspNet.Identity;
using System.IO;
using TeknikServis.BLL.Helpers;
using System.Web.Helpers;
using TeknikServis.BLL.Services.Senders;
using Admin.Models.ViewModels;
using TeknikServis.BLL.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace TeknikServis.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                var userStore = NewUserStore();
                var userManager = NewUserManager();
                var roleManager = NewRoleManager();

                var rm = model.RegisterViewModel;

                var user = await userManager.FindByNameAsync(rm.UserName);
                if (user != null)
                {
                    ModelState.AddModelError("UserName", "Bu kullanıcı adı daha önceden alınmış !");
                    //return View("Index", model);
                    TempData["Model"] = new ErrorViewModel()
                    {
                        Text = $"Bu kullanıcı adı daha önce alınmıştır !",
                        ActionName = "Index",
                        ControllerName = "Account",
                        ErrorCode = 404
                    };
                    return RedirectToAction("Error", "Home");

                }
                var YeniKullanici = new User()
                {
                    UserName = rm.UserName,
                    Email = rm.Email,
                    Ad = rm.Ad,
                    Soyad = rm.Soyad,
                    ActivationCode = StringHelpers.GetCode()                    
                };

                var result = await userManager.CreateAsync(YeniKullanici, rm.Password);

                if (result.Succeeded)
                {
                    if (userStore.Users.Count() == 1)
                    {
                        await userManager.AddToRoleAsync(YeniKullanici.Id, "Admin");                      
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(YeniKullanici.Id, "Musteri");
                    }

                    string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                     (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    var emailService = new EmailService();
                    var body = $"Merhaba <b>{YeniKullanici.Ad} {YeniKullanici.Soyad}</b><br>Hesabınızı onaylamak yada aktif etmek için aşağıdaki linke tıklayınız !<br> <a href='{SiteUrl}/account/activation?code={YeniKullanici.ActivationCode}'> Hesabınızı aktifleştimek için tıklayınız !</a> ";
                    await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Aktivasyon Kodu"}, YeniKullanici.Email);

                }
                else
                {
                    var err = "";
                    foreach (var ResultErrors in result.Errors)
                    {
                        err = err + ResultErrors + " ";
                    }

                    ModelState.AddModelError("", err);
                    return View("Index", model);
                }

                TempData["Message"] = "Kaydınız tamamlanmıştır. Üyelik bilgilerinizle giriş yapabilirsiniz.";
                return RedirectToAction("Index");
                
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Kayıt Sırasında Bir Hata Oluştu",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 404
                };
                return RedirectToAction("Error", "Home");

            }


        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(RegisterLoginViewModel model)
        {

            if (!ModelState.IsValid)
                {
                    return View("Index", model);
                }

            try
            {          
                var userManager = NewUserManager();
                var user = await userManager.FindAsync(model.LoginViewModel.UserName, model.LoginViewModel.Password);

                if(user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı şifre girişi yaptınız !");
                    return View("Index", model);
                }
                var authManager = HttpContext.GetOwinContext().Authentication;

                var userIdendity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                authManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties()
                {
                    IsPersistent = model.LoginViewModel.RememberMe
                }, userIdendity);

                return RedirectToAction("Index", "Home");

            }
            catch (Exception)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Giriş Sırasında Bir Hata Oluştu",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 404
                };
                return RedirectToAction("Error", "Home");
            }

        }

        public ActionResult GirisSayfasi()
        {
            return RedirectToAction("Index", "Account");
        }


        [HttpGet]
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index", "Account");

        }

        [HttpGet]
        public ActionResult UserProfile()
        {

            try
            {

                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id);
                var data = new ProfilePasswordViewModel()
                {
                    UserProfileViewModel = new UserProfileViewModel()
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Ad = user.Ad,
                        TelefonNO = user.PhoneNumber,
                        Soyad = user.Soyad,
                        UserName = user.UserName,
                        AvatarPath = string.IsNullOrEmpty(user.AvatarPath) ? "/assets/img/avatars/avatar3.jpg" : user.AvatarPath
                    }
                };
                return View(data);
            }
            catch (Exception ex)
            {

                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Giriş Sırasında Bir Hata Oluştu",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 404
                };
                return RedirectToAction("Error", "Home");
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProfile(ProfilePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            try
            {
                var userManager = NewUserManager();
                var user = await userManager.FindByIdAsync(model.UserProfileViewModel.Id);

                user.Ad = model.UserProfileViewModel.Ad;
                user.Soyad = model.UserProfileViewModel.Soyad;
                user.PhoneNumber = model.UserProfileViewModel.TelefonNO;
                if (user.Email != model.UserProfileViewModel.Email)
                {
                    //todo tekrar aktivasyon maili gönderilmeli. rolü de aktif olmamış role çevrilmeli.
                }
                user.Email = model.UserProfileViewModel.Email;

                if (model.UserProfileViewModel.PostedFile != null &&
                    model.UserProfileViewModel.PostedFile.ContentLength > 0)
                {
                    var file = model.UserProfileViewModel.PostedFile;
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
                    img.Resize(45, 45, false);
                  //  img.AddTextWatermark("Wissen");
                    img.Save(dosyayolu);
                    var oldPath = user.AvatarPath;
                    user.AvatarPath = "/Upload/" + fileName + extName;

                    System.IO.File.Delete(Server.MapPath(oldPath));
                }


                await userManager.UpdateAsync(user);
                TempData["Message"] = "Güncelleme işlemi başarılı";
                return RedirectToAction("UserProfile");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "UserProfile",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ProfilePasswordViewModel model)
        {
            try
            {
                var userManager = NewUserManager();
                var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
                var user = NewUserManager().FindById(id);
                var data = new ProfilePasswordViewModel()
                {
                    UserProfileViewModel = new UserProfileViewModel()
                    {
                        Email = user.Email,
                        Id = user.Id,
                        Ad = user.Ad,
                        TelefonNO = user.PhoneNumber,
                        Soyad = user.Soyad,
                        UserName = user.UserName
                    }
                };
                model.UserProfileViewModel = data.UserProfileViewModel;
                if (!ModelState.IsValid)
                {
                    model.ChangePasswordViewModel = new ChangePasswordViewModel();
                    return View("UserProfile", model);
                }


                var result = await userManager.ChangePasswordAsync(
                    HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId(),
                    model.ChangePasswordViewModel.OldPassword, model.ChangePasswordViewModel.NewPassword);

                if (result.Succeeded)
                {
                    //todo kullanıcıyı bilgilendiren bir mail atılır
                    return RedirectToAction("Logout", "Account");
                }
                else
                {
                    var err = "";
                    foreach (var resultError in result.Errors)
                    {
                        err += resultError + " ";
                    }
                    ModelState.AddModelError("", err);
                    model.ChangePasswordViewModel = new ChangePasswordViewModel();
                    return View("UserProfile", model);
                }
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "UserProfile",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Activation (string code)
        {
            try
            {
                var userStore = NewUserStore();
                var user = userStore.Users.FirstOrDefault(x => x.ActivationCode == code);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        ViewBag.Message = $"<span class='alert alert-success'>Bu hesap daha önce aktive edilmiştir.</span>";
                        
                    }
                    else
                    {
                        user.EmailConfirmed = true;

                        userStore.Context.SaveChanges();
                        ViewBag.Message = $"<span class='alert alert-success'>Aktivasyon işleminiz başarılı</span>";
                    }
                }
                else
                {
                    ViewBag.Message = $"<span class='alert alert-danger'>Aktivasyon başarısız</span>";
                }
            }
            catch (Exception)
            {

                ViewBag.Message = "<span class='alert alert-danger'>Aktivasyon işleminde bir hata oluştu</span>";
            }
            return View();
            
        

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            try
            {
                var userStore = NewUserStore();
                var userManager = NewUserManager();
                var user = await userStore.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, $"{model.Email} mail adresine kayıtlı bir üyeliğe erişilemedi");
                    return View(model);
                }

                var newPassword = StringHelpers.GetCode().Substring(0, 6);
                await userStore.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(newPassword));
                var result = userStore.Context.SaveChanges();
                if (result == 0)
                {
                    TempData["Model"] = new ErrorViewModel()
                    {
                        Text = $"Bir hata oluştu",
                        ActionName = "RecoverPassword",
                        ControllerName = "Account",
                        ErrorCode = 500
                    };
                    return RedirectToAction("Error", "Home");
                }

                var emailService = new EmailService();
                var body = $"Merhaba <b>{user.Ad} {user.Soyad}</b><br>Hesabınızın parolası sıfırlanmıştır<br> Yeni parolanız: <b>{newPassword}</b> <p>Yukarıdaki parolayı kullanarak sistemize giriş yapabilirsiniz.</p>";
                emailService.Send(new IdentityMessage() { Body = body, Subject = $"{user.UserName} Şifre Kurtarma" }, user.Email);

                //TempData["Message"] = "Yeni E-Mail adresinize gönderilmiştir.";
            }
             

            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "RecoverPassword",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }


            return View();
        }

      
    }
}