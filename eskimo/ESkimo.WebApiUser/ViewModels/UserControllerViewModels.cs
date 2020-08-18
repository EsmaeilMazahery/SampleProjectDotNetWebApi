using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertUserViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string username { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string family { get; set; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { get; set; }

        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        [RegularExpression(ConstantValidations.EmailRegEx)]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { get; set; }

        public bool enable { get; set; }
        
        public List<RolesKey> selectedRoles { set; get; }
    }

    public class UpdateUserViewModel
    {
        [Key]
        public int userId { get; set; }

        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string username { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string family { get; set; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { get; set; }
        
        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        [RegularExpression(ConstantValidations.EmailRegEx)]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { get; set; }

        public bool enable { get; set; }

        public List<RolesKey> selectedRoles { set; get; }
    }
}
