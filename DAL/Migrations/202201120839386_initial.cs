namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        full_repo_name = c.String(),
                        path_to_file = c.String(),
                        name = c.String(),
                        text = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Files");
        }
    }
}
