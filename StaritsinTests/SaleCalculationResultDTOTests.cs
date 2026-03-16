using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaritsinLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinTests
{
    [TestClass]
    public class SaleCalculationResultDTOTests
    {
        [TestMethod]
        public void SaleCalculationResultDTO_WhenCreated_DefaultValuesAreZero()
        {
            var dto = new SaleCalculationResultDTO();

            Assert.AreEqual(0m, dto.BasePrice);
            Assert.AreEqual(0, dto.DiscountPercent);
            Assert.AreEqual(0m, dto.UnitPrice);
            Assert.AreEqual(0m, dto.TotalAmount);
        }

        [TestMethod]
        public void SaleCalculationResultDTO_WhenAssigned_StoresBasePrice()
        {
            var dto = new SaleCalculationResultDTO();
            dto.BasePrice = 1500.00m;
            Assert.AreEqual(1500.00m, dto.BasePrice);
        }

        [TestMethod]
        public void SaleCalculationResultDTO_WhenAssigned_StoresDiscountPercent()
        {
            var dto = new SaleCalculationResultDTO();
            dto.DiscountPercent = 10;
            Assert.AreEqual(10, dto.DiscountPercent);
        }

        [TestMethod]
        public void SaleCalculationResultDTO_WhenAssigned_StoresUnitPrice()
        {
            var dto = new SaleCalculationResultDTO();
            dto.UnitPrice = 1350.00m;
            Assert.AreEqual(1350.00m, dto.UnitPrice);
        }

        [TestMethod]
        public void SaleCalculationResultDTO_WhenAssigned_StoresTotalAmount()
        {
            var dto = new SaleCalculationResultDTO();
            dto.TotalAmount = 6750.00m;
            Assert.AreEqual(6750.00m, dto.TotalAmount);
        }

        [TestMethod]
        public void SaleCalculationResultDTO_UnitPriceLessThanBasePrice_WhenDiscountIsPositive()
        {
            var dto = new SaleCalculationResultDTO
            {
                BasePrice = 1000m,
                DiscountPercent = 10,
                UnitPrice = 900m
            };

            Assert.IsTrue(dto.UnitPrice < dto.BasePrice);
        }

        [TestMethod]
        public void SaleCalculationResultDTO_UnitPriceEqualsBasePrice_WhenDiscountIsZero()
        {
            var dto = new SaleCalculationResultDTO
            {
                BasePrice = 1000m,
                DiscountPercent = 0,
                UnitPrice = 1000m
            };

            Assert.AreEqual(dto.BasePrice, dto.UnitPrice);
        }
    }
}
