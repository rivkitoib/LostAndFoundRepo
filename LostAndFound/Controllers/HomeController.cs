using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LostAndFound.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        DbHandle DB = new DbHandle();
        public ActionResult Index(string category)
        {
            //  SendImageSample.send3();
            ViewBag.category = DB.headCategories.ToList();
            ViewBag.location = DB.locations.ToList();

            ////ViewBag.category2.First(x => x.Id == item.idCategory).Name;
            //if (category == null)
            //{
            //    var t = (from f in DB.finds.ToList()
            //             join sc in DB.subCategories.ToList()
            //             on f.subCategory.id equals sc.id
            //             join p in DB.locations.ToList()
            //             on f.location.Id equals p.Id
            //             join c in DB.headCategories.ToList()
            //             on sc.headCategory.Id equals c.Id

            //             select new View()
            //             {
            //                 id = f.id,
            //                 idSubCategory = f.subCategory.id,
            //                 hebrewDate = f.hebrewDate,
            //                 name = f.finderName,
            //                 email = f.email,
            //                 notes = f.notes,
            //                 date = f.dateFound.ToString(),
            //                 description = f.description,
            //                 cellphone = f.cellphone,
            //                 subcategoryname = sc.name,
            //                 categoryId = sc.headCategory.Id,
            //                 categoryName = c.Name,
            //                 PlaceOrEvent = p.PlaceOrEvent
            //             });
            //    ViewBag.tbl = t.ToList();

            //    return View();
            //}

            //List<string> lsName = new List<string>();
            //if (category != "הכל")
            //{
            //    int id = DB.headCategories.First(x => x.Name == category).Id;
            //    var ls = DB.subCategories.Where(x => x.headCategory.Id == id).ToList();

            //    foreach (var item in ls)
            //    {
            //        lsName.Add(item.name);
            //    }
            //}
            //return Json(lsName, JsonRequestBehavior.AllowGet);
            return View();
        }
        [HttpGet]
        public ActionResult CreateFind()
        {

            ViewBag.headCategories = DB.headCategories.ToList();
            ViewBag.locations = DB.locations.ToList();
            ViewBag.sub = getSubCategories(1);
            return View(new Find { description = "ssss", cellphone = "0522222222", notes = "wwww", finderName = "rivkale", email = "r@gmail.com" });

        }
        public ActionResult SelectedHeadCategory(string headCategory)
        {
            List<string> lsSubCategory;
            int id;
            if (headCategory != null)
                id = int.Parse(headCategory);
            else
                id = 1;//
            lsSubCategory = getSubCategories(id);
            ViewBag.sub = lsSubCategory;
            return Json(lsSubCategory, JsonRequestBehavior.AllowGet);
        }
        private List<string> getSubCategories(int headCategoryId)
        {
            _headCategory = headCategoryId;
            List<string> lsSubCategory = new List<string>();
            var ls = DB.subCategories.Where(x => x.headCategory.Id == headCategoryId).Where(x => x.name != "אחר").ToList();
            foreach (var item in ls)
            {
                lsSubCategory.Add(item.name);
            }
            ViewBag.sub = lsSubCategory;
            return lsSubCategory;
        }
        int _headCategory = 1;


        [HttpPost]
        public ActionResult CreateFind2(HttpPostedFileBase findFile)
        {
            var filename = $"{Guid.NewGuid().ToString()}.jpg";
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["OriginalImageFolder"], filename);
            if (findFile != null)
            {
                var file = System.IO.File.Create(Path.Combine(ConfigurationManager.AppSettings["OriginalImageFolder"], filename));
                findFile.InputStream.CopyTo(file);
                file.Close();
                findFile.InputStream.Close();
            }
            var imgBase64 = GetPropertyValue("imagedata");
            if (!string.IsNullOrEmpty(imgBase64))
            {
                var imageData = imgBase64.Split(',')[1];
                var bytes = Convert.FromBase64String(imageData);
                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                }
                image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
            }
            CreateFindInDB();
            return View();
        }
        private string GetPropertyValue(string propName)
        {
            return Request.Form.GetValues(propName).FirstOrDefault() ?? "";
        }

        private void CreateFindInDB()
        {
            //set find object
            Find newFind = new Find();
            string subCategory = GetPropertyValue("subCategory");
            if (subCategory == "אחר")
                newFind.subCategory.id = DB.subCategories.First(x => x.headCategory.Id == _headCategory && x.name == "אחר").id;
            else
                newFind.subCategory = DB.subCategories.First(x => x.name == subCategory);
            int locationId = int.Parse(GetPropertyValue("location"));
            newFind.location = DB.locations.First(x => x.Id == locationId);
            newFind.dateFound = DateTime.Parse(GetPropertyValue("dateFound"));
            //picture
            newFind.notes = GetPropertyValue("notes");
            newFind.finderName = GetPropertyValue("finderName");
            newFind.cellphone = GetPropertyValue("cellphone");
            newFind.description = GetPropertyValue("description");
            newFind.email = GetPropertyValue("email");
            DB.finds.Add(newFind);
            try
            {
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                throw;
            }
        }

        [HttpPost]
        public ActionResult CreateFind(HttpContext postedFile)
        {

            return View();
        }
        //[HttpPost]
        //public void SaveImage(string base64image)
        //{
        //    if (string.IsNullOrEmpty(base64image))
        //        return;

        //    var t = base64image.Substring(22);  // remove data:image/png;base64,

        //    byte[] bytes = Convert.FromBase64String(t);

        //    Image image;
        //    using (MemoryStream ms = new MemoryStream(bytes))
        //    {
        //        image = Image.FromStream(ms);
        //    }
        //    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
        //    var fullPath = Path.Combine(Server.MapPath("~/Content/Images/"), randomFileName);
        //    image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
        //}

        [HttpPost]
        public ActionResult SendImage(FileResult c)
        {
            string v = "gj";

            return Json(v);
        }
        [HttpGet]
        public ActionResult SendImage()
        {
            string v = "gj";

            return View();
        }
        public ActionResult Search(string subCategory, string place, DateTime fromDate, DateTime toDate, string text, string hiddenCategory)
        {

            var findsList = DB.finds.Where(x => x != null);
            int helpId;
            //sorting by place
            if (place != "הכל")
            {
                helpId = DB.locations.First(x => x.PlaceOrEvent == place).Id;
                findsList = findsList.Where(x => x.location.Id == helpId);
            }
            //sorting by category or subCategory
            if (hiddenCategory != null && hiddenCategory != "הכל")
            {
                if (subCategory != "הכל")
                {
                    helpId = DB.subCategories.First(x => x.name == subCategory).id;
                    findsList = findsList.Where(x => x.subCategory.id == helpId);
                }
                else
                {
                    helpId = DB.headCategories.First(x => x.Name == hiddenCategory).Id;
                    var ITSsubCategories = DB.subCategories.Where(x => x.headCategory.Id == helpId);

                    findsList = from a in ITSsubCategories
                                join b in findsList
                                on a.id equals b.subCategory.id
                                select b;
                }
            }
            //sorting by date
            findsList = findsList.Where(x => DateTime.Compare(x.dateFound, fromDate) >= 0);
            findsList = findsList.Where(x => DateTime.Compare(toDate, x.dateFound) >= 0);
            //findsList = findsList.Where(x => x.dateFound.Year >= fromDate.Year && x.dateFound.Month >= fromDate.Month && x.dateFound.Day >= fromDate.Day);
            //findsList = findsList.Where(x => x.dateFound.Year <= toDate.Year && x.dateFound.Month <= toDate.Month && x.dateFound.Day <= toDate.Day);
            // findsList = findsList.Where(x => ==true);
            // findsList = findsList.Where(x => IsGreaterOrEqual(toDate,x.dateFound)==true);
            //findsList = from a in findsList
            //            where IsGreaterOrEqual(a.dateFound, fromDate)
            //            select a;
            //findsList = from a in findsList
            //            where IsGreaterOrEqual(toDate, a.dateFound)
            //            select a;
            //sorting by text
            if (text != "")
            {
                findsList = findsList.Where(x => x.description.Contains(text) || x.notes.Contains(text));
            }
            var t = (from f in DB.finds.ToList()
                     join sc in DB.subCategories.ToList()
                     on f.subCategory.id equals sc.id
                     join p in DB.locations.ToList()
                     on f.location.Id equals p.Id
                     join c in DB.headCategories.ToList()
                     on sc.headCategory.Id equals c.Id
                     select new View()
                     {
                         id = f.id,
                         idSubCategory = f.subCategory.id,
                         hebrewDate = f.hebrewDate,
                         name = f.finderName,
                         email = f.email,
                         notes = f.notes,
                         date = f.dateFound.ToString(),
                         description = f.description,
                         cellphone = f.cellphone,
                         subcategoryname = sc.name,
                         categoryId = sc.headCategory.Id,
                         categoryName = c.Name,
                         PlaceOrEvent = p.PlaceOrEvent
                     });
            ViewBag.tbl = t.ToList();


            return PartialView();
        }

    }
}