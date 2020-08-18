using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertMemberViewModel
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

        public bool enable { get; set; } = true;

        public DateTime registerDate { set; get; } = DateTime.Now;

    }

    public class UpdateMemberViewModel
    {
        [Key]
        public int memberId { get; set; }

        [Required]
        [MaxLength(ConstantValidations.UsernameLength)]
        public string username { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string family { get; set; }

        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        [RegularExpression(ConstantValidations.EmailRegEx)]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { get; set; }

        public bool enable { get; set; } = true;

        public DateTime registerDate { set; get; } = DateTime.Now;
    }
}
