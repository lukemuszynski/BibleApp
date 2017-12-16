using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BibliaApp
{

    public class ApplicationDbContext : DbContext
    {
        const string ConnectionString = "Data Source=CMVWR72;Initial Catalog=BibliaApp.Program+ApplicationDbContext;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public const string ConnectionStringHome =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BibliaApp.Program+ApplicationDbContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public const string ConnectionString002 =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BibleApp02;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ApplicationDbContext() : base(ConnectionString002)
        {

        }

        public ApplicationDbContext(string connectionString) : base(connectionString)
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PassageDomainObject>()
                .ToTable("dbo.Passage")
                .HasKey(x => x.Guid)
                .Property(x => x.Guid).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .IsRequired();
            modelBuilder.Entity<PassageDomainObject>()
                .Property(x => x.PassageText)
                .IsRequired().HasMaxLength(null);
            modelBuilder.Entity<PassageDomainObject>()
                .Property(x => x.Book)
                .IsRequired().HasMaxLength(50);
            modelBuilder.Entity<PassageDomainObject>()
                .Property(x => x.PassageNumber)
                .IsRequired();
            modelBuilder.Entity<PassageDomainObject>()
                .Property(x => x.BookGuid)
                .IsRequired();

            modelBuilder.Entity<BookDomainObject>()
                .ToTable("dbo.Book")
                .HasKey(x => x.Guid)
                .Property(x => x.Guid).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<BookDomainObject>()
                .Property(x => x.BookName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<BookDomainObject>()
                .Property(x => x.BookFullName)
                .HasMaxLength(50);
            modelBuilder.Entity<BookDomainObject>()
                .Property(x => x.SubbookNumber)
                .IsRequired();
            modelBuilder.Entity<BookDomainObject>()
                .Property(x => x.BookGlobalNumber)
                .IsRequired();

            modelBuilder.Entity<BookExtendedDomainObject>()
                .ToTable("dbo.BookExtended")
                .HasKey(x => x.Guid)
                .Property(x => x.Guid).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.BookName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.PassagesJson)
                .IsRequired()
                .HasMaxLength(null);
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.BookFullName)
                .IsRequired()
                .HasMaxLength(null);
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.SubbookNumber)
                .IsRequired();
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.BookGlobalNumber)
                .IsRequired();
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.NextBookGuid)
                .IsRequired();
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.PreviousBookGuid)
                .IsRequired();


            modelBuilder.Entity<CommentDomainObject>()
                        .ToTable("dbo.Comment")
                        .HasKey(x => x.Guid)
                        .Property(x => x.Guid).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.IsYoutubeVideo)
                .IsRequired();
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.IsAudioFile)
                .IsRequired();
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(5000);
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.StartIndex)
                .IsRequired()
                .HasMaxLength(2);
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.EndIndex)
                .IsRequired()
                .HasMaxLength(2);
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.AddTime)
                .IsRequired();
            modelBuilder.Entity<CommentDomainObject>()
              .Property(x => x.ManageCommentKeyGuid)
              .IsRequired();
        }
        

        public virtual DbSet<PassageDomainObject> Passages { get; set; }
        public virtual DbSet<BookDomainObject> Books { get; set; }
        public virtual DbSet<BookExtendedDomainObject> BooksExtended { get; set; }
        public virtual DbSet<CommentDomainObject> Comments { get; set; }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
