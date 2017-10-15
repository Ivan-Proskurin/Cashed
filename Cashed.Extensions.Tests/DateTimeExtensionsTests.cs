using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cashed.Extensions.Tests
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        [TestMethod]
        public void StartOfTheMonth_MiddleOfMonth_ReturnStart()
        {
            Assert.AreEqual(new DateTime(2017, 3, 1), new DateTime(2017, 3, 25).StartOfTheMonth());
        }

        [TestMethod]
        public void StartOfTheMonth_EndOfMonth_ReturnStart()
        {
            Assert.AreEqual(new DateTime(2017, 3, 1), new DateTime(2017, 3, 31).StartOfTheMonth());
        }

        [TestMethod]
        public void StartOfTheMonth_StartOfMonth_ReturnStart()
        {
            Assert.AreEqual(new DateTime(2017, 3, 1), new DateTime(2017, 3, 1).StartOfTheMonth());
        }

        [TestMethod]
        public void EndOfTheMonth_MiddleOfMonth_ReturnEnd()
        {
            Assert.AreEqual(new DateTime(2018, 01, 01), new DateTime(2017, 12, 22).EndOfTheMonth());
        }

        [TestMethod]
        public void EndOfTheMonth_StartOfMonth_ReturnEnd()
        {
            Assert.AreEqual(new DateTime(2018, 01, 01), new DateTime(2017, 12, 1).EndOfTheMonth());
        }

        [TestMethod]
        public void EndOfTheMonth_EndOfMonth_ReturnEnd()
        {
            Assert.AreEqual(new DateTime(2018, 01, 01), new DateTime(2017, 12, 31).EndOfTheMonth());
        }

        [TestMethod]
        public void ParseStandard_OkString_ReturnRightDt()
        {
            Assert.AreEqual(new DateTime(2017, 5, 23, 12, 34, 00), "2017.05.23 12:34".ParseDtFromStandardString());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseStandard_LongString_ReturnsException()
        {
            Assert.AreEqual(new DateTime(2017, 5, 23, 12, 34, 00), "2017.05.23 12:34:20.23".ParseDtFromStandardString());
        }
    }
}