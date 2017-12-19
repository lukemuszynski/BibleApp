﻿using BibleAppApi.Models;
using BibleAppCore.DataLayer.Repository;
using BibleAppCore.DataLayer.TransferObjects;
using BibliaApp;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BibleAppCore.Controllers
{
    [Route("api/Books")]
    public class BooksController : Controller
    {
        private IRepository Repository { get; set; }

        public BooksController(IRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("GetBook/{guid}")]
        public async Task<BookExtendedDomainObject> GetBook(Guid guid)
        {

            BookExtendedDomainObject ret = await Repository.GetBookByGuid(guid);

            ret.OnRead();
            return ret;
        }


        [HttpGet("GetBooks")]
        public async Task<List<Book>> GetAllBooks()
        {
            try
            {
                List<Book> result = new List<Book>();
                List<BookDomainObject> bookDomainObjects = await Repository.GetAllBooks();

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

     

    }
}