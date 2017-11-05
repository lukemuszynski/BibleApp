namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        BookName = c.String(nullable: false, maxLength: 50),
                        BookFullName = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Guid);
            
            CreateTable(
                "dbo.Passage",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        PassageText = c.String(nullable: false),
                        Book = c.String(nullable: false, maxLength: 50),
                        BookGuid = c.Guid(nullable: false),
                        PassageNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Guid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Passage");
            DropTable("dbo.Book");
        }
    }
}
