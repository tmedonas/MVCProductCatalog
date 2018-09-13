using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tm_api_mvc.Models
{
    public class ProductDto
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; } = DateTime.Now;
    }
}