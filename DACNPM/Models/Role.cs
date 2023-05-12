using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
#nullable disable
namespace DACNPM.Models
{
    public class Role
    {
        [Key]
        [Display(Name = "Role ID")]
        public int RoleID { get; set; }

        [Display(Name = "Role Name")]
        [StringLength(50, MinimumLength = 4)]
        public string RoleName { get; set; }
    }
}
