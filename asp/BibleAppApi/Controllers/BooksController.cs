using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using BibliaApp;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;
using BibleAppApi.Models;

//using System.Data.Objects;
namespace BibleAppApi.Controllers
{
    [RoutePrefix("api/Books")]
    public class BooksController : ApiController
    {
        [HttpGet]
        [Route("GetBook/{guid}")]
        public async Task<BookExtendedDomainObject> GetBook([FromUri]Guid guid)
        {
            BookExtendedDomainObject ret;
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                ret = await dbContext.BooksExtended.FirstOrDefaultAsync(x => x.Guid == guid);
                var comments = await dbContext.Comments.Where(x => x.BookGuid == guid).ToListAsync();
                comments.ForEach(x => x.ManageCommentKeyGuid = Guid.Empty);
                ret.Comments = comments;
            }
            ret.OnRead();
            return ret;
        }


        [HttpGet]
        [Route("GetBooks")]
        public async Task<List<Book>> GetAllBooks()
        {
            try
            {
                List<Book> result = new List<Book>();
                List<BookDomainObject> bookDomainObjects;

                using (ApplicationDbContext dbContext = new ApplicationDbContext())
                {
                    bookDomainObjects = await dbContext.Books.ToListAsync();
                }

                var bookNames = bookDomainObjects.Select(x => x.BookFullName).Distinct().ToList();

                bookNames.ForEach(bookFullName => result.Add(new Book()
                {
                    StartGlobalIndex =
                        bookDomainObjects.Where(y => y.BookFullName == bookFullName)
                            .Select(x => x.BookGlobalNumber)
                            .Min(),
                    BookFullName = bookFullName,
                    Subbooks = bookDomainObjects.Where(y => y.BookFullName == bookFullName).ToList()
                }));

                return result;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetComments")]
        public async Task<List<CommentDomainObject>> GetAllComments()
        {

            List<CommentDomainObject> comments;

            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                comments = await dbContext.Comments.OrderByDescending(x => x.AddTime).ToListAsync();
            }
            comments.ForEach(x => x.ManageCommentKeyGuid = Guid.Empty);

            return comments;

        }

        [HttpPost]
        [Route("AddComment")]
        public async Task<BookExtendedDomainObject> AddComment([FromBody]CommentDomainObject comment)
        {
            if (!string.IsNullOrEmpty(comment.Url))
            {
                string urlToLower = comment.Url.ToLower();
                if (urlToLower.Contains("www.youtube.com/watch?v="))
                {
                    if (comment.Url.Contains('&'))
                        comment.Url = comment.Url.FindInternalOf("watch?v=", "&");
                    else
                    {
                        comment.Url = comment.Url.Replace("https://", "").Replace("www.youtube.com/watch?v=", "");
                    }
                    comment.IsYoutubeVideo = true;
                    comment.IsAudioFile = false;
                }
                else if (urlToLower.EndsWith(".mp3") || urlToLower.EndsWith(".wav") || urlToLower.Contains("soundcloud.com"))
                {
                    comment.IsYoutubeVideo = false;
                    comment.IsAudioFile = true;
                }
                else
                {
                    comment.IsYoutubeVideo = false;
                    comment.IsAudioFile = false;
                }
            }
            else
            {
                comment.IsYoutubeVideo = false;
                comment.IsAudioFile = false;
            }
            BookExtendedDomainObject book;
            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                book = await dbContext.BooksExtended.FirstOrDefaultAsync(x => x.Guid == comment.BookGuid);
                if (book != null)
                {
                    comment.Guid = Guid.NewGuid();
                    comment.AddTime = DateTime.UtcNow;
                    comment.ManageCommentKeyGuid = Guid.NewGuid();
                    dbContext.Comments.Add(comment);
                    await dbContext.SaveChangesAsync();
                    book.Comments = dbContext.Comments.Where(x => x.BookGuid == book.Guid).ToList();
                }
            }

            return book;
        }

        [HttpPost]
        [Route("DeleteComment")]
        public async Task<HttpResponseMessage> DeleteComment([FromBody]CommentDomainObject comment)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var commentToDelete =
                    await dbContext.Comments.FirstOrDefaultAsync(
                        x => x.Guid == comment.Guid && x.ManageCommentKeyGuid == comment.ManageCommentKeyGuid);
                if (commentToDelete != null)
                {
                    dbContext.Comments.Remove(commentToDelete);
                    await dbContext.SaveChangesAsync();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

    }
}