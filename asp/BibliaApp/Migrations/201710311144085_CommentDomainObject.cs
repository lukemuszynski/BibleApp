namespace BibliaApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentDomainObject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Guid = c.Guid(nullable: false),
                        Url = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        IsYoutubeVideo = c.Boolean(nullable: false),
                        IsAudioFile = c.Boolean(nullable: false),
                        Text = c.String(nullable: false),
                        StartIndex = c.String(nullable: false),
                        EndIndex = c.String(nullable: false),
                        BookGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Guid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comment");
        }
    }
}
