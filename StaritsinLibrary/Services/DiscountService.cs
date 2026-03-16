using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Services
{
    public class DiscountService
    {
        public int CalculateDiscount(int totalQuantity)
        {
            if (totalQuantity < 10000) return 0;
            if (totalQuantity < 50000) return 5;
            if (totalQuantity < 300000) return 10;
            return 15;
        }
    }
}
