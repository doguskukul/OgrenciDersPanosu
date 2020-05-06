using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OgrenciDersPanosu.Areas.Admin.Models;
using OgrenciDersPanosu.Controllers;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static OgrenciDersPanosu.Areas.Admin.Models.RoleModel;

namespace OgrenciDersPanosu.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Roles()
        {
            return View(roleManager.Roles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            if (ModelState.IsValid)
            {
                var result = roleManager.Create(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var role = roleManager.FindById(id);
            if (role != null)
            {
                var result = roleManager.Delete(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Role Bulunamadı" });
            }
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var role = roleManager.FindById(id);
            var members = new List<ApplicationUser>();
            var nonMembers = new List<ApplicationUser>();
            foreach (var user in userManager.Users.ToList())
            {
                var list = userManager.IsInRole(user.Id, role.Name) ?
                    members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public ActionResult Edit(RoleUpdateModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    result = userManager.AddToRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    result = userManager.RemoveFromRole(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Aranılan rol yok." });
        }

        public ActionResult UserList()
        {
            var roles = new List<ApplicationRole>();

            var users = userManager.Users.ToList().Select(i => new UserWithRole
            {
                user = i,
                Roles = userManager.GetRoles(i.Id)
            });
            return View(users);

        }

        public ActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterAdmin(RegisterAdmin model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = model.AdminId;
                user.Name = model.AdminIsim;
                user.Surname = model.AdminSoyisim;
                var result = userManager.Create(user, model.Sifre);

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
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
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("index", "Home", new { Area = "" });


        }



        public ActionResult SelectRoleForRegister()
        {
            return View();
        }

        public ActionResult getUserList()
        {
            var roles = new List<ApplicationRole>();

            var users = userManager.Users.ToList().Select(i => new UserWithRole
            {
                user = i,
                Roles = userManager.GetRoles(i.Id)
            });
            return View(users);
        }
        public ActionResult DersAta()
        {
            return View();
        }
        public ActionResult OgretmeniAta(string ogretmenId, string dersId)
        {
            var updateDers = dbcontext.Dersler.FirstOrDefault(i => i.DersId == dersId);
            if (updateDers != null)
            {
                updateDers.Ogretmen.OgretmenId = ogretmenId;
                dbcontext.SaveChanges();
            }
            return Redirect("DersAta");
        }
    }
}