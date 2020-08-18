
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;

namespace ESkimo.DomainLayer.Models
{
    public class Role
    {
        [Key]
        public RolesKey roleKey { set; get; }

        [Display(Name = "نام")]
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Display(Name = "توضیحات")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        public virtual IList<Rel_RoleUser> users { get; set; }
    }
}
