//בס"ד
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LostAndFound.Models
{
    public class View
    {
        public int id { get; set; }
        public int idSubCategory { get; set; }
        public string hebrewDate { get; set; }
        public string date { get; set; }
        public string PlaceOrEvent { get; set; }
        public string name { get; set; }
        public string myclass { get; set; }
        public string description { get; set; }
        public string notes { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public int categoryId { get; set; }
        public string subcategoryname { get; set; }
        public string categoryName { get; set; }
    }
}