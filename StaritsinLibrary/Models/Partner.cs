using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Models
{
    public class Partner
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите тип партнёра")]
        public int PartnerTypeId { get; set; }

        [Required(ErrorMessage = "Укажите наименование")]
        [MaxLength(255)]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите ФИО директора")]
        [MaxLength(255)]
        public string DirectorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите email")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите телефон")]
        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите адрес")]
        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;


        [Range(0, int.MaxValue, ErrorMessage = "Рейтинг должен быть >= 0")]
        public int Rating { get; set; }

        public PartnerType PartnerType { get; set; }
        public ICollection<PartnerSale> Sales { get; set; } = new List<PartnerSale>();
    }
}
