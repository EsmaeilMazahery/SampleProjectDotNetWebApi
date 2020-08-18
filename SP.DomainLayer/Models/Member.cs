
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SP.DomainLayer.ViewModel;
using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using Newtonsoft.Json;

namespace SP.DomainLayer.Models
{
    public class Member
    {
        [Key]
        public int memberId { get; set; }

        [Display(Name = "نام")]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string family { get; set; }

        [Display(Name = "تصویر پروفایل")]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { get; set; }

        [Display(Name = "رمزعبور")]
        [Required]
        [MaxLength(ConstantValidations.PasswordLength)]
        public string password { get; set; }

        [MaxLength(ConstantValidations.PasswordLength)]
        public string newPassword { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required]
        [RegularExpression(ConstantValidations.MobileRegEx)]
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string mobile { get; set; }

        [Display(Name = "تایید موبایل")]
        public bool verifyMobile { set; get; }

        [Display(Name = "ایمیل")]
        [RegularExpression(ConstantValidations.EmailRegEx)]
        [MaxLength(ConstantValidations.EmailLength)]
        public string email { get; set; }

        [Display(Name = "تایید ایمیل")]
        public bool verifyEmail { set; get; }

        [Display(Name = "فعال")]
        public bool enable { get; set; }

        [Display(Name = "تکمیل ثبت نام")]
        public bool complate { get; set; } = false;

        [Display(Name = "در حال حذف")]
        public bool deletePending { set; get; } = false;

        [MaxLength(ConstantValidations.DescriptionLength)]
        public string tokenFireBase { set; get; }

        [Display(Name = "زمان حذف")]
        public DateTime deleteTimePending { set; get; } = DateTime.Now;

        public DateTime registerDate { set; get; } = DateTime.Now;
        public int countLogin { set; get; } = 0;

        public VerifiedStatus verified { set; get; } = VerifiedStatus.NotChecked;

        public string extraInfo { set; get; }

        //searchScore
        public decimal searchScore { set; get; }

        [NotMapped]
        public ExtraInfoUser ExtraInfoUser
        {
            get
            {
                if (!string.IsNullOrEmpty(extraInfo))
                    return JsonConvert.DeserializeObject<ExtraInfoUser>(extraInfo);
                else return new ExtraInfoUser();
            }
            set
            {
                extraInfo = JsonConvert.SerializeObject(value);
            }
        }

        [Display(Name = "آخرین بروزرسانی اطلاعات")]
        public DateTime lastUpdate { get; set; }


        public virtual IList<Media> medias { set; get; }
        public virtual IList<Service> services { set; get; }
        public virtual IList<UserVisit> userVisits { set; get; }
        public virtual IList<LogUser> logUsers { set; get; }
        public virtual IList<Notify> Notifies { set; get; }

        public virtual IList<SmsLog> smsLogs { set; get; }
    }
}
