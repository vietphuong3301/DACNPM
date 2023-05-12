using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
#nullable disable
namespace DACNPM.Models
{
    public class ProductType
    {
        [Key]
        [Display(Name = "Type ID")]
        [StringLength(20)]

        public string TypeID { get; set; }

        [Display(Name = "Type Name")]
        [StringLength(200, MinimumLength = 2)]

        public string Type { get; set; }
    }
}
