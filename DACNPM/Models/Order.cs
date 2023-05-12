using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
#nullable disable
namespace DACNPM.Models
{
    public class Order
    {
        [Display(Name = "Order ID")]
        public int OrderID { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Note")]
        public string? Note { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Created At")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(1)]
        public int Status { get; set; } = 0;


        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
        public List<OrderDetails>? OrderDetails { get; set; }
    }
}
