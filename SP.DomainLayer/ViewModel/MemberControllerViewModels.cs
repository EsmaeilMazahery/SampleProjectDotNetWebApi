
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.ViewModel
{

    public class ExtraInfoUser
    {
        public List<TimedEvent> timedEvents { set; get; } = new List<TimedEvent>();
    }

    public class TimedEvent
    {
        public DateTime dateTime { set; get; } = DateTime.Now;
            
        public string type { set; get; }
    }

    public class ListMemberModel
    {
        [Display(Name = "نام")]
        public string name { get; set; }

        public PaggingViewModelBase pagging { set; get; }

        public List<ListItemMemberModel> list { set; get; }
    }

    public class ListItemMemberModel
    {
        public int userId { get; set; }
        [Display(Name = "نام")]
        public string name { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string family { get; set; }

        [Display(Name = "آدرس ایمیل")]
        public string email { set; get; }

        [Display(Name = "تلفن همراه")]
        public string mobile { get; set; }
    }

    public class ListLogMemberModel
    {
        public int? userId { get; set; }

        public LogUserType? type { get; set; }
        public DateTime? fromDate { set; get; } = null;
        public DateTime? toDate { set; get; } = null;

        public PaggingViewModelBase pagging { set; get; }

        public List<ListLogItemMemberModel> list { set; get; }
    }

    public class ListLogItemMemberModel
    {
        public DateTime dateTime { set; get; }

        public string description { set; get; }
        public int logUserId { set; get; }

        public LogUserType type { set; get; }
        public int userId { set; get; }
    }




    public class RegisterMemberViewModel
    {

        [Display(Name = "نام")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string family { get; set; }

        [Display(Name = "تاریخ تولد")]
        public DateTime birthDay { set; get; } = DateTime.Now;


        [Display(Name = "تصویر پروفایل")]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { get; set; }


        [Display(Name = "رمزعبور")]
        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

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

        [Display(Name = "نقش ها")]
        public List<RolesKey> roles { get; set; }


    }

    public class EditMemberViewModel
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

        [Display(Name = "تاریخ تولد")]
        public DateTime birthDay { set; get; } = DateTime.Now;

        [Display(Name = "رمزعبور")]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [Display(Name = "آدرس ایمیل")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { set; get; }


        [Display(Name = "تصویر پروفایل")]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { get; set; }


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

        [Display(Name = "نقش ها")]
        public List<RolesKey> roles { get; set; }

    }
}