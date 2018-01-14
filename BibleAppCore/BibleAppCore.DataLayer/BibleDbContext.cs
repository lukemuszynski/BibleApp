using BibliaApp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BibleAppCore.DataLayer
{
    public class BibleDbContext : DbContext
    {

        public BibleDbContext(DbContextOptions<BibleDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PassageDomainObject>()
                .HasKey(x => x.Guid);
            modelBuilder.Entity<PassageDomainObject>()
                .Property(p => p.Guid)
                .ValueGeneratedNever()
                .IsRequired();
            modelBuilder.Entity<PassageDomainObject>()
                .Property(x => x.PassageText)
                .IsRequired()
                .HasMaxLength(8000);
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
                .HasKey(x => x.Guid);
            modelBuilder.Entity<BookDomainObject>()
                .Property(x => x.Guid)
                .ValueGeneratedNever()
                .IsRequired();
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
                .HasKey(x => x.Guid);

            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.Guid)
                .ValueGeneratedNever();

            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.BookName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.PassagesJson)
                .IsRequired()
                .HasMaxLength(8000);
            modelBuilder.Entity<BookExtendedDomainObject>()
                .Property(x => x.BookFullName)
                .IsRequired()
                .HasMaxLength(8000);
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
                        .HasKey(x => x.Guid);
            modelBuilder.Entity<CommentDomainObject>().Property(x => x.Guid)
                .ValueGeneratedNever();
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
                .Property(x => x.AddTime)
                .HasDefaultValue(DateTime.MinValue);
            modelBuilder.Entity<CommentDomainObject>()
              .Property(x => x.ManageCommentKeyGuid)
              .IsRequired();
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.UserGuid)
                .IsRequired()
                .HasDefaultValue(Guid.Empty);
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.UserLogin)
                .IsRequired()
                .HasDefaultValue("SYSTEM");
            modelBuilder.Entity<CommentDomainObject>()
                .Property(x => x.IsPrivate)
                .IsRequired();

            modelBuilder.Entity<UserDomainObject>()
                .HasKey(x => x.Guid);
            modelBuilder.Entity<UserDomainObject>()
                .HasIndex(x => x.Login)
                .IsUnique();
            modelBuilder.Entity<UserDomainObject>()
                .HasIndex(x => x.EmailAddress)
                .IsUnique();
            modelBuilder.Entity<UserDomainObject>().Property(x => x.Guid)
                .ValueGeneratedNever();
            modelBuilder.Entity<UserDomainObject>()
                .Property(x => x.EmailAddress)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<UserDomainObject>()
                .Property(x => x.Login)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<UserDomainObject>()
                .Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(500);
           
        }

        internal DbSet<PassageDomainObject> Passages { get; set; }
        internal DbSet<BookDomainObject> Books { get; set; }
        internal DbSet<BookExtendedDomainObject> BooksExtended { get; set; }
        internal DbSet<CommentDomainObject> Comments { get; set; }
        internal DbSet<UserDomainObject> Users { get; set; }
    }
}
