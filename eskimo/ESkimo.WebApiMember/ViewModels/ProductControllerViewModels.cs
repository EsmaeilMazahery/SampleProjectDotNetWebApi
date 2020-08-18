using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiMember.ViewModels
{
    

    public class BasketViewModel
    {
        public string discountCode { set; get; }
        public int? periodTypeId { set; get; }
        public int? memberLocationId { set; get; }
        public PayType payType { set; get; }
        public PaymentType paymentType { set; get; }
        public IList<BasketItem> products { set; get; } = new List<BasketItem>();

        public DiscountCode discountCodeData { set; get; }
        public MemberLocation memberLocation { set; get; }
        public string messageDiscountCode { set; get; }
        public decimal amountDiscountFactor { set; get; }
        public decimal amountDiscountPeriod { set; get; }
        public IEnumerable<Product> list { set; get; }
    }

    public class BasketItem
    {
        public int productId { set; get; }
        public int count { set; get; }
    }


    public class InsertProductViewModel
    {
        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string dkpCode { set; get; }

        public decimal minPrice { set; get; }

        public decimal minPromotion { set; get; }

        public int brandId { set; get; }
        
        public int categoryId { set; get; }
        
        public int digiTypeId { set; get; }
        
        public virtual List<int> selectedColors { get; set; }
        public virtual List<int> selectedDigiAttributeValues { get; set; }
    }

    public class UpdateProductViewModel
    {
        [Key]
        public int productId { get; set; }

        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string dkpCode { set; get; }

        public decimal minPrice { set; get; }

        public decimal minPromotion { set; get; }

        public int brandId { set; get; }

        public int categoryId { set; get; }

        public int digiTypeId { set; get; }

        public virtual List<int> selectedColors { get; set; }
        public virtual List<int> selectedDigiAttributeValues { get; set; }
    }
}
