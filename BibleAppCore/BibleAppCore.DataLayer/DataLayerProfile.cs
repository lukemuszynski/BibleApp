using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BibleAppApi.Models;
using BibleAppCore.Contracts.Contract.ViewModel;
using BibliaApp;

namespace BibleAppCore.DataLayer
{
    public class DataLayerProfile : Profile
    {
        public DataLayerProfile()
        {
            CreateMap<User, UserDomainObject>().ReverseMap();
            CreateMap<Comment, CommentDomainObject>().ReverseMap();
            CreateMap<Passage, PassageDomainObject>().ReverseMap();
            CreateMap<Book, BookDomainObject>().ReverseMap();
            CreateMap<BookExtended, BookExtendedDomainObject>().ReverseMap();
        }
    }
}
