using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.Models
{
    public class Service
    {
        [Key]
        public int serviceId { get; set; }

        [Display(Name = "نام")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string name { get; set; }

        [Display(Name = "نام محل کار")]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string nameWorkPlace { get; set; }

        [Display(Name = "آدرس سایت")]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string website { get; set; }

        [Display(Name = "تصویر مدرک تحصیلی")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string majorImg { get; set; }

        [Display(Name = "نام مدرک تحصیلی")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string majorText { get; set; }

        [Display(Name = "نام واحد کار")]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string unitOfWork { set; get; }

        [Display(Name = "لوگو")]
        public string logo { set; get; }

        public VerifiedStatus verified { set; get; } = VerifiedStatus.NotChecked;
        public int sort { set; get; }

        [Display(Name = "کاربر")]
        [Required]
        public int memberId { set; get; }
        public virtual Member member { set; get; }

        [Display(Name = "توضیحات")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        [Display(Name = "فعال")]
        public bool enable { get; set; }

        [Display(Name = "زمان ثبت")]
        public DateTime registerDate { get; set; } = DateTime.Now;

        public virtual IList<Media> cataloges { set; get; }
    }
}