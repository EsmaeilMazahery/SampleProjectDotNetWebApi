using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class Product
    {
        [Key]
        public int productId { get; set; }
        
        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string imageAddress { get; set; }

        public decimal amountBase { set; get; }

        public bool enable { set; get; } = true;

        public string attributes { set; get; }

        public string description { set; get; }

        public int brandId { set; get; }

        public virtual Brand brand { set; get; }

        public int categoryId { set; get; }

        public virtual Category category { set; get; }

        public int productTypeId { set; get; }

        public virtual ProductType productType { set; get; }

        public virtual IList<ProductPrice> productPrices { get; set; } = new List<ProductPrice>();
        public virtual IList<ProductPriceWholesale> productPriceWholesales { get; set; } = new List<ProductPriceWholesale>();

        public virtual IList<Comment> comments { get; set; } = new List<Comment>();
    }
}
