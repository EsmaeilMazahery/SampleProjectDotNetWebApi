using ESkimo.Infrastructure.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace ESkimo.DomainLayer.Models
{
    public class Rel_CategoryProductType
    {
        [Key]
        public int categoryId { get; set; }
        public virtual Category category { get; set; }

        [Key]
        public int productTypeId { get; set; }
        public virtual ProductType productType { get; set; }
    }
}