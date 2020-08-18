using ESkimo.DomainLayer.Models;
using ESkimo.Infrastructure.Constants;
using ESkimo.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESkimo.WebApiUser.ViewModels
{
    public class InsertProductViewModel
    {
        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        public decimal amountBase { set; get; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string imageAddress { get; set; }

        public bool enable { set; get; } = true;

        public string attributes { set; get; }

        public string description { set; get; }

        public int brandId { set; get; }

        public int categoryId { set; get; }

        public int productTypeId { set; get; }

        public virtual IList<ProductPrice> productPrices { get; set; } = new List<ProductPrice>();
        public virtual IList<ProductPriceWholesale> productPriceWholesales { get; set; } = new List<ProductPriceWholesale>();

    }

    public class UpdateProductViewModel
    {
        [Key]
        public int productId { get; set; }

        [Required]
        [MaxLength(Infrastructure.Constants.ConstantValidations.NameLength)]
        public string name { get; set; }

        public decimal amountBase { set; get; }

        [MaxLength(ConstantValidations.WebAddressLength)]
        public string imageAddress { get; set; }

        public bool enable { set; get; } = true;

        public string attributes { set; get; }

        public string description { set; get; }

        public int brandId { set; get; }

        public int categoryId { set; get; }

        public int productTypeId { set; get; }

        public virtual IList<ProductPrice> productPrices { get; set; } = new List<ProductPrice>();
        public virtual IList<ProductPriceWholesale> productPriceWholesales { get; set; } = new List<ProductPriceWholesale>();
    }
}
