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
        public ActionResult Index(string category)
        {
            ViewBag.category = DB.headCategories.ToList();
            ViewBag.place = DB.locations.ToList();

            //ViewBag.category2.First(x => x.Id == item.idCategory).Name;
            if (category == null)
            {
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

                return View();
            }

            List<string> lsName = new List<string>();
            if (category != "הכל")
            {
                int id = DB.headCategories.First(x => x.Name == category).Id;
                var ls = DB.subCategories.Where(x => x.headCategory.Id == id).ToList();

                foreach (var item in ls)
                {
                    lsName.Add(item.name);
                }
            }
            return Json(lsName, JsonRequestBehavior.AllowGet);
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
            DB.subCategories.Where(sub => sub.id == id).ToList();
            //var ls = DB.SubCategory.Where(x => x.categoryId == id).Where(x => x.name != "אחר").ToList();

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

        [HttpGet]
        public ActionResult SearchFinds(string subCategory, string place, DateTime fromDate, DateTime toDate, string text, string hiddenCategory,bool searchArchive = false)
        {
            var findsFilterQuery = DB.finds.ToList().Select(x => x);

            if (searchArchive)
            {
                var archives = DB.archive.ToList();
                findsFilterQuery = findsFilterQuery.Union(archives.Select(x => Find.convertarchiveToFind(x)));
                // findsFilterQuery = findsFilterQuery.Union(DB.finds.Select(x => (x)));
            }

            var c = findsFilterQuery.Select(x => x).ToList();


            //sorting by place
            if (place != "הכל")
            {
                findsFilterQuery = findsFilterQuery.Where(x => x.location.PlaceOrEvent.Equals(place));
            }
            //sorting by category or subCategory
            if (hiddenCategory != null && hiddenCategory != "הכל")
            {
                if (subCategory != "הכל")
                {
                    findsFilterQuery = findsFilterQuery.Where(x => x.subCategory.name.Equals(subCategory));
                }
                else
                {
                    var subCategoriesFilter = DB.subCategories.Where(x => x.headCategory.Name.Equals(hiddenCategory));

                    findsFilterQuery = from a in subCategoriesFilter
                                       join b in findsFilterQuery
                                       on a.id equals b.subCategory.id
                                       select b;
                }
            }
            //sorting by date
            findsFilterQuery = findsFilterQuery.Where(x => DateTime.Compare(x.dateFound.Date, fromDate.Date) >= 0);
            findsFilterQuery = findsFilterQuery.Where(x => DateTime.Compare(toDate.Date, x.dateFound.Date) >= 0);

            //sorting by text
            if (text != "")
            {
                findsFilterQuery = findsFilterQuery.Where(x => x.description.Contains(text) || x.notes.Contains(text));
            }


            var filteredFinds = findsFilterQuery.Select(f => new View()
            {
                id = f.id,
                idSubCategory = f.subCategory.id,
                name = f.finderName,
                email = f.email,
                notes = f.notes,
                date = f.dateFound.ToString(),
                description = f.description,
                cellphone = f.cellphone,
                subcategoryname = f.subCategory.name,
                categoryId = f.subCategory.headCategory.Id,
                categoryName = f.subCategory.headCategory.Name,
                PlaceOrEvent = f.location.PlaceOrEvent
            });
            
            ViewBag.tbl = filteredFinds.ToList();

            return PartialView();
        }




    }
}