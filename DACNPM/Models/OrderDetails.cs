using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
#nullable disable
namespace DACNPM.Models
{
    public class OrderDetails
    {
        [Key]

        public int OrderDetailID { get; set; }
        public int Quantity { get; set; }
        public double SubPrice { get; set; }


        public string ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product? Product { get; set; }
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order? Order { get; set; }
    }
}
