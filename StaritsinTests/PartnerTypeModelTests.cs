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
    public class PartnerTypeModelTests
    {
        [TestMethod]
        public void PartnerType_WhenCreated_HasEmptyCollectionOfPartners()
        {
            var partnerType = new PartnerType();

            Assert.IsNotNull(partnerType.Partners);
        }

        [TestMethod]
        public void PartnerType_WhenCreated_DefaultTypeNameIsEmpty()
        {
            var partnerType = new PartnerType();
            Assert.AreEqual(string.Empty, partnerType.TypeName);
        }

        [TestMethod]
        public void PartnerType_WhenAssigned_StoresTypeName()
        {
            var partnerType = new PartnerType();
            partnerType.TypeName = "ООО";
            Assert.AreEqual("ООО", partnerType.TypeName);
        }

        [TestMethod]
        public void PartnerType_WhenAssigned_StoresId()
        {
            var partnerType = new PartnerType();
            partnerType.Id = 3;
            Assert.AreEqual(3, partnerType.Id);
        }
    }
}
