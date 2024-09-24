using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable
namespace DACNPM.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Product ID")]
        [StringLength(20)]

        public string ProductID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Price")]
        public double Price { get; set; }

        [Display(Name = "Image")]
        public string ProductImage { get; set; }
        [Display(Name = "Image1")]
        public string ProductImage1 { get; set; }
        [Display(Name = "Image2")]
        public string ProductImage2 { get; set; }
        [Display(Name = "Image3")]
        public string ProductImage3 { get; set; }

        public string Description { get; set; }
        public int Stock { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Created At")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public string TypeID { get; set; }
        [ForeignKey("TypeID")]
        public ProductType? ProductType { get; set; }
        public string CategoryID { get; set; }
        [ForeignKey("CategoryID")]

        public Category? Category { get; set; }
    }
}
