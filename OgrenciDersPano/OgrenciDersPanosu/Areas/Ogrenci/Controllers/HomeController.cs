using OgrenciDersPanosu.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OgrenciDersPanosu.Areas.Ogrenci.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Ogrenci/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("index", "Home", new { Area = "" });
        }
    }
}