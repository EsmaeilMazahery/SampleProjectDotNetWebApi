using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SP.Infrastructure.Constants;

namespace SP.DomainLayer.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Display(Name = "نام")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string family { get; set; }

        [Display(Name = "تصویر پروفایل")]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime birthDay { set; get; } = DateTime.Now;

        [Display(Name = "رمزعبور")]
        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [MaxLength(ConstantValidations.PasswordLength)]
        public string newPassword { get; set; }

        [Display(Name = "آدرس ایمیل")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { set; get; }

        [Display(Name = "تلفن همراه")]
        [Required]
        [RegularExpression(@"^9[0-9]{9}$")]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        [Display(Name = "آدرس")]
        [MaxLength(ConstantValidations.AddressLength)]
        public string address { set; get; }

        [Display(Name = "توضیحات")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        [Display(Name = "فعال")]
        public bool enable { get; set; }

        public virtual IList<Rel_RolesUsers> rolesUsers { get; set; }
        public virtual IList<UserSmsMessage> userSmsMessages { get; set; }
        public virtual IList<UserSessionUpdate> userSessionUpdates { get; set; }
        public virtual IList<SmsLog> smsLogs { get; set; }

    }
}