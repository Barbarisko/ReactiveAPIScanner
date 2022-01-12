namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class list1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Keys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KeyString = c.String(),
                        SearchResults_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SearchResults", t => t.SearchResults_Id)
                .Index(t => t.SearchResults_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Keys", "SearchResults_Id", "dbo.SearchResults");
            DropIndex("dbo.Keys", new[] { "SearchResults_Id" });
            DropTable("dbo.Keys");
        }
    }
}
