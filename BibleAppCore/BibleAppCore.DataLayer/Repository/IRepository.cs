using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BibliaApp;
using BibleAppCore.DataLayer.TransferObjects;

namespace BibleAppCore.DataLayer.Repository
{
    public interface IRepository
    {
        Task<BookExtendedDomainObject> GetBookByGuid(Guid guid);
        Task<List<BookDomainObject>> GetAllBooks();
        Task<List<CommentDomainObject>> GetAllComments();
        Task<RepositoryResponse<BookExtendedDomainObject>> AddComment(CommentDomainObject comment);
        Task<RepositoryResponse<CommentDomainObject>> DeleteComment(CommentDomainObject comment);
    }
}
