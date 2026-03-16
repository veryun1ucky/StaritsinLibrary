using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Unit { get; set; } = "шт";

        public decimal Price { get; set; }

        public ICollection<PartnerSale> Sales { get; set; } = new List<PartnerSale>();
    }
}
