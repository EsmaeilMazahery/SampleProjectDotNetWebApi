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
    public class InsertDiscountFactorViewModel
    {
        public decimal minPrice { set; get; }

        public decimal maxDiscount { set; get; }

        public decimal percent { set; get; }

        public decimal discount { set; get; }

        public DateTime? startDate { set; get; }

        public DateTime? endDate { set; get; }

        //ثبت نام کاربر برای مشتریان جدید
        public int maxRegisterDate { set; get; }

        public bool enable { set; get; }
    }

    public class UpdateDiscountFactorViewModel
    {
        public int discountFactorId { set; get; }

        public decimal minPrice { set; get; }

        public decimal maxDiscount { set; get; }

        public decimal percent { set; get; }

        public decimal discount { set; get; }

        public DateTime? startDate { set; get; }

        public DateTime? endDate { set; get; }

        //ثبت نام کاربر برای مشتریان جدید
        public int maxRegisterDate { set; get; }

        public bool enable { set; get; }
    }
}
