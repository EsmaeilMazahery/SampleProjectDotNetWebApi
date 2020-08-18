using ESkimo.Infrastructure.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ESkimo.DomainLayer.Models
{
    public class Rel_DiscountCodeCategory
    {
        [Key]
        public int discountCodeId { get; set; }
        public virtual DiscountCode discountCode { get; set; }

        [Key]
        public int categoryId { get; set; }
        public virtual Category category { get; set; }
    }
}