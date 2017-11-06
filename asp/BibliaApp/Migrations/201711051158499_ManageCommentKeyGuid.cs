namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManageCommentKeyGuid : DbMigration
    {
        public override void Up()
        {
            Guid guid = Guid.NewGuid();
            Sql($"UPDATE [dbo].[Comment] SET Guid = '{guid}' WHERE Title IS NULL");
            AddColumn("dbo.Comment", "ManageCommentKeyGuid", c =>  c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comment", "ManageCommentKeyGuid");
        }
    }
}
