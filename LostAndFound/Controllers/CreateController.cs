using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LostAndFound.Controllers
{
    public class CreateController : Controller
    {
        [HttpGet]
        public ActionResult CreateFind33()
        {

            return View("estie");

        }
        // GET: Home
        DbHandle DB = new DbHandle();

        [HttpGet]
        public ActionResult CreateFind()
        {
            //SendMail.send();

            ViewBag.headCategories = DB.headCategories.ToList();
            ViewBag.locations = DB.locations.ToList();
            ViewBag.sub = getSubCategories(1);
            return View(new Find { cellphone = "0522222222", notes = "wwww", finderName = "rivkale", email = "r@gmail.com" });

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
        public ActionResult CreateFind2(HttpPostedFileBase importFile)
        {

            var filename = $"{Guid.NewGuid().ToString()}.jpg";
            string dbFileName = "DB_" + filename;
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["OriginalImageFolder"], filename);
            var imgBase64 = GetPropertyValue("imagedata");
            bool specifiedImgae = true;
            if (importFile != null)
            {
                Image img;
                using (img = Image.FromStream(importFile.InputStream))
                {
                    var sizes = GetPropertyValue("sizes").Split(';');
                    int h = int.Parse(sizes[0]);
                    int w = int.Parse(sizes[1]);
                    Bitmap b;
                    using (b = new Bitmap(img, new Size(w, h)))
                    {
                        b.Save(fullPath);
                        importFile.InputStream.Close();

                    }
                    b = null;
                    img.Dispose();

                }
                img = null;
            }
            else if (imgBase64 != "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCACWASwDAREAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFAEBAAAAAAAAAAAAAAAAAAAAAP/EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/AJ/4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP/Z")
            {
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
                    image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                }
            }
            else
            {
                fullPath = ConfigurationManager.AppSettings["DefaultFindPicture"];
                specifiedImgae = false;
            }
            if (specifiedImgae)
            {
                var imageToEdit = new Bitmap(fullPath);

                var info = GetPropertyValue("coverInfo").Split(';');

                int x = int.Parse(info[2]), width = int.Parse(info[1]), y = int.Parse(info[3]), height = int.Parse(info[0]);
                Color color = Color.LightGray;
                for (; x < width; x++)
                    for (int y1 = y; y1 < y + height; y1++)
                        imageToEdit.SetPixel(x, y1, color);
                fullPath = Path.Combine(ConfigurationManager.AppSettings["DBImageFolder"], dbFileName);
                imageToEdit.Save(fullPath, ImageFormat.Jpeg);


            }
            CreateFindInDB(dbFileName);
            return RedirectToAction("Index", "Home");
        }

        private string GetPropertyValue(string propName, string defval = "")
        {
            var v = Request.Form.GetValues(propName).FirstOrDefault() ?? defval;
            if (string.IsNullOrEmpty(v))
            {
                v = defval;
            }
            return v;
        }

        private void CreateFindInDB(string pathToImage)
        {
            //set find object
            Find newFind = new Find();
            string subCategory = GetPropertyValue("subCategory", "אחר");
            if (subCategory == "אחר")
                newFind.subCategory.id = DB.subCategories.First(x => x.headCategory.Id == _headCategory && x.name == "אחר").id;
            else
                newFind.subCategory = DB.subCategories.First(x => x.name == subCategory);
            int locationId = int.Parse(GetPropertyValue("location"));
            newFind.location = DB.locations.First(x => x.Id == locationId);
            newFind.dateFound = DateTime.Parse(GetPropertyValue("dateFound", DateTime.Now.ToString()));
            //picture
            newFind.notes = GetPropertyValue("notes");
            newFind.finderName = GetPropertyValue("finderName");
            newFind.cellphone = GetPropertyValue("cellphone");
            newFind.description = GetPropertyValue("description");
            newFind.email = GetPropertyValue("email");
            newFind.picture = Path.Combine("~\\Images\\ForDB", pathToImage);
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
            //if (newFind.email != "")
            //    SendMail.sendFinderFind(newFind);


        }

        [HttpPost]
        public ActionResult CreateFind(HttpContext postedFile)
        {

            return View();
        }
    }
}