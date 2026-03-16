using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaritsinLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinTests
{
    [TestClass]
    public class DiscountServiceTests
    {
        private DiscountService _discountService;

        [TestInitialize]
        public void Setup()
        {
            _discountService = new DiscountService();
        }


        [TestMethod]
        public void CalculateDiscount_WhenQuantityIsZero_ReturnsZero()
        {
            int quantity = 0;

            int discount = _discountService.CalculateDiscount(quantity);

            Assert.AreEqual(0, discount);
        }

        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs9999_ReturnsZero()
        {
            Assert.AreEqual(0, _discountService.CalculateDiscount(9999));
        }


        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs10000_ReturnsFivePercent()
        {
            Assert.AreEqual(5, _discountService.CalculateDiscount(10000));
        }

        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs30000_ReturnsFivePercent()
        {
            Assert.AreEqual(5, _discountService.CalculateDiscount(30000));
        }

        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs49999_ReturnsFivePercent()
        {
            Assert.AreEqual(5, _discountService.CalculateDiscount(49999));
        }


        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs50000_ReturnsTenPercent()
        {
            Assert.AreEqual(10, _discountService.CalculateDiscount(50000));
        }

        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs175000_ReturnsTenPercent()
        {
            Assert.AreEqual(10, _discountService.CalculateDiscount(175000));
        }

        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs299999_ReturnsTenPercent()
        {
            Assert.AreEqual(10, _discountService.CalculateDiscount(299999));
        }


        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs300000_ReturnsFifteenPercent()
        {
            Assert.AreEqual(15, _discountService.CalculateDiscount(300000));
        }

        [TestMethod]
        public void CalculateDiscount_WhenQuantityIs1000000_ReturnsFifteenPercent()
        {
            Assert.AreEqual(15, _discountService.CalculateDiscount(1000000));
        }
    }
}
