using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using BonusMvcStok.Models.Entity;
using Microsoft.Ajax.Utilities;

namespace BonusMvcStok.Controllers
{
    public class UrunController : Controller
    {
        DbMvcStokEntities db = new DbMvcStokEntities();
        // GET: Urun
        public ActionResult Index(string p)
        {
            //İlişkili tablolarda silme işlemini kullanmadım.
            // var urunler = db.tblurunler.Where(x=>x.durum==true).ToList();
            var urunler =db.tblurunler.Where(x=> x.durum == true);
            if (!string.IsNullOrEmpty(p))
            {
                urunler = urunler.Where(x=>x.ad.Contains(p) && x.durum==true);
            }
            return View(urunler.ToList());

        }
        [HttpGet]
        public ActionResult YeniUrun()
        {
            List<SelectListItem> ktg =(from x in db.tblkategori.ToList()
                                       select new SelectListItem
                                       {
                                           Text=x.ad,
                                           Value=x.id.ToString()

                                       }).ToList();
            ViewBag.drop = ktg;
            return View();
        }
        [HttpPost]
        public ActionResult YeniUrun(tblurunler t)
        {
            var ktgr = db.tblkategori.Where(x=>x.id==t.tblkategori.id).FirstOrDefault();
            t.tblkategori = ktgr;
            db.tblurunler.Add(t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UrunSil(tblurunler p1)
        {
            //Sil butonuna tıkladığımızda database'de durumu false yapması gerekiyor.
            //Ürünler index'inde kategori kısmı boş.
            var urunbul = db.tblurunler.Find(p1.id);
            urunbul.durum = false;
            db.SaveChanges();                
            return RedirectToAction("Index");
            

        }
        public ActionResult UrunGetir(int id)
        {
            List<SelectListItem> kat = (from x in db.tblkategori.ToList()
                                         select new SelectListItem
                                         {
                                             Text = x.ad,
                                             Value = x.id.ToString()
                                         }).ToList();
            var urn = db.tblurunler.Find(id);
            ViewBag.urunkategori=kat;
            return View("UrunGetir", urn);
        }
        public ActionResult UrunGuncelle(tblurunler p)
        {
            var urun = db.tblurunler.Find(p.id);
            urun.marka = p.marka;
            urun.satisfiyat = p.satisfiyat;
            urun.stok = p.stok;
            urun.alisfiyat = p.alisfiyat;
            urun.ad = p.ad;
            var ktg = db.tblkategori.Where(x => x.id == p.tblkategori.id).FirstOrDefault();
            urun.kategori = ktg.id;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}