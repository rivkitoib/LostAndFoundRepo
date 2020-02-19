using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LostAndFound.Controllers
{
    public class deleteFindController : Controller
    {
        private DbHandle DB = new DbHandle();
        
        public ActionResult loadConfirmDelete(string findId)
        {
            Session["findIdGlob"] = int.Parse(findId);
            return View("confirmPhoneNumber");
        }


        [HttpPost]
        public ActionResult confirmDelete(string finderPhoneNumber)
        {
            if (Session["findIdGlob"] != null)
            {
                int findIdGlob = Convert.ToInt32(Session["findIdGlob"]);
                string finderPhoneNumberFromDB = DB.finds.First(x => x.id == findIdGlob).cellphone;
                if (finderPhoneNumberFromDB == finderPhoneNumber)
                {
                    var found = DB.finds.First(x => x.id == findIdGlob);
                    DB.finds.Remove(found);
                    DB.SaveChanges();
                    return View();
                }
            }
            return View("deleteDenyed");
        }
    }
}