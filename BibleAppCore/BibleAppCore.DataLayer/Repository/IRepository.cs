using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BibleAppApi.Models;
using BibleAppCore.Contracts.Contract.ViewModel;
using BibliaApp;
using BibleAppCore.DataLayer.TransferObjects;

namespace BibleAppCore.DataLayer.Repository
{
    public interface IRepository
    {
        Task<BookExtended> GetBookByGuid(Guid guid, Guid? userGuid = null);
        Task<List<Book>> GetAllBooks();
        Task<List<Comment>> GetAllComments(Guid? userGuid = null);
        Task<RepositoryResponse<BookExtended>> AddComment(Comment comment);
        Task<RepositoryResponse<Comment>> DeleteComment(Comment comment, Guid? userGuid);
        Task<List<Comment>> GetMyComments(Guid? userGuid);
    }
}
