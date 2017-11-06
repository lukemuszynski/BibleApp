namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsOrganiczenia : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comment", "Url", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Comment", "Title", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Comment", "StartIndex", c => c.String(nullable: false, maxLength: 2));
            AlterColumn("dbo.Comment", "EndIndex", c => c.String(nullable: false, maxLength: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comment", "EndIndex", c => c.String(nullable: false));
            AlterColumn("dbo.Comment", "StartIndex", c => c.String(nullable: false));
            AlterColumn("dbo.Comment", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Comment", "Url", c => c.String(nullable: false));
        }
    }
}
