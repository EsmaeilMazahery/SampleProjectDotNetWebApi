
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SP.DomainLayer.Models;
using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.ViewModel
{

    public class ListServiceModel
    {
        [Display(Name = "نام")]
        public string name { get; set; }

        [Display(Name = "کاربر")]
        public int? memberId { get; set; }

        public PaggingViewModelBase pagging { set; get; }

        public List<ListItemServiceModel> list { set; get; }
    }

    public class ListItemServiceModel
    {
        public int memberId { get; set; }

        [Display(Name = "نام")]
        public string name { get; set; }

        [Display(Name = "نام واحد کار")]
        public string unitOfWork { set; get; }

        [Display(Name = "لوگو")]
        public string logo { set; get; }

        [Display(Name = "فعال")]
        public bool enable { get; set; }
    }

    public class RegisterServiceViewModel
    {
        [Key]
        public int serviceId { get; set; }

        [Display(Name = "نام")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string name { get; set; }
        public int memberId { get; set; }

    }

    public class RegisterAdminServiceViewModel
    {
        [Key]
        public int serviceId { get; set; }

        [Display(Name = "نام")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string name { get; set; }

        [Display(Name = "نام واحد کار")]
        public string unitOfWork { set; get; }

        [Display(Name = "لوگو")]
        public string logo { set; get; }

        [Display(Name = "توضیحات")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        [Display(Name = "فعال")]
        public bool enable { get; set; }

        public int memberId { get; set; }

        public virtual IList<int> locations { set; get; }

    }


    public class EditServiceViewModel
    {
        [Key]
        public int serviceId { get; set; }

        [Display(Name = "نام")]
        [Required]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string name { get; set; }

        [Display(Name = "نام واحد کار")]
        public string unitOfWork { set; get; }

        [Display(Name = "لوگو")]
        public string logo { set; get; } 

        [Display(Name = "توضیحات")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string description { set; get; }

        [Display(Name = "نام محل کار")]
        [MaxLength(ConstantValidations.NameMaxLength)]
        public string nameWorkPlace { get; set; }

        [Display(Name = "آدرس سایت")]
        [MaxLength(ConstantValidations.WebAddressLength)]
        public string website { get; set; }

        [Display(Name = "تصویر مدرک تحصیلی")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string majorImg { get; set; }

        [Display(Name = "رشته تحصیلی")]
        [MaxLength(ConstantValidations.DescriptionLength)]
        public string majorText { get; set; }


        [Display(Name = "رتبه شرکت")]
        public int rating { set; get; }

        public bool isTemp { get; set; }

        [Display(Name = "فعال")]
        public bool enable { get; set; }


        public virtual IList<int> locations { set; get; }
    }
}