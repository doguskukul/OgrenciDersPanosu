using OgrenciDersPanosu.Controllers;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
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
        public ActionResult OgrenciBilgisi()
        {
            string id = User.Identity.Name;
            if (id != User.Identity.Name)
            {
                return RedirectToAction("OgrenciBilgisi", "Home", new { id = User.Identity.Name });
            }
            var OgrenciNo = User.Identity.Name;
            var ogrenci = dbcontext.Ogrenciler.FirstOrDefault(w => w.OgrenciId == OgrenciNo);
            return View(ogrenci);
        }

        public ActionResult OgrenciNotuListele()
        {
            var OgrenciNo = User.Identity.Name;
            var ogrenci = dbcontext.Ogrenciler.FirstOrDefault(w => w.OgrenciId == OgrenciNo);
            return View(ogrenci);
        }

        public ActionResult DersSecimi()
        {
            List<Ders> MevcutOlmayanDersler = new List<Ders>();
            var dersler = from ders in dbcontext.Dersler select ders;
            var mevcutDersler = from ders in dbcontext.Dersler
                                join not in dbcontext.Notlar on ders.DersId
                                equals not.Ders.DersId
                                where not.Ogrenci.OgrenciId == User.Identity.Name
                                select ders;
            foreach (Ders aDers in dersler)
            {
                MevcutOlmayanDersler.Add(aDers);
            }
            foreach (Ders aDers in dersler)
            {
                foreach (Ders aMevcutDers in mevcutDersler)
                {
                    if (aDers.DersId == aMevcutDers.DersId)
                    {
                        MevcutOlmayanDersler.Remove(aDers);
                    }
                }
            }
            ViewBag.mevcut = mevcutDersler;
            ViewBag.mevcutOlmayan = MevcutOlmayanDersler;
            return View(dersler);
        }
        public ActionResult SecilenDers(string dersId)
        {
            string ogrId = User.Identity.Name;
            string notId = string.Concat(dersId, ogrId);
            Not not = new Not();
            not.Ogrenci.OgrenciId = ogrId;
            not.Ders.DersId = dersId;
            not.NotId = notId;
            dbcontext.Notlar.Add(not);
            dbcontext.SaveChanges();
            return RedirectToAction("DersSecimi");
        }
        public ActionResult SilinecekDers(string dersId)
        {
            string ogrId = User.Identity.Name;
            var not = (from aNot in dbcontext.Notlar join aDers in dbcontext.Dersler on aNot.Ders.DersId equals aDers.DersId join aOgrenci in dbcontext.Ogrenciler on aNot.Ogrenci.OgrenciId equals aOgrenci.OgrenciId where aNot.Ogrenci.OgrenciId == ogrId && aDers.DersId == dersId select aNot).FirstOrDefault();
            dbcontext.Notlar.Remove(not);
            dbcontext.SaveChanges();
            return RedirectToAction("DersSecimi");
        }
    }
}