using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertDiscountCodeViewModel
    {
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.IdentityLength)]
        public string code { set; get; }

        public int countSell { set; get; }

        public decimal minPrice { set; get; }

        public decimal maxDiscount { set; get; }

        public decimal percent { set; get; }

        public decimal discount { set; get; }

        public DateTime? startDate { set; get; }

        public DateTime? endDate { set; get; }

        //تعداد استفاده هر شخص
        public int? countUse { set; get; } = null;

        //ثبت نام کاربر برای مشتریان جدید
        public int maxRegisterDate { set; get; }

        //در صورت فعال بودن تمام تخفیف های دیگر از بین می رود
        public bool activeAlone { set; get; }

        public bool enable { set; get; }

        public IList<int> selectedBrands { set; get; }
        public IList<int> selectedCategories { set; get; }
    }

    public class UpdateDiscountCodeViewModel
    {
        public int discountCodeId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.IdentityLength)]
        public string code { set; get; }

        public int countSell { set; get; }

        public decimal minPrice { set; get; }

        public decimal maxDiscount { set; get; }

        public decimal percent { set; get; }

        public decimal discount { set; get; }

        public DateTime? startDate { set; get; }

        public DateTime? endDate { set; get; }

        //تعداد استفاده هر شخص
        public int? countUse { set; get; } = null;

        //ثبت نام کاربر برای مشتریان جدید
        public int maxRegisterDate { set; get; }

        //در صورت فعال بودن تمام تخفیف های دیگر از بین می رود
        public bool activeAlone { set; get; }

        public bool enable { set; get; }
        public IList<int> selectedBrands { set; get; }
        public IList<int> selectedCategories { set; get; }
    }
}
