using ESkimo.Infrastructure.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ESkimo.DomainLayer.Models
{
    public class Rel_CategoryBrand
    {
        [Key]
        public int categoryId { get; set; }
        public virtual Category category { get; set; }

        [Key]
        public int brandId { get; set; }
        public virtual Brand brand { get; set; }
    }
}