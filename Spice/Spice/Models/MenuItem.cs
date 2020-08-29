using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Spicyness { get; set; }
        public enum Spicy_Enum { NotSpicy, Level1, Level2, Level3 }
        public string Image { get; set; }
        // Foreign Keys
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [DisplayName("Subcategory")]
        public int SubCategoryId { get; set; } 
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }
        [Range(1, int.MaxValue, ErrorMessage ="It has to be bigger than $1")]
        public double Price { get; set; }
    }
}
