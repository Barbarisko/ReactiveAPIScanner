namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ghk : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SearchResults", "KeyWord", c => c.String());
            DropColumn("dbo.SearchResults", "SearchResult");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SearchResults", "SearchResult", c => c.String());
            DropColumn("dbo.SearchResults", "KeyWord");
        }
    }
}
