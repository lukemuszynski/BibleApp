using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BibleAppApi.Models;
using BibleAppCore.Contracts.Contract.Security;
using BibleAppCore.Contracts.Contract.ViewModel;
using BibleAppCore.DataLayer.TransferObjects;
using BibliaApp;
using Microsoft.EntityFrameworkCore;

namespace BibleAppCore.DataLayer.Repository
{
    public class AuthRepository : IAuthRepository
    {
        public BibleDbContext DbContext { get; set; }

        private const string IX_Users_EmailAddress = "IX_Users_EmailAddress";
        private const string IX_Users_Login = "IX_Users_Login";

        public AuthRepository(BibleDbContext bibleDbContext)
        {
            DbContext = bibleDbContext;
        }
        public async Task<RepositoryResponse<User>> ValidateCredentails(Credentials credentials)
        {
            RepositoryResponse<User> repositoryResponse = new RepositoryResponse<User>();

            UserDomainObject user = await DbContext.Users.FirstOrDefaultAsync(usr => usr.Login == credentials.Login && usr.Password == credentials.Password);

            if (user == null)
                repositoryResponse.Successful = false;
            else
                repositoryResponse.Value = Mapper.Map<User>(user);

            return repositoryResponse;
        }

        public async Task<RepositoryResponse<User>> RegisterUser(User registerUserData)
        {
            RepositoryResponse<User> repositoryResponse = new RepositoryResponse<User>();
            registerUserData.Guid = Guid.NewGuid();
            DbContext.Users.Add(Mapper.Map<UserDomainObject>(registerUserData));
            try
            {
                var res = await DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                bool? containsLoginIndexException = e.InnerException?.Message?.Contains(IX_Users_Login);
                bool? containsEmailIndexException = e.InnerException?.Message?.Contains(IX_Users_EmailAddress);
                if ((containsLoginIndexException ?? false) && (containsEmailIndexException ?? false))
                {
                    repositoryResponse.Exception = e;
                    repositoryResponse.RepositoryResponseMessage = RepositoryResponse<User>
                        .RepositoryResponseMessageEnum.UserIndexAndEmailIndexException;
                }
                else if (containsLoginIndexException ?? false)
                {
                    repositoryResponse.Exception = e;
                    repositoryResponse.RepositoryResponseMessage = RepositoryResponse<User>
                        .RepositoryResponseMessageEnum.UserIndexException;
                }else if (containsEmailIndexException ?? false)
                {
                    repositoryResponse.Exception = e;
                    repositoryResponse.RepositoryResponseMessage = RepositoryResponse<User>
                        .RepositoryResponseMessageEnum.EmailIndexException;
                }
                return repositoryResponse;
            }
            catch (Exception e)
            {
                repositoryResponse.Exception = e;
                repositoryResponse.RepositoryResponseMessage = RepositoryResponse<User>
                    .RepositoryResponseMessageEnum.SaveException;
                return repositoryResponse;
            }
            repositoryResponse.Value = registerUserData;
            return repositoryResponse;
        }
    }
}
