namespace LostAndFound.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;


    public partial class DbHandle : DbContext
    {
   

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }




        public virtual DbSet<HeadCategory> headCategories { get; set; }
        public virtual DbSet<Find> finds { get; set; }
        public virtual DbSet<Location> locations { get; set; }
        public virtual DbSet<SubCategory> subCategories { get; set; }
        public virtual DbSet<Archive> archive { get; set; }
    }

}