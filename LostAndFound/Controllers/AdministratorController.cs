using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LostAndFound.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        DbHandle DB = new DbHandle();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult addHeadCategory()
        {
            return PartialView("defineCategories");
        }
        [HttpPost]
        public ActionResult addHeadCategory(string headCategryName)
        {
            Models.HeadCategory hc = new HeadCategory();
            hc.Name = headCategryName;
            DB.headCategories.Add(hc);
            return View();
        }
    }
}