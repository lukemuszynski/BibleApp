using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BibliaApp;
using System.Linq;
using AutoMapper;
using BibleAppApi.Models;
using BibleAppCore.Contracts.Contract.ViewModel;
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

        public async Task<BookExtended> GetBookByGuid(Guid guid, Guid? userGuid = null)
        {
            BookExtendedDomainObject book = await DbContext.BooksExtended.FindAsync(guid);
            List<CommentDomainObject> comments;
            if (userGuid != null)
            {
                comments = await DbContext.Comments.Where(x => x.BookGuid == guid && (x.IsPrivate == false || x.UserGuid == userGuid)).ToListAsync();
            }
            else
            {
                comments = await DbContext.Comments.Where(x => x.BookGuid == guid && x.IsPrivate == false).ToListAsync();
            }
            comments.ForEach(x => x.ManageCommentKeyGuid = Guid.Empty);
            book.Comments = comments;
            book.OnRead();

            return Mapper.Map<BookExtended>(book);
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return Mapper.Map<List<Book>>(await DbContext.Books.ToListAsync());
        }

        public async Task<List<Comment>> GetAllComments(Guid? userGuid = null)
        {
            List<CommentDomainObject> comments;
            if (userGuid != null)
            {
                comments = await DbContext.Comments.Where(x=> x.IsPrivate == false || x.UserGuid == userGuid).ToListAsync();
            }
            else
            {
                comments = await DbContext.Comments.Where(x => x.IsPrivate == false).ToListAsync();
            }
            return Mapper.Map<List<Comment>>(comments);
        }

        public async Task<RepositoryResponse<BookExtended>> AddComment(Comment comment)
        {
            RepositoryResponse<BookExtended> repositoryResponse = new RepositoryResponse<BookExtended>();
            try
            {
                var book = await DbContext.BooksExtended.FirstOrDefaultAsync(x => x.Guid == comment.BookGuid);
                if (book != null)
                {
                    comment.Guid = Guid.NewGuid();
                    comment.AddTime = DateTime.UtcNow;
                    comment.ManageCommentKeyGuid = Guid.NewGuid();
                    DbContext.Comments.Add(Mapper.Map<CommentDomainObject>(comment));
                    await DbContext.SaveChangesAsync();
                    book.Comments = await DbContext.Comments.Where(x => (x.BookGuid == book.Guid) && (x.IsPrivate == false || x.UserGuid == comment.UserGuid)).ToListAsync();
                    repositoryResponse.Value = Mapper.Map<BookExtended>(book);
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

        public async Task<RepositoryResponse<Comment>> DeleteComment(Comment comment)
        {
            RepositoryResponse<Comment> repositoryResponse = new RepositoryResponse<Comment>();
            try
            {
                var commentToDelete =
                    await DbContext.Comments.FirstOrDefaultAsync(
                        x => x.Guid == comment.Guid && x.ManageCommentKeyGuid == comment.ManageCommentKeyGuid);
                repositoryResponse.Value = Mapper.Map<Comment>(commentToDelete);
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
