﻿using System;
using System.Threading.Tasks;
using Cashed.DataAccess.Contract;
using Cashed.DataAccess.Model.Base;
using Cashed.Logic.Contract;
using Cashed.Logic.Contract.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Cashed.Logic.Tests
{
    [TestClass]
    public class CategoriesCommandsTests
    {
        private readonly Mock<INamedModelQueryRepository<Category>> _queriesMock;
        private readonly Mock<ICommandRepository<Category>> _commandsMock;
        private readonly Mock<IProductQueries> _productQueriesMock;
        private readonly Mock<IProductCommands> _productCommandsMock;
        private readonly Mock<IUnitOfWork> _uowMock;

        public CategoriesCommandsTests()
        {
            _queriesMock = new Mock<INamedModelQueryRepository<Category>>();
            _queriesMock.Setup(m => m.GetByName(It.IsAny<string>())).Returns<string>((s) => Task.FromResult<Category>(null));
            _commandsMock = new Mock<ICommandRepository<Category>>();
            _commandsMock.Setup(m => m.Create(It.IsAny<Category>()));
            _productQueriesMock = new Mock<IProductQueries>();
            _productCommandsMock = new Mock<IProductCommands>();
            _uowMock = new Mock<IUnitOfWork>();
            _uowMock.Setup(m => m.GetQueryRepository<Category>()).Returns(_queriesMock.Object);
            _uowMock.Setup(m => m.GetCommandRepository<Category>()).Returns(_commandsMock.Object);
            _uowMock.Setup(m => m.GetNamedModelQueryRepository<Category>()).Returns(_queriesMock.Object);
            _uowMock.Setup(m => m.Commit()).Returns(Task.Run(() => { }));
        }

        [TestMethod]
        public void UpdateWithUniqueName_UpdatesNormal()
        {

            var categoriesCommands = new CategoriesCommands(
                _uowMock.Object, _productCommandsMock.Object, _productQueriesMock.Object);
            var model = new CategoryModel { Id = -1, Name = "New category" };
            var t = categoriesCommands.Update(model);
            t.Wait();

            _commandsMock.Verify(m => m.Create(It.IsAny<Category>()));
            _uowMock.Verify(m => m.Commit());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateWithNonUniqueName_ThrowsArgumentException()
        {
            _queriesMock.Setup(m => m.GetByName(It.IsAny<string>())).Returns<string>(
                s => Task.FromResult(new Category()));

            var categoriesCommands = new CategoriesCommands(
                _uowMock.Object, _productCommandsMock.Object, _productQueriesMock.Object);
            var model = new CategoryModel { Id = -1, Name = "New category" };
            try
            {
                categoriesCommands.Update(model).Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions != null)
                    throw ex.InnerException;
                throw;
            }
        }
    }
}
