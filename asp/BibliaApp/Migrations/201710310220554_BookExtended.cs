namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookExtended : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookExtended",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        BookName = c.String(nullable: false, maxLength: 50),
                        BookFullName = c.String(nullable: false),
                        PassagesJson = c.String(nullable: false),
                        CommentsJson = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Guid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BookExtended");
        }
    }
}
