﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeknikServis.Web.UI.Controllers
{
    public class HomeController : Controller
    {

   
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
           
            return View();
        }
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }
    }
}