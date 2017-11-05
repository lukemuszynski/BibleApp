namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteCommentsFromExtended : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BookExtended", "CommentsJson");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookExtended", "CommentsJson", c => c.String(nullable: false));
        }
    }
}
