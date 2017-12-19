using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BibliaApp;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BibleAppCore.DataLayer.TransferObjects;

namespace BibleAppCore.DataLayer.Repository
{
    public class Repository : IRepository
    {
        private BibleDbContext DbContext { get; set; }

        public Repository(BibleDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<BookExtendedDomainObject> GetBookByGuid(Guid guid)
        {
            BookExtendedDomainObject book = await DbContext.BooksExtended.FindAsync(guid);
            var comments = await DbContext.Comments.Where(x => x.BookGuid == guid).ToListAsync();
            comments.ForEach(x => x.ManageCommentKeyGuid = Guid.Empty);
            book.Comments = comments;
            book.OnRead();

            return book;
        }

        public async Task<List<BookDomainObject>> GetAllBooks()
        {

            return await DbContext.Books.ToListAsync();

        }

        public async Task<List<CommentDomainObject>> GetAllComments()
        {
            return await DbContext.Comments.OrderByDescending(x => x.AddTime).ToListAsync();
        }

        public async Task<RepositoryResponse<BookExtendedDomainObject>> AddComment(CommentDomainObject comment)
        {
            RepositoryResponse<BookExtendedDomainObject> repositoryResponse = new RepositoryResponse<BookExtendedDomainObject>();
            try
            {
                var book = await DbContext.BooksExtended.FirstOrDefaultAsync(x => x.Guid == comment.BookGuid);
                repositoryResponse.Value = book;

                if (book != null)
                {
                    comment.Guid = Guid.NewGuid();
                    comment.AddTime = DateTime.UtcNow;
                    comment.ManageCommentKeyGuid = Guid.NewGuid();
                    DbContext.Comments.Add(comment);
                    await DbContext.SaveChangesAsync();
                    book.Comments = await DbContext.Comments.Where(x => x.BookGuid == book.Guid).ToListAsync();
                }
                else
                {
                    repositoryResponse.Successful = false;
                }
            }
            catch (Exception e)
            {
                repositoryResponse.Exception = e;
            }
            return repositoryResponse;
        }

        public async Task<RepositoryResponse<CommentDomainObject>> DeleteComment(CommentDomainObject comment)
        {
            RepositoryResponse<CommentDomainObject> repositoryResponse = new RepositoryResponse<CommentDomainObject>();
            try
            {
                var commentToDelete =
                    await DbContext.Comments.FirstOrDefaultAsync(
                        x => x.Guid == comment.Guid && x.ManageCommentKeyGuid == comment.ManageCommentKeyGuid);
                repositoryResponse.Value = commentToDelete;
                if (commentToDelete != null)
                {
                    DbContext.Comments.Remove(commentToDelete);
                    await DbContext.SaveChangesAsync();
                    return repositoryResponse;
                }
                repositoryResponse.Successful = false;
                return repositoryResponse;
            }
            catch (Exception e)
            {
                repositoryResponse.Exception = e;
            }

            return repositoryResponse;
        }
    }
}
