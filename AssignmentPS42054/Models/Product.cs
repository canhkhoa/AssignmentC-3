using System;
using System.ComponentModel.DataAnnotations;

namespace AssignmentPS42054.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the product name.")]
        [StringLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the price.")]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a valid price.")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Please enter the quantity.")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid quantity.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please select a create date.")]
        public DateTime CreateDate { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        public int? Review { get; set; }

        public string? ProductImage { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
    }
}
