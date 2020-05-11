using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OgrenciDersPanosu.Controllers;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OgrenciDersPanosu.Areas.Ogretmen.Controllers
{
    [Authorize(Roles = "Ogretmen")]
    public class HomeController : BaseController
    {
        // GET: Ogretmen/Home
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

        public ActionResult OgrenciNotlariniGoruntule(string dersId)
        {
            var notlar = (from not in dbcontext.Notlar where not.Ders.DersId == dersId select not).ToList();
            var ogretmen = dbcontext.Ogretmenler.FirstOrDefault(i => i.OgretmenId == User.Identity.Name);
            if (ogretmen.Dersler.Count(i => i.DersId == dersId) == 0)
            {
                return RedirectToAction("DersListele", "Home");
            }

            return View(notlar);
        }
        public ActionResult DersListele()
        {
            string id = User.Identity.Name;
            OgretmenModel aUser = dbcontext.Ogretmenler.Find(User.Identity.Name);
            return View(aUser);
        }
        public ActionResult OgrenciNotlariniGuncelle(string notId, string dersId)
        {
            Not not = dbcontext.Notlar.Find(notId);
            Ders ders = dbcontext.Dersler.Find(dersId);

            if (ders.Notlar.Count(i => i.NotId == notId) == 0)
            {
                return RedirectToAction("DersListele", "Home");
            }

            return View(not);
        }
        [HttpPost]
        public ActionResult OgrenciNotlariniGuncelle(Not model)
        {
            var notupdate = dbcontext.Notlar.FirstOrDefault(i => i.NotId == model.NotId);
            Ders ders = dbcontext.Dersler.Find(notupdate.DersId);
            if (notupdate != null)
            {
                notupdate.Sinav1 = model.Sinav1;
                notupdate.Sinav2 = model.Sinav2;
                notupdate.Sinav3 = model.Sinav3;
                notupdate.Sozlu1 = model.Sozlu1;
                notupdate.Sozlu2 = model.Sozlu2;
                notupdate.Sozlu3 = model.Sozlu3;
                dbcontext.SaveChanges();
            }
            return RedirectToAction("OgrenciNotlariniGoruntule", new { dersId = notupdate.DersId });
        }

        public ActionResult DersOlustur()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DersOlustur(Ders model)
        {
            if (ModelState.IsValid)
            {
                OgretmenModel ogretmen = dbcontext.Ogretmenler.Find(User.Identity.Name);
                Ders ders = new Ders();
                ders.DersId = model.DersId;
                ders.DersAdi = model.DersAdi;
                ders.Ogretmen = ogretmen;
                ders.OgretmenId = ogretmen.OgretmenId;
                dbcontext.Dersler.Add(ders);
                dbcontext.SaveChanges();
            }
            return View(model);
        }

        public ActionResult Derslik(string dersId)
        {
            Ders ders = dbcontext.Dersler.Find(dersId);
            var ogretmen = dbcontext.Ogretmenler.FirstOrDefault(i => i.OgretmenId == User.Identity.Name);
            if (ogretmen.Dersler.Count(i => i.DersId == dersId) == 0)
            {
                return RedirectToAction("DersListele", "Home");
            }
            return View(ders);
        }

        [ValidateInput(false)]
        public ActionResult Gonderi_Yap(string dersId, string text)
        {
            if (ModelState.IsValid)
            {
                Derslik_Gonderi gonderi = new Derslik_Gonderi();
                int id;
                if (dbcontext.Gonderiler.Count() != 0)
                {
                    var son_gonderi = dbcontext.Gonderiler.OrderByDescending(w => w.zaman).First();  //zamana göre son gönderiyi belirleme
                    id = int.Parse(son_gonderi.GonderiId) + 1;                               //id son gönderinin id sinin 1 fazlası olmalı
                }
                else
                {
                    id = 0;
                }
                gonderi.GonderiId = id.ToString();
                gonderi.Gonderi = text;
                gonderi.zaman = DateTime.Now;
                OgretmenModel ogretmen = dbcontext.Ogretmenler.Find(User.Identity.Name);
                gonderi.gonderenIsmi = ogretmen.Ad + " " + ogretmen.Soyad;
                Ders ders = dbcontext.Dersler.Find(dersId);
                ders.Gonderiler.Add(gonderi);
                dbcontext.Gonderiler.Add(gonderi);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Derslik", "Home", new { dersId = dersId });
        }
    }
}