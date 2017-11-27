using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model;
using Cashed.DataAccess.Model.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashed.DataAccess.Db.Tests
{
    [TestClass]
    public class CashedDbContextTests
    {
        [TestMethod]
        public void QueryProductRepository_ReturnsProductRepository()
        {
            var uof = new CashedDatabaseUnitOfWork(new CashedDbContext());
            var r = uof.GetQueryRepository<Product>();
            Assert.IsTrue(r is IQueryRepository<Product>);
        }

        [TestMethod]
        public void QuerySameRepository_ReturnsSameObject()
        {
            var uof = new CashedDatabaseUnitOfWork(new CashedDbContext());
            var r1 = uof.GetQueryRepository<Category>();
            var r2 = uof.GetQueryRepository<Category>();
            Assert.IsTrue(object.ReferenceEquals(r1, r2));
        }

        [TestMethod]
        public void QueryCommandRepository_ReturnsCategoryRepository()
        {
            var uof = new CashedDatabaseUnitOfWork(new CashedDbContext());
            var r = uof.GetCommandRepository<Category>();
            Assert.IsTrue(r is ICommandRepository<Category>);
        }

        [TestMethod]
        public void QuerySameCommandRepository_ReturnsSameObject()
        {
            var uof = new CashedDatabaseUnitOfWork(new CashedDbContext());
            var r1 = uof.GetCommandRepository<Category>();
            var r2 = uof.GetCommandRepository<Category>();
            Assert.IsTrue(object.ReferenceEquals(r1, r2));
        }
    }
}
