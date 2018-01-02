using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BibleAppApi.Models;
using BibleAppCore.Contracts.Contract.Security;
using BibleAppCore.Contracts.Contract.ViewModel;
using BibliaApp;

namespace BibleAppCore
{
    public class CoreProfile : Profile
    {
        public CoreProfile()
        {
            CreateMap<Book, Subbook>();
            CreateMap<Credentials, BearerToken>();
            CreateMap<User, BearerToken>();
            CreateMap<RegisterUserData, User>();
        }
    }
}
