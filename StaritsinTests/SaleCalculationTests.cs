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
    public class SaleCalculationTests
    {
        private readonly DiscountService _discountService = new DiscountService();


        [TestMethod]
        public void UnitPrice_WhenDiscountIsZero_EqualsBasePrice()
        {

            decimal basePrice = 1000m;
            int discountPercent = _discountService.CalculateDiscount(0);

            decimal unitPrice = CalculateUnitPrice(basePrice, discountPercent);

            Assert.AreEqual(1000.00m, unitPrice);
        }

        [TestMethod]
        public void UnitPrice_WhenDiscountIsFivePercent_CorrectlyReduced()
        {
            decimal basePrice = 1000m;
            int discountPercent = _discountService.CalculateDiscount(10000);

            decimal unitPrice = CalculateUnitPrice(basePrice, discountPercent);

            Assert.AreEqual(950.00m, unitPrice);
        }

        [TestMethod]
        public void UnitPrice_WhenDiscountIsTenPercent_CorrectlyReduced()
        {
            decimal basePrice = 1000m;
            int discountPercent = _discountService.CalculateDiscount(50000); 

            decimal unitPrice = CalculateUnitPrice(basePrice, discountPercent);

            Assert.AreEqual(900.00m, unitPrice);
        }

        [TestMethod]
        public void UnitPrice_WhenDiscountIsFifteenPercent_CorrectlyReduced()
        {
            decimal basePrice = 1000m;
            int discountPercent = _discountService.CalculateDiscount(300000); 

            decimal unitPrice = CalculateUnitPrice(basePrice, discountPercent);

            Assert.AreEqual(850.00m, unitPrice);
        }

        [TestMethod]
        public void UnitPrice_WhenBasePriceHasDecimals_RoundedToTwoPlaces()
        {
            decimal basePrice = 333.33m;
            int discountPercent = 10; 

            decimal unitPrice = CalculateUnitPrice(basePrice, discountPercent);

            Assert.AreEqual(300.00m, unitPrice);
        }

        [TestMethod]
        public void UnitPrice_WhenBasePriceIsZero_ReturnsZero()
        {
            decimal unitPrice = CalculateUnitPrice(0m, 15);
            Assert.AreEqual(0.00m, unitPrice);
        }


        [TestMethod]
        public void TotalAmount_EqualsUnitPriceMultipliedByQuantity()
        {
            decimal unitPrice = 900.00m;
            int quantity = 5;

            decimal totalAmount = CalculateTotalAmount(unitPrice, quantity);

            Assert.AreEqual(4500.00m, totalAmount);
        }

        [TestMethod]
        public void TotalAmount_WhenQuantityIsOne_EqualUnitPrice()
        {
            decimal unitPrice = 750.50m;
            decimal totalAmount = CalculateTotalAmount(unitPrice, 1);
            Assert.AreEqual(750.50m, totalAmount);
        }

        [TestMethod]
        public void TotalAmount_WhenQuantityIsLarge_CalculatesCorrectly()
        {
            decimal unitPrice = 100.00m;
            int quantity = 10000;

            decimal totalAmount = CalculateTotalAmount(unitPrice, quantity);

            Assert.AreEqual(1000000.00m, totalAmount);
        }


        [TestMethod]
        public void FullCalculation_PartnerWithNoSales_GetsZeroDiscount()
        {
            int totalQuantity = 0;
            decimal basePrice = 500m;
            int quantity = 10;

            int discountPercent = _discountService.CalculateDiscount(totalQuantity);
            decimal unitPrice = CalculateUnitPrice(basePrice, discountPercent);
            decimal totalAmount = CalculateTotalAmount(unitPrice, quantity);

            Assert.AreEqual(0, discountPercent);
            Assert.AreEqual(500m, unitPrice);
            Assert.AreEqual(5000m, totalAmount);
        }

        [TestMethod]
        public void FullCalculation_PartnerWith62000Sales_GetsTenPercentDiscount()
        {
            int totalQuantity = 62000;
            decimal basePrice = 1200m;
            int quantity = 3;

            int discountPercent = _discountService.CalculateDiscount(totalQuantity);
            decimal unitPrice = CalculateUnitPrice(basePrice, discountPercent);
            decimal totalAmount = CalculateTotalAmount(unitPrice, quantity);

            Assert.AreEqual(10, discountPercent);
            Assert.AreEqual(1080m, unitPrice);
            Assert.AreEqual(3240m, totalAmount);
        }

        [TestMethod]
        public void FullCalculation_PartnerWith300000Sales_GetsFifteenPercentDiscount()
        {
            int totalQuantity = 300000;
            decimal basePrice = 800m;
            int quantity = 2;

            int discountPercent = _discountService.CalculateDiscount(totalQuantity);
            decimal unitPrice = CalculateUnitPrice(basePrice, discountPercent);
            decimal totalAmount = CalculateTotalAmount(unitPrice, quantity);

            Assert.AreEqual(15, discountPercent);
            Assert.AreEqual(680m, unitPrice);
            Assert.AreEqual(1360m, totalAmount);
        }


        private static decimal CalculateUnitPrice(decimal basePrice, int discountPercent)
        {
            return System.Math.Round(
                basePrice * (1m - discountPercent / 100m),
                2,
                System.MidpointRounding.AwayFromZero);
        }

        private static decimal CalculateTotalAmount(decimal unitPrice, int quantity)
        {
            return System.Math.Round(
                unitPrice * quantity,
                2,
                System.MidpointRounding.AwayFromZero);
        }
    }
}
