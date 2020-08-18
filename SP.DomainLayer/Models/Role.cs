
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.Models
{
    public class Role
    {
        [Key]
        public RolesKey roleKey { set; get; }

        [Display(Name = "نام")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string name { set; get; }

        [Display(Name = "توضیحات")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public virtual IList<Rel_RolesUsers> rolesUsers { get; set; }
    }
}
