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

        [HttpGet]
        public ActionResult Settings()
        {

            ViewBag.HeadCategories = DB.headCategories.ToList();
            ViewBag.SubCategories = DB.subCategories.ToList();
            ViewBag.Locations = DB.locations.ToList();
            ViewBag.email = ConfigurationManager.AppSettings["Email"];
            ViewBag.emailPass = ConfigurationManager.AppSettings["EmailPassword"];
            ViewBag.password = ConfigurationManager.AppSettings["adminPassword"];

       
            //DB.locations
            return View();
        }

        public ActionResult SaveSettings(string categoriesStream,string locationStream,string email,string emailPass,string password)
        {
            SettingsObject settings= new SettingsObject();
            HeadCategory headCategory;
            String[] categories = settings.categoriesStream.Split('&');
            foreach (string category in categories)
            {
                String[] subCategories = category.Split('+');
                headCategory = new HeadCategory();
                headCategory.Name = subCategories[0];

                SubCategory subCategory;
                foreach(string subName in subCategories)
                {
                    subCategory = new SubCategory();
                    subCategory.name = subName;
                    subCategory.headCategory = headCategory;
                }
                subCategory = new SubCategory();
                subCategory.name = "אחר";
                subCategory.headCategory = headCategory;


            }
            String[] locations = settings.locationStream.Split('+');
            Location location;
            foreach(string loc in locations)
            {
                location = new Location();
                location.PlaceOrEvent = loc;
            }
            AddOrUpdateAppSettings("Email", settings.email);
            AddOrUpdateAppSettings("adminPassword", settings.password);
            //encrypt
            AddOrUpdateAppSettings("EmailPassword", settings.emailPass);
            
            return  RedirectToAction("Index","Home");
        }
        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
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
        public class SettingsObject
        {
           public string categoriesStream;
            public string locationStream;
            public string email;
            public string emailPass;
            public string password;
            // string archive;
        }

    }
  
}