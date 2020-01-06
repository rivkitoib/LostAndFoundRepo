using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LostAndFound.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        DbHandle DB = new DbHandle();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateFind(string headCategory)
        {
            ViewBag.headCategories = DB.headCategories.ToList();
            ViewBag.locations = DB.locations.ToList();
            //List<string> lsName = new List<string>();
            int id;
            if (headCategory != null)
                id = int.Parse(headCategory);
            else
                id = 1;//default
            DB.subCategories.Where(sub => sub.categoryId == id).ToList();
            //var ls = db.SubCategory.Where(x => x.categoryId == id).Where(x => x.name != "אחר").ToList();

            //foreach (var item in ls)
            //{
            //    lsName.Add(item.name);
            //}
            //ViewBag.sub = lsName;
            if (headCategory == null)
                return View();
            //return Json(lsName, JsonRequestBehavior.AllowGet);
            return View();

        }
        [HttpPost]
        public ActionResult CreateFind()
        {
            
            return View();
        }

    }
}