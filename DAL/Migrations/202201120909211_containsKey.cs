namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class containsKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "containsKey", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "containsKey");
        }
    }
}
