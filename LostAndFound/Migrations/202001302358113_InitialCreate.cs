namespace LostAndFound.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archives",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 50),
                        picture = c.String(),
                        dateFound = c.DateTime(nullable: false),
                        hebrewDate = c.String(),
                        notes = c.String(maxLength: 20),
                        finderName = c.String(nullable: false),
                        cellphone = c.String(nullable: false),
                        email = c.String(nullable: false),
                        location_Id = c.Int(),
                        subCategory_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Locations", t => t.location_Id)
                .ForeignKey("dbo.SubCategories", t => t.subCategory_id)
                .Index(t => t.location_Id)
                .Index(t => t.subCategory_id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlaceOrEvent = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Finds",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        description = c.String(nullable: false, maxLength: 50),
                        picture = c.String(),
                        dateFound = c.DateTime(nullable: false),
                        hebrewDate = c.String(),
                        notes = c.String(maxLength: 20),
                        finderName = c.String(nullable: false),
                        cellphone = c.String(nullable: false),
                        email = c.String(nullable: false),
                        location_Id = c.Int(),
                        subCategory_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Locations", t => t.location_Id)
                .ForeignKey("dbo.SubCategories", t => t.subCategory_id)
                .Index(t => t.location_Id)
                .Index(t => t.subCategory_id);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        headCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.HeadCategories", t => t.headCategory_Id)
                .Index(t => t.headCategory_Id);
            
            CreateTable(
                "dbo.HeadCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Archives", "subCategory_id", "dbo.SubCategories");
            DropForeignKey("dbo.Archives", "location_Id", "dbo.Locations");
            DropForeignKey("dbo.SubCategories", "headCategory_Id", "dbo.HeadCategories");
            DropForeignKey("dbo.Finds", "subCategory_id", "dbo.SubCategories");
            DropForeignKey("dbo.Finds", "location_Id", "dbo.Locations");
            DropIndex("dbo.SubCategories", new[] { "headCategory_Id" });
            DropIndex("dbo.Finds", new[] { "subCategory_id" });
            DropIndex("dbo.Finds", new[] { "location_Id" });
            DropIndex("dbo.Archives", new[] { "subCategory_id" });
            DropIndex("dbo.Archives", new[] { "location_Id" });
            DropTable("dbo.HeadCategories");
            DropTable("dbo.SubCategories");
            DropTable("dbo.Finds");
            DropTable("dbo.Locations");
            DropTable("dbo.Archives");
        }
    }
}
