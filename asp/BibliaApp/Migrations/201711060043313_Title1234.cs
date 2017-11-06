namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Title1234 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Passage", "Title1", c => c.String());
            AddColumn("dbo.Passage", "Title2", c => c.String());
            AddColumn("dbo.Passage", "Meantitle1", c => c.String());
            AddColumn("dbo.Passage", "Meantitle2", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Passage", "Meantitle2");
            DropColumn("dbo.Passage", "Meantitle1");
            DropColumn("dbo.Passage", "Title2");
            DropColumn("dbo.Passage", "Title1");
        }
    }
}
