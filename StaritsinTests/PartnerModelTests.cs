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
    public class PartnerModelTests
    {

        [TestMethod]
        public void Partner_WhenCreated_HasEmptyCollectionOfSales()
        {
            var partner = new Partner();

            Assert.IsNotNull(partner.Sales);
            Assert.AreEqual(0, ((List<PartnerSale>)partner.Sales).Count);
        }

        [TestMethod]
        public void Partner_WhenCreated_DefaultRatingIsZero()
        {
            var partner = new Partner();
            Assert.AreEqual(0, partner.Rating);
        }

        [TestMethod]
        public void Partner_WhenCreated_DefaultCompanyNameIsEmpty()
        {
            var partner = new Partner();
            Assert.AreEqual(string.Empty, partner.CompanyName);
        }


        [TestMethod]
        public void Partner_WhenAssigned_StoresCompanyName()
        {
            var partner = new Partner();

            partner.CompanyName = "СтройМастер";

            Assert.AreEqual("СтройМастер", partner.CompanyName);
        }

        [TestMethod]
        public void Partner_WhenAssigned_StoresRating()
        {
            var partner = new Partner();
            partner.Rating = 8;
            Assert.AreEqual(8, partner.Rating);
        }

        [TestMethod]
        public void Partner_WhenAssigned_StoresDirectorName()
        {
            var partner = new Partner();
            partner.DirectorName = "Иванов Иван Иванович";
            Assert.AreEqual("Иванов Иван Иванович", partner.DirectorName);
        }

        [TestMethod]
        public void Partner_WhenAssigned_StoresEmail()
        {
            var partner = new Partner();
            partner.Email = "info@company.ru";
            Assert.AreEqual("info@company.ru", partner.Email);
        }

        [TestMethod]
        public void Partner_WhenAssigned_StoresPhone()
        {
            var partner = new Partner();
            partner.Phone = "+7 495 123 45 67";
            Assert.AreEqual("+7 495 123 45 67", partner.Phone);
        }

        [TestMethod]
        public void Partner_WhenAssigned_StoresAddress()
        {
            var partner = new Partner();
            partner.Address = "г. Москва, ул. Ленина, д. 1";
            Assert.AreEqual("г. Москва, ул. Ленина, д. 1", partner.Address);
        }


        [TestMethod]
        public void Partner_WhenPartnerTypeAssigned_StoresPartnerType()
        {
            var partnerType = new PartnerType { Id = 1, TypeName = "ООО" };
            var partner = new Partner();

            partner.PartnerType = partnerType;
            partner.PartnerTypeId = partnerType.Id;

            Assert.AreEqual("ООО", partner.PartnerType.TypeName);
            Assert.AreEqual(1, partner.PartnerTypeId);
        }

        [TestMethod]
        public void Partner_WhenSaleAdded_SalesCollectionContainsIt()
        {
            var partner = new Partner();
            var sale = new PartnerSale { Quantity = 500 };

            partner.Sales = new List<PartnerSale> { sale };

            var salesList = (List<PartnerSale>)partner.Sales;
            Assert.AreEqual(1, salesList.Count);
            Assert.AreEqual(500, salesList[0].Quantity);
        }
    }
}
