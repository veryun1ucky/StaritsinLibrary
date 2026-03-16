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
    public class SaleListItemDTOTests
    {
        [TestMethod]
        public void SaleListItemDTO_WhenCreated_DefaultStringPropertiesAreEmpty()
        {
            var dto = new SaleListItemDTO();

            Assert.AreEqual(string.Empty, dto.PartnerName);
            Assert.AreEqual(string.Empty, dto.ProductName);
        }

        [TestMethod]
        public void SaleListItemDTO_WhenAssigned_StoresAllNumericFields()
        {
            var dto = new SaleListItemDTO();

            dto.Id = 42;
            dto.PartnerId = 1;
            dto.ProductId = 2;
            dto.Quantity = 500;
            dto.BasePrice = 1000.00m;
            dto.DiscountPercent = 10;
            dto.UnitPrice = 900.00m;
            dto.TotalAmount = 450000.00m;

            Assert.AreEqual(42, dto.Id);
            Assert.AreEqual(1, dto.PartnerId);
            Assert.AreEqual(2, dto.ProductId);
            Assert.AreEqual(500, dto.Quantity);
            Assert.AreEqual(1000.00m, dto.BasePrice);
            Assert.AreEqual(10, dto.DiscountPercent);
            Assert.AreEqual(900.00m, dto.UnitPrice);
            Assert.AreEqual(450000.00m, dto.TotalAmount);
        }

        [TestMethod]
        public void SaleListItemDTO_WhenAssigned_StoresPartnerName()
        {
            var dto = new SaleListItemDTO();
            dto.PartnerName = "СтройМастер";
            Assert.AreEqual("СтройМастер", dto.PartnerName);
        }

        [TestMethod]
        public void SaleListItemDTO_WhenAssigned_StoresProductName()
        {
            var dto = new SaleListItemDTO();
            dto.ProductName = "Паркет тип 1";
            Assert.AreEqual("Паркет тип 1", dto.ProductName);
        }

        [TestMethod]
        public void SaleListItemDTO_WhenAssigned_StoresSaleDate()
        {
            var dto = new SaleListItemDTO();
            var expectedDate = new DateTime(2025, 3, 20);
            dto.SaleDate = expectedDate;
            Assert.AreEqual(expectedDate, dto.SaleDate);
        }

        [TestMethod]
        public void SaleListItemDTO_TotalAmount_ConsistentWithUnitPriceAndQuantity()
        {
            var dto = new SaleListItemDTO
            {
                UnitPrice = 900.00m,
                Quantity = 5,
                TotalAmount = 4500.00m   
            };

            decimal expected = dto.UnitPrice * dto.Quantity;
            Assert.AreEqual(expected, dto.TotalAmount);
        }
    }
}
