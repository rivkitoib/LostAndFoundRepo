using LostAndFound.Crypto;
using LostAndFound.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace LostAndFound.Controllers
{
    public class AdministratorController : Controller
    {
        // GET: Administrator
        DbHandle DB = new DbHandle();

        [HttpGet]
        public ActionResult Settings()
        {

            // ViewBag.HeadCategories = DB.headCategories.ToList();
            // ViewBag.SubCategories = DB.subCategories.ToList();
            // ViewBag.Locations = DB.locations.ToList();
            ViewBag.email = ConfigurationManager.AppSettings["Email"];
            ViewBag.emailPass = ConfigurationManager.AppSettings["EmailPassword"];
            // ViewBag.password = ConfigurationManager.AppSettings["adminPassword"];


            //DB.locations
            return View();
        }

        public ActionResult SaveSettings(string categoriesStream, string locationStream, string email, string emailPass, string password)
        {
            
            HeadCategory headCategory;
            if (categoriesStream != null)
            {
                String[] categories = categoriesStream.Split('&');
                foreach (string category in categories)
                {
                    String[] subCategories = category.Split('+');
                    headCategory = new HeadCategory();
                    headCategory.Name = subCategories[0];
                    DB.headCategories.Add(headCategory);
                    DB.SaveChanges();
                    SubCategory subCategory;
                    foreach (string subName in subCategories)
                    {
                        subCategory = new SubCategory();
                        subCategory.name = subName;
                        subCategory.headCategory = headCategory;
                        DB.subCategories.Add(subCategory);
                    }
                    subCategory = new SubCategory();
                    subCategory.name = "אחר";
                    subCategory.headCategory = headCategory;


                }
            }
                if (locationStream != null) { 
            String[] locations = locationStream.Split('+');
            Location location;
            foreach (string loc in locations)
            {
                location = new Location();
                location.PlaceOrEvent = loc;
                DB.locations.Add(location);
            }
            DB.SaveChanges();
            }
            AddOrUpdateAppSettings("Email", email);
          AddOrUpdateAppSettings("EmailPassword", emailPass);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Archive()
        {
            Object archive2 = new object();
            var sendsToArchive = DB.finds.Where(find => DateTime.Compare(find.dateFound, DateTime.Now) < 10).ToList();

            foreach (Find find in sendsToArchive)
            {
                sendToArchive(find, false);
            }
            RedirectResult r = new RedirectResult("/Administrator/Settings");
            return r;
        }

        public void sendToArchive(Find find, Boolean isReturned)
        {
            Archive archive = new Archive();
            archive.description = find.description;
            archive.notes = find.notes;
            archive.location = find.location;
            archive.picture = archive.picture;
            archive.subCategory = find.subCategory;
            archive.finderName = find.finderName;
            archive.email = find.email;
            archive.cellphone = find.cellphone;
            archive.dateFound = find.dateFound;
            archive.status = isReturned;
            archive.dateStatus = DateTime.Now;
            DB.finds.Remove(find);
            DB.SaveChanges();

            DB.archive.Add(archive);
            DB.SaveChanges();


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


        public class SettingsObject
        {
            public string categoriesStream;
            public string locationStream;
            public string email;
            public string emailPass;
            public string password;

        }

    }

}