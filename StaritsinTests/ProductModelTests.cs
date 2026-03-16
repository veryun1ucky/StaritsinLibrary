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
    public class ProductModelTests
    {
        [TestMethod]
        public void Product_WhenCreated_HasEmptyCollectionOfSales()
        {
            var product = new Product();

            Assert.IsNotNull(product.Sales);
        }

        [TestMethod]
        public void Product_WhenCreated_DefaultUnitIsItem()
        {
            var product = new Product();
            Assert.AreEqual("шт", product.Unit);
        }

        [TestMethod]
        public void Product_WhenCreated_DefaultProductNameIsEmpty()
        {
            var product = new Product();
            Assert.AreEqual(string.Empty, product.ProductName);
        }

        [TestMethod]
        public void Product_WhenAssigned_StoresProductName()
        {
            var product = new Product();
            product.ProductName = "Ламинат премиум";
            Assert.AreEqual("Ламинат премиум", product.ProductName);
        }

        [TestMethod]
        public void Product_WhenAssigned_StoresUnit()
        {
            var product = new Product();
            product.Unit = "м²";
            Assert.AreEqual("м²", product.Unit);
        }

        [TestMethod]
        public void Product_WhenAssigned_StoresId()
        {
            var product = new Product();
            product.Id = 5;
            Assert.AreEqual(5, product.Id);
        }
    }
}
