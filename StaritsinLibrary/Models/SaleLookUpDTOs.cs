using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Models
{
    public class SalePartnerLookupDTO
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }


    public class SaleProductLookupDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
