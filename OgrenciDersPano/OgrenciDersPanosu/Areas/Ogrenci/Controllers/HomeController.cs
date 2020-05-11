﻿using OgrenciDersPanosu.Controllers;
using OgrenciDersPanosu.identity;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
            var ogrenci = dbcontext.Ogrenciler.Find(OgrenciNo);
            return View(ogrenci);
        }

        public ActionResult DersSecimi()
        {
            List<Ders> MevcutOlmayanDersler = new List<Ders>();
            var dersler = dbcontext.Dersler.ToList();
            OgrenciModel ogrenci = dbcontext.Ogrenciler.Find(User.Identity.Name);
            var notlar = dbcontext.Notlar.Where(i => i.Ogrenci.OgrenciId == ogrenci.OgrenciId);
            IQueryable<Ders> mevcutDersler = Enumerable.Empty<Ders>().AsQueryable();
            foreach (Not not in notlar) {
                mevcutDersler = dbcontext.Dersler.Where(i => i.DersId == not.DersId);
            }
            
            ViewBag.mevcut = mevcutDersler.ToList() ;
            return View(dersler);
        }
        public ActionResult SecilenDers(string dersId)
        {
            string ogrId = User.Identity.Name;
            string notId = string.Concat(dersId, ogrId);
            OgrenciModel ogrenci = dbcontext.Ogrenciler.Find(ogrId);
            Ders ders = dbcontext.Dersler.Find(dersId);
            Not not = new Not();
            not.Ogrenci = ogrenci;
            not.OgrenciId = ogrenci.OgrenciId;
            not.Ders = ders;
            not.DersId = ders.DersId;
            not.NotId = notId;
            dbcontext.Notlar.Add(not);
            dbcontext.SaveChanges();
            return RedirectToAction("DersSecimi");
        }
        public ActionResult SilinecekDers(string dersId)
        {
            string ogrId = User.Identity.Name;
            Ders ders = dbcontext.Dersler.Find(dersId);
            foreach (var not in dbcontext.Notlar.Where(o => o.DersId == dersId && o.OgrenciId == ogrId))
            dbcontext.Notlar.Remove(not);
            dbcontext.SaveChanges();
            return RedirectToAction("DersSecimi");
        }

        public ActionResult Derslik(string dersId)
        {
            Ders ders = dbcontext.Dersler.Find(dersId);
            var ogrenci = dbcontext.Ogrenciler.FirstOrDefault(i => i.OgrenciId == User.Identity.Name);
            if (ogrenci.Notlar.Count(i => i.DersId == dersId) == 0)
            {
                return RedirectToAction("OgrenciBilgisi", "Home");
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
                if(dbcontext.Gonderiler.Count() != 0)
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
                OgrenciModel ogrenci = dbcontext.Ogrenciler.Find(User.Identity.Name);
                gonderi.gonderenIsmi = ogrenci.Ad + " " + ogrenci.Soyad;
                Ders ders = dbcontext.Dersler.Find(dersId);
                ders.Gonderiler.Add(gonderi);
                dbcontext.Gonderiler.Add(gonderi);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Derslik", "Home", new { dersId = dersId});
        }
    }
}