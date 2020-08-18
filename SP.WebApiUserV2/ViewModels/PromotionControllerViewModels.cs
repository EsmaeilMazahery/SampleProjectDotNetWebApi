using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SP.WebApiUser.ViewModels
{
    public class InsertPromotionViewModel
    {
        public int promotionCode { set; get; }

        //عنوان نمایش پروموشن
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string title { set; get; }

        //نوع پروموشن
        public string type { set; get; }

        //تحت کمپین
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string campaign { set; get; }

        public string promotionCategory { set; get; }

        public decimal minDiscount { set; get; }
        public decimal minPercentDiscount { set; get; }
        public decimal minPrice { set; get; }
        public decimal maxPrice { set; get; }

        public DateTime startTime { set; get; }

        public DateTime endTime { set; get; }

        //مهلت
        public DateTime deadline { set; get; }

        //تعداد تنوع
        public int countProductPrice { set; get; }

        public virtual List<int> selectedCategories { get; set; }
    }

    public class UpdatePromotionViewModel
    {
        public int promotionId { set; get; }

        public int promotionCode { set; get; }

        //عنوان نمایش پروموشن
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string title { set; get; }

        //نوع پروموشن
        public string type { set; get; }

        //تحت کمپین
        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string campaign { set; get; }

        public string promotionCategory { set; get; }

        public decimal minDiscount { set; get; }
        public decimal minPercentDiscount { set; get; }
        public decimal minPrice { set; get; }
        public decimal maxPrice { set; get; }

        public DateTime startTime { set; get; }

        public DateTime endTime { set; get; }

        //مهلت
        public DateTime deadline { set; get; }

        //تعداد تنوع
        public int countProductPrice { set; get; }

        public virtual List<int> selectedCategories { get; set; }

    }
}
