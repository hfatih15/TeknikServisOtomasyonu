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

namespace TeknikServis.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
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
                    return View("Index", model);
                }
                var YeniKullanici = new User()
                {
                    UserName = rm.UserName,
                    Email = rm.Email,
                    Ad = rm.Ad,
                    Soyad = rm.Soyad
                };

                var result = await userManager.CreateAsync(YeniKullanici, rm.Password);

                if (result.Succeeded)
                {
                    if (userStore.Users.Count() == 1)
                    {
                        await userManager.AddToRoleAsync(YeniKullanici.Id, "Admin");
                        //Mail gelicek
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(YeniKullanici.Id, "Musteri");
                    }
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


    }
}