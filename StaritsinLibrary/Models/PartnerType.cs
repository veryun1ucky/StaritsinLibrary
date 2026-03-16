using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Models
{
    public class PartnerType
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;

        public ICollection<Partner> Partners { get; set; } = new List<Partner>();
    }
}
