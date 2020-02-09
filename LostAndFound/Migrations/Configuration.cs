﻿namespace LostAndFound.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LostAndFound.Models.DbHandle>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "LostAndFound.Models.DbHandle";
        }

        protected override void Seed(LostAndFound.Models.DbHandle context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
