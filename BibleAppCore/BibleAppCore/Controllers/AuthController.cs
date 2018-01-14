using BibleAppApi.Models;
using BibleAppCore.DataLayer.Repository;
using BibleAppCore.Utilities.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BibleAppCore.Contracts.Contract.ErrorCodes;
using BibleAppCore.Contracts.Contract.Security;
using BibleAppCore.Contracts.Contract.ViewModel;
using BibleAppCore.DataLayer.TransferObjects;
using BibliaApp;

namespace BibleAppCore.Controllers
{
    [Route("api/Auth")]
    public class AuthController : BaseController
    {

        private IAuthRepository Repository { get; set; }
        public IEncyptionProvider EncyptionProvider { get; set; }

        public AuthController(IAuthRepository repository, IEncyptionProvider encyptionProvider)
        {
            Repository = repository;
            EncyptionProvider = encyptionProvider;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]Credentials credentials)
        {
            credentials.Password = EncyptionProvider.HashPassword(credentials.Password);
            var repositoryResponse = await Repository.ValidateCredentails(credentials);
            if (!repositoryResponse.Successful)
                return new ForbidResult();

            BearerToken bearerToken = EncyptionProvider.CreateBearerToken(Mapper.Map<BearerToken>(repositoryResponse.Value));
            return new JsonResult(bearerToken);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserData registerUserData)
        {
            var newUser = Mapper.Map<User>(registerUserData);
            newUser.Password = EncyptionProvider.HashPassword(newUser.Password);
            var repositoryResponse = await Repository.RegisterUser(newUser);
            if (!repositoryResponse.Successful)
            {
                if (repositoryResponse.RepositoryResponseMessage == RepositoryResponse<User>.RepositoryResponseMessageEnum.EmailIndexException)
                    return new BadRequestObjectResult(AuthControllerErrorCodes.EmailUsedError);
                if (repositoryResponse.RepositoryResponseMessage == RepositoryResponse<User>.RepositoryResponseMessageEnum.UserIndexException)
                    return new BadRequestObjectResult(AuthControllerErrorCodes.LoginUsedError);
                if (repositoryResponse.RepositoryResponseMessage == RepositoryResponse<User>.RepositoryResponseMessageEnum.UserIndexAndEmailIndexException)
                    return new BadRequestObjectResult(AuthControllerErrorCodes.LoginAndEmailUsedError);
                return new BadRequestResult();
            }

            BearerToken bearerToken = EncyptionProvider.CreateBearerToken(Mapper.Map<BearerToken>(repositoryResponse.Value));
            return new JsonResult(bearerToken);
        }
    }
}
