using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tm_api.Models
{
    public class ProductDto
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        private byte[] _photo;
        public byte[] Photo
        {
            get { return _photo; }
            set
            {
                if (value != null)//to keep same photo while editing
                    _photo = value;
            }
        }
        [Required]
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}