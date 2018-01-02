using BibleAppApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BibleAppCore.Contracts.Contract.Security;
using BibleAppCore.Contracts.Contract.ViewModel;
using BibleAppCore.DataLayer.TransferObjects;
using BibliaApp;

namespace BibleAppCore.DataLayer.Repository
{
    public interface IAuthRepository
    {
        Task<RepositoryResponse<User>> ValidateCredentails(Credentials credentials);
        Task<RepositoryResponse<User>> RegisterUser(User registerUserData);
    }
}
