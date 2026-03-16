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
    public class PartnerSaleModelUpdatedTests
    {
        [TestMethod]
        public void PartnerSale_WhenCreated_DefaultDecimalFieldsAreZero()
        {
            var sale = new PartnerSale();

            Assert.AreEqual(0m, sale.BasePrice);
            Assert.AreEqual(0, sale.DiscountPercent);
            Assert.AreEqual(0m, sale.UnitPrice);
            Assert.AreEqual(0m, sale.TotalAmount);
        }

        [TestMethod]
        public void PartnerSale_WhenCreated_DefaultCommentIsNull()
        {
            var sale = new PartnerSale();
            Assert.IsNull(sale.Comment);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresBasePrice()
        {
            var sale = new PartnerSale();
            sale.BasePrice = 1000.00m;
            Assert.AreEqual(1000.00m, sale.BasePrice);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresDiscountPercent()
        {
            var sale = new PartnerSale();
            sale.DiscountPercent = 10;
            Assert.AreEqual(10, sale.DiscountPercent);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresUnitPrice()
        {
            var sale = new PartnerSale();
            sale.UnitPrice = 900.00m;
            Assert.AreEqual(900.00m, sale.UnitPrice);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresTotalAmount()
        {
            var sale = new PartnerSale();
            sale.TotalAmount = 4500.00m;
            Assert.AreEqual(4500.00m, sale.TotalAmount);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresComment()
        {
            var sale = new PartnerSale();
            sale.Comment = "Срочная поставка";
            Assert.AreEqual("Срочная поставка", sale.Comment);
        }

        [TestMethod]
        public void PartnerSale_UnitPriceConsistentWithBaseAndDiscount()
        {
            var sale = new PartnerSale
            {
                BasePrice = 1000m,
                DiscountPercent = 10,
                UnitPrice = 900m
            };

            decimal expected = sale.BasePrice * (1m - sale.DiscountPercent / 100m);
            Assert.AreEqual(expected, sale.UnitPrice);
        }

        [TestMethod]
        public void PartnerSale_TotalAmountConsistentWithUnitPriceAndQuantity()
        {
            var sale = new PartnerSale
            {
                UnitPrice = 900m,
                Quantity = 5,
                TotalAmount = 4500m
            };

            decimal expected = sale.UnitPrice * sale.Quantity;
            Assert.AreEqual(expected, sale.TotalAmount);
        }
    }
}
