using System.Data.Entity.Migrations;
namespace LostAndFound.Migrations
{
    public class migrationTrial
    {


        public partial class AddPostClass : DbMigration
        {
            public override void Up()
            {
                CreateStoredProcedure(
                    "dbo.getAllFinds",
                     p => new
                     {
                     },

        body:
            @"select * from Finds"
    );

            }


        }
    }

}