using Logic.Cashed.Contract.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashed.Logic.Tests
{
    [TestClass]
    public class PaginationInfoTests
    {
        [TestMethod]
        public void FromArgs_FirstPage10TotalItems_ReturnsRightTotalPagesCount()
        {
            var args = new GetModelListArgs
            {
                PageNumber = 1,
                ItemsPerPage = 5,
                IncludeDeleted = false
            };
            var info = PaginationInfo.FromArgs(args, 10);
            
            Assert.AreEqual(1, info.PageNumber);
            Assert.AreEqual(5, info.ItemsPerPage);
            Assert.AreEqual(2, info.TotalPageCount);
            Assert.AreEqual(0, info.Skipped);
            Assert.AreEqual(5, info.Taken);
        }

        [TestMethod]
        public void FromArgs_FirstPageZeroTotalItems_ReturnsRightTotalPagesCount()
        {
            var args = new GetModelListArgs
            {
                PageNumber = 1,
                ItemsPerPage = 5,
                IncludeDeleted = false
            };
            var info = PaginationInfo.FromArgs(args, 0);

            Assert.AreEqual(1, info.PageNumber);
            Assert.AreEqual(5, info.ItemsPerPage);
            Assert.AreEqual(0, info.TotalPageCount);
            Assert.AreEqual(0, info.Skipped);
            Assert.AreEqual(5, info.Taken);
        }

        [TestMethod]
        public void FromArgs_FirstPage9TotalItems_ReturnsRightTotalPagesCount()
        {
            var args = new GetModelListArgs
            {
                PageNumber = 2,
                ItemsPerPage = 5,
                IncludeDeleted = false
            };
            var info = PaginationInfo.FromArgs(args, 9);

            Assert.AreEqual(2, info.PageNumber);
            Assert.AreEqual(5, info.ItemsPerPage);
            Assert.AreEqual(2, info.TotalPageCount);
            Assert.AreEqual(5, info.Skipped);
            Assert.AreEqual(5, info.Taken);
        }

        [TestMethod]
        public void FromArgs_LastPageTotalItems_ReturnsRightPageNumber()
        {
            var args = new GetModelListArgs
            {
                PageNumber = -1,
                ItemsPerPage = 5,
                IncludeDeleted = false
            };
            var info = PaginationInfo.FromArgs(args, 14);

            Assert.AreEqual(3, info.PageNumber);
            Assert.AreEqual(5, info.ItemsPerPage);
            Assert.AreEqual(3, info.TotalPageCount);
            Assert.AreEqual(10, info.Skipped);
            Assert.AreEqual(5, info.Taken);
        }
    }
}