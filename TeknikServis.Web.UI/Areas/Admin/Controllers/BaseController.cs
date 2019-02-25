using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Identity;
using TeknikServis.Models.Enums;

namespace TeknikServis.Web.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BaseController : Controller
    {
        protected List<SelectListItem> GetUserList()
        {
            var data = new List<SelectListItem>();
            MemberShipTools.NewUserStore().Users
                .ToList()
                .ForEach(x =>
                {
                    data.Add(new SelectListItem()
                    {
                        Text = $"{x.Ad} {x.Soyad}",
                        Value = x.Id
                    });
                });
            return data;
        }

        protected List<SelectListItem> GetRoleList()
        {
            var data = new List<SelectListItem>();
            MemberShipTools.NewRoleStore().Roles
                .ToList()
                .ForEach(x =>
                {
                    data.Add(new SelectListItem()
                    {
                        Text = $"{x.Name}",
                        Value = x.Id
                    });
                });
            return data;
        }


    
    }
}