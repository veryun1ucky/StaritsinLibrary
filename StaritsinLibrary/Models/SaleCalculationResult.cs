using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Models
{
    public class SaleCalculationResultDTO
    {
        public decimal BasePrice { get; set; }

        public int DiscountPercent { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
