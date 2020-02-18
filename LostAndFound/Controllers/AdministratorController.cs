using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using LostAndFound.Crypto;

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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string password)
        {
            string adminPassword = ConfigurationManager.AppSettings["adminPassword"];
            string encryptedPassword = Hash256.GenerateSHA256String(password);
            if (!encryptedPassword.Equals(adminPassword))
            {
               return View();
            }
            return RedirectToAction("Index");
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