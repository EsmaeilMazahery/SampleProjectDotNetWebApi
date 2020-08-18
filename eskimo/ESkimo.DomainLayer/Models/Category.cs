using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class Category
    {
        public int categoryId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public bool enable { set; get; } = true;

        public int? parentId { set; get; }

        public virtual Category parent { set; get; }

        public virtual IList<Category> children { get; set; } = new List<Category>();

        public virtual IList<Product> products { get; set; }

        public virtual IList<Rel_CategoryBrand> brands { get; set; }

        public virtual IList<Rel_CategoryProductType> productTypes { get; set; }

        public virtual IList<Rel_DiscountCodeCategory> discountCodes { get; set; }
    }
}
