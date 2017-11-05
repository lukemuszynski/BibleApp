namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentAddTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "AddTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comment", "AddTime");
        }
    }
}
