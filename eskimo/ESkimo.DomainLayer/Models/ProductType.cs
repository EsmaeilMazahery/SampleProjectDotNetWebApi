using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    //شامل کوچک - بزرگ - 
    public class ProductType
    {
        public int productTypeId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name{ set;get;}

        public bool enable { set; get; } = true;

        public virtual IList<Product> products { get; set; }
        public virtual IList<Rel_CategoryProductType> categories { get; set; }
    }
}
