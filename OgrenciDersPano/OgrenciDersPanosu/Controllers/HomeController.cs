using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OgrenciDersPanosu.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public HomeController()
        {
            userManager.PasswordValidator = new PasswordValidator()
            {
                RequireDigit = true,
                RequiredLength = 7,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonLetterOrDigit = true
            };
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        // GET: Home
        public ActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            else if (User.IsInRole("Ogrenci"))
            {
                return RedirectToAction("index", "Home", new { Area = "Ogrenci" });
            }
            else if (User.IsInRole("Ogretmen"))
            {
                return RedirectToAction("index", "Home", new { Area = "Ogretmen" });
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LoginAdmin(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            string name = User.Identity.Name;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LoginAdmin(LoginAdmin model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Find(model.AdminId, model.Sifre);
                if (user != null)
                {
                    var roles = userManager.GetRoles(user.Id);

                    if (roles.Count(i => i == "admin") != 0)
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;

                        var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                        var authProperties = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };
                        authManager.SignOut();
                        authManager.SignIn(authProperties, identity);
                        return RedirectToAction("index", "home", new { area = "Admin" });
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LoginOgrenci(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            string name = User.Identity.Name;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LoginOgrenci(LoginOgrenci model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Find(model.OgrenciNo, model.Sifre);
                if (user != null)
                {
                    var roles = userManager.GetRoles(user.Id);

                    if (roles.Count(i => i == "Ogrenci") != 0)
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;

                        var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                        var authProperties = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };
                        authManager.SignOut();
                        authManager.SignIn(authProperties, identity);

                        return RedirectToAction("index", "home", new { area = "Ogrenci" });
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LoginOgretmen(string returnUrl)
        {
            string name = User.Identity.Name;
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LoginOgretmen(LoginOgretmen model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var user = userManager.Find(model.OgretmenId, model.Sifre);
                if (user != null)
                {
                    var roles = userManager.GetRoles(user.Id);

                    if (roles.Count(i => i == "Ogretmen") != 0)
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;

                        var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                        var authProperties = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };
                        authManager.SignOut();
                        authManager.SignIn(authProperties, identity);
                        return RedirectToAction("index", "home", new { area = "Ogretmen" });
                    }
                    else
                    {
                        ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Yanlış Kullanıcı Adı veya Şifre");
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult RegisterOgrenci()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterOgrenci(RegisterOgrenci model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = model.OgrenciNo;
                user.Name = model.OgrenciIsim;
                user.Surname = model.OgrenciSoyisim;
                var result = userManager.Create(user, model.Sifre);

                if (result.Succeeded)
                {
                    OgrenciModel aOgrenci = new OgrenciModel();
                    aOgrenci.Ad = model.OgrenciIsim;
                    aOgrenci.Soyad = model.OgrenciSoyisim;
                    aOgrenci.OgrenciId = model.OgrenciNo;
                    dbcontext.Ogrenciler.Add(aOgrenci);
                    dbcontext.SaveChanges();
                    
                    userManager.AddToRole(user.Id, "Ogrenci");
                    return RedirectToAction("Index", new { id = User.Identity.Name });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }
        public ActionResult RegisterOgretmen()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterOgretmen(RegisterOgretmen model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = model.OgretmenId;
                user.Name = model.OgretmenIsim;
                user.Surname = model.OgretmenSoyisim;
                var result = userManager.Create(user, model.Sifre);

                if (result.Succeeded)
                {
                    OgretmenModel aOgretmen = new OgretmenModel();
                    aOgretmen.Ad = model.OgretmenIsim;
                    aOgretmen.Soyad = model.OgretmenSoyisim;
                    aOgretmen.OgretmenId = model.OgretmenId;
                    dbcontext.Ogretmenler.Add(aOgretmen);
                    dbcontext.SaveChanges();
                    userManager.AddToRole(user.Id, "Ogretmen");
                    return RedirectToAction("Index", new { id = User.Identity.Name });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }
        public ActionResult SelectRoleForRegister()
        {
            return View();
        }
    }
}