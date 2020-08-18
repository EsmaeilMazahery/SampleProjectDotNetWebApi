
using System.ComponentModel.DataAnnotations;
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.Models
{
   public class PropertiseOption
    {
        [Display(Name = "Key")]
        [Required]
        [Key]
        public string key { set; get; }

        public string text { set; get; }

        public PropertiseKey propertiseKey { set; get; }
        public virtual Propertise propertise { set; get; }
    }
}
