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
    public class CreateController : Controller
    {
        // GET: Home
        DbHandle DB = new DbHandle();
     
        [HttpGet]
        public ActionResult CreateFind()
        {
            ViewBag.headCategories = DB.headCategories.ToList();
            ViewBag.locations = DB.locations.ToList();
            ViewBag.sub = getSubCategories(1);
            return View(new Find { description = "ssssss", cellphone = "0522222222", notes = "wwww", finderName = "rivkale", email = "r@gmail.com" });

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
            var fullPath = Path.Combine(ConfigurationManager.AppSettings["OriginalImageFolder"], filename);
            Bitmap imageToEdit=null;
            if (importFile != null)
            {
                var file = System.IO.File.Create(Path.Combine(ConfigurationManager.AppSettings["OriginalImageFolder"], filename));
                importFile.InputStream.CopyTo(file);
                file.Close();
                importFile.InputStream.Close();
                imageToEdit = new Bitmap(file);
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
                image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                imageToEdit= new Bitmap(image);
 
            }
          //var info = GetPropertyValue("coverInfo");
          //int beginX=0, width=0, beginY=10, height=10;
          //Color color = Color.Red;
          //for(int x=beginX;x<beginX+width;x++)
          //    for(int y=beginY;y<beginY+height;y++)
          //          imageToEdit.SetPixel(x,y,color );
          //var path = Path.Combine(ConfigurationManager.AppSettings["DBImageFolder"],"DB_"+ filename);
          //imageToEdit.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            CreateFindInDB(fullPath);
            return View();
        }
        private string GetPropertyValue(string propName)
        {
            return Request.Form.GetValues(propName).FirstOrDefault() ?? "---";
        }

        private void CreateFindInDB(string pathToImage)
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
            newFind.picture = pathToImage;
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
      }
}