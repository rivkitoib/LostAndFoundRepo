namespace LostAndFound.Models
{
    using System.Data.Entity;

    public partial class DbHandle : DbContext
    {


        public DbHandle() : base("VeHashevotaEntities")
        {

        }




        public virtual DbSet<HeadCategory> headCategories { get; set; }
        public virtual DbSet<Find> finds { get; set; }
        public virtual DbSet<Location> locations { get; set; }
        public virtual DbSet<SubCategory> subCategories { get; set; }
        public virtual DbSet<Archive> archive { get; set; }

    }

}