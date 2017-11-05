namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class numerKsiegiGlobal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Book", "SubbookNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Book", "BookGlobalNumber", c => c.Int(nullable: false));
            AddColumn("dbo.BookExtended", "SubbookNumber", c => c.Int(nullable: false));
            AddColumn("dbo.BookExtended", "BookGlobalNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookExtended", "BookGlobalNumber");
            DropColumn("dbo.BookExtended", "SubbookNumber");
            DropColumn("dbo.Book", "BookGlobalNumber");
            DropColumn("dbo.Book", "SubbookNumber");
        }
    }
}
