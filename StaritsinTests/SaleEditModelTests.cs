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
    public class SaleEditModelTests
    {
        [TestMethod]
        public void SaleEditModel_WhenCreated_DefaultValuesAreCorrect()
        {
            var model = new SaleEditModel();

            Assert.AreEqual(0, model.Id);
            Assert.AreEqual(0, model.PartnerId);
            Assert.AreEqual(0, model.ProductId);
            Assert.AreEqual(0, model.Quantity);
        }

        [TestMethod]
        public void SaleEditModel_WhenAssigned_StoresPartnerId()
        {
            var model = new SaleEditModel();
            model.PartnerId = 5;
            Assert.AreEqual(5, model.PartnerId);
        }

        [TestMethod]
        public void SaleEditModel_WhenAssigned_StoresProductId()
        {
            var model = new SaleEditModel();
            model.ProductId = 3;
            Assert.AreEqual(3, model.ProductId);
        }

        [TestMethod]
        public void SaleEditModel_WhenAssigned_StoresQuantity()
        {
            var model = new SaleEditModel();
            model.Quantity = 100;
            Assert.AreEqual(100, model.Quantity);
        }

        [TestMethod]
        public void SaleEditModel_WhenAssigned_StoresSaleDate()
        {
            var model = new SaleEditModel();
            var expectedDate = new DateTime(2025, 6, 15);

            model.SaleDate = expectedDate;

            Assert.AreEqual(expectedDate, model.SaleDate);
        }

        [TestMethod]
        public void SaleEditModel_WhenAssigned_StoresComment()
        {
            var model = new SaleEditModel();
            model.Comment = "Тестовый комментарий";
            Assert.AreEqual("Тестовый комментарий", model.Comment);
        }

        [TestMethod]
        public void SaleEditModel_WhenCommentIsNull_DoesNotThrow()
        {
            var model = new SaleEditModel();
            model.Comment = null;
            Assert.IsNull(model.Comment);
        }
    }
}
