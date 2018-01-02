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
