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
    public class PartnerSaleModelTests
    {
        [TestMethod]
        public void PartnerSale_WhenCreated_DefaultSaleDateIsToday()
        {
            var sale = new PartnerSale();

            Assert.AreEqual(DateTime.Today, sale.SaleDate);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresQuantity()
        {
            var sale = new PartnerSale();
            sale.Quantity = 1000;
            Assert.AreEqual(1000, sale.Quantity);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresSaleDate()
        {
            var sale = new PartnerSale();
            var expectedDate = new DateTime(2024, 6, 15);

            sale.SaleDate = expectedDate;

            Assert.AreEqual(expectedDate, sale.SaleDate);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresPartnerId()
        {
            var sale = new PartnerSale();
            sale.PartnerId = 42;
            Assert.AreEqual(42, sale.PartnerId);
        }

        [TestMethod]
        public void PartnerSale_WhenAssigned_StoresProductId()
        {
            var sale = new PartnerSale();
            sale.ProductId = 7;
            Assert.AreEqual(7, sale.ProductId);
        }

        [TestMethod]
        public void PartnerSale_WhenProductAssigned_StoresProduct()
        {
            var product = new Product { Id = 1, ProductName = "Паркет тип 1", Unit = "м²" };
            var sale = new PartnerSale();

            sale.Product = product;
            sale.ProductId = product.Id;

            Assert.AreEqual("Паркет тип 1", sale.Product.ProductName);
            Assert.AreEqual("м²", sale.Product.Unit);
        }
    }
}
