using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
   public class DiscountCode
    {
        public int discountCodeId { set; get; }
        
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [Required]
        [MaxLength(ConstantValidations.IdentityLength)]
        public string code { set; get; }

        public decimal minPrice { set; get; }

        public decimal? maxDiscount { set; get; }

        public decimal? percent { set; get; }

        public decimal? discount { set; get; }

        public DateTime? startDate { set; get; }

        public DateTime? endDate { set; get; }

        //تعداد استفاده هر شخص
        public int? countUse { set; get; } = null;

        //ثبت نام کاربر برای مشتریان جدید
        //تعداد روز گذشته از ثبت نام
        public int? maxRegisterDate { set; get; }

        //در صورت فعال بودن تمام تخفیف های دیگر از بین می رود
        public bool activeAlone { set; get; }

        public bool enable { set; get; }

        public virtual IList<Rel_DiscountCodeCategory> categories { get; set; } = new List<Rel_DiscountCodeCategory>();
        public virtual IList<Rel_DiscountCodeBrand> brands { get; set; } = new List<Rel_DiscountCodeBrand>();
        public virtual IList<Factor> factors { get; set; }

        public int? memberId { set; get; }
        public virtual Member member { set; get; }
    }
}
