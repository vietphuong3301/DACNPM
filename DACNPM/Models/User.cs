using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;
#nullable disable
namespace DACNPM.Models
{
    public class User
    {
        [Key]
        [Display(Name = "ID")]
        [StringLength(20)]

        public string UserID { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }

        [Display(Name = "Address")]
        public string FullAddress { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created At")]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public Role? Role { get; set; }
    }
}
