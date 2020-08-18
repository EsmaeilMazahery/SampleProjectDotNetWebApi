using ESkimo.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESkimo.DomainLayer.Models
{
    public class FactorItem
    {
        public int factorItemId { set; get; }

        [Required]
        [MaxLength(ConstantValidations.NameLength)]
        public string name { set; get; }

        public decimal amountBasePerItem { set; get; }

        public decimal amountPerItem { set; get; }

        public int count { set; get; }

        public decimal amount { set; get; }

        public int? productPriceId { set; get; }
        public virtual ProductPrice productPrice { set; get; }

        public int factorId { set; get; }
        public virtual Factor factor { set; get; }
    }
}
