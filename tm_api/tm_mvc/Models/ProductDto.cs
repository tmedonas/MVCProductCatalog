using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tm_mvc.Models
{
    public class ProductDto
    {
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        [Range(0, 999)]
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}