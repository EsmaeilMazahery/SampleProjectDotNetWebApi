using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class ProductPrice
    {
        public int productPriceId { set; get; }
        
        public int productId { set; get; }
        public virtual Product product { set; get; }
        
        public decimal amount { set; get; }

        //موجودی
        public int count { set; get; }

        public int minCountSell { set; get; }

        public bool enable { set; get; }

        public virtual IList<FactorItem> factorItems { get; set; }
    }

    public class ProductPriceWholesale
    {
        public int productPriceWholesaleId { set; get; }

        public int productId { set; get; }
        public virtual Product product { set; get; }

        public decimal amount { set; get; }

        //موجودی
        public int count { set; get; }

        public int minCountSell { set; get; }

        public bool enable { set; get; }
    }
}
