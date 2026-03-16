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
    public class ProductModelUpdatedTests
    {
        [TestMethod]
        public void Product_WhenCreated_DefaultPriceIsZero()
        {
            var product = new Product();
            Assert.AreEqual(0m, product.Price);
        }

        [TestMethod]
        public void Product_WhenAssigned_StoresPrice()
        {
            var product = new Product();
            product.Price = 1200.50m;
            Assert.AreEqual(1200.50m, product.Price);
        }

        [TestMethod]
        public void Product_WhenPriceIsZero_StoresCorrectly()
        {
            var product = new Product();
            product.Price = 0m;
            Assert.AreEqual(0m, product.Price);
        }

        [TestMethod]
        public void Product_WhenPriceIsLarge_StoresCorrectly()
        {
            var product = new Product();
            product.Price = 999999.99m;
            Assert.AreEqual(999999.99m, product.Price);
        }
    }
}
