using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiMember.ViewModels
{
    public class InsertProductPriceViewModel
    {
        //نام تنوع
        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        //محصول
        public int productId { set; get; }

        //فروشنده
        public int sellerId { set; get; }

        //گارانتی
        public int guaranteeId { set; get; }

        //کد رنگ
        public int colorId { set; get; }

        //قیمت کمینه پایه
        public decimal minBasePrice { set; get; }

        //قیمت بیشینه پایه
        public decimal maxBasePrice { set; get; }

        // پله افزایش
        public decimal stepIncrease { set; get; }

        //کاهش پایه
        public decimal stepDecrease { set; get; }

        //فعال
        public bool enable { set; get; }

        //کالای اصلی
        public bool original { set; get; }

        //بازه ارسال
        public int sendRange { set; get; }

        //موجودی نزد فروشنده
        public int inventory { set; get; }

        //تعداد در سبد
        public int countBasket { set; get; }

        //تعداد در سبد خرید
        public int countBasketSale { set; get; }

        //قیمت کمینه پروموشن
        public decimal promotionMinPrice { set; get; }

        //قیمت بیشینه پروموشن
        public decimal promotionMaxPrice { set; get; }

        //موجودی در پروموشن
        public decimal promotionInventory { set; get; }

        //امتیاز
        public decimal rate { set; get; }

        //رضایت خرید محصول
        public decimal satisfyingSale { set; get; }

        //موجودی در انبار دیجی کالا
        public int digikalaDepo { set; get; }

        //اخرین تغییر
        public DateTime lastChangeDate { set; get; } = DateTime.Now;
    }

    public class UpdateProductPriceViewModel
    {
        public int productPriceId { set; get; }

        //نام تنوع
        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        //محصول
        public int productId { set; get; }

        //فروشنده
        public int sellerId { set; get; }

        //گارانتی
        public int guaranteeId { set; get; }

        //کد رنگ
        public int colorId { set; get; }

        //قیمت کمینه پایه
        public decimal minBasePrice { set; get; }

        //قیمت بیشینه پایه
        public decimal maxBasePrice { set; get; }

        // پله افزایش
        public decimal stepIncrease { set; get; }

        //کاهش پایه
        public decimal stepDecrease { set; get; }

        //فعال
        public bool enable { set; get; }

        //کالای اصلی
        public bool original { set; get; }

        //بازه ارسال
        public int sendRange { set; get; }

        //موجودی نزد فروشنده
        public int inventory { set; get; }

        //تعداد در سبد
        public int countBasket { set; get; }

        //تعداد در سبد خرید
        public int countBasketSale { set; get; }

        //قیمت کمینه پروموشن
        public decimal promotionMinPrice { set; get; }

        //قیمت بیشینه پروموشن
        public decimal promotionMaxPrice { set; get; }

        //موجودی در پروموشن
        public decimal promotionInventory { set; get; }


        // پله افزایش پروموشن
        public decimal stepIncreasePromotion { set; get; }

        //کاهش پله پروموشن
        public decimal stepDecreasePromotion { set; get; }

        //تعداد در سبد پروموشن
        public int countBasketPromotion { set; get; }

        //تعداد در پروموشن
        public int countPromotion { set; get; }

        //امتیاز
        public decimal rate { set; get; }

        //رضایت خرید محصول
        public decimal satisfyingSale { set; get; }

        //موجودی در انبار دیجی کالا
        public int digikalaDepo { set; get; }

        //اخرین تغییر
        public DateTime lastChangeDate { set; get; } = DateTime.Now;
    }
}
