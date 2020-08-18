using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class DiscountFactor
    {
        public int discountFactorId { set; get; }

        public decimal? minPrice { set; get; } = null;

        public decimal? percent { set; get; } = null;

        public decimal? maxDiscount { set; get; } = null;

        public decimal? discount { set; get; } = null;

        public DateTime? startDate { set; get; } = null;
        public DateTime? endDate { set; get; } = null;

        //ثبت نام کاربر برای مشتریان جدید
        public int maxRegisterDate { set; get; }

        public bool enable { set; get; }

        public virtual IList<Factor> factors { get; set; }
    }
}
