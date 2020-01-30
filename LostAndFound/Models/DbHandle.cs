namespace LostAndFound.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Collections;
    using System.Data.Common;
    using System.Data;
    using System.IO;
    using System.Data.SqlClient;
   
    public partial class DbHandle : DbContext
    {


        public DbHandle():base("lostAndFoundEntities")
        {
                
        }




        public virtual DbSet<HeadCategory> headCategories { get; set; }
        public virtual DbSet<Find> finds { get; set; }
        public virtual DbSet<Location> locations { get; set; }
        public virtual DbSet<SubCategory> subCategories { get; set; }
        public virtual DbSet<Archive> archive { get; set; }
    }

}