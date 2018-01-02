using System;
using System.Threading.Tasks;
using BibleAppCore.DataLayer;
using BibleAppCore.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BibleAppTests
{
    [TestFixture]
    public class BooksControllerTests
    {
        [Test]
        public async Task GetBooksTest()
        {
            //IRepository repository = new Repository(new BibleDbContext(new DbContextOptions<BibleDbContext>()).);
        }

    }
}
