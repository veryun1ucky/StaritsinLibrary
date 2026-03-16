using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Models
{
    public class PartnerSale
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Today;

        public decimal BasePrice { get; set; }

        public int DiscountPercent { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }

        public string Comment { get; set; }

        public Partner Partner { get; set; }
        public Product Product { get; set; }
    }
}
