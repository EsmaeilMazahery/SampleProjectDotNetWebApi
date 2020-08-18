using ESkimo.Infrastructure.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ESkimo.DomainLayer.Models
{
    public class Rel_DiscountCodeBrand
    {
        [Key]
        public int discountCodeId { get; set; }
        public virtual DiscountCode discountCode { get; set; }

        [Key]
        public int brandId { get; set; }
        public virtual Brand brand { get; set; }
    }
}