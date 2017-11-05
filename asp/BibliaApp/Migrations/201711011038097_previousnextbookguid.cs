namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class previousnextbookguid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookExtended", "NextBookGuid", c => c.Guid(nullable: false));
            AddColumn("dbo.BookExtended", "PreviousBookGuid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookExtended", "PreviousBookGuid");
            DropColumn("dbo.BookExtended", "NextBookGuid");
        }
    }
}
