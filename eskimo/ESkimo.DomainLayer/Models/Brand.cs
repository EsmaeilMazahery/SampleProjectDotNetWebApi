using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class Brand
    {
        public int brandId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string image { set; get; }

        public bool enable { set; get; } = true;

        public virtual IList<Product> products { get; set; }
        public virtual IList<Rel_CategoryBrand> categories { get; set; }
        public virtual IList<Rel_DiscountCodeBrand> discountCodes { get; set; }
    }
}
