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
        public ActionResult CreateFind(string headCategory)
        {
            ViewBag.headCategories = DB.headCategories.ToList();
            //ViewBag.locations = DB.locations.ToList();
            ////List<string> lsName = new List<string>();
            //int id;
            //if (headCategory != null)
            //    id = int.Parse(headCategory);
            //else
            //    id = 1;//default
            //DB.subCategories.Where(sub => sub.id == id).ToList();
            ////var ls = DB.SubCategory.Where(x => x.categoryId == id).Where(x => x.name != "אחר").ToList();

            ////foreach (var item in ls)
            ////{
            ////    lsName.Add(item.name);
            ////}
            ////ViewBag.sub = lsName;
            //if (headCategory == null)
            //    return View();
            //return Json(lsName, JsonRequestBehavior.AllowGet);
            return View();

        }
        [HttpPost]
        public ActionResult CreateFind()
        {

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